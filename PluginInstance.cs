﻿using UnityEngine;

namespace RecipeHint;

public abstract class PluginInstance<T>: MonoBehaviour where T : MonoBehaviour{
    public static T Instance;

    protected virtual void Awake() {
        PluginInstance<T>.Instance = this as T;
        Plugin.Logger.LogInfo($"PluginInstance<{typeof(T).Name}> initialized.");
    }

    protected virtual void OnDestroy() {
        Object.Destroy(base.gameObject);
    }
}