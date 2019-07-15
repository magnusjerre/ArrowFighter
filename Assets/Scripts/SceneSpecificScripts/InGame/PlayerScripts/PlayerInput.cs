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
        public bool Fire2;
        public bool DodgeRight;
        public bool DodgeLeft;
        public bool Boost;
        public bool Accept;
        public bool JoinLeave;

        public PlayerInput(float moveX, float moveY, float lookX, float lookY, bool fire, bool fire2, bool dodgeRight, bool dodgeLeft, bool boost, bool accept, bool joinLeave)
        {
            MoveX = moveX;
            MoveY = moveY;
            LookX = lookX;
            LookY = lookY;
            Fire = fire;
            Fire2 = fire2;
            DodgeRight = dodgeRight;
            DodgeLeft = dodgeLeft;
            Boost = boost;
            Accept = accept;
            JoinLeave = joinLeave;
        }

        public Vector3 MoveDirection => new Vector3(MoveX, 0, MoveY);
        public Vector3 LookDirection => new Vector3(LookX, 0, LookY);

        public override string ToString()
        {
            return string.Format("{{moveX: {0}, moveY: {1}, lookX: {2}, lookY: {3}, fire: {4}, fire2: {5}, dodgeRight: {6}, dodgeLeft: {7}, boost: {8}}, accept: {9}, joinLeave: {10}}",
            MoveX, MoveY, LookX, LookY, Fire, Fire2, DodgeRight, DodgeLeft, Boost, Accept, JoinLeave);
        }
    }
}
