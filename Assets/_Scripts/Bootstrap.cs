using _Scripts.InventoryLogic.Crafting;
using _Scripts.InventoryLogic.Inventory;
using UnityEngine;
using Zenject;

namespace _Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private InventoryController _inventoryController;
        [Inject] private CraftingController _craftingController;

        private void Awake()
        {
            _inventoryController.Initialize();
            _craftingController.Initialize();
        }
    }
}
