using SpaceShooter;
using UnityEngine;
using System;
using Common;
using System.Collections;

namespace TowerDefense
{
    public class TDPlayer : Player
    {
        [SerializeField] private int m_Mana;

        [SerializeField] private int m_MaxMana;

        public static new TDPlayer Instance 
        { get 
            { 
                return Player.Instance as TDPlayer; 
            } 
        }

        private void Update()
        {
            //print(Instance);   
        }

        [SerializeField] private Abilities m_Abilities;

        //~
        private event Action<int> OnGoldUpdate;
        public void GoldUpdateSubscribe(Action<int> act)
        {
            //print(Instance);
            OnGoldUpdate += act;
            act(Instance.m_Gold);
        }
        public event Action<int> OnLifeUpdate;
        public void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Instance.NumLives);
        }
        public event Action<int> OnManaUpdate;
        public void ManaUpdateSubscribe(Action<int> act)
        {
            OnManaUpdate += act;
            act(Instance.m_Mana);
        }
        

        [SerializeField] private int m_Gold = 0;
  
        public void ChangeGold(int change)
        {
            
            m_Gold += change;
            OnGoldUpdate(m_Gold);
        }

        public void ReduceLife(int change)
        {

            TakeDamage(change);
            OnLifeUpdate(NumLives);
        }
        private event Action<int> ChangeMana;
        public bool ReduceMana(int change)
        {
            if (m_Mana >= change)
            {
                m_Mana -= change;
                OnManaUpdate(m_Mana);
                ChangeMana(m_Mana);

                if (m_Mana < change)
                {
                    SoundPlayer.Instance.Play(Sound.Bell);
                }
                return true;
            }
            return false;   
        }
        public void RestoreMana(int change)
        {
            m_Mana = Mathf.Min(m_Mana + change, m_MaxMana);

            OnManaUpdate(m_Mana);
            ChangeMana(m_Mana);
        }
        private float m_ManaRegenInterval = 1;
        IEnumerator RegenMana()
        {
            yield return new WaitForSeconds(m_ManaRegenInterval);
            RestoreMana(1);
            StartCoroutine(RegenMana());
        }

        // TODO: верим в то, что золота на постройку достаточно
        [SerializeField] private Tower m_TowerPrefab;
        public void TryBuild(TowerAsset towerAsset, Transform buildSite, ProjectileProperties projectileProps)
        {
            //print(buildSite);
            ChangeGold(-towerAsset.GoldCost);
            var tower = Instantiate(m_TowerPrefab, buildSite.position, Quaternion.identity);
            tower.Use(towerAsset);
            Destroy(buildSite.gameObject);
        }

        [SerializeField] private UpgradeAsset healthUpgrade;
        [SerializeField] private UpgradeAsset goldUpgrade;
        private void Start()
        {
            var level = Upgrades.GetUpgradeLevel(healthUpgrade);
            TakeDamage(-level * 5);

            level = Upgrades.GetUpgradeLevel(goldUpgrade);
            m_Gold += level * 5;

            StartCoroutine(RegenMana());

            m_Abilities = FindObjectOfType<Abilities>();    

            ChangeMana += m_Abilities.OnManaEnough();
        }
    }
}

