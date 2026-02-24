using SpaceShooter;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense
{
    public class Upgrades : SingletonBase<Upgrades>
    {
        public const string fileName = "upgrades.dat";

        [Serializable]
        private class UpgradeSave
        {
            [SerializeReference]
            public UpgradeAsset asset;
            public int level = 0;
            public int ID;
        }
        [SerializeField] UpgradeSave[] save;

        private new void Awake()
        {
            base.Awake();

            //ResetValues();

            CopyUpgradesList();

            Saver<UpgradeSave[]>.TryLoad(fileName, ref save);

            FillEpisodes();



        }
        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset == asset)
                {
                    upgrade.level++;

                   

                    Saver<UpgradeSave[]>.Save(fileName, Instance.save);




                }
            }
            //SaveGUID();
        }

        public static int GetTotalCost()
        {
            int result = 0;
            foreach (var upgrade in Instance.save) 
            {
                for (int i = 0; i < upgrade.level; i++)
                {
                    result += upgrade.asset.costByLevel[i];
                    
                }
            }
            return result;
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset == asset)
                {
                    return upgrade.level;
                }
            }
            return 0;
        }

        [SerializeField] private UpgradeAsset[] m_AllUpgradeAssets;

        /// <summary>
        /// Заполнить список улучшений данными о видах апгрейдов. 
        /// </summary>
        private void CopyUpgradesList()
        {
            List<UpgradeSave> saveL = new List<UpgradeSave>();
            foreach (var upgrade in m_AllUpgradeAssets)
            {
                var upg = new UpgradeSave();
                upg.asset = upgrade;
                upg.ID = upgrade.GUID;
                upg.level = 0;
                saveL.Add(upg);
            }
            save = saveL.ToArray();
        }

        /// <summary>
        /// Подцепить скриптовые объекты апгрейдов по известному задаваемому
        /// в редакторе идентификатору.
        /// Апгрейдам присваивается значение от 20000 до 20099
        /// </summary>
        private void FillEpisodes()
        {
            foreach (var upgrade in save)
            {
                upgrade.asset = GetUpgradeAssetByID(upgrade.ID);
               
            }
        }

        /// <summary>
        /// Получить апгрейд по задаваемому в редакторе идентификатору
        /// Апгрейдам присваивается значение от 20000 до 20099
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public UpgradeAsset GetUpgradeAssetByID(int GUID)
        {
            foreach (var asset in m_AllUpgradeAssets)
            {
                if (asset.GUID == GUID)
                {
                    return asset;
                }
            }
            return null;
        }

        /// <summary>
        /// Обнулить уровни апгрейдов.
        /// </summary>
        public void ResetValues()
        {
            foreach (var upgrade in Instance.save)
            {
                upgrade.level = 0;
            }
        }
    }
}
