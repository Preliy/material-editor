using UnityEngine;

namespace Preliy.Utils
{
    [CreateAssetMenu]
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
    }
}

