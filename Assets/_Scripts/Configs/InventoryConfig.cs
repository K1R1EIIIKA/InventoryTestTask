using UnityEngine;
using Zenject;

namespace _Scripts.Configs
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "Inventory/Inventory Config")]
    public class InventoryConfig : ScriptableObjectInstaller
    {
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
        [field: SerializeField] public ItemData[] ItemsPool { get; private set; }
        [field: SerializeField] public RecipeData[] RecipesPool { get; private set; }
        
        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
        }
    }   
}
