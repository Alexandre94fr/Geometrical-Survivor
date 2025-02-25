using System;
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
        [Tooltip("Use that value as a getter, you can use '_PassiveCapacityProperties._UpgradesPerLevels.Count' to get the same result.")]
        [ReadOnlyInInspector] public int _UpgradableNumber;

        [Header(" Boosts :")]
        public List<GamePropertiesStruct> _UpgradesPerLevels;
    }

    [Serializable]
    public struct GamePropertiesStruct
    {
        [Header(" Character properties :")]
        public float _MovementSpeed;
        public float _NanomachineCollectionRadius;

        [Header(" Capacity properties :")]
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

    void OnValidate()
    {
        _PassiveCapacityProperties._UpgradableNumber = _PassiveCapacityProperties._UpgradesPerLevels.Count;
    }
}