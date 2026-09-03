using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class WorldEventBus
{
    private static readonly Dictionary<string, Action> listeners = new();

    public static void Register(string id, Action callback)
    {
        if (!listeners.ContainsKey(id)) listeners[id] = null;
        listeners[id] += callback;
    }

    public static void Unregister(string id, Action callback)
    {
        if (listeners.ContainsKey(id)) listeners[id] -= callback;
    }

    public static void Raise(string id)
    {
        if (listeners.TryGetValue(id, out Action action) && action != null)
            action.Invoke();
    }
}
