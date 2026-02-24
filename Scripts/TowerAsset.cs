using Common;
using SpaceShooter;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public class TowerAsset : ScriptableObject
    {
        public int GoldCost = 15;
        public Sprite Sprite;
        public Sprite GUISprite;
        public ProjectileProperties ProjectileType;

        public TurretProperties turretProperties;

        [SerializeField] private UpgradeAsset requiredUpgrade;
        [SerializeField] private int requiredUpgradeLevel;

        public bool isAvailable() => !requiredUpgrade || 
            requiredUpgradeLevel <= Upgrades.GetUpgradeLevel(requiredUpgrade);
        public TowerAsset[] m_UpgradesTo;  
        
    }
    
}

