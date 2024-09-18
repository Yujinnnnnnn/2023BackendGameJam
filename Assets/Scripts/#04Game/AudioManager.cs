using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project9
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("#BGM")]
        public AudioClip[] bgmClips;
        public float bgmVolume;
        AudioSource bgmPlayer;
        AudioHighPassFilter bgmEffect;

        [Header("#SFX")]
        public AudioClip[] sfxClips;
        public float sfxVolume;
        public int channels;
        AudioSource[] sfxPlayers;
        int channelIndex;

        public enum Bgm { OutGame, InGame }

        public enum Sfx { ButtonClick, MonsterKill, DamageHuman, GetItem, GameVictroy, GameOver }

        void Awake()
        {
            instance = this;
            Init();
        }

        void Init()
        {
            // 배경음 플레이어 초기화
            GameObject bgmObject = new GameObject("BgmPlayer");
            bgmObject.transform.parent = transform;
            bgmPlayer = bgmObject.AddComponent<AudioSource>();
            bgmPlayer.playOnAwake = false;
            bgmPlayer.loop = true;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.clip = bgmClips[(int)Bgm.OutGame];
            bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

            // 효과음 플레이어 초기화
            GameObject sfxObject = new GameObject("SfxPlayer");
            sfxObject.transform.parent = transform;
            sfxPlayers = new AudioSource[channels];

            for (int index = 0; index < sfxPlayers.Length; index++) {
                sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
                sfxPlayers[index].playOnAwake = false;
                sfxPlayers[index].bypassListenerEffects = true;
                sfxPlayers[index].volume = sfxVolume;
            }

            EfxObjs = new List<GameObject>();
            EfxObjs.Add(Resources.Load<GameObject>("Efx/CFXM_Firework"));
            EfxObjs.Add(Resources.Load<GameObject>("Efx/CFXM_Flash"));
            EfxObjs.Add(Resources.Load<GameObject>("Efx/CFXM_Hit_A Red"));
            EfxObjs.Add(Resources.Load<GameObject>("Efx/CFXM_Poof"));
            EfxObjs.Add(Resources.Load<GameObject>("Efx/CFXM_SmallExplosion"));

            PlayBgm(true, Bgm.OutGame);
        }

        public List<GameObject> EfxObjs;

        public void PlayBgm(bool isPlay, Bgm bgm = Bgm.OutGame)
        {            
            if (isPlay) {
                bgmPlayer.clip = bgmClips[(int)bgm];
                bgmPlayer.Play();
            }
            else {
                bgmPlayer.Stop();
            }
        }

        public void EffectBgm(bool isPlay)
        {
            bgmEffect.enabled = isPlay;
        }

        public void PlaySfx(Sfx sfx)
        {
            for (int index = 0; index < sfxPlayers.Length; index++) {
                int loopIndex = (index + channelIndex) % sfxPlayers.Length;

                if (sfxPlayers[loopIndex].isPlaying)
                    continue;

                //int ranIndex = 0;
                //if (sfx == Sfx.Hit || sfx == Sfx.Melee) {
                //    ranIndex = Random.Range(0, 2);
                //}

                channelIndex = loopIndex;
                //sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
                sfxPlayers[loopIndex].Play();
                break;
            }
        }

        public void PlaySfxButton()
        {
            PlaySfx(Sfx.ButtonClick);
        }
    }
}
