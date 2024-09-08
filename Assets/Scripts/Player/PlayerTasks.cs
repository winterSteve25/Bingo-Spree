using System.Collections.Generic;
using Tasks;
using UnityEngine;

namespace Player
{
    public class PlayerTasks : MonoBehaviour
    {
        [SerializeField] private int bingoDimension;
        [SerializeField] private BingoUI bingoUI;
        
        private List<IPlayableTask> _tasks;
        private BingoTask _bingo;

        private void Start()
        {
            _tasks = new List<IPlayableTask>();
            _bingo = new BingoTask(bingoDimension, bingoUI);
            _bingo.GenerateBingo();
        }
    }
}