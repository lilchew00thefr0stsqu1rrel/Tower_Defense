using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class LevelWaveCondition : MonoBehaviour, ILevelCondition
    {
        private bool isCompleted;

        private void Start()
        {
            FindObjectOfType<EnemyWaveManager>().OnAllWavesDead += () =>
            {
                isCompleted = true;
            };
        }
        
        public bool IsCompleted { get { return isCompleted;  } }
    }
}

