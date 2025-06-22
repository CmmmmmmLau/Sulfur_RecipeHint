using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using RecipeHint;
using RecipeHint.Components;

[HarmonyWrapSafe]
[HarmonyPatch(typeof(CraftingManager), "Start")]
public class CraftingManagerPatch {
    private static void Postfix(CraftingManager __instance) {
        PluginInstance<RecipeManager>.Instance.CacheRecipe(__instance);
    }
}