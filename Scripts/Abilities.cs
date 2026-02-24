using SpaceShooter;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace TowerDefense
{
    public class Abilities : SingletonBase<Abilities>
    {
        [SerializeField] private UpgradeAsset m_SlowUpgrade;
        [SerializeField] private UpgradeAsset m_FireUpgrade;
        private void Start()
        {
            var level = Upgrades.GetUpgradeLevel(m_SlowUpgrade);
            TimeButton.transform.parent.gameObject.SetActive(level > 0);
            m_TimeAbility.AddDuration((level - 1) * 3f);


            level = Upgrades.GetUpgradeLevel(m_FireUpgrade);
            FireButton.transform.parent.gameObject.SetActive(level > 0);
            m_FireAbility.AddDamage((level - 1) * 3);
        }

        [Serializable]
        public class FireAbility
        {
            [SerializeField] private int m_Cost = 5;
            public int Cost => m_Cost;
            [SerializeField] private int m_Damage = 5;
            [SerializeField] private Color m_TargetingColor;


            [SerializeField] private GameObject m_FireballPrefab;
            public void Use()
            {
                Instance.m_TargetingCircle.gameObject.SetActive(true);   

                ClickProtection.Instance.Activate((Vector2 v) =>
                {
                    if (!TDPlayer.Instance.ReduceMana(m_Cost))
                    {
                        return;
                    };

                    Vector3 position = v;
                    position.z = -Camera.main.transform.position.z;
                    position = Camera.main.ScreenToWorldPoint(position);
                    foreach (var collider in Physics2D.OverlapCircleAll(position, 5))
                    {
                        print(collider.transform.parent != null);
                        if (collider.transform.parent != null)
                        {
                            if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                            {
                          
                                enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Magic);
                            
                            }
                        }
                    }
                    ;


                    Instance.m_TargetingCircle.gameObject.SetActive(false);

                    Instantiate(m_FireballPrefab, position, Quaternion.identity);
                });
            }

            public void AddDamage(int damage)
            {
                m_Damage += damage;
            }

            
        }

     

        [Serializable]
        public class TimeAbility
        {
            [SerializeField] private int m_Cost = 10;
            public int Cost => m_Cost;
            [SerializeField] private float m_Cooldown = 15f;
            [SerializeField] private float m_Duration = 5;
            private bool m_AtCooldown = false;
            public bool AtCooldown => m_AtCooldown; 
            public void Use()
            {

                void Slow(Enemy ship)
                {
                    ship.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
                }

                foreach (var ship in FindObjectsOfType<SpaceShip>())                
                    ship.HalfMaxLinearVelocity();


                EnemyWaveManager.OnEnemySpawn += Slow;

                IEnumerator Restore()
                {
                    yield return new WaitForSeconds(m_Duration);
                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                        ship.RestoreMaxLinearVelocity();
                    EnemyWaveManager.OnEnemySpawn -= Slow;
                }

                Instance.StartCoroutine(Restore());

                IEnumerator TimeAbilityButton()
                {
                    m_AtCooldown = true;
                    Instance.TimeButton.interactable = false;
                    yield return new WaitForSeconds(m_Cooldown);
                    Instance.TimeButton.interactable = true;
                    m_AtCooldown = false;
                }

                Instance.StartCoroutine(TimeAbilityButton());

                if (!TDPlayer.Instance.ReduceMana(m_Cost))
                {
                    return;
                };
            }

            public void AddDuration(float seconds)
            {
                m_Duration += seconds;
            }
        }
        [SerializeField] private Image m_TargetingCircle;
        [SerializeField] private Button FireButton;
        [SerializeField] private Button TimeButton;
        [SerializeField] private FireAbility m_FireAbility;



        public Action<int> OnManaEnough()
        {
            return (mana) =>
            {
                FireButton.interactable = mana >= m_FireAbility.Cost;
                TimeButton.interactable = !m_TimeAbility.AtCooldown && mana >= m_TimeAbility.Cost;
            };
        }

        public void UseFireAbility() => m_FireAbility.Use();
        [SerializeField] private TimeAbility m_TimeAbility;
        public void UseTimeAbility() => m_TimeAbility.Use();


        private void Update()
        {
             if (m_TargetingCircle.gameObject.activeSelf)
                m_TargetingCircle.transform.position = Input.mousePosition;
        }
    }
}


