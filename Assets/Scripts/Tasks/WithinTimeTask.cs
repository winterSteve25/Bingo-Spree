using Items;
using Player;
using UnityEngine;

namespace Tasks
{
    [CreateAssetMenu(menuName = "Tasks/Within Time", fileName = "New Within Time Task")]
    public class WithinTimeTask : TaskSO
    {
        [SerializeField] private float timeRequired;
        
        private class T : IPlayableTask
        {
            private readonly float _timeRequired;

            public T(float timeRequired)
            {
                _timeRequired = timeRequired;
            }

            public string LeftText()
            {
                return $"Completed under {_timeRequired}s";
            }

            public string RightText()
            {
                return PlayerTasks.Instance.CompletionTime <= _timeRequired ? "Yes" : "No";
            }

            public int GetScore()
            {
                return PlayerTasks.Instance.CompletionTime <= _timeRequired ? 200 : 0;
            }

            public void Start()
            {
            }

            public void ItemPickedUp(Item item)
            {
            }
        }
        
        public override IPlayableTask Create()
        {
            return new T(timeRequired);
        }
    }
}