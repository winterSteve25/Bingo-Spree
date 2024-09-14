using Items;
using UnityEngine;

namespace Objects
{
    public class Basket : MonoBehaviour
    {
        [SerializeField] private Item[] items;
        [SerializeField] private Transform[] transforms1;
        [SerializeField] private Transform[] transforms2;
        [SerializeField] private Transform[] transforms3;
        [SerializeField] private Transform[] transforms4;
        [SerializeField] private Transform[] transforms5;

        [SerializeField] private WorldItem worldItemPrefab;
        
        private void Start()
        {
            Transform[][] tfs = { transforms1, transforms2, transforms3, transforms4, transforms5 };

            for (var i = 0; i < tfs.Length; i++)
            {
                var tf = tfs[i];
                foreach (var t in tf)
                {
                    WorldItem item = Instantiate(worldItemPrefab, transform);
                    item.Init(items[i]);
                    item.transform.position = t.position;
                }
            }
        }
    }
}