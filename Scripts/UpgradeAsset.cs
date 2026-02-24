
using System;
using UnityEditor;
using UnityEngine;

namespace TowerDefense
{
    [SerializeField]
    [CreateAssetMenu]
    public class UpgradeAsset : ScriptableObject
    {
        [Header("Внешний вид")]
        public Sprite sprite;

        [Header("Игровые параметры")]
        public int[] costByLevel = { 3 };

        public int GUID;
    }
}

