using System;
using System.Collections.Generic;
using Items;
using Tasks;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerTasks : MonoBehaviour
    {
        private static PlayerTasks _instance;
        public static PlayerTasks Instance => _instance;

        [SerializeField] private int bingoDimension;
        [SerializeField] private BingoUI bingoUI;
        [SerializeField] private TMP_Text timeText;

        private List<IPlayableTask> _tasks;
        private BingoTask _bingo;
        private float _time;
        private bool _completed;

        public BingoTask BingoTask => _bingo;
        public float CompletionTime => _time;

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            StartGame();
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

        private void Update()
        {
            if (_completed) return;
            _time += Time.deltaTime;

            var mili = (int)((_time - Math.Floor(_time)) * 100);
            timeText.text = $"{(int)_time / 60:D2}:{(int)_time % 60:D2}:{mili:D2}";
        }

        public void StartGame()
        {
            _tasks = new List<IPlayableTask>();
            _bingo = new BingoTask(bingoDimension, bingoUI);
            _bingo.GenerateBingo();
            _completed = false;
            _time = 0;
        }

        public void EndGame()
        {
            _completed = true;
        }
    }
}