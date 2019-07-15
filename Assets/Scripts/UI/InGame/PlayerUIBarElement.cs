using Jerre.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.UI.InGame
{
    [ExecuteAlways]
    public class PlayerUIBarElement : MonoBehaviour, IAFEventListener
    {
        public RectTransform VerticalUpgradeBar;
        public Text HealthText;
        public Text ScoreText;
        public Color BackgroundColor;
        public Color TextColor;

        public int PlayerNumber;
        public int health = 100;
        public string score = "0";
        public float upgradeProgress = 1 / 3f;
        public Color upgradeColor = Color.green;

        public Vector3 Position;

        public Image Separator1, Separator2;

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
        }

        void OnDestroy()
        {
            AFEventManager.INSTANCE.RemoveListener(this);
        }

        private void Start()
        {
            GetComponent<Image>().color = BackgroundColor;
            HealthText.color = TextColor;
            ScoreText.color = TextColor;
            Separator1.color = TextColor;
            Separator2.color = TextColor;
            SetUpgradeColor(upgradeColor);
            SetUpgradeProgress(upgradeProgress);
            SetHealth(health);
            SetScoreText(score);
            GetComponent<RectTransform>().localPosition = Position;
        }

        void Update()
        {
            if (!Application.IsPlaying(gameObject))
            {
                // Editor logic
                GetComponent<Image>().color = BackgroundColor;
                HealthText.color = TextColor;
                ScoreText.color = TextColor;
                Separator1.color = TextColor;
                Separator2.color = TextColor;
                SetUpgradeColor(Color.green);
                SetUpgradeProgress(0.5f);
            }
        }

        public void SetUpgradeColor(Color color)
        {
            this.upgradeColor = color;
            VerticalUpgradeBar.GetComponent<Image>().color = color;
        }

        public void SetUpgradeProgress(float fractionOfOne)
        {
            this.upgradeProgress = fractionOfOne;
            VerticalUpgradeBar.anchorMax = new Vector2(VerticalUpgradeBar.anchorMax.x, Mathf.Clamp01(fractionOfOne));
        }

        public void SetHealth(int health)
        {
            this.health = health;
            HealthText.text = "" + health;
        }

        public void SetScoreText(string score)
        {
            this.score = score;
            ScoreText.text = score;
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.WEAPON_UPGRADE:
                    {
                        var payload = (WeaponUpgradePayload)afEvent.payload;
                        if (payload.PlayerNumber == PlayerNumber)
                        {
                            SetUpgradeColor(payload.UpgradeColor);
                            SetUpgradeProgress(payload.UpgradeProgress);
                        }
                        break;
                    }
                case AFEventType.SCORE:
                    {
                        var payload = (ScorePayload)afEvent.payload;                        
                        SetScoreText("" + payload.playerScore);
                        break;
                    }
                case AFEventType.HEALTH_DAMAGE:
                    {
                        var payload = (HealthDamagePayload)afEvent.payload;
                        SetHealth(payload.HealthLeft);
                        break;
                    }
                case AFEventType.RESPAWN:
                    {
                        var payload = (RespawnPayload)afEvent.payload;
                        SetHealth(payload.Health);
                        break;
                    }
            }
            return false;
        }
    }
}
