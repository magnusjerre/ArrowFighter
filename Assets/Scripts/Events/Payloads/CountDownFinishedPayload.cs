namespace Jerre.Events
{
    public struct CountDownFinishedPayload
    {
        public string TimerName;

        public CountDownFinishedPayload(string timerName)
        {
            TimerName = timerName;
        }
    }
}
