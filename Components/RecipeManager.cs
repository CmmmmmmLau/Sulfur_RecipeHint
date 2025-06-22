using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PerfectRandom.Sulfur.Core;
using UnityEngine;

namespace RecipeHint.Components;

public class RecipeManager: MonoBehaviour {
    public Dictionary<string, List<CraftingRecipe>> GenericRecipeCache { get; private set; }
    public Dictionary<string, List<CraftingRecipe>> CookingRecipeCache { get; private set; }
    public Dictionary<string, List<CraftingRecipe>> EnchantRecipeCache { get; private set; }
    public HashSet<CraftingRecipe> AvailableRecipes { get; private set; }
    public HashSet<string> AvailableIngredients { get; private set; }

    public void CacheRecipe(CraftingManager manager) {
        Plugin.Logger.LogInfo("Caching recipes by ingredient");
        
        Plugin.Logger.LogInfo("Caching generic recipes");
        BuildRecipeCache(manager.genericRecipes, GenericRecipeCache);
        
        Plugin.Logger.LogInfo("Caching cooking recipes");
        BuildRecipeCache(manager.cookingRecipes, CookingRecipeCache);
        
        Plugin.Logger.LogInfo("Caching enchantment recipes");
        BuildRecipeCache(manager.enchantmentRecipes, EnchantRecipeCache);
        
        Plugin.Logger.LogInfo("Recipe caching complete");
    }

    private void BuildRecipeCache(List<CraftingRecipe> recipes, Dictionary<string, List<CraftingRecipe>> cache) {
        foreach (var recipe in recipes) {
            var ingredients = recipe.itemsNeeded.Select(definition => definition.item.identifier).ToList();

            foreach (var ingredient in ingredients) {
                if (cache.ContainsKey(ingredient)) {
                    cache[ingredient].Add(recipe);
                } else {
                    cache[ingredient] = [recipe];
                }
            }
        }
    }

    public (HashSet<CraftingRecipe>,  HashSet<string>) CacheAvailableRecipes(Dictionary<string, int> inventory, Dictionary<string, List<CraftingRecipe>> recipes) {
        Plugin.Logger.LogInfo("Caching available recipes based on inventory");
        
        var lists = new HashSet<CraftingRecipe>();
        var items = new HashSet<string>();

        foreach (var item in inventory.Keys) {
            if (recipes.TryGetValue(item, out var recipesList)) {
                lists.UnionWith(recipesList.Where(recipe => {
                    return recipe.itemsNeeded.All(ingredient => ingredient.quantity <= inventory.GetValueOrDefault(ingredient.item.identifier, 0));
                }));
            }
        }
        
        items.UnionWith(lists.Select(recipe => recipe.itemsNeeded.Select(ingredient => ingredient.item.identifier)).SelectMany(x => x));
        
        Plugin.Logger.LogInfo($"Cached {AvailableRecipes.Count} available recipes based on inventory");
        
        return (lists, items);
    }
}