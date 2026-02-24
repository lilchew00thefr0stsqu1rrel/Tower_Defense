using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerDefense
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private Image upgradeIcon;
        [SerializeField] private Text level, costText;

        private int costNumber = 0;
        [SerializeField] private Button buyButton;
        [SerializeField] private UpgradeAsset asset;
        public void Initialize()
        {
            upgradeIcon.sprite = asset.sprite;
            var savedLevel = Upgrades.GetUpgradeLevel(asset);

            print("~~~~");
            if (savedLevel >= asset.costByLevel.Length)
            {
                level.text = $"Lvl: {savedLevel} (Max)";
                buyButton.interactable = false;
                buyButton.transform.Find("Image (1)").gameObject.SetActive(false);
                buyButton.transform.Find("Text").gameObject.SetActive(false);
                costText.text = "X";
                costNumber = int.MaxValue;
            }
            else
            {
                level.text = $"Lvl: {savedLevel + 1}";
               
                costNumber = asset.costByLevel[savedLevel];
                costText.text = costNumber.ToString();
            }
                

        }

    

        public void Buy()
        {
            Upgrades.BuyUpgrade(asset);
            Initialize();
        }

        public void CheckCost(int money)
        {
            buyButton.interactable = money >= costNumber;
        }
    }
}

