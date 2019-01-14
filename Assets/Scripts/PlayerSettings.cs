using UnityEngine;

namespace Jerre
{
    public class PlayerSettings : MonoBehaviour
    {
        [SerializeField] private int playerNumber;
        public int PlayerNumber
        {
            get { return playerNumber;  }
        }
        public bool SetPlayerNumber(int number)
        {
            if (playerNumber < 1)
            {
                playerNumber = number;
                return true;
            }
            return false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
