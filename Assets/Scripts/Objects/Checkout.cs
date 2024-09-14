using Player;
using Tasks;
using UI;
using UnityEngine;

namespace Objects
{
    public class Checkout : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerTasks.Instance.EndGame();
            CompletionUI.Show();
        }
    }
}