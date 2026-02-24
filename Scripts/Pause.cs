using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class Pause : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PauseGame()
        {
            (LevelController.Instance as TDLevelController).PauseLevelActivity();
        }
        public void ResumeGame()
        {
            (LevelController.Instance as TDLevelController).ResumeLevelActivity();
        }
        public void ToLevelMap()
        {
            SceneManager.LoadScene(1);
        }
    }
}

