namespace TowerDefense
{
    public enum Sound
    {
        Arrow = 0,
        ArrowHit = 1,
        EnemyDie = 2,
        EnemyWin = 3,
        PlayerWin = 4,
        PlayerLose = 5,
        BGM = 6,
        Javelin = 7,
        JavelinHit = 8,
        Rune = 9,
        FireballBoil = 10,
        Bell = 11,
        FerretsDook = 12,
    }
    public static class SoundExtensions
    {
        public static void Play(this Sound sound)
        {
            SoundPlayer.Instance.Play(sound);
        }
    }
}

