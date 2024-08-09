﻿using Euphoria.Engine.InputSystem.Bindings;

namespace Euphoria.Engine.InputSystem.Actions;

public class AxisAction : IInputAction
{
    public readonly IInputBinding<float>[] Bindings;

    public float Value;

    public AxisAction(params IInputBinding<float>[] bindings)
    {
        Bindings = bindings;
    }

    public void Update()
    {
        Value = 0;
        
        foreach (IInputBinding<float> binding in Bindings)
            Value += binding.Value;
    }
}