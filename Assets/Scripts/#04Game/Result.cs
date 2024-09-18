using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Project9
{
    public class Result : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public GameObject VictoryCutScene;
        public GameController gameController;

        void Awake()
        {
            gameController = FindObjectOfType<GameController>();
        }

        public void Lose()
        {
            SetScore();
        }

        public void Win()
        {
            VictoryCutScene.SetActive(true);
            SetScore();
        }

        void SetScore()
        {
            if (gameController == null) return;
            int score = GameManager.instance.waveIndex * 100 + GameManager.instance.enemyKillCount * 20 + GameManager.instance.getItemCount * 50;
            scoreText.text = $"{score}";
            gameController.GameOver(score);
        }

        public void LoadSceneLobby()
        {
            Utils.LoadScene("Lobby");
        }
    }
}
