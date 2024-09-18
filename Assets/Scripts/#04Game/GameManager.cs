using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project9
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [Header("# Game Control")]
        public bool isLive;
        public bool isPlayerLive => health > 0;
        public float waveTime;
        public float totalgameTime;
        //public float maxGameTime = 2 * 10f;
        [Header("# Player Info")]
        public int playerId;
        public float health;
        public float maxHealth = 100;
        public float humanHealth;
        public float humanMaxHealth = 100;
        public int level;
        public int kill;
        public int exp;
        public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
        [Header("# Game Object")]
        public PoolManager pool;
        public Player player;
        public Human human;
        public Transform humanDummy;
        public LevelUp uiLevelUp;
        public Result uiResult;
        public Transform uiJoy;
        public GameObject enemyCleaner;
        public GameObject StopPanel;

        /// <summary>
        /// ���� : ������ ���� ����
        /// </summary>
        public int getItemCount;

        /// <summary>
        /// ���� : ���� ���� �� ���� ����
        /// </summary>
        public int enemyKillCount;

        [Header("# Wave Data")]
        public WaveData[] waveDatas;

        [Header("# Item Data")]
        public ItemData[] itemDatas;

        void Awake()
        {
            instance = this;
            Application.targetFrameRate = 60;
        }

        public void GameStart(int id)
        {
            playerId = id;
            health = maxHealth;
            humanHealth = humanMaxHealth;

            waveTime = 0;
            totalgameTime = 0;

            player.gameObject.SetActive(true);
            uiLevelUp.Select(playerId % 2);
            Resume();

            AudioManager.instance.PlayBgm(true, AudioManager.Bgm.InGame);

            player.SkinSetting();
            //AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        }

        public void GameOver()
        {
            StartCoroutine(GameOverRoutine());
        }

        IEnumerator GameOverRoutine()
        {
            isLive = false;

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Lose();
            Stop();

            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GameOver);
        }

        public void GameVictroy()
        {
            StartCoroutine(GameVictroyRoutine());
        }

        IEnumerator GameVictroyRoutine()
        {
            isLive = false;
            enemyCleaner.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Win();
            Stop();

            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GameVictroy);
        }

        public void GameRetry()
        {
            SceneManager.LoadScene(0);
        }

        public void GameQuit()
        {
            Application.Quit();
        }

        void Update()
        {
            if (!isLive)
                return;

            waveTime += Time.deltaTime;
            totalgameTime += Time.deltaTime;

            if (waveIndex < 5 && waveTime >= waveDatas[waveIndex].Time)
            {
                NextWave();
            }

            //if (Input.GetKeyDown(KeyCode.N))
            //{
            //    GameObject newWeapon = new GameObject();
            //    var weapon = newWeapon.AddComponent<Weapon>();
            //    weapon.Init(GameManager.instance.itemDatas[0]);
            //}

            //if (waveTime > maxGameTime) {
            //    gameTime = maxGameTime;
            //    GameVictroy();
            //}
        }

        public int waveIndex = 0;

        public void NextWave()
        {
            waveTime = 0;
            ++waveIndex;

            if (waveDatas.Length <= waveIndex)
            {
                GameVictroy();
                return;
            }
            human.SetHumanStat(waveDatas[waveIndex]);
        }

        public void GetExp()
        {
            if (!isLive)
                return;

            exp++;

            if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)]) {
                level++;
                exp = 0;
                uiLevelUp.Show();
            }
        }

        public void Stop()
        {
            isLive = false;
            Time.timeScale = 0;
            uiJoy.localScale = Vector3.zero;
            StopPanel.SetActive(true);
        }

        public void Resume()
        {
            isLive = true;
            Time.timeScale = 1;
            uiJoy.localScale = Vector3.one;
        }
    }
}
