﻿using UnityEngine;

namespace Jerre
{
    public class PlayerSettings : MonoBehaviour
    {
        public int playerNumber;
        public int MaxHealth;

        public float MaxSpeed = 100f;
        public float MaxAcceleration = 1000f;

        public float MaxLookRotationSpeedDegs = 360 * Mathf.Deg2Rad;
        public float FireRate = 4;
        public float BombPauseTime = 4f;

        public Color color;

        public float DodgeSpeed = 200f;
        public float DodgeDuration = 0.25f;
        public float DodgePauseDuration = 2f;

        public float BoostSpeed = 200f;
        public float BoostDuration = 1f;
        public float BoostPauseDuration = 2f;
        public float BoostAcceleration = 2000f;

        // Start is called before the first frame update
        void Start()
        {
            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            for (var i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = color;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
   
    }
}
