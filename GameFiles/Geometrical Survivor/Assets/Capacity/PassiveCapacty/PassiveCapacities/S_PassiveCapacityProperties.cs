using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_PassiveCapacityProperties", menuName = "ScriptableObject/PassiveCapacityProperties")]
public class S_PassiveCapacityProperties : ScriptableObject
{
    [Serializable]
    public struct PassiveCapacityPropertiesStruct
    {
        [Header(" Basics :")]
        public string _Name;

        [Header(" Economy :")]
        public int _Price;

        [Header(" Upgrades :")]
        public int _UpgradableNumber;
        public float _UpgradeBoostDefaultFactor;

        [Header(" Boosts :")]
        public float _MovementSpeed;
        public float _NanomachineCollectionReach;

        [Space]
        public float _Damage;
        public float _AttackReach;
        public float _ArmingTime;
        public float _CooldownTime;

        [Space]
        public float _InvulnerabilityTime;
        public float _AttackLifetime;

        [Space]
        public float _StunningTime; 
        public float _SelfStunningTimeWhenSucceed;
        public float _SelfStunningTimeWhenFailed;
    }

    public PassiveCapacityPropertiesStruct _PassiveCapacityProperties;
}