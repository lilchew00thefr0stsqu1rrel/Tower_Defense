using System;
using UnityEngine;
using Common;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;

        [SerializeField] private AIPointPatrol m_PatrolPoint;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        [SerializeField] private float m_ReachPointRadius;

        [SerializeField] private float m_LeadSpeedMeasureTime;

        [SerializeField] private float m_LeadTime;

        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructible m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;


        private int m_PointIndex;


        private Stopwatch m_LeadMeasureStopwatch;
        private Stopwatch m_LeadUseStopwatch;

        private Rigidbody2D m_Rigid;


        private Rigidbody2D m_TargetRigid;  // ~

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitTimers();

            m_Rigid = m_SpaceShip.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }

        private void ActionFindNewMovePosition()
        {
            //Debug.Log(m_MovePosition + " Dolls");

            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                if (m_SelectedTarget != null)  // If there are some targets.
                {
                    //m_MovePosition = m_SelectedTarget.transform.position;

                    //Debug.Log(m_MovePosition + " Lauma K");

                    m_MovePosition = MakeLead(m_TargetRigid);

                }
                else
                {
                    bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).magnitude
                        < m_PatrolPoint.Radius;

                    if (isInsidePatrolZone == true) // Near patrol point
                    {
                        GetNewPoint();
                        
                    }
                    else  // To patrol point
                    {
                        m_MovePosition = m_PatrolPoint.transform.position;
                    }

                    


                    //if ((transform.position - m_MovePosition).magnitude < m_ReachPointRadius)
                    //    if (m_PointIndex < m_PatrolPoint.Length - 1)
                    //        m_PointIndex++;
                    //Debug.Log((transform.position - m_MovePosition).magnitude + " / "+ m_PointIndex + " / Kuu");
                }
            }

        }

        protected virtual void GetNewPoint()
        {
            if (m_RandomizeDirectionTimer.IsFinished == true)
            {
                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius
                    + m_PatrolPoint.transform.position;

                m_MovePosition = newPoint;

                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
            }
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + transform.right * 100.0f;

                //Debug.Log("Imunlaukr");
            }
        }

        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAlignTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;

        }

        private const float MAX_ANGLE = 45.0f;

        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        private void ActionFindNewAttackTarget()
        {

            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                if (m_SelectedTarget == null) m_TargetRigid = null;
                else m_TargetRigid = m_SelectedTarget.GetComponent<Rigidbody2D>();

                m_FindNewTargetTimer.Start(m_ShootDelay);
            }

        }
        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    m_FireTimer.Start(m_ShootDelay);
                }
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AlLDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

                if (v.TeamId == Destructible.TeamIdNeutral) continue;

                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if (dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = (Destructible) v;
                   // m_TargetRigid = v.GetComponent<Rigidbody2D>();
                }
            }

            return potentialTarget;
        }

        private Vector3 MakeLead(Rigidbody2D targetRigid)
        {
            if (targetRigid == null) return Vector3.zero;

            Vector3 targetVelocity = targetRigid.velocity;


            // Distance between this ship (AI) and the target.
            float targetDistance = (targetRigid.transform.position - transform.position).magnitude;


            //Debug.Log("targetDistance: " + targetDistance);

            float shipFlightTime = targetDistance / m_Rigid.velocity.magnitude;

            // Vector as line segment between current and lead positions.
            Vector3 leadLineSegmentVector = targetVelocity * shipFlightTime;

            Vector3 leadPoint = targetRigid.transform.position + leadLineSegmentVector;

            //Debug.Log(leadLineSegmentVector + " Kuutar");

            return leadPoint;
        }

        //private Vector3 MeasureSpeed(Transform tf)
        //{
        //    // � ������ ���������
        //    if (m_LeadMeasureStopwatch.IsRunning == false)
        //    {
        //        targetPos0 = tf.position;
        //        m_LeadMeasureStopwatch.Start();                
        //    }

        //    //if (m_LeadMeasureStopwatch.IsRunning == true)
        //    //{
        //    //    m_LeadUseStopwatch.Start();
        //    //}

        //    // ������� � ���� ���������� �� ���� ��������� ��������
        //    //if (m_LeadUseStopwatch.IsRunning == true)
        //    //{
        //    //    m_LeadMeasureStopwatch.Start();
        //    //    m_LeadUseStopwatch.IsFinished = false;
        //    //}


        //    // � ����� ���������, � ������ ����, ��� ������� ��������� ���������� 
        //    if (m_LeadUseStopwatch.IsRunning == false)
        //    {
        //        targetPos1 = tf.position;
        //        m_LeadUseStopwatch.Start();

   

        //        // ����������� �� ���� ��������� �������� ������������.
        //        Vector3 displacement = targetPos1 - targetPos0;

        //        // �������� �� ��������� ����� ����� ���������� � ���� ���������
        //        displacement *= (m_LeadSpeedMeasureTime + m_LeadTime) / m_LeadSpeedMeasureTime;

        //        //Debug.Log(displacement + " Aria");

        //        Vector3 speed = displacement / m_LeadSpeedMeasureTime;
        //        return speed;




        //    }

        //    Debug.Log(m_LeadMeasureStopwatch.IsRunning + " Whoo");
        //    Debug.Log(m_LeadUseStopwatch.IsRunning + " Doll");
        //    return Vector3.zero;

            
        //}

        public void SetPatrolBehaviour(AIPointPatrol patrolPoint)
        {
            m_PatrolPoint = patrolPoint;
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);

           m_LeadMeasureStopwatch = new Stopwatch(m_LeadSpeedMeasureTime, "Whoo");
            m_LeadUseStopwatch = new Stopwatch(m_LeadTime, "Doll");
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);

            m_LeadMeasureStopwatch.RemoveTime(Time.deltaTime);
            m_LeadUseStopwatch.RemoveTime(Time.deltaTime);
        }

        private void SetPatrolBehaviour()
        {
            m_AIBehaviour = AIBehaviour.Patrol;
        }

        #endregion



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(m_MovePosition, 2);

            //Gizmos.color = Color.yellow;
            //if (m_SelectedTarget != null)
            //Gizmos.DrawLine(m_SelectedTarget.transform.position, transform.position);
        }
#endif
    }
}

