using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class OnStartSound : MonoBehaviour
    {
        [SerializeField] private Sound m_Sound;
        // Start is called before the first frame update
        void Start()
        {

            m_Sound.Play();
        }

      
    }

}
