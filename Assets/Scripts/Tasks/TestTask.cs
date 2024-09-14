using Items;
using UnityEngine;

namespace Tasks
{
    public class TestTask : IPlayableTask
    {
        private int _num;
        
        public string LeftText()
        {
            return "-Random Task";
        }

        public string RightText()
        {
            return _num.ToString();
        }

        public int GetScore()
        {
            return _num;
        }

        public void Start()
        {
            _num = Random.Range(0, 101);
        }

        public void ItemPickedUp(Item item)
        {
        }
    }
}