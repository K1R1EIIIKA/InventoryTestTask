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

        public bool FindMatch(
            ItemSlot[,] grid,
            out RecipeData recipe,
            out List<(ItemSlot slot, int count)> usedSlots)
        {
            recipe = null;
            usedSlots = null;

            foreach (var r in _recipes)
            {
                if (!r.IsShaped)
                {
                    if (MatchShapeless(r, grid, out usedSlots))
                    {
                        recipe = r;
                        return true;
                    }
                }
                else
                {
                    if (MatchShaped(r, grid))
                    {
                        recipe = r;
                        usedSlots = CollectShapedSlots(r, grid); 
                        return true;
                    }
                }
            }

            return false;
        }

        private bool MatchShapeless(
            RecipeData recipe,
            ItemSlot[,] grid,
            out List<(ItemSlot slot, int count)> usedSlots)
        {
            usedSlots = new List<(ItemSlot, int)>();

            List<ItemSlot> free = new();
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (!grid[x, y].IsEmpty)
                        free.Add(grid[x, y]);
                }
            }

            foreach (var ing in recipe.Ingredients)
            {
                bool matched = false;

                for (int i = 0; i < free.Count; i++)
                {
                    var slot = free[i];

                    if (slot.Item == ing.Item && slot.Count >= ing.Count)
                    {
                        matched = true;
                        usedSlots.Add((slot, ing.Count)); 
                        free.RemoveAt(i);                 
                        break;
                    }
                }

                if (!matched)
                {
                    usedSlots.Clear();
                    return false;
                }
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

        private List<(ItemSlot slot, int count)> CollectShapedSlots(RecipeData r, ItemSlot[,] grid)
        {
            var result = new List<(ItemSlot slot, int count)>();

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var req = r.Pattern[y * 3 + x];
                    if (req.Item == null) continue;

                    result.Add((grid[x, y], req.Count));
                }
            }

            return result;
        }

        public void PayCraft(List<(ItemSlot slot, int count)> usedSlots)
        {
            foreach (var u in usedSlots)
                u.slot.Remove(u.count);
        }
    }
}
