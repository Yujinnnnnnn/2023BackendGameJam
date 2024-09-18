using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Project9
{
    public class Spawner : MonoBehaviour
    {
        public Transform[] spawnPoint;
        public SpawnData[] spawnData;
        //public float levelTime;

        //public int level;
        public float timer;
        public float enemyTimer;

        void Awake()
        {
            spawnPoint = GetComponentsInChildren<Transform>();
            //levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        }

        void Update()
        {
            if (!GameManager.instance.isLive)
                return;

            var deltaTime = Time.deltaTime;

            timer += deltaTime;
            enemyTimer += deltaTime;
            var waveIndex = GameManager.instance.waveIndex;

            var itemTick = GameManager.instance.waveDatas[waveIndex].Time / GameManager.instance.waveDatas[waveIndex].SpawnItemCount;
            if (timer > itemTick)
            {
                timer = 0;
                SpawnItem();
            }

            var enemyTick = GameManager.instance.waveDatas[waveIndex].Time / GameManager.instance.waveDatas[waveIndex].EnemyItemCount;
            if (enemyTimer > enemyTick)
            {
                enemyTimer = 0;
                SpawnEnemy();
            }

            //if (timer > spawnData[level].bulletSpawnTime) {
            //    timer = 0;
            //    Spawn();
            //}
        }

        public int GetTargetIndex(ref int[] pers)
        {
            int num = UnityEngine.Random.Range(0, pers.Sum());
            float cumulative = 0f;
            int target = 0;
            for (int i = 0; i < pers.Length; i++)
            {
                cumulative += pers[i];
                //Debug.Log("i  : " + i + " / cumulative : " + cumulative + " / num : " + num);
                if (num <= cumulative)
                {
                    target = i;
                    break;
                }
            }

            return target;
        }

        void SpawnEnemy()
        {
            var waveIndex = GameManager.instance.waveIndex;

            GameObject enemy = GameManager.instance.pool.Get(0);

            // 랜덤한 스폰 포지션
            enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;

            var targetIndex = GetTargetIndex(ref GameManager.instance.waveDatas[waveIndex].EnemyPers);

            // enemy 종류 결정
            var itemData = GameManager.instance.waveDatas[waveIndex].EnemyDatas[targetIndex];

            // 난이도에 따른 spawn
            enemy.GetComponent<Enemy>().Init(itemData);
        }

        void SpawnItem()
        {
            var waveIndex = GameManager.instance.waveIndex;

            GameObject enemy = GameManager.instance.pool.Get(0);

            // 랜덤한 스폰 포지션
            enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;

            var targetIndex = GetTargetIndex(ref GameManager.instance.waveDatas[waveIndex].ItemPers);

            Debug.Log("GetTargetIndex : " + targetIndex);

            // enemy 종류 결정
            var itemData = GameManager.instance.waveDatas[waveIndex].ItemDatas[targetIndex];

            // 난이도에 따른 spawn
            enemy.GetComponent<Enemy>().Init(itemData);
            //enemy.GetComponent<Enemy>().Init(spawnData[GameManager.instance.waveIndex]);

            // 게임잼 주석
            //enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            //if (isRandom)
            //{
            //    enemy.GetComponent<Enemy>().Init(spawnData[Random.Range(0, spawnData.Length)]);
            //}
            //else
            //{
            //    // 난이도에 따른 spawn
            //    enemy.GetComponent<Enemy>().Init(spawnData[level]);
            //}
        }
    }

    [System.Serializable]
    public class SpawnData
    {
        public float bulletSpawnTime;
        public float itemSpawnTime;
        public int spriteType;
        public float speed;
    }
}
