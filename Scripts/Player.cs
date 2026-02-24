using UnityEngine;
using System;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        public static SpaceShip SelectedSpaceShip;

        [SerializeField] private int m_NumLives;
        public event Action OnPlayerDead;
        [SerializeField] private SpaceShip m_PlayerShipPrefab;
        public SpaceShip ActiveShip => m_Ship;

        // private FollowCamera m_FollowCamera;
        // private ShipInputController m_ShipInputController;
        private Transform m_SpawnPoint;

        // public FollowCamera FollowCamera => m_FollowCamera;

        //public void Construct(FollowCamera followCamera, ShipInputController shipInputController, Transform spawnPoint)
        //{
        //    m_FollowCamera = followCamera;
        //    m_ShipInputController = shipInputController;
        //    m_SpawnPoint = spawnPoint;
        //}

        private SpaceShip m_Ship;

        private int m_Score;
        private int m_NumKills;

        private int m_MaxNumLives;

        public int Score => m_Score;
        public int NumKills => m_NumKills;
        public int NumLives { get { return m_NumLives; } }


        private Vector3 m_ShipPos;

        [SerializeField] private PlayerData m_PlayerData;
        public PlayerData UsedData => m_PlayerData;
        public SpaceShip ShipPrefab
        {
            get
            {
                if (SelectedSpaceShip == null)
                {
                    return m_PlayerShipPrefab;
                }
                else
                {
                    return SelectedSpaceShip;
                }
            }
        }




        void Start()
        {
            //Debug.Log("Cinnamon");

            if (m_Ship)
            {
                Respawn();

                ResetShipMaxHP();


                

                m_PlayerData.HasRockets = false;
            }

            m_MaxNumLives = m_NumLives;


        }

        public void ResetShipMaxHP()
        {
            m_Ship.ResetMaxHitPoints();
        }

        private void OnShipDeath()
        {
            //m_ExplosionEffect.Explode(m_ShipPos);




            m_NumLives--;

          

            
                if (m_NumLives > 0)
                {
                    Respawn();
                    
                }

            if (NumLives == 0)
            {
                m_PlayerData.HasRockets = false;
            }


            

            //Debug.Log(m_NumLives > 0);

            
        }

  
        private void Respawn()
        {
            //Debug.Log("Ruu");


            var newPlayerShip = Instantiate(ShipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            //m_ShipPos = newPlayerShip.transform.position;

            m_Ship.EventOnDeath.AddListener(OnShipDeath);  //

           

            if (m_PlayerData.HasRockets) m_Ship.LoadRocket();
            if (m_PlayerData.ShipMaxHitPoints > 0) m_Ship.SetMaxHitPoints(m_PlayerData.ShipMaxHitPoints);   
            

            //m_FollowCamera.SetTarget(m_Ship.transform);
            //m_ShipInputController.SetTargetShip(m_Ship);

            
        }




        public void AddKill()
        {
            m_NumKills += 1;
        }

        public void AddScore(int num)
        {
            m_Score += num;
        }

        protected void TakeDamage(int m_Damage)
        {
            //print(m_NumLives);
            m_NumLives -= m_Damage;
            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDead?.Invoke();               
            }
        }

        

        //public void HideShip()
        //{
        //    m_Ship.gameObject.SetActive(false);
        //}


    }
}
