using System.Collections.Generic;
using _Scripts.Configs;
using _Scripts.InventoryLogic.Item;

namespace _Scripts.InventoryLogic.Interfaces
{
    public interface ICraftingController
    {
        void Initialize();
        bool FindMatch(
            ItemSlot[,] grid,
            out RecipeData recipe,
            out List<(ItemSlot slot, int count)> usedSlots);
        void PayCraft(List<(ItemSlot slot, int count)> usedSlots);
    }
}
