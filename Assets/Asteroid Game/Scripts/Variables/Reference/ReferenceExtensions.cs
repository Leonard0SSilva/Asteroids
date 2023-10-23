using System.Collections.Generic;
using UnityEngine;

public static class ReferenceExtension
{
    public static void Set(this StringReference reference, string value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            StringVariable variable = reference.variable as StringVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this FloatReference reference, float value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            FloatVariable variable = reference.variable as FloatVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this IntReference reference, int value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            IntVariable variable = reference.variable as IntVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this BoolReference reference, bool value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            BoolVariable variable = reference.variable as BoolVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this Vector3Reference reference, Vector2 value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            Vector2Variable variable = reference.variable as Vector2Variable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void SetX(this Vector3Reference reference, float value)
    {
        if (reference.useConstant)
        {
            reference.constantValue.x = value;
        }
        else
        {
            Vector2Variable variable = reference.variable as Vector2Variable;
            variable.value.x = value;
        }
    }

    public static void SetY(this Vector3Reference reference, float value)
    {
        if (reference.useConstant)
        {
            reference.constantValue.y = value;
        }
        else
        {
            Vector2Variable variable = reference.variable as Vector2Variable;
            variable.value.y = value;
        }
    }

    public static void Set(this ColorReference reference, Color value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            ColorVariable variable = reference.variable as ColorVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this AudioClipReference reference, AudioClipSettings value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            AudioClipVariable variable = reference.variable as AudioClipVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this SpriteReference reference, Sprite value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            SpriteVariable variable = reference.variable as SpriteVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }


    public static void Set(this GameObjectReference reference, GameObject value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            GameObjectVariable variable = reference.variable as GameObjectVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this GameObjectListReference reference, List<GameObject> value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            GameObjectListVariable variable = reference.variable as GameObjectListVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this StringListReference reference, List<string> value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            StringListVariable variable = reference.variable as StringListVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }

    public static void Set(this IntListReference reference, List<int> value)
    {
        if (reference.useConstant)
        {
            reference.constantValue = value;
        }
        else
        {
            IntListVariable variable = reference.variable as IntListVariable;
            variable.Set(value);
        }
        reference.onValueChange?.Invoke(value);
    }
}