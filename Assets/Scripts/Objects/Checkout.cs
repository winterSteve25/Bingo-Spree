using Player;
using UI;
using UnityEngine;

namespace Objects
{
    public class Checkout : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            PlayerTasks.Instance.EndGame();
            CompletionUI.Show();
        }
    }
}