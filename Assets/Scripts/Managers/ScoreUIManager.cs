using UnityEngine;
using Jerre.UI;
using Jerre.Events;
using System.Collections.Generic;

namespace Jerre
{
    public class ScoreUIManager : MonoBehaviour, IAFEventListener
    {

        public ScoreUICanvas ScoreUICanvasPrefab;

        private AFEventManager eventManager;
        //int: playernumber
        private Dictionary<int, ScoreUICanvas> canvases;
        private ScoreManager scoreManager;

        private void Awake()
        {
            canvases = new Dictionary<int, ScoreUICanvas>();
            eventManager = GameObject.FindObjectOfType<AFEventManager>();
            eventManager.AddListener(this);

            scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        }

        void Start()
        {

        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.PLAYER_JOIN:
                    {
                        var payload = (PlayerJoinPayload)afEvent.payload;
                        if (!canvases.ContainsKey(payload.playerNumber))
                        {
                            var newCanvas = Instantiate(ScoreUICanvasPrefab);
                            newCanvas.PlayerNumber = payload.playerNumber;
                            newCanvas.ScoreManager = scoreManager;
                            canvases.Add(payload.playerNumber, newCanvas);
                        }
                        return false;
                    }
                case AFEventType.PLAYER_LEAVE:
                    {
                        var payload = (PlayerLeavePayload)afEvent.payload;
                        if (canvases.ContainsKey(payload.playerNumber))
                        {
                            var canvas = canvases[payload.playerNumber];
                            canvas.CleanUpForDestroy();
                            Destroy(canvas.gameObject);
                        }
                        return false;
                    }
            }

            return false;
        }
    }
}
