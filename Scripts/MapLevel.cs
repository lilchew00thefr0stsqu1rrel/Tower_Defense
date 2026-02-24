using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace TowerDefense
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Episode m_episode;
        [SerializeField] private RectTransform resultPanel;
        [SerializeField] private Image[] resultImages;

        public bool IsComplete { get { return
                    gameObject.activeSelf && resultPanel.gameObject.activeSelf; } }

        public void LoadLevel()
        {
            if (m_episode)
            {
                LevelSequenceController.Instance.StartEpisode(m_episode);
            }
        }

        private int score;
        public int Score { get { return score; } }

        public int Initialise()
        {
            

            //var score = MapCompletion.Instance.GetEpisodeScore(m_episode);
            score = MapCompletion.Instance.GetEpisodeScore(m_episode);
            print($"name, {score} {score > 0}");
            resultPanel.gameObject.SetActive(score > 0);
            for (int i = 0; i < score; i++)
            {
                resultImages[i].color = Color.white;
            }
            return score;

        }

     

    
    }
}

