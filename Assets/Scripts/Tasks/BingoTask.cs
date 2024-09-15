using System;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tasks
{
    public class BingoTask : IPlayableTask
    {
        private readonly int _bingoDimension;
        private readonly bool[,] _bingoCompletion;
        private readonly BingoUI _ui;

        private ItemStack[,] _bingo;
        private int _extraItems;

        private static readonly Lazy<Item[]> Items = new(() => Resources.LoadAll<Item>("Items/Objects"));

        public BingoTask(int bingoDimension, BingoUI ui)
        {
            _bingoDimension = bingoDimension;
            _bingoCompletion = new bool[bingoDimension, bingoDimension];
            _ui = ui;
        }

        public int GetNumOfBingos()
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

        public string LeftText()
        {
            return "- Completed Bingo";
        }

        public string RightText()
        {
            return $"x{GetNumOfBingos()}";
        }

        public int GetScore()
        {
            return GetNumOfBingos() * 500;
        }

        public void Start()
        {
            _bingo = new ItemStack[_bingoDimension, _bingoDimension];

            for (int i = 0; i < _bingoDimension; i++)
            {
                for (int j = 0; j < _bingoDimension; j++)
                {
                    Item item = Items.Value[Random.Range(0, Items.Value.Length)];
                    _bingo[i, j] = new ItemStack(item, Random.Range(item.Min, item.Max + 1));
                }
            }

            _ui.Init(_bingo, _bingoDimension);
        }

        public void ItemPickedUp(Item item)
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

            // if found will jump to broken so this wont be called
            _extraItems++;
            
            broken: ;
        }
    }
}