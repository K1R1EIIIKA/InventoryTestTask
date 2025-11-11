using _Scripts.Configs;
using _Scripts.InventoryLogic.Item;
using UnityEngine;

namespace _Scripts.InventoryLogic.Crafting
{
    public class CraftingResultUI : MonoBehaviour
    {
        [SerializeField] private ItemSlotUI _slotUI;
        [SerializeField] private CanvasGroup _canvasGroup;

        private ItemSlot _slot;

        public void Initialize()
        {
            _slot = new ItemSlot();
            _slotUI.Initialize(_slot);
        }
        
        public void SetResult(ItemData item, int count)
        {
            _slot.SetCraftingItem(item, count);
        }

        public void ClearResult()
        {
            _slot.ClearItem();
        }
    }
}
