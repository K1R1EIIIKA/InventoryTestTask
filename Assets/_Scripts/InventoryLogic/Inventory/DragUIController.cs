using _Scripts.InventoryLogic.Item;
using UnityEngine;
using Zenject;

namespace _Scripts.InventoryLogic.Inventory
{
    public class DragUIController : MonoBehaviour
    {
        [Inject] private InventoryUIController _inventoryUI;
        
        [SerializeField] private DragItemUI _dragVisual;

        public bool IsDragging => _dragSlot != null;
        
        private ItemSlot _dragSlot;
        private ItemSlotUI _dragOrigin;

        public void BeginDrag(ItemSlotUI origin)
        {
            if (origin.Slot.IsEmpty) return;

            _inventoryUI.HideTooltip();

            _dragOrigin = origin;

            _dragSlot = new ItemSlot();
            _dragSlot.SetItem(origin.Slot.Item, origin.Slot.Count);

            origin.Slot.ClearItem();

            _dragVisual.Show(_dragSlot.Item.Icon);
        }

        public void Drag()
        {
            
        }

        public void EndDrag(ItemSlotUI target)
        {
            if (_dragSlot == null)
            {
                _dragVisual.Hide();
                return;
            }

            if (target == null)
            {
                DropItemOutside();
                return;
            }

            HandleDrop(target);

            _dragVisual.Hide();
            _dragSlot = null;
            _dragOrigin = null;
        }


        private void HandleDrop(ItemSlotUI target)
        {
            var slotA = _dragSlot;
            var slotB = target.Slot;

            if (slotB.IsEmpty)
            {
                slotB.SetItem(slotA.Item, slotA.Count);
                return;
            }

            if (slotB.CanStack(slotA.Item))
            {
                int leftover = slotB.Add(slotA.Count);

                if (leftover > 0)
                    _dragOrigin.Slot.SetItem(slotA.Item, leftover);

                return;
            }

            var tmpItem = slotB.Item;
            var tmpCount = slotB.Count;

            slotB.SetItem(slotA.Item, slotA.Count);
            _dragOrigin.Slot.SetItem(tmpItem, tmpCount);
        }


        private void DropItemOutside()
        {
            _dragSlot = null;
            _dragVisual.Hide();
        }


        public void SplitStack(ItemSlotUI slotUI)
        {
            if (slotUI.Slot.IsEmpty) return;
            if (!slotUI.Slot.Item.Stackable) return;
            if (slotUI.Slot.Count < 2) return;

            int half = slotUI.Slot.Count / 2;

            slotUI.Slot.Remove(half);

            _dragSlot = new ItemSlot();
            _dragSlot.SetItem(slotUI.Slot.Item, half);

            _dragOrigin = slotUI;
            _dragVisual.Show(_dragSlot.Item.Icon);
        }
    }
}
