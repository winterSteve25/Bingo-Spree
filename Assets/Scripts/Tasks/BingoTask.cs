using System;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tasks
{
    public class BingoTask
    {
        private readonly int _bingoDimension;
        private readonly bool[,] _bingoCompletion;
        private readonly BingoUI _ui;

        private ItemStack[,] _bingo;

        private static readonly Lazy<Item[]> Items = new Lazy<Item[]>(() => Resources.LoadAll<Item>("Items/Objects"));

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
                    _bingo[i, j] = new ItemStack(Items.Value[Random.Range(0, Items.Value.Length)], Random.Range(1, 5));
                }
            }

            _ui.Init(_bingo, _bingoDimension);
        }

        public void Update(Item item)
        {
            for (int i = 0; i < _bingoDimension; i++)
            {
                for (int j = 0; j < _bingoDimension; j++)
                {
                    ItemStack iS = _bingo[i, j];

                    if (iS.Item != item || iS.amount <= 0)
                    {
                        continue;
                    }

                    iS.amount -= 1;
                    _bingo[i, j] = iS;

                    if (iS.amount == 0)
                    {
                        _bingoCompletion[i, j] = true;
                    }

                    _ui.Get(i, j).UpdateAmount(iS.amount);
                    goto broken;
                }
            }

            broken: ;
        }

        private int GetNumOfBingos()
        {
            int bingoCount = 0;

            // rows
            for (int i = 0; i < _bingoDimension; i++)
            {
                for (int j = 0; j < _bingoDimension; j++)
                {
                    if (!_bingoCompletion[i, j]) goto nextItr;
                }

                bingoCount++;
                nextItr: ;
            }

            // columns
            for (int j = 0; j < _bingoDimension; j++)
            {
                for (int i = 0; i < _bingoDimension; i++)
                {
                    if (!_bingoCompletion[i, j]) goto nextItr;
                }

                bingoCount++;
                nextItr: ;
            }

            // down to up diag
            for (int i = 0; i < _bingoDimension; i++)
            {
                if (!_bingoCompletion[i, i]) break;
                if (i != _bingoDimension - 1) continue;
                bingoCount++;
            }

            // top to down diag
            for (int i = 0; i < _bingoDimension; i++)
            {
                if (!_bingoCompletion[i, _bingoDimension - i - 1]) break;
                if (i != _bingoDimension - 1) continue;
                bingoCount++;
            }

            return bingoCount;
        }
    }
}