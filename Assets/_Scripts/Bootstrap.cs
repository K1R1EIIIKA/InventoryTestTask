using System;
using _Scripts.Inventory;
using UnityEngine;
using Zenject;

namespace _Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private InventoryController _inventoryController;

        private void Awake()
        {
            _inventoryController.Initialize();
        }
    }
}
