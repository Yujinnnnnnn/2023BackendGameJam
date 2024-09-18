using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project9
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Scriptble Object/WaveData")]
    public class WaveData : ScriptableObject
    {
        [Header("# Wave Index ")]
        public int Index;

        [Header("# Wave 별 시간(s) ")]
        public float Time;

        [Header("# Wave 별 Human 속도")]
        public float HumanSpeed;

        [Header("# Wave 별 Human 회전 쿨타임")]
        public float HumanTurnCoolingTime;

        [Header("# Wave 별 Human 최대 이동 거리")]
        public float HumanMaxDistance;
        
        [Header("# 아이템 스폰 수(s) ")]
        public int SpawnItemCount;

        [Header("# 아이템 데이터 ")]
        public ItemData[] ItemDatas;

        [Header("# 아이템 확률 ")]
        public Int32[] ItemPers;

        [Header("# 적군 스폰 수(s) ")]
        public int EnemyItemCount;

        [Header("# 적군 데이터 ")]
        public ItemData[] EnemyDatas;

        [Header("# 적군 확률 ")]
        public Int32[] EnemyPers;

        // Spwan 에서 처리

        //[Header("# Wave에 발생할 발사체 수 ")] // 발사체 수를 설정하고, 발사 빈도는 Wave시간/발사체 수 로 계산
        //public float SpawnBulletCount;

        //[Header("# Wave에 발생할 아이템 수 ")] // 발사체와 동일한 방식으로 설정, n초기 주기로 생성
        //public float SpawnItemCount;
    }
}
