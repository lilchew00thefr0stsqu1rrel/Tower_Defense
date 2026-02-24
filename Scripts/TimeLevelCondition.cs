using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class TimeLevelCondition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private float timeLimit = 4.0f;

        private void Start()
        {
            timeLimit += Time.time;
        }
        public bool IsCompleted => Time.time > timeLimit && !LevelController.Instance.IsLost;
    }
}

