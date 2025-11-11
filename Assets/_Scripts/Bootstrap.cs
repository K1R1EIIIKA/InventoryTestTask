using _Scripts.InventoryLogic.Crafting;
using _Scripts.InventoryLogic.Interfaces;
using _Scripts.InventoryLogic.Inventory;
using UnityEngine;
using Zenject;

namespace _Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private IInventoryController _inventoryController;
        [Inject] private ICraftingController _craftingController;

        private void Awake()
        {
            _inventoryController.Initialize();
            _craftingController.Initialize();
        }
    }
}
