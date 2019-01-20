using UnityEngine;

namespace Jerre.Events
{
    public class AFEvents
    {
        public static AFEvent Kill(int killerPlayerNumber, int killedPlayerNumber)
        {
            return new AFEvent(AFEventType.KILLED, new KilledEventPayload(killerPlayerNumber, killedPlayerNumber));
        }

        public static AFEvent GameOver(int winnerPlayerNumber)
        {
            return new AFEvent(AFEventType.GAME_OVER, new GameOverPayload(winnerPlayerNumber));
        }

        public static AFEvent PlayerJoin(int playerNumber, Color playerColor)
        {
            return new AFEvent(AFEventType.PLAYER_JOIN, new PlayerJoinPayload(playerNumber, playerColor));
        }

        public static AFEvent PlayerLeave(int playerNumber)
        {
            return new AFEvent(AFEventType.PLAYER_LEAVE, new PlayerLeavePayload(playerNumber));
        }
    }
}
