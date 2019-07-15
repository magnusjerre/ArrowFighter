using Jerre.Weapons;
using UnityEngine;

namespace Jerre
{
    public class PeriodicSpawner : MonoBehaviour
    {
        public WeaponPickup[] Prefabs;
        public Transform SpawnPositionsParent;

        public bool RandomSpawnPosition;
        public bool RandomPrefabSpawn;
        public bool RandomSpawnTime;

        public float DefaultTimeBetweenSpawns = 5f;
        public float MaxTimeBetweenSpawns = 10f;
        public int MaxConcurrentSpawnedElements = 2;

        private float TimeUntilNextSpawn = 0;
        private int nextPrefabIndex = 0;
        private int nextSpawnIndex = 0;

        void Start()
        {
            Random.InitState(1);
        }

        void Update()
        {
            TimeUntilNextSpawn -= Time.deltaTime;
            if (TimeUntilNextSpawn > 0f) return;

            TimeUntilNextSpawn = RandomSpawnTime ? Random.Range(DefaultTimeBetweenSpawns, MaxTimeBetweenSpawns) : DefaultTimeBetweenSpawns;

            var currentActiveSpawns = 0;
            for (var i = 0; i < SpawnPositionsParent.childCount; i++)
            {
                currentActiveSpawns += SpawnPositionsParent.GetChild(i).childCount;
            }

            if (currentActiveSpawns >= MaxConcurrentSpawnedElements || currentActiveSpawns >= SpawnPositionsParent.childCount)
            {
                return;
            }

            var prefabIndex = nextPrefabIndex;
            if (RandomPrefabSpawn)
            {
                nextPrefabIndex = Random.Range(0, Prefabs.Length);
            }
            else
            {
                nextPrefabIndex = (nextPrefabIndex + 1) % Prefabs.Length;
            }

            var spawnIndex = nextSpawnIndex;
            if (RandomSpawnPosition)
            {
                nextSpawnIndex = Random.Range(0, SpawnPositionsParent.childCount);
            }
            else
            {
                nextSpawnIndex = (nextSpawnIndex + 1) % SpawnPositionsParent.childCount;
            }
            while (SpawnPositionsParent.GetChild(spawnIndex).childCount > 0)
            {
                spawnIndex = (spawnIndex + 1) % SpawnPositionsParent.childCount;
            }

            Instantiate(Prefabs[prefabIndex], SpawnPositionsParent.GetChild(spawnIndex));
        }

    }
}