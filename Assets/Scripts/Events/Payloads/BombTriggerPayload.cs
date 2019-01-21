namespace Jerre.Events
{
    public struct BombTriggerPayload
    {
        public int OwnerPlayerNumber;
        public bool TriggeredByPlayerInput;

        public BombTriggerPayload(int ownerPlayerNumber, bool triggeredByPlayerInput)
        {
            OwnerPlayerNumber = ownerPlayerNumber;
            TriggeredByPlayerInput = triggeredByPlayerInput;
        }
    }
}
