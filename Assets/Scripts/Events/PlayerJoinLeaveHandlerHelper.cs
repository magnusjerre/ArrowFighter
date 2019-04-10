namespace Jerre.Events
{
    public class PlayerJoinLeaveHandlerHelper : IAFEventListener
    {
        public delegate void HandleJoin(PlayerJoinPayload payload);
        public delegate void HandleLeave(PlayerLeavePayload payload);

        HandleJoin joinHandler;
        HandleLeave leaveHandler;

        public IAFEventListener listener;

        public PlayerJoinLeaveHandlerHelper(HandleJoin joinHandler, HandleLeave leaveHandler)
        {
            this.joinHandler = joinHandler;
            this.leaveHandler = leaveHandler;
        }

        public void RegisterListener(IAFEventListener listener)
        {
            AFEventManager.INSTANCE.AddListener(listener);
        }

        public void PostEvent(AFEvent afEvent) {
            AFEventManager.INSTANCE.PostEvent(afEvent);
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.PLAYER_JOIN:
                    {
                        joinHandler((PlayerJoinPayload)afEvent.payload);
                        break;
                    }
                case AFEventType.PLAYER_LEAVE:
                    {
                        leaveHandler((PlayerLeavePayload)afEvent.payload);
                        break;
                    }
            }
            return false;
        }
    }
}
