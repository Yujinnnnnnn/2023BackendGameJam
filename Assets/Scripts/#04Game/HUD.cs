using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Project9
{
    public class HUD : MonoBehaviour
    {
        public enum InfoType { Exp, Level, Kill, Time, WaveTime, WaveText, Health, CatHealth, HumanHealth }
        public InfoType type;

        TextMeshProUGUI myText;
        Slider mySlider;

        void Awake()
        {
            if (type == InfoType.Health)
            {
                return; // ∞‘¿”¿Î¡÷ºÆ
            }

            myText = GetComponent<TextMeshProUGUI>();
            mySlider = GetComponent<Slider>();
        }

        void LateUpdate()
        {
            if (mySlider == null && myText == null) return;
            if (!GameManager.instance.isLive) return;

            switch (type) {
                case InfoType.Exp:
                    float curExp = GameManager.instance.exp;
                    float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                    mySlider.value = curExp / maxExp;
                    break;
                case InfoType.Level:
                    myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                    break;
                case InfoType.Kill:
                    myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                    break;
                case InfoType.Time:
                    float remainTime = GameManager.instance.totalgameTime;
                    int min = Mathf.FloorToInt(remainTime / 60);
                    int sec = Mathf.FloorToInt(remainTime % 60);
                    myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                    break;
                case InfoType.WaveText:
                    int nowWave = GameManager.instance.waveIndex;
                    myText.text = $"{nowWave + 1} / 5 Wave";
                    break;
                case InfoType.WaveTime:
                    float curWaveTime = GameManager.instance.waveTime;
                    float maxWaveTime = GameManager.instance.waveDatas[GameManager.instance.waveIndex].Time;
                    mySlider.value = curWaveTime / maxWaveTime;
                    break;
                case InfoType.CatHealth:
                    float curHealth = GameManager.instance.health;
                    float maxHealth = GameManager.instance.maxHealth;
                    mySlider.value = curHealth / maxHealth;
                    break;
                case InfoType.HumanHealth:
                    float curHumanHealth = GameManager.instance.humanHealth;
                    float maxHumanHealth = GameManager.instance.humanMaxHealth;
                    mySlider.value = curHumanHealth / maxHumanHealth;
                    break;
            }
        }
    }
}
