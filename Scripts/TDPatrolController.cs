using SpaceShooter;

using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense
{
    public class TDPatrolController : AIController
    {
        private Path m_Path;
        private int pathIndex;

        [SerializeField] private UnityEvent OnEndPath;

        public void SetPath(Path newPath)
        {
            m_Path = newPath;
            SetPatrolBehaviour(m_Path[pathIndex]);
        }

        protected override void GetNewPoint()
        {
            pathIndex += 1;
            if (m_Path.Length > pathIndex)
            {
                SetPatrolBehaviour(m_Path[pathIndex]);
            }
            else
            {
                OnEndPath.Invoke(); 
                Destroy(gameObject);
            }
        }
    }
}

