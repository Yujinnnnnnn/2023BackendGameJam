using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project9
{
    [CreateAssetMenu(fileName = "Unit", menuName = "Scriptble Object/UnitData")]
    public class UnitData : ScriptableObject
    {
        [Header("# Unit Index ")]
        public int Index;

        [Header("# 이동속도 ")]
        public float MoveSpeed;

        [Header("# 체력 ")]
        public float Hp;

        [Header("# 리소스 정보 ")]
        public string Resource;
    }
}
