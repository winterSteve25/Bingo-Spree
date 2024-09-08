using System;
using UnityEngine;

namespace Tasks
{
    public class BingoTask
    {
        private int _bingoDimension;
        private bool[,] _bingoCompletion;

        public BingoTask(int bingoDimension)
        {
            _bingoDimension = bingoDimension;
        }
    }
}