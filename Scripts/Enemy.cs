using UnityEngine;
using SpaceShooter;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;


namespace TowerDefense
{
    [RequireComponent(typeof(Destructible))]
    [RequireComponent(typeof(TDPatrolController))]
    public class Enemy : MonoBehaviour
    {
        public enum ArmorType { Base=0, Mage=1 }

        private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions =
        {
            (int power, TDProjectile.DamageType type, int armor) =>
            { // ArmorType.Base
                switch (type)
                {
                    case TDProjectile.DamageType.Magic: return power;
                    default: return Mathf.Max(power -  armor, 1);
                }
            },
            (int power, TDProjectile.DamageType type, int armor) =>
            { // ArmorType.Mage
                if (TDProjectile.DamageType.Base == type)
                    armor = armor / 2; 
                   
                 
                return Mathf.Max(power -  armor, 1);
            }

        };

        [SerializeField] private ArmorType m_ArmorType;

        [SerializeField] private int m_Damage = 1;
        [SerializeField] private int m_Gold = 1;
        [SerializeField] private int m_Armor = 1;

        private Destructible m_Destructible;

        public event Action OnEnd;

        private void Awake()
        {
            m_Destructible = GetComponent<Destructible>();
        }

        private void OnDestroy() 
        { 
            OnEnd?.Invoke(); 
        }

        public void Use(EnemyAsset asset)
        {
            var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);

            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animations;

            GetComponent<SpaceShip>().Use(asset);

            GetComponentInChildren<CircleCollider2D>().radius = asset.radius;
            GetComponentInChildren<CircleCollider2D>().offset = asset.offset;

            m_Damage = asset.damage;
            m_Armor = asset.armor;
            m_ArmorType = asset.armorType;
            m_Gold = asset.gold;

            
        }

    
        

        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_Damage);
        }
        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(m_Gold);
        }
        public void TakeDamage(int damage, TDProjectile.DamageType damageType)
        {
            m_Destructible.ApplyDamage(ArmorDamageFunctions[(int) m_ArmorType](damage, damageType, m_Armor));
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
            if (a)
            {
                (target as Enemy).Use(a);
            }
        }
    }
#endif
}
