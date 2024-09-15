using System;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC
{
    public class NPCSpawner : MonoBehaviour
    {
        private float _time;
        private float _timeTotal;

        [SerializeField] private float interval;
        [SerializeField] private GameObject npcPrefab;

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            _time += deltaTime;
            _timeTotal += deltaTime;

            if (!(_time >= interval)) return;
            
            _time = 0;
            int additionals = (_timeTotal / LevelData.Instance.timeLimit) switch
            {
                <= 0.1f => 0,
                <= 0.4f => 1,
                <= 0.6f => 2,
                <= 0.8f => 3,
                _ => 5,
            };
            
            int num = Random.Range(1, 2 + additionals);

            for (int i = 0; i < num; i++)
            {
                var go = Instantiate(npcPrefab);
                go.transform.position = transform.position;
            }
        }
    }
}