using System.Collections.Generic;
using _Scripts.Configs;
using _Scripts.InventoryLogic.Item;
using Zenject;

namespace _Scripts.InventoryLogic.Crafting
{   
    public class CraftingController
    {
        [Inject] private InventoryConfig _inventoryConfig;
        [Inject] private CraftingUIController _craftingUIController;
        
        private RecipeData[] _recipes;
        
        public void Initialize()
        {
            _recipes = _inventoryConfig.RecipesPool;
            
            _craftingUIController.Initialize();
        }
        
        public RecipeData FindMatch(ItemSlot[,] grid)
        {
            foreach (var recipe in _recipes)
            {
                if (recipe.IsShaped)
                {
                    if (MatchShaped(recipe, grid))
                        return recipe;
                }
                else
                {
                    if (MatchShapeless(recipe, grid))
                        return recipe;
                }
            }

            return null;
        }

        private bool MatchShapeless(RecipeData recipe, ItemSlot[,] grid)
        {
            Dictionary<ItemData, int> collected = new();

            for (int y = 0; y < 3; y++)
            for (int x = 0; x < 3; x++)
            {
                var slot = grid[x, y];
                if (slot.IsEmpty) continue;

                if (!collected.ContainsKey(slot.Item))
                    collected[slot.Item] = 0;

                collected[slot.Item] += slot.Count;
            }

            foreach (var ing in recipe.Ingredients)
            {
                if (!collected.TryGetValue(ing.Item, out var have)) return false;
                if (have < ing.Count) return false;
            }

            return true;
        }

        private bool MatchShaped(RecipeData recipe, ItemSlot[,] grid)
        {
            for (int y = 0; y < 3; y++)
            for (int x = 0; x < 3; x++)
            {
                var req = recipe.Pattern[y * 3 + x];
                var slot = grid[x, y];

                if (req.Item == null)
                {
                    if (!slot.IsEmpty) return false;
                    continue;
                }

                if (slot.IsEmpty) return false;
                if (slot.Item != req.Item) return false;
                if (slot.Count < req.Count) return false;
            }

            return true;
        }

        public void PayCraft(ItemSlot[,] grid, RecipeData recipe)
        {
            if (recipe.IsShaped) PayShaped(grid, recipe);
            else PayShapeless(grid, recipe);
        }

        private void PayShapeless(ItemSlot[,] grid, RecipeData recipe)
        {
            foreach (var ing in recipe.Ingredients)
            {
                int need = ing.Count;

                for (int y = 0; y < 3 && need > 0; y++)
                for (int x = 0; x < 3 && need > 0; x++)
                {
                    var slot = grid[x, y];
                    if (slot.Item != ing.Item) continue;

                    need = slot.Remove(need);
                }
            }
        }

        private void PayShaped(ItemSlot[,] grid, RecipeData recipe)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var req = recipe.Pattern[y * 3 + x];
                    if (req.Item == null) continue;

                    grid[x, y].Remove(req.Count);
                }
            }
        }
    }
}
