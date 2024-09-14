using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "New Item", fileName = "New Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private int minAmountPerTask = 1;
        [SerializeField] private int maxAmountPerTask = 4;

        public Sprite Icon => icon;
        public int Min => minAmountPerTask;
        public int Max => maxAmountPerTask;
    }
}