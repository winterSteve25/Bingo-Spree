using System;

namespace Tasks
{
    public struct Penalty
    {
        public readonly string text;
        public readonly int score;

        public Penalty(string text, int score)
        {
            this.text = text;
            this.score = score;
        }
    }
}