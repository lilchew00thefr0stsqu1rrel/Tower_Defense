using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource { Gold, Life, Mana }
        public UpdateSource source = UpdateSource.Gold;
        private Text m_Text;
        void Start()
        {
            m_Text = GetComponent<Text>();


            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.Instance.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                    TDPlayer.Instance.LifeUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Mana:
                    TDPlayer.Instance.ManaUpdateSubscribe(UpdateText);
                    break;
            }

        }


        private void UpdateText(int money)
        {
            if (!m_Text) return;  // 20 X 2025
            m_Text.text = money.ToString();
        }
    }
}

