using System;
using System.Collections.Generic;
using Tasks;
using UnityEngine;

namespace Player
{
    public class PlayerTasks : MonoBehaviour
    {
        private List<IPlayableTask> _tasks;

        private void Start()
        {
            _tasks = new List<IPlayableTask>();
        }
    }
}