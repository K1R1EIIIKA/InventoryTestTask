using _Scripts.Configs;

namespace _Scripts.InventoryLogic.Interfaces
{
    public interface IInventoryController
    {
        int Width { get; }
        int Height { get; }
        ItemData[] ItemsPool { get; }
        void Initialize();
        bool TryAddItem(ItemData item, int count);
        void SortInventory();
    }
}
