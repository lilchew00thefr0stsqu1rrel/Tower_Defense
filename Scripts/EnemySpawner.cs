using Common;
using System.Reflection;
using TowerDefense;
using UnityEngine;

namespace SpaceShooter
{
    public class EnemySpawner : Spawner
    {

     


        /// <summary>
        /// —сылки на что спавнить
        /// </summary>
        [SerializeField] private Enemy m_EnemyPrefab;


        [SerializeField] private Path m_Path;

        [SerializeField] private EnemyAsset[] m_EnemyAssets;


        protected override GameObject GenerateSpawnedEntity()
        {
            var e = Instantiate(m_EnemyPrefab);
            e.Use(m_EnemyAssets[Random.Range(0, m_EnemyAssets.Length)]);
            e.GetComponent<TDPatrolController>().SetPath(m_Path);
            return e.gameObject;
        }

    }

}








        


  
