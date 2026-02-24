using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public static event Action<Enemy> OnEnemySpawn;
        [SerializeField] private Path[] paths;
        [SerializeField] private EnemyWave currentWave;

        [SerializeField] private Enemy m_EnemyPrefab;

        public event Action OnAllWavesDead;
        private int activeEnemyCount = 0;

        private void RecordEnemyDead() 
        { 
            if (--activeEnemyCount == 0)
            {
               
                 ForceNextWave();
               
            }; 
        }
        private void Start()
        {
           
            currentWave.Prepare(SpawnEnemies);
        }

        private void SpawnEnemies()
        {
            foreach ((EnemyAsset asset, int count, int pathIndex) in currentWave.EnumerateSquads())
            {
                if (pathIndex < paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var e = Instantiate(m_EnemyPrefab, 
                            paths[pathIndex].StartArea.GetRandomInsideZone(), Quaternion.identity);
                        e.OnEnd += RecordEnemyDead;
                        e.Use(asset);
                        e.GetComponent<TDPatrolController>().SetPath(paths[pathIndex]);
                        activeEnemyCount += 1;
                        OnEnemySpawn?.Invoke(e);
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid path index in {name}");
                }
                
                
            }

            currentWave = currentWave.PrepareNext(SpawnEnemies);
        }

        public void ForceNextWave()
        {
            if (currentWave)
            {
                TDPlayer.Instance.ChangeGold((int)currentWave.GetRemainingTime());
                SpawnEnemies();
                SoundPlayer.Instance.Play(Sound.FerretsDook);
            }
            else
            {
                if (activeEnemyCount == 0) OnAllWavesDead?.Invoke();
            }
            
        }
    }
}

