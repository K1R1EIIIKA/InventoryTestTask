using _Scripts.InventoryLogic.Crafting;
using _Scripts.InventoryLogic.Interfaces;
using _Scripts.InventoryLogic.Inventory;
using UnityEngine;
using Zenject;

namespace _Scripts.Installers
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryUIController _inventoryUIController;
        [SerializeField] private DragUIController _dragUIController;
        [SerializeField] private CraftingUIController _craftingUIController;
        
        public override void InstallBindings()
        {
            Container.Bind<IInventoryController>().To<InventoryController>().AsSingle().NonLazy();
            Container.Bind<ICraftingController>().To<CraftingController>().AsSingle().NonLazy();
            
            Container.Bind<IInventoryUIController>()
                .FromInstance(_inventoryUIController)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<IDragUIController>()
                .FromInstance(_dragUIController)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<ICraftingUIController>()
                .FromInstance(_craftingUIController)
                .AsSingle()
                .NonLazy();
        }
    }
}
