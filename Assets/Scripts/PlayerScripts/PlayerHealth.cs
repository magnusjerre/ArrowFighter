using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerSettings))]
    public class PlayerHealth : MonoBehaviour
    {
        [HideInInspector]
        public int MaxHealth;

        [SerializeField]
        private int healthLeft;
        public int HealthLeft
        {
            get { return healthLeft;  }
        }

        private PlayerSettings settings;

        // Use this for initialization
        void Start()
        {
            settings = GetComponent<PlayerSettings>();
            MaxHealth = settings.MaxHealth;
        }

        public void DoDamage(int damage)
        {
            healthLeft -= damage;
            if (healthLeft <= 1)
            {
                healthLeft = 0;
                Debug.Log("Player died!");
                ResetHealth();  //Temporary thing
            }
        }

        public void ResetHealth()
        {
            healthLeft = MaxHealth;
        }
    }
}
