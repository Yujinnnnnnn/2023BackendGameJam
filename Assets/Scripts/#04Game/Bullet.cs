using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project9
{
    public class Bullet : MonoBehaviour
    {
        public float damage;
        public int per;

        Rigidbody2D rigid;

        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        public void Init(float damage, int per, Vector3 dir)
        {
            this.damage = damage;
            this.per = per;

            if (per >= 0) {
                rigid.velocity = dir * 25f;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            //if (!collision.CompareTag("Enemy") || per == -100)
            //    return;

            if (collision.CompareTag("Enemy") && per == -100)
            {
                per--;
                Debug.Log("!! collision : " + collision.name + " : " + per);

                if (per < 0)
                {
                    if (this.name == "EnemyCleaner")
                    {
                        return;
                    }

                    if (rigid != null)
                    {
                        rigid.velocity = Vector2.zero;
                    }

                    gameObject.SetActive(false);
                }

                var enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.Die();
                }
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Area") || per == -100)
                return;

            gameObject.SetActive(false);
        }
    }
}