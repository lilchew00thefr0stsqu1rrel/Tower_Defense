using UnityEngine;



namespace SpaceShooter
{
    public class AIPointPatrol : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        private static readonly Color GizmoColor = new Color(0.8f, 0.7f, 1, 0.3f);

    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawSphere(transform.position, m_Radius);
        }
    #endif

    }
}

