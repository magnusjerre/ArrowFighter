using UnityEngine;

namespace Jerre
{
    public struct PlayerInput
    {
        public float MoveX;
        public float MoveY;
        public float LookX;
        public float LookY;
        public bool Fire;
        public bool DodgeRight;
        public bool DodgeLeft;
        public bool Boost;

        public PlayerInput(float moveX, float moveY, float lookX, float lookY, bool fire, bool dodgeRight, bool dodgeLeft, bool boost)
        {
            MoveX = moveX;
            MoveY = moveY;
            LookX = lookX;
            LookY = lookY;
            Fire = fire;
            DodgeRight = dodgeRight;
            DodgeLeft = dodgeLeft;
            Boost = boost;
        }

        public Vector3 MoveDirection => new Vector3(MoveX, 0, MoveY);
        public Vector3 LookDirection => new Vector3(LookX, 0, LookY);

        public override string ToString()
        {
            return string.Format("{{moveX: {0}, moveY: {1}, lookX: {2}, lookY: {3}, fire: {4}, dodgeRight: {5}, dodgeLeft: {6}, boost: {7}}}",
            MoveX, MoveY, LookX, LookY, Fire, DodgeRight, DodgeLeft, Boost);
        }
    }
}
