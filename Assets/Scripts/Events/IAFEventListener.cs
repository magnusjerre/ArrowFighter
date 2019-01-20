namespace Jerre.Events
{
    public interface IAFEventListener
    {
        // returns true if no other listeners should be able to handle event after the current
        // listener has handled it.
        bool HandleEvent(AFEvent afEvent);
    }
}
