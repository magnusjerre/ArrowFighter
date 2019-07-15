using UnityEngine;

namespace Jerre
{
    public class ByeByeManager : MonoBehaviour
    {
        public float TimeUntilQuit = 2f;
        
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
