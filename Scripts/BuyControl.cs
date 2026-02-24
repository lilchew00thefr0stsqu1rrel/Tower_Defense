using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense
{
    public class BuyControl : MonoBehaviour
    {

        [SerializeField] private TowerBuyControl m_TowerBuyPrefab;
        private List<TowerBuyControl> m_ActiveControl;
        private RectTransform m_RectTransform;

        [SerializeField] private GameObject m_Tower1GUI;
        [SerializeField] private GameObject m_Tower2GUI;
        [SerializeField] private GameObject m_Tower3GUI;

        [SerializeField] private Transform m_BuildSite;

        //*-- События Unity
        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            BuildSite.OnClickEvent += MoveToBuildSite;
            gameObject.SetActive(false);

        }
        private void OnDestroy()
        {
            BuildSite.OnClickEvent -= MoveToBuildSite;
        }
        // --//

        private void MoveToBuildSite(BuildSite buildSite)
        {
            if (!m_RectTransform) return;
            if (buildSite)
            {
                print("Bui");
                m_BuildSite = buildSite.transform;  // ~~
                var position = Camera.main.WorldToScreenPoint(buildSite.transform.root.position);
                m_RectTransform.anchoredPosition = position;
          
                m_ActiveControl = new List<TowerBuyControl>();

                

                foreach (var asset in buildSite.buildableTowers)
                {
                    if (asset != null && asset.isAvailable())
                    {
                        var newControl = Instantiate(m_TowerBuyPrefab, transform);
                        m_ActiveControl.Add(newControl);
                        newControl.SetTowerAsset(asset);
                    }

                    
                }
                if (m_ActiveControl.Count > 0)
                {
                    gameObject.SetActive(true);
                    var angle = 360 / m_ActiveControl.Count;
                    for (int i = 0; i < m_ActiveControl.Count; i++)
                    {
                        var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.left * 80);
                        m_ActiveControl[i].transform.position += offset;
                    }
                    foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
                    {
                        tbc.SetBuildSite(buildSite.transform.root);
                    }
                }
              
            }
            else
            {
                if (m_ActiveControl != null)
                {
                    foreach (var control in m_ActiveControl) Destroy(control.gameObject);
                    m_ActiveControl.Clear();
                }
                gameObject.SetActive(false);
            }

        }


        public void SetMage()
        {
            m_Tower1GUI.SetActive(true);
            m_Tower2GUI.SetActive(false);
            m_Tower3GUI.SetActive(false);

            SetBuildSiteToTowerBuy();
        }

        public void SetArcher()
        {
            m_Tower2GUI.SetActive(true);
            m_Tower1GUI.SetActive(false);
            m_Tower3GUI.SetActive(false);

            SetBuildSiteToTowerBuy();
        }

        public void SetBallista()
        {
            m_Tower3GUI.SetActive(true);
            m_Tower2GUI.SetActive(false);
            m_Tower1GUI.SetActive(false);

            SetBuildSiteToTowerBuy();
        }


        private void SetBuildSiteToTowerBuy()
        {
            foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
            {
                tbc.SetBuildSite(m_BuildSite.transform);
            }
        }

       
    }
}

