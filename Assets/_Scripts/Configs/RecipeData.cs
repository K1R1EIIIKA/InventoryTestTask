using System;
using UnityEngine;

namespace _Scripts.Configs
{
    [CreateAssetMenu(fileName = "RecipeData", menuName = "Inventory/Recipe Data")]
    public class RecipeData : ScriptableObject
    {
        public ItemData Result;
        public int ResultCount = 1;

        public bool IsShaped;

        public Ingredient[] Ingredients;
        [SerializeField] public Ingredient[] Pattern = new Ingredient[9];

        [Serializable]
        public struct Ingredient
        {
            public ItemData Item;
            [Min(1)] public int Count;
        }

        private void OnValidate()
        {
            if (Pattern == null || Pattern.Length != 9)
                Pattern = new Ingredient[9];
        }
    }
}
