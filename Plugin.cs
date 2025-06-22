using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core;
using RecipeHint.Components;
using UnityEngine;

namespace RecipeHint;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger;
        
    private void Awake() {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;
        
        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }
    
    private void Start() {
        this.gameObject.AddComponent<RecipeManager>();
    }
}