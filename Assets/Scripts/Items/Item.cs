using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "New Item", fileName = "New Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;

        public Sprite Icon => icon;
        public string Name => displayName;
    }
}