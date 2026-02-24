using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class NextWaveGUI : MonoBehaviour
    {
        [SerializeField] private Text bonusAmount;
        private EnemyWaveManager manager;
        [SerializeField] private float timeToNextWave;

        [SerializeField] private Image progressFill;
        private float wavePrepareTime;

        // Start is called before the first frame update
        void Start()
        {
            manager = FindObjectOfType<EnemyWaveManager>();
            EnemyWave.OnWavePrepare += (float time) =>
            {
                timeToNextWave = time;
                wavePrepareTime = time;
            };


        }

        public void CallWave()
        {
            manager.ForceNextWave();

        }
        private void Update()
        {
            var bonus = (int)timeToNextWave;
            if (bonus < 0) bonus = 0;
            bonusAmount.text = bonus.ToString();
            timeToNextWave -= Time.deltaTime;

            progressFill.fillAmount = timeToNextWave / wavePrepareTime;
        }
    }

}
