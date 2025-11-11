using _Scripts.Inventory;
using UnityEngine;
using Zenject;

namespace _Scripts.Installers
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryUIController _inventoryUIController;
        
        public override void InstallBindings()
        {
            Container.Bind<InventoryController>().AsSingle().NonLazy();
            
            Container.Bind<InventoryUIController>()
                .FromInstance(_inventoryUIController)
                .AsSingle()
                .NonLazy();
        }
    }
}
