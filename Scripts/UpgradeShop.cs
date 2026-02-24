using UnityEngine;
using UnityEngine.UI;
using System;

namespace TowerDefense
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int money;
        [SerializeField] private Text moneyText;
        [SerializeField] private BuyUpgrade[] sales;
        private void Start()
        {
            foreach (var slot in sales)
            {
                slot.Initialize();
                slot.transform.Find("Button").GetComponent<Button>().onClick
                    .AddListener(UpdateMoney);
            }
            UpdateMoney();
        }

        public void UpdateMoney()
        {
            ///print(money);
            money = MapCompletion.Instance.TotalScore;

            ///print(money);
            money -= Upgrades.GetTotalCost();

           /// print(money);
            moneyText.text = money.ToString();
            foreach (var slot in sales)
            {
                slot.CheckCost(money);
            }
        }
    }
}

