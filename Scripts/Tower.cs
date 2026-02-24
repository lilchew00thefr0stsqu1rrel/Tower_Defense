using Common;
using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius = 5f;
        private Turret[] turrets;
        private Rigidbody2D target = null;
        [SerializeField] private ProjectileProperties m_ProjProps;

        [SerializeField] private float m_Lead = 1f;


        public void SetProjectileProperties(ProjectileProperties projProps)
        {
            m_ProjProps = projProps;
        }


        private void Awake()
        {
            turrets = GetComponentsInChildren<Turret>();
        }

        public void Use(TowerAsset asset)
        {

            GetComponentInChildren<SpriteRenderer>().sprite = asset.Sprite;
            turrets = GetComponentsInChildren<Turret>();
            foreach (var turret in turrets)
            {
                turret.AssignLoadout(asset.turretProperties);
            }
            GetComponentInChildren<BuildSite>().SetBuildableTowers(asset.m_UpgradesTo);
        }
        private void Start()
        {
        }
        private void Update()
        {
            if (target)
            {
                if (Vector3.Distance(target.transform.position, transform.position) <= m_Radius)
                {

                    foreach (var turret in turrets)
                    {
                        turret.transform.up = target.transform.position - turret.transform.position
                             + (Vector3) target.velocity * m_Lead;
                        turret.SetProjProps(m_ProjProps);
                        turret.Fire();
                    }
                }
                else
                {
                    target = null;
                }
            }
            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);
                if (enter)
                {
                    target = enter.transform.root.GetComponent<Rigidbody2D>();

                }
            }
            
        }

        public void SetTurrets(TurretProperties turretProps)
        {
            foreach (var turret in turrets)
            {
                turret.AssignLoadout(turretProps);
            }
        }


      
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            if (target)
            {
                Gizmos.DrawWireSphere(target.transform.position, 0.1f);
                Gizmos.DrawRay(transform.position, target.transform.position - transform.position);

                Gizmos.DrawWireSphere(transform.position, m_Radius);
            }
           
        }

       
#endif
    }

}
