using Common;
using TowerDefense;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;

        [SerializeField] private ProjectileProperties m_ProjectileProperties;
        public void SetProjProps(ProjectileProperties projectileProperties)
        {
            m_ProjectileProperties = projectileProperties;
        }


        [SerializeField] private UpgradeAsset damageUpdate;
        int level = 0;

        #region UnityEvent

        private void Start()
        {           
            m_Ship = transform.parent.GetComponent<SpaceShip>();


            level = Upgrades.GetUpgradeLevel(damageUpdate);
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;
            else if (Mode == TurretMode.Auto)
            {
                Fire();                
            }
        }

        #endregion

        #region Public API

        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship)
            {
                if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
                    return;

                if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
                    return;
            }



            TDProjectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<TDProjectile>();
            projectile.SetProperties(m_ProjectileProperties);

            projectile.AddDamage(level * 5);
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;
            
            

            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

              

            {
                // SFX
            }
        }

        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode) return;

            m_RefireTimer = 0;

            m_TurretProperties = props;
        }



        #endregion
    }

}
