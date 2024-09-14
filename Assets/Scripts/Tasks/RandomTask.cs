using Items;
using UnityEngine;

namespace Tasks
{
    [CreateAssetMenu(menuName = "New Random Task")]
    public class RandomTask : TaskSO
    {
        private class T : IPlayableTask
        {
            private int _num;

            public string LeftText()
            {
                return "- Random Task";
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

        public override IPlayableTask Create()
        {
            return new T();
        }
    }
}