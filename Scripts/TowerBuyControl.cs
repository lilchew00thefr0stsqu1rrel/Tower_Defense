using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TowerBuyControl : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_TowerAsset;
        public void SetTowerAsset(TowerAsset asset)
        { 
            m_TowerAsset = asset; 
        }

        [SerializeField] private Text m_Text;

        [SerializeField] private Button m_Button;
        [SerializeField] private Transform buildSite;
        public void SetBuildSite(Transform value) 
        {
            buildSite = value; 
        }

        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck);
            m_Text.text = m_TowerAsset?.GoldCost.ToString();
            m_Button.GetComponent<Image>().sprite = m_TowerAsset.GUISprite;
        }

        private void GoldStatusCheck(int gold)
        {
            if (!m_Button) return;  //20 X 2025
            if (m_TowerAsset && gold >= m_TowerAsset.GoldCost != m_Button.interactable)
            {
                m_Button.interactable = !m_Button.interactable;
                m_Text.color = m_Button.interactable ? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TowerAsset, buildSite, m_TowerAsset.ProjectileType);
            BuildSite.HideControls();
        }

    }
}

