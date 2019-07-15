using UnityEngine;

namespace Jerre
{
    public class QuitApplication : MonoBehaviour
    {
        public float TimeUntilQuit = 1.5f;
        
        void Start()
        {
            Invoke("Quit", TimeUntilQuit);
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}
