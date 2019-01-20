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
    }
}
