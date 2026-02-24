using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using Common;

namespace TowerDefense
{
    public class TDProjectile : ProjectileBase
    {
        public enum DamageType { Base, Magic }
        [SerializeField] private DamageType m_DamageType;
        [SerializeField] private Sound m_ShotSound = Sound.Arrow;
        [SerializeField] private Sound m_HitSound = Sound.ArrowHit;

        protected override void OnStart()
        {
            m_ShotSound.Play();
        }
        protected override void OnHit(RaycastHit2D hit)
        {
            m_HitSound.Play();
            var enemy = hit.collider.transform.root.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(m_Damage, m_DamageType);
            }
        }

        public void AddDamage(int damage)
        {
            m_Damage += damage;
        }

        protected override void OnTriggerEnter2DAdditive()
        {
          
            m_HitSound.Play();
        }
    }
}


