using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
namespace TowerDefense
{
    public class MapCompletion : SingletonBase<MapCompletion>
    {
        public const string fileName = "completion.dat";

        [Serializable]
        private class EpisodeScore
        {
            [SerializeReference]
            public Episode episode;
            public int score;
            public int ID;
        }

        [SerializeField] private EpisodeScore[] completionData;
        public int TotalScore { private set; get; }
        private new void Awake()
        {
            base.Awake();
            print("Awake");

            CopyEpisodesList();

            Saver<EpisodeScore[]>.TryLoad(fileName, ref completionData);


            FillEpisodes();

        }



        public static void SaveEpisodeResult(int levelScore)
        {
            if (Instance)
            {// Сохранение новых очков прохождения.
                foreach (var item in Instance.completionData)
                {
                    if (item.episode == LevelSequenceController.Instance.CurrentEpisode)
                    {
                        if (levelScore > item.score)
                        {
                            Instance.TotalScore += levelScore - item.score;
                            item.score = levelScore;
                            Saver<EpisodeScore[]>.Save(fileName, Instance.completionData);
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"Episode complete with score {levelScore}");
            }
        }

     

        
       


       
  

        public Action ResetMapCompletion()
        {
            return () =>
            {
                foreach (var episode in completionData)
                {
                    episode.score = 0;
                }
                Saver<EpisodeScore[]>.Save(fileName, completionData);
            };
        }

        public int GetEpisodeScore(Episode m_episode)
        {
            foreach (var data in completionData)
            {
                if (data.episode == m_episode)
                {
                    return data.score;  
                }
            }
            return 0;
        }

        [SerializeField] private Episode[] m_AllEpisodes;

        /// <summary>
        /// Заполнить список прохождения эпизодов данными о самих эпизодах. 
        /// </summary>
        private void CopyEpisodesList()
        {
            List<EpisodeScore> completionDataL = new List<EpisodeScore>();
            foreach(var episode in m_AllEpisodes)
            {
                var epis = new EpisodeScore();
                epis.episode = episode;
                epis.ID = episode.EpisodeID;
                epis.score = 0;
                completionDataL.Add(epis);
            }
            completionData = completionDataL.ToArray();
        }

        /// <summary>
        /// Подцепить скриптовые объекты эпизодов по известному задаваемому
        /// в редакторе идентификатору.
        /// Эпизодам присваивается значение от 10000 до 10099
        /// </summary>
        private void FillEpisodes()
        {   
            TotalScore = 0;
            foreach (var data in completionData)
            { 
                data.episode = GetEpisodeByID(data.ID);
                TotalScore += data.score;
            }
        }

        /// <summary>
        /// Получить эпизод по задаваемому в редакторе идентификатору
        /// Эпизодам присваивается значение от 10000 до 10099
        /// </summary>
        /// <param name="EpisodeID"></param>
        /// <returns></returns>
        public Episode GetEpisodeByID(int EpisodeID)
        {
            foreach (var episode in m_AllEpisodes)
            {
                if (episode.EpisodeID == EpisodeID)
                {
                    return episode;
                }
            }
            return null;
        }

        /// <summary>
        /// Обнулить счёт.
        /// </summary>
        public void ResetValues()
        {
            foreach (var data in Instance.completionData)
            {
                data.score = 0;
            }
            TotalScore = 0;
        }
    }
}

