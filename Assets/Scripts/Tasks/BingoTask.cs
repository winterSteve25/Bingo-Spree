using Items;
using UnityEngine;

namespace Tasks
{
    public class BingoTask
    {
        private readonly int _bingoDimension;
        private readonly bool[,] _bingoCompletion;
        private readonly BingoUI _ui;

        private ItemStack[,] _bingo;

        public BingoTask(int bingoDimension, BingoUI ui)
        {
            _bingoDimension = bingoDimension;
            _bingoCompletion = new bool[bingoDimension, bingoDimension];
            _ui = ui;
        }

        public void GenerateBingo()
        {
            _bingo = new ItemStack[_bingoDimension, _bingoDimension];

            for (int i = 0; i < _bingoDimension; i++)
            {
                for (int j = 0; j < _bingoDimension; j++)
                {
                    _bingo[i, j] = new ItemStack(Resources.Load<Item>("Items/Objects/canned thing"), Random.Range(1, 5));
                }
            }
            
            _ui.Init(_bingo, _bingoDimension);
        }
    }
}