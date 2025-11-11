using _Scripts.InventoryLogic.Item;

namespace _Scripts.InventoryLogic.Interfaces
{
    public interface IInventoryUIController
    {
        void ShowTooltip(ItemSlot slot);
        void HideTooltip();
        void MoveTooltip();
        void Initialize(int width, int height);
        ItemSlot[,] Slots { get; }
        bool IsTooltipVisible { get; }
    }
}
