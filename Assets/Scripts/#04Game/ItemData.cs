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

        [Header("# Heal, MoveSpeed, EnemyAtk 사용")]
        public float Value;

        [Header("# 보호막 갯수")]
        public int Count;

        [Header("# 타겟에게 발사되는 속도")]
        public float Speed;

        [Header("# Weapon")]
        public GameObject projectile;
        public Sprite hand;
    }
}
