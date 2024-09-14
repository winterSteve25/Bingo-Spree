using System;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects
{
    public class Aisle : MonoBehaviour
    {
        [SerializeField] private Item[] items;
        [SerializeField] private int itemsPerSide;
        [SerializeField] private WorldItem worldItemPrefab;

        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        [SerializeField] private Transform startRight;
        [SerializeField] private Transform[] extraSlots;
        
        private void Start()
        {
            Vector3 line = end.position - start.position;
            line.z = 0;
            
            Vector3 offset = line / itemsPerSide;
            offset.z = 0;

            for (int i = 0; i < itemsPerSide; i++)
            {
                WorldItem it = Instantiate(worldItemPrefab, transform);
                it.transform.position = start.position + offset * i;
                it.Init(items[Random.Range(0, items.Length)]);
                
                WorldItem itRight = Instantiate(worldItemPrefab, transform);
                itRight.transform.position = startRight.position + offset * i;
                itRight.Init(items[Random.Range(0, items.Length)]);
            }

            foreach (var extraSlot in extraSlots)
            {
                WorldItem it = Instantiate(worldItemPrefab, transform);
                it.transform.position = extraSlot.position;
                it.Init(items[Random.Range(0, items.Length)]);
            }
        }
    }
}