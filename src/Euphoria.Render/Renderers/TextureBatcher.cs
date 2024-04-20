﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using grabs.Graphics;
using grabs.ShaderCompiler.DXC;
using u4.Math;
using Buffer = grabs.Graphics.Buffer;

namespace Euphoria.Render.Renderers;

public class TextureBatcher : IDisposable
{
    public const uint MaxBatchSize = 2048;

    private const uint NumVertices = 4;
    private const uint NumIndices = 6;

    private const uint MaxVertices = NumVertices * MaxBatchSize;
    private const uint MaxIndices = NumIndices * MaxBatchSize;

    private Device _device;

    private Vertex[] _vertices;
    private uint[] _indices;
    
    private Buffer _vertexBuffer;
    private Buffer _indexBuffer;

    private Buffer _transformBuffer;

    private Pipeline _pipeline;

    private List<DrawQueueItem> _drawQueue;

    public TextureBatcher(Device device)
    {
        _device = device;

        _vertices = new Vertex[MaxVertices];
        _indices = new uint[MaxIndices];

        _vertexBuffer =
            device.CreateBuffer(new BufferDescription(BufferType.Vertex, MaxVertices * Vertex.SizeInBytes, true));
        _indexBuffer = device.CreateBuffer(new BufferDescription(BufferType.Index, MaxIndices * sizeof(uint), true));

        _transformBuffer = device.CreateBuffer(BufferType.Constant, Matrix4x4.Identity, true);
        
        string textureShader = File.ReadAllText("EngineContent/Shaders/Texture.hlsl");
        ShaderModule vTexModule = device.CreateShaderModule(ShaderStage.Vertex,
            Compiler.CompileToSpirV(textureShader, "Vertex", ShaderStage.Vertex, true), "Vertex");
        ShaderModule pTexModule = device.CreateShaderModule(ShaderStage.Pixel,
            Compiler.CompileToSpirV(textureShader, "Pixel", ShaderStage.Pixel, true), "Pixel");

        _pipeline = device.CreatePipeline(new PipelineDescription(vTexModule, pTexModule, new[]
        {
            new InputLayoutDescription(Format.R32G32_Float, 0, 0, InputType.PerVertex), // Position
            new InputLayoutDescription(Format.R32G32_Float, 8, 0, InputType.PerVertex), // TexCoord
            new InputLayoutDescription(Format.R32G32B32A32_Float, 16, 0, InputType.PerVertex) // Tint
        }));
        
        vTexModule.Dispose();
        pTexModule.Dispose();

        _drawQueue = new List<DrawQueueItem>();
    }

    public void Draw(Texture texture, Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight, Vector4 tint)
    {
        _drawQueue.Add(new DrawQueueItem(texture, topLeft, topRight, bottomLeft, bottomRight, tint));
    }

    internal void DispatchDrawQueue(CommandList cl, Size<int> viewportSize)
    {
        cl.UpdateBuffer(_transformBuffer, 0,
            Matrix4x4.CreateOrthographicOffCenter(0, viewportSize.Width, viewportSize.Height, 0, -1, 1));
        
        uint currentDraw = 0;
        Texture currentTexture = null;
        
        foreach (DrawQueueItem item in _drawQueue)
        {
            if (item.Texture != currentTexture || currentDraw >= MaxBatchSize)
            {
                FlushVertices(cl, currentDraw, currentTexture);
                currentDraw = 0;
            }

            currentTexture = item.Texture;

            uint vCurrent = currentDraw * NumVertices;
            uint iCurrent = currentDraw * NumIndices;

            _vertices[vCurrent + 0] = new Vertex(item.TopLeft, new Vector2(0, 0), item.Tint);
            _vertices[vCurrent + 1] = new Vertex(item.TopRight, new Vector2(1, 0), item.Tint);
            _vertices[vCurrent + 2] = new Vertex(item.BottomRight, new Vector2(1, 1), item.Tint);
            _vertices[vCurrent + 3] = new Vertex(item.BottomLeft, new Vector2(0, 1), item.Tint);

            _indices[iCurrent + 0] = 0 + vCurrent;
            _indices[iCurrent + 1] = 1 + vCurrent;
            _indices[iCurrent + 2] = 3 + vCurrent;
            _indices[iCurrent + 3] = 1 + vCurrent;
            _indices[iCurrent + 4] = 2 + vCurrent;
            _indices[iCurrent + 5] = 3 + vCurrent;

            currentDraw++;
        }
        
        FlushVertices(cl, currentDraw, currentTexture);
        
        // TODO: Probably should NOT clear the draw queue here, not sure.
        _drawQueue.Clear();
    }

    private void FlushVertices(CommandList cl, uint drawCount, Texture texture)
    {
        if (drawCount == 0)
            return;
        
        cl.UpdateBuffer(_vertexBuffer, 0, drawCount * NumVertices * Vertex.SizeInBytes,
            new ReadOnlySpan<Vertex>(_vertices));

        cl.UpdateBuffer(_indexBuffer, 0, drawCount * NumIndices * sizeof(uint), new ReadOnlySpan<uint>(_indices));
        
        cl.SetPipeline(_pipeline);
        
        cl.SetVertexBuffer(0, _vertexBuffer, Vertex.SizeInBytes, 0);
        cl.SetIndexBuffer(_indexBuffer, Format.R32_UInt);
        
        cl.SetConstantBuffer(0, _transformBuffer);
        cl.SetTexture(1, texture.ApiTexture);
        
        cl.DrawIndexed(drawCount * NumIndices);
    }
    
    public void Dispose()
    {
        _pipeline.Dispose();
        _transformBuffer.Dispose();
        _indexBuffer.Dispose();
        _vertexBuffer.Dispose();
    }

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct Vertex
    {
        public readonly Vector2 Position;
        public readonly Vector2 TexCoord;
        public readonly Vector4 Tint;

        public Vertex(Vector2 position, Vector2 texCoord, Vector4 tint)
        {
            Position = position;
            TexCoord = texCoord;
            Tint = tint;
        }

        public const uint SizeInBytes = 32;
    }

    private readonly struct DrawQueueItem
    {
        public readonly Texture Texture;
        public readonly Vector2 TopLeft;
        public readonly Vector2 TopRight;
        public readonly Vector2 BottomLeft;
        public readonly Vector2 BottomRight;
        public readonly Vector4 Tint;

        public DrawQueueItem(Texture texture, Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight, Vector4 tint)
        {
            Texture = texture;
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
            Tint = tint;
        }
    }
}