using EasyTransition;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TransitionSettings settings;
        
        public void StartGame()
        {
            TransitionManager.Instance().Transition(1, settings, 0f);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}