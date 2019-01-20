namespace Jerre.Events
{
    public struct AFEvent
    {
        public AFEventType type;
        public object payload;

        public AFEvent(AFEventType type, object payload)
        {
            this.type = type;
            this.payload = payload;
        }
    }
}
