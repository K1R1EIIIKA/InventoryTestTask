using _Scripts.InventoryLogic.Item;

namespace _Scripts.InventoryLogic.Interfaces
{
    public interface IDragUIController
    {
        bool IsDragging { get; }
        void BeginDrag(ItemSlotUI origin);
        void Drag();
        void EndDrag(ItemSlotUI target);
        void SplitStack(ItemSlotUI slotUI);
    }
}
