using UnityEngine;

namespace _Scripts.Item
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField, TextArea] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public bool Stackable { get; private set; }
        [field: SerializeField] public int MaxStack { get; private set; }
    }
}
