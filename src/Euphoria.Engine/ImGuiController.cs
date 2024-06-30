﻿using System;
using System.Numerics;
using Euphoria.Render;
using ImGuiNET;

namespace Euphoria.Engine;

internal static class ImGuiController
{
    private static IntPtr _context;
    
    internal static void Initialize(Graphics graphics, Window window)
    {
        _context = graphics.ImGuiRenderer.ImGuiContext;
        
        window.MouseMove += OnMouseMove;
        window.MouseButtonDown += OnMouseButtonDown;
        window.MouseButtonUp += OnMouseButtonUp;
        window.KeyDown += OnKeyDown;
        window.KeyUp += OnKeyUp;
        window.MouseScroll += OnMouseScroll;
        window.TextInput += OnTextInput;
    }

    internal static void Update(float dt)
    {
        ImGui.SetCurrentContext(_context);

        ImGuiIOPtr io = ImGui.GetIO();
        io.DeltaTime = dt;
        
        ImGui.NewFrame();
    }
    
    private static void OnMouseMove(Vector2 position, Vector2 delta)
    {
        ImGui.SetCurrentContext(_context);
        ImGui.GetIO().AddMousePosEvent(position.X, position.Y);
    }
    
    private static void OnMouseButtonDown(MouseButton button)
    {
        ImGui.SetCurrentContext(_context);
        ImGui.GetIO().AddMouseButtonEvent((int) MouseButtonToImGui(button), true);
    }
    
    private static void OnMouseButtonUp(MouseButton button)
    {
        ImGui.SetCurrentContext(_context);
        ImGui.GetIO().AddMouseButtonEvent((int) MouseButtonToImGui(button), false);
    }
    
    private static void OnKeyDown(Key key)
    {
        ImGui.SetCurrentContext(_context);
        ImGui.GetIO().AddKeyEvent(KeyToImGui(key), true);
    }
    
    private static void OnKeyUp(Key key)
    {
        ImGui.SetCurrentContext(_context);
        ImGui.GetIO().AddKeyEvent(KeyToImGui(key), false);
    }
    
    private static void OnMouseScroll(Vector2 scroll)
    {
        ImGui.SetCurrentContext(_context);
        ImGui.GetIO().AddMouseWheelEvent(scroll.X, scroll.Y);
    }

    private static void OnTextInput(char c)
    {
        ImGui.SetCurrentContext(_context);
        ImGui.GetIO().AddInputCharacter(c);
    }
    
    private static ImGuiMouseButton MouseButtonToImGui(MouseButton button)
    {
        return button switch
        {
            MouseButton.Left => ImGuiMouseButton.Left,
            MouseButton.Middle => ImGuiMouseButton.Middle,
            MouseButton.Right => ImGuiMouseButton.Right,
        };
    }

    private static ImGuiKey KeyToImGui(Key key)
    {
        return key switch
        {
            Key.Unknown => ImGuiKey.None,
            Key.Backspace => ImGuiKey.Backspace,
            Key.Tab => ImGuiKey.Tab,
            Key.Enter => ImGuiKey.Enter,
            Key.Escape => ImGuiKey.Escape,
            Key.Space => ImGuiKey.Space,
            Key.Hash => ImGuiKey.None,
            Key.Apostrophe => ImGuiKey.Apostrophe,
            Key.Comma => ImGuiKey.Comma,
            Key.Minus => ImGuiKey.Minus,
            Key.Period => ImGuiKey.Period,
            Key.ForwardSlash => ImGuiKey.Slash,
            Key.Num0 => ImGuiKey._0,
            Key.Num1 => ImGuiKey._1,
            Key.Num2 => ImGuiKey._2,
            Key.Num3 => ImGuiKey._3,
            Key.Num4 => ImGuiKey._4,
            Key.Num5 => ImGuiKey._5,
            Key.Num6 => ImGuiKey._6,
            Key.Num7 => ImGuiKey._7,
            Key.Num8 => ImGuiKey._8,
            Key.Num9 => ImGuiKey._9,
            Key.Semicolon => ImGuiKey.Semicolon,
            Key.Equals => ImGuiKey.Equal,
            Key.LeftBracket => ImGuiKey.LeftBracket,
            Key.Backslash => ImGuiKey.Backslash,
            Key.RightBracket => ImGuiKey.RightBracket,
            Key.Backquote => ImGuiKey.GraveAccent,
            Key.A => ImGuiKey.A,
            Key.B => ImGuiKey.B,
            Key.C => ImGuiKey.C,
            Key.D => ImGuiKey.D,
            Key.E => ImGuiKey.E,
            Key.F => ImGuiKey.F,
            Key.G => ImGuiKey.G,
            Key.H => ImGuiKey.H,
            Key.I => ImGuiKey.I,
            Key.J => ImGuiKey.J,
            Key.K => ImGuiKey.K,
            Key.L => ImGuiKey.L,
            Key.M => ImGuiKey.M,
            Key.N => ImGuiKey.N,
            Key.O => ImGuiKey.O,
            Key.P => ImGuiKey.P,
            Key.Q => ImGuiKey.Q,
            Key.R => ImGuiKey.R,
            Key.S => ImGuiKey.S,
            Key.T => ImGuiKey.T,
            Key.U => ImGuiKey.U,
            Key.V => ImGuiKey.V,
            Key.W => ImGuiKey.W,
            Key.X => ImGuiKey.X,
            Key.Y => ImGuiKey.Y,
            Key.Z => ImGuiKey.Z,
            Key.Delete => ImGuiKey.Delete,
            Key.Insert => ImGuiKey.Insert,
            Key.Right => ImGuiKey.RightArrow,
            Key.Left => ImGuiKey.LeftArrow,
            Key.Down => ImGuiKey.DownArrow,
            Key.Up => ImGuiKey.UpArrow,
            Key.PageUp => ImGuiKey.PageUp,
            Key.PageDown => ImGuiKey.PageDown,
            Key.Home => ImGuiKey.Home,
            Key.End => ImGuiKey.End,
            Key.CapsLock => ImGuiKey.CapsLock,
            Key.ScrollLock => ImGuiKey.ScrollLock,
            Key.NumLock => ImGuiKey.NumLock,
            Key.PrintScreen => ImGuiKey.PrintScreen,
            Key.Pause => ImGuiKey.Pause,
            Key.F1 => ImGuiKey.F1,
            Key.F2 => ImGuiKey.F2,
            Key.F3 => ImGuiKey.F3,
            Key.F4 => ImGuiKey.F4,
            Key.F5 => ImGuiKey.F5,
            Key.F6 => ImGuiKey.F6,
            Key.F7 => ImGuiKey.F7,
            Key.F8 => ImGuiKey.F8,
            Key.F9 => ImGuiKey.F9,
            Key.F10 => ImGuiKey.F10,
            Key.F11 => ImGuiKey.F11,
            Key.F12 => ImGuiKey.F12,
            Key.F13 => ImGuiKey.F13,
            Key.F14 => ImGuiKey.F14,
            Key.F15 => ImGuiKey.F15,
            Key.F16 => ImGuiKey.F16,
            Key.F17 => ImGuiKey.F17,
            Key.F18 => ImGuiKey.F18,
            Key.F19 => ImGuiKey.F18,
            Key.F20 => ImGuiKey.F20,
            Key.F21 => ImGuiKey.F21,
            Key.F22 => ImGuiKey.F22,
            Key.F23 => ImGuiKey.F23,
            Key.F24 => ImGuiKey.F24,
            Key.Keypad0 => ImGuiKey.Keypad0,
            Key.Keypad1 => ImGuiKey.Keypad1,
            Key.Keypad2 => ImGuiKey.Keypad2,
            Key.Keypad3 => ImGuiKey.Keypad3,
            Key.Keypad4 => ImGuiKey.Keypad4,
            Key.Keypad5 => ImGuiKey.Keypad5,
            Key.Keypad6 => ImGuiKey.Keypad6,
            Key.Keypad7 => ImGuiKey.Keypad7,
            Key.Keypad8 => ImGuiKey.Keypad8,
            Key.Keypad9 => ImGuiKey.Keypad9,
            Key.KeypadDecimal => ImGuiKey.KeypadDecimal,
            Key.KeypadDivide => ImGuiKey.KeypadDivide,
            Key.KeypadMultiply => ImGuiKey.KeypadMultiply,
            Key.KeypadSubtract => ImGuiKey.KeypadSubtract,
            Key.KeypadAdd => ImGuiKey.KeypadAdd,
            Key.KeypadEnter => ImGuiKey.KeypadEnter,
            Key.KeypadEqual => ImGuiKey.KeypadEqual,
            Key.LeftShift => ImGuiKey.LeftShift,
            Key.LeftControl => ImGuiKey.LeftCtrl,
            Key.LeftAlt => ImGuiKey.LeftAlt,
            Key.LeftSuper => ImGuiKey.LeftSuper,
            Key.RightShift => ImGuiKey.RightShift,
            Key.RightControl => ImGuiKey.RightCtrl,
            Key.RightAlt => ImGuiKey.RightAlt,
            Key.RightSuper => ImGuiKey.RightSuper,
            Key.Menu => ImGuiKey.Menu,
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        };
    }
}