using System;
using UnityEngine;

// The 'Variable' class and its generic counterpart 'Variable<T>' are abstract classes designed
// for creating and managing variables. These classes provide a foundation for
// creating different types of variables, handling value changes, and initialization.
// The generic 'Variable<T>' class is derived from 'Variable' and is capable of handling
// variables of specific types (T). It includes features like value change events and debugging.
[Serializable]
public abstract class Variable : ScriptableObject
{
    public virtual T GetValue<T>()
    {
        return default;
    }
}

[Serializable]
public abstract class Variable<T> : Variable
{
    public bool debug;
#if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
#endif
    public T value = default;
    public Action<T> onValueChange = null;

    public void Set(T Value)
    {
        if (debug)
        {
            Debug.Log($"Setting {name}: {Value}");
        }
        onValueChange?.Invoke(Value);
        value = Value;
    }

    public override T1 GetValue<T1>()
    {
        return (T1)Convert.ChangeType(value, typeof(T1));
    }
}