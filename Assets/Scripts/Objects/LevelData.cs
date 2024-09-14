using System;
using NPC;
using Tasks;
using UnityEngine;

namespace Objects
{
    public class LevelData : MonoBehaviour
    {
        private static LevelData _instance;
        public static LevelData Instance => _instance;
        
        public float timeLimit;
        public TaskSO[] additionalTasks;
        public Path[] npcPaths;

        private void Awake()
        {
            _instance = this;
        }
    }
}