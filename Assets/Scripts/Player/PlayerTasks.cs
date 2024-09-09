using System;
using System.Collections.Generic;
using Items;
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

        private void OnEnable()
        {
            GetComponent<PlayerPickup>().OnItemPickedUp += OnOnItemPickedUp;
        }
        
        private void OnDisable()
        {
            GetComponent<PlayerPickup>().OnItemPickedUp -= OnOnItemPickedUp;
        }

        private void OnOnItemPickedUp(Item item)
        {
            _bingo.Update(item);
        }
    }
}