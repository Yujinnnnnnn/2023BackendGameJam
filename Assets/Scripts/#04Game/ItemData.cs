using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project9
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
    public class ItemData : ScriptableObject
    {
        //public enum ItemType { Melee, Range, Glove, Shoe, Heal }
        public enum ItemType { HumanShield, HumanHeal, PlayerMoveSpeed, PlayerHeal, EnemyAtk }

        [Header("# Main Info")]
        public ItemType itemType;
        public int itemId;
        public string itemName;

        [Header("# Heal, MoveSpeed, EnemyAtk ���")]
        public float Value;

        [Header("# ��ȣ�� ����")]
        public int Count;

        [Header("# Ÿ�ٿ��� �߻�Ǵ� �ӵ�")]
        public float Speed;

        [Header("# Weapon")]
        public GameObject projectile;
        public Sprite hand;
    }
}
