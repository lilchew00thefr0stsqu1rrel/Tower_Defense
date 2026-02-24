using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefense
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private WarningMenu warningMenu;

        

        private void Start()
        {
            continueButton.interactable = FileHandler.HasFile(MapCompletion.fileName);
            warningMenu.gameObject.SetActive(false);
            warningMenu.SetMainMenu(gameObject);
        }

        private void OpenWarningMenu()
        {
            gameObject.SetActive(false);
            warningMenu.gameObject.SetActive(true);
        }
        public void NewGame()
        {
            if (FileHandler.HasFile(MapCompletion.fileName))
            {
                OpenWarningMenu();
            }
            else
            {
                FileHandler.Reset(MapCompletion.fileName);
                FileHandler.Reset(Upgrades.fileName);
                SceneManager.LoadScene(1);
            }   
        }

        public void Continue()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }

}
