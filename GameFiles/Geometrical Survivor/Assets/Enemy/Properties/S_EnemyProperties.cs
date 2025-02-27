using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_EnemyProperties", menuName = "ScriptableObject/EnemyProperties")]
public class S_EnemyProperties : ScriptableObject
{
    [Serializable]
    public struct EnemyPropertiesStruct
    {
        [Header(" Basic :")]
        public string _Name;
        public Sprite _Sprite;

        [Header(" Movement :")]
        public int _MovementSpeed;

        [Header(" Combat :")]
        public int _MaxHealthPoints;

        [Space]
        public int _AttackDamage;
        public float _AttackCooldownTime;

        [Header(" Experience :")]
        public int _NanomachinesDroppedWhenKilled;
    }

    public EnemyPropertiesStruct _EnemyProperties;
}