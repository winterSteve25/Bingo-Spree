using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Objects;
using Tasks;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerTasks : MonoBehaviour
    {
        private static PlayerTasks _instance;
        public static PlayerTasks Instance => _instance;

        [SerializeField] private int bingoDimension;
        [SerializeField] private BingoUI bingoUI;
        [SerializeField] private CanvasGroup comboUI;
        [SerializeField] private Slider countDown;
        [SerializeField] private TMP_Text timeText;

        private List<IPlayableTask> _tasks;
        private float _time;
        private bool _completed;
        [NonSerialized] public bool CollidedWithNPC;

        public float CompletionTime => _time;
        public List<IPlayableTask> Tasks => _tasks;

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
            foreach (var task in _tasks)
            {
                task.ItemPickedUp(item);
            }
        }

        private void Update()
        {
            if (_completed) return;
            _time += Time.deltaTime;

            var mili = (int)((_time - Math.Floor(_time)) * 100);
            timeText.text = $"{(int)_time / 60:D2}:{(int)_time % 60:D2}:{mili:D2}";

            countDown.value = countDown.maxValue - _time;

            if (!(_time >= countDown.maxValue)) return;
            EndGame();
            CompletionUI.Show(true);
        }

        private void StartGame()
        {
            _tasks = new List<IPlayableTask>();
            _completed = false;
            _time = 0;
            _tasks.Add(new BingoTask(bingoDimension, bingoUI));
            _tasks.AddRange(LevelData.Instance.additionalTasks.Select(x => x.Create()));
            PlayerInput.Disabled = false;

            comboUI.gameObject.SetActive(true);
            countDown.gameObject.SetActive(true);
            countDown.maxValue = LevelData.Instance.timeLimit;
            countDown.minValue = 0;
            countDown.value = countDown.maxValue;

            foreach (var task in _tasks)
            {
                task.Start();
            }
        }

        public void EndGame()
        {
            _completed = true;
            timeText.text = "";
            countDown.gameObject.SetActive(false);
            PlayerInput.Disabled = true;
            comboUI.gameObject.SetActive(false);
        }
    }
}