using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Configs.Editor
{
    public class RecipeEditorWindow : EditorWindow
    {
        private readonly List<RecipeData> _recipes = new List<RecipeData>();
        private Vector2 _listScroll;
        private Vector2 _editScroll;

        private RecipeData _selected;

        [MenuItem("Tools/Crafting/Recipe Editor")]
        public static void Open()
        {
            GetWindow<RecipeEditorWindow>("Recipe Editor");
        }

        private void OnEnable()
        {
            LoadAllRecipes();
        }

        private void LoadAllRecipes()
        {
            _recipes.Clear();

            string[] guids = AssetDatabase.FindAssets("t:RecipeData");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var recipe = AssetDatabase.LoadAssetAtPath<RecipeData>(path);
                if (recipe) _recipes.Add(recipe);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            DrawRecipeList();
            DrawRecipeEditor();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawRecipeList()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(250));
            EditorGUILayout.LabelField("All Recipes", EditorStyles.boldLabel);

            _listScroll = EditorGUILayout.BeginScrollView(_listScroll);

            foreach (var recipe in _recipes)
            {
                GUI.backgroundColor = recipe == _selected ? Color.yellow : Color.white;

                if (GUILayout.Button(recipe.name))
                    _selected = recipe;

                GUI.backgroundColor = Color.white;
            }

            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);

            if (GUILayout.Button("Add New Recipe"))
            {
                CreateNewRecipe();
            }

            if (_selected && GUILayout.Button("Delete Selected"))
            {
                DeleteRecipe(_selected);
            }
            
            if (GUILayout.Button("Refresh"))
                LoadAllRecipes();

            EditorGUILayout.EndVertical();
        }

        private void DrawRecipeEditor()
        {
            EditorGUILayout.BeginVertical("box");

            if (_selected == null)
            {
                EditorGUILayout.LabelField("Select a recipe to edit...");
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.LabelField("Edit Recipe", EditorStyles.boldLabel);

            _editScroll = EditorGUILayout.BeginScrollView(_editScroll);

            EditorGUI.BeginChangeCheck();

            var newName = EditorGUILayout.TextField("Recipe Name", _selected.name);
   
            if (newName != _selected.name)
            {
                _selected.name = newName;

                string path = AssetDatabase.GetAssetPath(_selected);

                if (!string.IsNullOrEmpty(path))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                    if (fileName != newName)
                    {
                        AssetDatabase.RenameAsset(path, newName);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
            
            EditorGUILayout.Space(10);
            _selected.Result = (ItemData)EditorGUILayout.ObjectField("Result", _selected.Result, typeof(ItemData), false);
            _selected.ResultCount = EditorGUILayout.IntField("Result Count", _selected.ResultCount);

            _selected.IsShaped = EditorGUILayout.Toggle("Is Shaped", _selected.IsShaped);

            EditorGUILayout.Space(10);

            if (!_selected.IsShaped)
                DrawShapeless();
            else
                DrawShaped();

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(_selected);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawShapeless()
        {
            EditorGUILayout.LabelField("Shapeless Ingredients", EditorStyles.boldLabel);

            SerializedObject so = new SerializedObject(_selected);
            SerializedProperty ingredientsProp = so.FindProperty("Ingredients");

            EditorGUILayout.PropertyField(ingredientsProp, true);

            so.ApplyModifiedProperties();
        }

        private void DrawShaped()
        {
            EditorGUILayout.LabelField("Shaped Pattern 3×3", EditorStyles.boldLabel);

            SerializedObject so = new SerializedObject(_selected);
            SerializedProperty patternProp = so.FindProperty("Pattern");

            if (patternProp.arraySize != 9)
                patternProp.arraySize = 9;

            const int size = 3;

            for (int y = 0; y < size; y++)
            {
                EditorGUILayout.BeginHorizontal();

                for (int x = 0; x < size; x++)
                {
                    int index = y * size + x;
                    var cell = patternProp.GetArrayElementAtIndex(index);

                    DrawIngredientCell(cell);
                }

                EditorGUILayout.EndHorizontal();
            }

            so.ApplyModifiedProperties();
        }

        private void DrawIngredientCell(SerializedProperty ingredient)
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(140));

            SerializedProperty item = ingredient.FindPropertyRelative("Item");
            SerializedProperty count = ingredient.FindPropertyRelative("Count");

            EditorGUILayout.PropertyField(item, GUIContent.none);
            EditorGUILayout.PropertyField(count, GUIContent.none);

            EditorGUILayout.EndVertical();
        }

        private void CreateNewRecipe()
        {
            RecipeData recipe = CreateInstance<RecipeData>();
            recipe.name = "NewRecipe";

            const string folderPath = "Assets/Resources/Configs/Recipes";

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Configs");
                AssetDatabase.CreateFolder("Assets/Resources/Configs", "Recipes");
            }

            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(
                $"{folderPath}/NewRecipe.asset"
                );

            AssetDatabase.CreateAsset(recipe, uniquePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _recipes.Add(recipe);
            _selected = recipe;
        }


        private void DeleteRecipe(RecipeData recipe)
        {
            if (!EditorUtility.DisplayDialog("Delete Recipe?",
                $"Are you sure you want to delete '{recipe.name}'?", "Yes", "No"))
                return;

            string path = AssetDatabase.GetAssetPath(recipe);
            AssetDatabase.DeleteAsset(path);

            _recipes.Remove(recipe);
            _selected = null;

            AssetDatabase.SaveAssets();
        }
    }
}
