using _Scripts.InventoryLogic.Crafting;
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
            Container.Bind<InventoryController>().AsSingle().NonLazy();
            Container.Bind<CraftingController>().AsSingle().NonLazy();
            
            Container.Bind<InventoryUIController>()
                .FromInstance(_inventoryUIController)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<DragUIController>()
                .FromInstance(_dragUIController)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<CraftingUIController>()
                .FromInstance(_craftingUIController)
                .AsSingle()
                .NonLazy();
        }
    }
}
