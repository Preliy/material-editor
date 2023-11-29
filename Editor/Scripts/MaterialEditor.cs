using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Preliy.Utils
{
    public class MaterialEditor : EditorWindow
    {
        private static MaterialEditor Instance { get; set; }

        private MaterialPalette MaterialPalette
        {
            get => _materialPalette;
            set => SetMaterialPallet(value);
        }

        public Material QuickMaterial
        {
            get => _quickMaterial;
            set => _quickMaterial = value;
        }

        private const string Uxml = "Uxml/MaterialEditor";
        private const string EditorPrefsPalette = "MaterialEditor_PalettePath";
        private const string Tag = "<b><color=#737cff>Material Editor</color></b>";
        private const string UssNameQuickMaterialObject = "quickMaterialObject";
        private const string UssNameMaterialPaletteObject = "materialPaletteObject";
        private const string UssNameMaterialsContainer = "materialsContainer";
        private const string UssNameApplyButton = "applyButton";

        private Material _quickMaterial;
        private MaterialPalette _materialPalette;
        private List<Button> _materialButtons = new ();
        private List<ObjectField> _materialSlots = new ();
        
        #region MenuItems

        [MenuItem("Tools/Material Editor/Material Editor Panel &m")]
        public static void ShowWindow()
        {
            var window = GetWindow<MaterialEditor>();
            window.titleContent = new GUIContent("Material Editor");
        }

        [MenuItem("Tools/Material Editor/Apply Material Preset 1 &1", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 2 &2", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 3 &3", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 4 &4", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 5 &5", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 6 &6", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 7 &7", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 8 &8", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 9 &9", true, 400)]
        [MenuItem("Tools/Material Editor/Apply Material Preset 0 &0", true, 400)]
        public static bool VerifyMaterialAction()
        {
            return Instance != null && Selection.count > 0;
        }

        [MenuItem("Tools/Material Editor/Apply Material Preset 1 &1", false, 400)]
        public static void ApplyMaterial0()
        {
            ApplyMaterial(0);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 2 &2", false, 400)]
        public static void ApplyMaterial1()
        {
            ApplyMaterial(1);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 3 &3", false, 400)]
        public static void ApplyMaterial2()
        {
            ApplyMaterial(2);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 4 &4", false, 400)]
        public static void ApplyMaterial3()
        {
            ApplyMaterial(3);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 5 &5", false, 400)]
        public static void ApplyMaterial4()
        {
            ApplyMaterial(4);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 6 &6", false, 400)]
        public static void ApplyMaterial5()
        {
            ApplyMaterial(5);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 7 &7", false, 400)]
        public static void ApplyMaterial6()
        {
            ApplyMaterial(6);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 8 &8", false, 400)]
        public static void ApplyMaterial7()
        {
            ApplyMaterial(7);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 9 &9", false, 400)]
        public static void ApplyMaterial8()
        {
            ApplyMaterial(8);
        }
        
        [MenuItem("Tools/Material Editor/Apply Material Preset 0 &0", false, 400)]
        public static void ApplyMaterial9()
        {
            ApplyMaterial(9);
        }
        
        #endregion
        
        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;

            if (_materialPalette != null)
            {
                EditorPrefs.SetString(EditorPrefsPalette, AssetDatabase.GetAssetPath(_materialPalette));
            }
            else
            {
                EditorPrefs.DeleteKey(EditorPrefsPalette);
            }
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.Add(Resources.Load<VisualTreeAsset>(Uxml).CloneTree());

            var quickMaterial = root.Q<ObjectField>(UssNameQuickMaterialObject);
            quickMaterial.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                QuickMaterial = (Material)evt.newValue;
            });

            var materialsContainer = root.Q(UssNameMaterialsContainer);
            _materialButtons = materialsContainer.Query<Button>().ToList();
            _materialSlots = materialsContainer.Query<ObjectField>().ToList();

            for (var i = 0; i < _materialButtons.Count; i++)
            {
                var index = (i == 9 ? 0 : i + 1).ToString();
                _materialButtons[i].text = $"Alt+{index}";
                var materialIndex = i;
                _materialButtons[i].clicked += () => ApplyMaterial(materialIndex);
            }

            for (var i = 0; i < _materialSlots.Count; i++)
            {
                var slotIndex = i;
                _materialSlots[i].RegisterCallback<ChangeEvent<Object>>((evt) =>
                {
                    UpdateMaterialInPallet(slotIndex, (Material)evt.newValue);
                });
            }

            var materialPaletteObject = root.Q<ObjectField>(UssNameMaterialPaletteObject);
            materialPaletteObject.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                MaterialPalette = (MaterialPalette)evt.newValue;
            });
            
            if (EditorPrefs.HasKey(EditorPrefsPalette))
            {
                materialPaletteObject.value = (MaterialPalette)AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString(EditorPrefsPalette), typeof(MaterialPalette));
            }

            var applyButton = root.Q<Button>(UssNameApplyButton);
            applyButton.clicked += () => ApplyMaterial(_quickMaterial);
        }

        private void SetMaterialPallet(MaterialPalette materialPalette)
        {
            _materialPalette = materialPalette;

            foreach (var slot in _materialSlots)
            {
                slot.SetValueWithoutNotify(null);
            }

            if (_materialPalette == null) return;
            for (var i = 0; i < materialPalette.Materials.Length; i++)
            {
                _materialSlots[i].SetValueWithoutNotify(materialPalette.Materials[i]);
            }
        }

        private void UpdateMaterialInPallet(int index, Material material)
        {
            _materialPalette[index] = material;
            EditorUtility.SetDirty(_materialPalette);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private static void ApplyMaterial(int materialIndex)
        {
            if (Instance.MaterialPalette == null)
            {
                Log(LogType.Error, "Can't apply material! Material Palette is null!");
                return;
            }

            if (materialIndex > Instance.MaterialPalette.Materials.Length - 1)
            {
                Log(LogType.Error, "Can't apply material! Material Preset index isn't valid!");
                return;
            }

            ApplyMaterial(Instance.MaterialPalette[materialIndex]);
        }
        
        private static void ApplyMaterial(Material material)
        {
            if (material == null)
            {
                Log(LogType.Error, "Can't apply material! Material Preset reference is null!");
                return;
            }

            var selectedObjects = Selection.gameObjects;
            var renderers = new List<Renderer>();
            var materialCounter = 0;

            foreach (var selectedObject in selectedObjects)
            {
                renderers.AddRange(selectedObject.GetComponentsInChildren<Renderer>());
            }

            if (renderers.Count > 0)
            {
                // ReSharper disable once CoVariantArrayConversion
                Undo.RecordObjects(renderers.ToArray(), "Apply Preset Material");

                foreach (var renderer in renderers)
                {
                    var materials = new Material[renderer.sharedMaterials.Length];
                    if (renderer.sharedMaterials.Length > 1)
                    {
                        for (var i = 0; i < materials.Length; i++)
                        {
                            materials[i] = material;
                        }

                        renderer.sharedMaterials = materials;
                        materialCounter++;
                    }
                    else
                    {
                        renderer.sharedMaterial = material;
                        materialCounter++;
                    }
                }
            }

            Log(LogType.Log, $"Apply {materialCounter} Material(s) in {renderers.Count} Renderer(s) for {Selection.gameObjects.Length} selected GameObject(s)");
        }

        private static void Log(LogType logType, object message)
        {
            Debug.unityLogger.Log(logType, Tag, message);
        }
    }
}