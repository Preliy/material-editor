using UnityEngine;

namespace Preliy.Utils
{
    [CreateAssetMenu(fileName = "Custom Material Palette", menuName = "Material Editor/Custom Material Palette", order = 2000)]
    public class MaterialPalette : ScriptableObject
    {
        public Material[] Materials => _materials;
        
        [SerializeField]
        private Material[] _materials;
        
        public Material this[int i]
        {
            get => _materials[i];
            set => _materials[i] = value;
        }

        public void Reset()
        {
            _materials = new Material[10]; 
        }
    }
}

