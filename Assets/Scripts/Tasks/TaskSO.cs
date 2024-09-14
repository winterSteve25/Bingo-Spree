using Items;
using UnityEngine;

namespace Tasks
{
    public abstract class TaskSO : ScriptableObject
    {
        public abstract IPlayableTask Create();
    }
}