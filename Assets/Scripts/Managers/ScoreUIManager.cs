using Jerre.Events;
using UnityEngine;

namespace Jerre.UI
{
    public class ScoreUIManager : MonoBehaviour, IAFEventListener
    {
        public RectTransform ScoreArea;
        public ScoreUIElement scoreUIPrefab;
        public int scorePadding = 10;
        private int nextNumberInLine = 1;

        private ScoreManager scoreManager;
        private AFEventManager eventManager;

        void Awake()
        {
            scoreManager = GameObject.FindObjectOfType<ScoreManager>();
            eventManager = GameObject.FindObjectOfType<AFEventManager>();
            eventManager.AddListener(this);
        }

        void Start()
        {
            
        }

        public void AddScoreForPlayer(int playerScore, int maxScore, int playerNumber, Color color)
        {
            var scoreUI = Instantiate(scoreUIPrefab, ScoreArea);
            scoreUI.PlayerNumber = playerNumber;
            scoreUI.PlayerColor = color;
            scoreUI.NumberInLine = nextNumberInLine++;
            scoreUI.Padding = scorePadding;
            scoreUI.InitialScore = playerScore;
            scoreUI.MaxScore = maxScore;
        }

        public void UpdateScoreForPlayer(int playerScore, int maxScore, int playerNumber)
        {
            for (var i = 0; i < ScoreArea.childCount; i++)
            {
                var scoreUI = ScoreArea.GetChild(i).GetComponent<ScoreUIElement>();
                if (scoreUI != null && scoreUI.PlayerNumber == playerNumber)
                {
                    scoreUI.UpdateScore(playerScore, maxScore);
                    break;
                }
            }
        }

        public void RemoveScoreForPlayer(int playerNumber)
        {
            ScoreUIElement childToRemove = null;
            for (var i = 0; i < ScoreArea.childCount; i++)
            {
                var scoreUI = ScoreArea.GetChild(i).GetComponent<ScoreUIElement>();
                if (scoreUI != null && scoreUI.PlayerNumber == playerNumber)
                {
                    childToRemove = scoreUI;
                    break;
                }
            }

            if (childToRemove == null)
            {
                return;
            }

            Destroy(childToRemove.gameObject);

            nextNumberInLine = 1;
            for (var i = 0; i < ScoreArea.childCount; i++)
            {
                var scoreUI = ScoreArea.GetChild(i).GetComponent<ScoreUIElement>();
                if (scoreUI != null)
                {
                    scoreUI.NumberInLine = nextNumberInLine++;
                }
            }
        }

        public void ResetNumberInLine()
        {
            nextNumberInLine = 1;
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.SCORE:
                    {
                        return HandleScoreUpdate((ScorePayload)afEvent.payload);
                    }
                case AFEventType.PLAYER_JOIN:
                    {
                        return HandlePlayerJoin((PlayerJoinPayload)afEvent.payload);
                    }
                case AFEventType.PLAYER_LEAVE:
                    {
                        return HandlePlayerLeave((PlayerLeavePayload)afEvent.payload);
                    }
                default: return false;
            }
            
        }

        private bool HandlePlayerJoin(PlayerJoinPayload payload)
        {
            AddScoreForPlayer(0, scoreManager.maxScore, payload.playerNumber, payload.color);
            return false;
        }

        private bool HandlePlayerLeave(PlayerLeavePayload payload)
        {
            RemoveScoreForPlayer(payload.playerNumber);
            return false;
        }

        private bool HandleScoreUpdate(ScorePayload payload)
        {
            UpdateScoreForPlayer(payload.playerScore, payload.maxScore, payload.playerNumber);
            return false;
        }
    }
}
