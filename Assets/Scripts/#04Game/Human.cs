using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace Project9
{
    public class Human : MonoBehaviour
    {
        [Header("# Wave")]
        public List<SkeletonMecanim> humanMecanimList = new List<SkeletonMecanim>();

        [Header("# Variable")]
        public float moveSpeed = 10.0f;
        public float turnCoolingTime = 10.0f;
        public float maxDistance = 10f;

        private float nowTurnCoolingTime = 0.0f;
        private float maxTurnDegree = 60;
        private float direction = 0;

        public bool isSometimesIgnoreGoingBack = false;
        [Range(0.1f, 100f)]
        public float ignoreGoingBackProbability = 50f;

        [Header("# Game Object")]
        public SkeletonMecanim humanMecanim;
        public Transform directionTr;
        public Transform center;

        private void Awake()
        {
            direction = 0f;
        }

        void Update()
        {
            if (nowTurnCoolingTime > 0)
            {
                nowTurnCoolingTime -= Time.deltaTime;
            }
            else
            {
                SetNewDirection();
                nowTurnCoolingTime = turnCoolingTime;
            }

            TurnHead();
            Move();
        }

        public void SetHumanStat(WaveData waveData)
        {
            moveSpeed       = waveData.HumanSpeed;
            turnCoolingTime = waveData.HumanTurnCoolingTime;
            maxDistance     = waveData.HumanMaxDistance;

            humanMecanim.gameObject.SetActive(false);
            humanMecanim = humanMecanimList[waveData.Index];
            humanMecanim.gameObject.SetActive(true);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!GameManager.instance.isLive) return;
            if (!collision.gameObject.CompareTag("Enemy")) return;

            //GameManager.instance.humanHealth -= Time.deltaTime * 10;

            //if (GameManager.instance.humanHealth < 0)
            //{
            //    GameManager.instance.GameOver();
            //}

            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                switch (enemy.itemData.itemType)
                {
                    case ItemData.ItemType.EnemyAtk:
                        {
                            GameManager.instance.humanHealth -= enemy.itemData.Value;

                            Debug.Log("damage Human health : " + GameManager.instance.humanHealth);

                            if (GameManager.instance.humanHealth <= 0)
                            {
                                //for (int index = 2; index < transform.childCount; index++)
                                //{
                                //    transform.GetChild(index).gameObject.SetActive(false);
                                //}

                                GameManager.instance.GameOver();
                            }

                            AudioManager.instance.PlaySfx(AudioManager.Sfx.DamageHuman);
                        }
                        break;
                }

                enemy.Die();
            }

            //collision.gameObject.SetActive(false);
        }

        #region 이동 처리
        /// <summary>
        /// 새로운 방향을 랜덤하게 설정하는 함수
        /// </summary>
        void SetNewDirection()
        {
            GoingBack();

            direction += Random.Range(
                -maxTurnDegree, 
                maxTurnDegree
            );

            if (direction < -180) direction += 360;
            if (direction > 180) direction -= 360;
        }

        /// <summary>
        /// maxDistance 보다 멀 시,
        /// center를 바라보도록 설정하는 함수
        /// </summary>
        void GoingBack()
        {
            // 예외 처리
            if (center == null) return;
            if (Vector3.Distance(transform.position, center.position) < maxDistance) return;
            if (isSometimesIgnoreGoingBack && Random.Range(0f, 100f) < ignoreGoingBackProbability) return;

            // center를 향하도록 설정
            Vector3 difference = center.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            direction = rotationZ;
        }

        /// <summary>
        /// 머리 방향을 설정하는 함수
        /// </summary>
        void TurnHead()
        {
            directionTr.rotation = Quaternion.Lerp(
                directionTr.rotation, 
                Quaternion.Euler(0, 0, direction), 
                Time.deltaTime
            );

            if (humanMecanim == null) return;
            humanMecanim.skeleton.FlipX = (directionTr.right.x < 0);
        }

        /// <summary>
        /// 아기 이동 함수
        /// </summary>
        void Move()
        {
            if (moveSpeed == 0) return;

            transform.position += directionTr.right * moveSpeed * Time.deltaTime;
        }
        #endregion
    }
}
