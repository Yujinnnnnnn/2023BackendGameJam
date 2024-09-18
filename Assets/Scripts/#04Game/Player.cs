using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project9
{
    public class Player : MonoBehaviour
    {
        public Vector2 inputVec;
        public float speed;
        public Scanner scanner;
        public Hand[] hands;
        //public RuntimeAnimatorController[] animCon; // ������ �ּ� ĳ���� ����
        public SkeletonMecanim skeletonMecanim;
        public Animator anim;

        Rigidbody2D rigid;
        Weapon weapon;
        //SpriteRenderer spriter;

        void Awake()
        {            
            rigid = GetComponent<Rigidbody2D>();
            scanner = GetComponent<Scanner>();
            hands = GetComponentsInChildren<Hand>(true);
        }

        void OnEnable()
        {
            speed *= Character.Speed;
            //anim.runtimeAnimatorController = animCon[GameManager.instance.playerId]; // ������ �ּ� ĳ���� ����
        }

        void Update()
        {
            if (!GameManager.instance.isLive)
                return;

            //inputVec.x = Input.GetAxisRaw("Horizontal");
            //inputVec.y = Input.GetAxisRaw("Vertical");
        }

        void FixedUpdate()
        {
            if (!GameManager.instance.isLive)
                return;

            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }

        void LateUpdate()
        {
            if (!GameManager.instance.isLive)
                return;

            anim.SetFloat("Speed", inputVec.magnitude);

            //skeletonMecanim.up

            //Debug.Log(inputVec.x);

            if (inputVec.x != 0)
            {
                skeletonMecanim.skeleton.FlipX = inputVec.x < 0;
                //skeletonMecanim.initialFlipX = inputVec.x > 0;
                //spriter.flipX = inputVec.x > 0;
                //spriter.flipX = inputVec.x < 0;   // ������ ����
            }

            if (inputVec.x != 0)
            {
                skeletonMecanim.skeleton.FlipX = inputVec.x > 0;
                //skeletonMecanim.initialFlipX = inputVec.x < 0;
                //spriter.flipX = inputVec.x > 0;
                //spriter.flipX = inputVec.x < 0;   // ������ ����
            }
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            if (!GameManager.instance.isLive)
                return;

            //var enemy = collision.gameObject.GetComponent<Enemy>();
            //if (enemy != null)
            //{
            //    enemy.dea
            //}

            // Debug.Log("DAM health : " + GameManager.instance.health);

            // ������ : ������ ó���� ����
            //GameManager.instance.health -= Time.deltaTime * 10;

            //if (GameManager.instance.health < 0)
            //{
            //    for (int index = 2; index < transform.childCount; index++)
            //    {
            //        transform.GetChild(index).gameObject.SetActive(false);
            //    }

            //    anim.SetTrigger("Dead");
            //    GameManager.instance.GameOver();
            //}
        }

        void OnMove(InputValue value) => inputVec = value.Get<Vector2>();

        public void OnMove(Vector2 value)
        {
            if (!GameManager.instance.isPlayerLive)
            {
                inputVec = Vector2.zero;
                return;
            }

            inputVec = value;
        }

        public void SkinSetting()
        {
            skeletonMecanim.Skeleton.SetSkin(LobbyData.CatSkinID);
        }

        public void SetAtt(ref ItemData itemData)
        {
            if (GameManager.instance.health <= 0)
            {
                return;
            }

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Att"))
            {
                anim.SetTrigger("Att");
            }

            switch (itemData.itemType)
            {
                case ItemData.ItemType.HumanShield:
                    {
                        //  �Ʊ� ���� ��ȣ��
                        GameObject newWeapon = new GameObject();
                        var weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(itemData);

                        ++GameManager.instance.getItemCount;
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.GetItem);

                        var obj = AudioManager.instance.EfxObjs[0];
                        var efx = Instantiate(obj, transform.position, transform.rotation);
                        efx.transform.parent = transform;
                    }
                    break;
                case ItemData.ItemType.PlayerMoveSpeed:
                    {
                        GameManager.instance.player.speed += itemData.Value;

                        ++GameManager.instance.getItemCount;
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.GetItem);

                        var obj = AudioManager.instance.EfxObjs[0];
                        var efx = Instantiate(obj, transform.position, transform.rotation);
                        efx.transform.parent = transform;
                    }
                    break;
                case ItemData.ItemType.HumanHeal:
                    {
                        GameManager.instance.humanHealth += itemData.Value;

                        ++GameManager.instance.getItemCount;
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.GetItem);

                        var obj = AudioManager.instance.EfxObjs[0];
                        var efx = Instantiate(obj, transform.position, transform.rotation);
                        efx.transform.parent = transform;
                    }
                    break;
                case ItemData.ItemType.PlayerHeal:
                    {
                        GameManager.instance.health += itemData.Value;

                        ++GameManager.instance.getItemCount;
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.GetItem);

                        var obj = AudioManager.instance.EfxObjs[0];
                        var efx = Instantiate(obj, transform.position, transform.rotation);
                        efx.transform.parent = transform;
                    }
                    break;
                case ItemData.ItemType.EnemyAtk:
                    {
                        GameManager.instance.health -= itemData.Value;

                        //var testIdx = Random.RandomRange(0, AudioManager.instance.EfxObjs.Count);
                        //Debug.Log("testIdx : " + testIdx);

                        // 0번이펙 괜찮

                        //Debug.Log("DAM health : " + GameManager.instance.health);

                        if (GameManager.instance.health <= 0)
                        {
                            //for (int index = 2; index < transform.childCount; index++)
                            //{
                            //    transform.GetChild(index).gameObject.SetActive(false);
                            //}

                            anim.SetTrigger("Dead");
                            //GameManager.instance.GameOver();
                        }

                        ++GameManager.instance.enemyKillCount;
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.MonsterKill);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
