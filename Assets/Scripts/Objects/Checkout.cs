using Player;
using Tasks;
using UnityEngine;

namespace Objects
{
    public class Checkout : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerTasks.Instance.EndGame();
            CompletionUI.Show();
        }
    }
}