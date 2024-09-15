using System;
using System.Collections.Generic;
using Audio;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tasks
{
    public class BingoTask : IPlayableTask
    {
        public static event Action OnBingo;
        public static event Action OnSlot;

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

        public void GetBingoPos(out List<int> rows, out List<int> cols, out bool topDown, out bool downTop)
        {
            List<int> rs = new List<int>();
            List<int> cs = new List<int>();
            bool td = false;
            bool dt = false;

            // rows
            for (int i = 0; i < _bingoDimension; i++)
            {
                for (int j = 0; j < _bingoDimension; j++)
                {
                    if (!_bingoCompletion[i, j]) goto nextItr;
                }

                rs.Add(i);
                nextItr: ;
            }

            // columns
            for (int j = 0; j < _bingoDimension; j++)
            {
                for (int i = 0; i < _bingoDimension; i++)
                {
                    if (!_bingoCompletion[i, j]) goto nextItr;
                }

                cs.Add(j);
                nextItr: ;
            }

            // down to up diag
            for (int i = 0; i < _bingoDimension; i++)
            {
                if (!_bingoCompletion[i, i]) break;
                if (i != _bingoDimension - 1) continue;
                dt = true;
            }

            // top to down diag
            for (int i = 0; i < _bingoDimension; i++)
            {
                if (!_bingoCompletion[i, _bingoDimension - i - 1]) break;
                if (i != _bingoDimension - 1) continue;
                td = true;
            }

            rows = rs;
            cols = cs;
            topDown = td;
            downTop = dt;
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
            // full bingo
            if (GetNumOfBingos() == _bingoDimension * 2 + 2)
            {
                AudioManager.Play(7, SourceType.UI);
            }

            return GetNumOfBingos() * 800;
        }

        public List<Penalty> GetPenalties()
        {
            List<Penalty> pens = new List<Penalty>();

            if (_extraItems > 0)
            {
                pens.Add(new Penalty($"{_extraItems} Unneeded items bought", _extraItems * 25));
            }

            return pens;
        }

        public void Start()
        {
            _bingo = new ItemStack[_bingoDimension, _bingoDimension];
            Dictionary<int, bool> d = new Dictionary<int, bool>();

            for (int i = 0; i < _bingoDimension; i++)
            {
                for (int j = 0; j < _bingoDimension; j++)
                {
                    int idx = Random.Range(0, Items.Value.Length);

                    if (d.ContainsKey(idx))
                    {
                        idx = Random.Range(0, Items.Value.Length);
                    }

                    d.TryAdd(idx, true);
                    Item item = Items.Value[idx];
                    _bingo[i, j] = new ItemStack(item, Random.Range(item.Min, item.Max + 1));
                }
            }

            _ui.Init(_bingo, _bingoDimension);
        }

        public void ItemPickedUp(Item item)
        {
            int prevBingoCount = GetNumOfBingos();

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
                        OnSlot?.Invoke();
                    }

                    _ui.Get(i, j).UpdateAmount(iS.amount);
                    AudioManager.Play(4, SourceType.SFX2);
                    goto broken;
                }
            }

            // if found will jump to broken so this wont be called
            _extraItems++;
            AudioManager.Play(3, SourceType.SFX);

            broken: ;
            if (GetNumOfBingos() - prevBingoCount != 0)
            {
                AudioManager.Play(5, SourceType.SFX3);
                OnBingo?.Invoke();

                GetBingoPos(out var rs, out var cs, out var td, out var dt);
                foreach (var r in rs)
                {
                    for (int i = 0; i < _bingoDimension; i++)
                    {
                        _ui.Get(r, i).BingoColor();
                    }
                }

                foreach (var r in cs)
                {
                    for (int i = 0; i < _bingoDimension; i++)
                    {
                        _ui.Get(i, r).BingoColor();
                    }
                }

                if (td)
                {
                    for (int i = 0; i < _bingoDimension; i++)
                    {
                        _ui.Get(i, _bingoDimension - i - 1).BingoColor();
                    }
                }

                if (dt)
                {
                    for (int i = 0; i < _bingoDimension; i++)
                    {
                        _ui.Get(i, i).BingoColor();
                    }
                }
            }
        }
    }
}