using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class BingoUI : MonoBehaviour
    {
        [SerializeField] private BingoItem itemPrefab;
        [SerializeField] private RectTransform rowPrefab;
        [SerializeField] private RectTransform bingo;

        public void Init(ItemStack[,] grid, int dimension)
        {
            for (int i = 0; i < dimension; i++)
            {
                RectTransform row = Instantiate(rowPrefab, bingo);
                
                for (int j = 0; j < dimension; j++)
                {
                    BingoItem slot = Instantiate(itemPrefab, row);
                    slot.Init(grid[i, j]);
                }
                
                LayoutRebuilder.ForceRebuildLayoutImmediate(row);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(bingo);
        }
    }
}