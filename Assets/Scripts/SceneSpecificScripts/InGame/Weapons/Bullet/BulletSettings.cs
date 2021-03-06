﻿using Jerre.GameSettings;
using UnityEngine;

namespace Jerre
{
    public class BulletSettings : MonoBehaviour
    {
        public float Speed = 200f;
        public float TimeToLive = 2f;
        public int Damage = 1;
        public int PlayerOwnerNumber;
        public Color color;
        public bool DestroyOnAnyOverlap;

        public ParticleSystem hitParticlesPrefab;

        void Awake()
        {
            Damage = GameSettingsState.INSTANCE.BasicWeaponsSettings.FireDamage;
        }
        // Start is called before the first frame update
        void Start()
        {
            GetComponentInChildren<MeshRenderer>().material.color = color;
        }
    }
}
