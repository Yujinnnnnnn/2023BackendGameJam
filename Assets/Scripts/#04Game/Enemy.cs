using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project9
{
    public class Enemy : MonoBehaviour
    {
        public float speed;
        public float health;
        public float maxHealth;
        //public RuntimeAnimatorController[] animCon;
        public Rigidbody2D target;

        bool isLive;

        Rigidbody2D rigid;
        Collider2D coll;
        //Animator anim;
        SpriteRenderer spriter;
        WaitForFixedUpdate wait;
        public ItemData itemData;

        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            coll = GetComponent<Collider2D>();
            //anim = GetComponent<Animator>();
            spriter = GetComponent<SpriteRenderer>();
            wait = new WaitForFixedUpdate();
        }

        void FixedUpdate()
        {
            if (!GameManager.instance.isLive)
                return;

            //if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            //    return;

            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }

        void LateUpdate()
        {
            if (!GameManager.instance.isLive)
                return;

            if (!isLive)
                return;

            spriter.flipX = target.position.x < rigid.position.x;
        }

        void OnEnable()
        {
            // 게임잼 타겟 Human 으로 변경
            //target = GameManager.instance.player.GetComponent<Rigidbody2D>();
            target = GameManager.instance.human.GetComponent<Rigidbody2D>();
            isLive = true;
            coll.enabled = true;
            rigid.simulated = true;
            spriter.sortingOrder = 2;
            //anim.SetBool("Dead", false);
            health = maxHealth;
        }

        public void Init(SpawnData data)
        {
            //anim.runtimeAnimatorController = animCon[data.spriteType];
            speed = data.speed;
            //spriter = data.ha
            //maxHealth = data.health;
            //health = data.health;
        }

        public void Init(ItemData data)
        {
            itemData = data;
            speed = data.Speed;
            spriter.sprite = data.hand;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null) return;

            if (!collision.gameObject.CompareTag("Player") || !isLive)
                return;

            Debug.Log(collision.gameObject.name);

            var player = collision.gameObject.GetComponent<Player>();
            player?.SetAtt(ref itemData);

            //isLive = false;
            //coll.enabled = false;
            //rigid.simulated = false;
            //spriter.sortingOrder = 1;
            Die();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            // 게임잼 주석

            return;

            //if (!collision.CompareTag("Bullet") || !isLive) // 게임잼 주석
            //    return;

            //Debug.Log(collision.name);
            //Debug.Log(collision.tag);
            if (!collision.CompareTag("Player") || !isLive)
                return;

            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;

            Debug.Log("Dead : " + this.name);
            //Dead();

            //anim.SetBool("Dead", true);
            //GameManager.instance.kill++;
            //GameManager.instance.GetExp(); // 게임잼 주석

            //if (GameManager.instance.isLive)
              //  AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

            //health -= collision.GetComponent<Bullet>().damage;

            // 게임잼 : 테스트 한방
            //health = 0;

            // TODO : Enemy fx 설정 시.
            //var efx = GameManager.instance.pool.Get(3).transform;
            //efx.parent = transform;
            //efx.localPosition = Vector3.zero;
            //efx.localRotation = Quaternion.identity;

            //StartCoroutine(KnockBack());

            //if (health > 0) {
            //    anim.SetTrigger("Hit");
            //    AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
            //}
            //else {
            //    isLive = false;
            //    coll.enabled = false;
            //    rigid.simulated = false;
            //    spriter.sortingOrder = 1;
            //    anim.SetBool("Dead", true);

            //    GameManager.instance.kill++;
            //    //GameManager.instance.GetExp(); // 게임잼 주석

            //    if (GameManager.instance.isLive)
            //        AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            //}
        }

        IEnumerator KnockBack()
        {
            yield return wait; // 다음 하나의 물리 프레임 딜레이
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 dirVec = transform.position - playerPos;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }

        public void Die()
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            gameObject.SetActive(false);
        }
    }
}
