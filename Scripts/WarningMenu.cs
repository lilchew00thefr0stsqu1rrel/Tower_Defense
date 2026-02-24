using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class WarningMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;

        public static event Action OnNewGame;  //lc

        public void SetMainMenu(GameObject mainMenu)
        {
            this.mainMenu = mainMenu;
        }

        public void Yes()
        {


            print("Turban!!!");
            
            FileHandler.Reset(MapCompletion.fileName);

            FileHandler.Reset(Upgrades.fileName);

            SceneManager.LoadScene(1);

            MapCompletion.Instance?.ResetValues();
            Upgrades.Instance?.ResetValues();
        }

        public void No()
        {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}

