using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class SpawnPointManager : MonoBehaviour
    {
        private SpawnPoint[] spawnPoints;
        private int nextSpawnIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public SpawnPoint GetNextSpawnPoint()
        {
            var spawnPoint = spawnPoints[nextSpawnIndex];
            nextSpawnIndex = (nextSpawnIndex + 1) % spawnPoints.Length;
            return spawnPoint;
        }
    }
}
