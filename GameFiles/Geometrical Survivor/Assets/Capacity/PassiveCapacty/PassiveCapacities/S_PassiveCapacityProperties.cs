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

        [Header(" Upgrades :")]
        [Tooltip("Use that value as a getter, you can use '_PassiveCapacityProperties._UpgradesPerLevels.Count' to get the same result.")]
        [ReadOnlyInInspector] public int _MaxLevel;
        [Tooltip("You can use this variable to know the current level of this passive capacity")]
        [ReadOnlyInInspector] public int _CurrentLevel;

        [Header(" Boosts :")]
        public List<GamePropertiesStruct> _UpgradesPerLevels;
    }

    [Serializable]
    public struct GamePropertiesStruct
    {
        [Header(" Economy :")]
        public int _Price;

        [Header(" Character properties :")]
        public int _MovementSpeed;

        [Space]
        public int _MaxHealthPoints;

        [Space]
        public int _NanomachineCollectionRadius;


        [Header(" Capacity properties :")]
        public int _Damage;
        public int _AttackReach;
        public int _ArmingTime;
        public float _CooldownTime;

        [Space]
        public int _InvulnerabilityTime;
        public int _AttackLifetime;

        [Space]
        public int _StunningTime;
        public float _AttackKnockback;
        public int _SelfStunningTimeWhenSucceed;
        public int _SelfStunningTimeWhenFailed;
    }

    public PassiveCapacityPropertiesStruct _PassiveCapacityProperties;

#if UNITY_EDITOR

    void OnValidate()
    {
        _PassiveCapacityProperties._MaxLevel = _PassiveCapacityProperties._UpgradesPerLevels.Count;

        // NOTE : It's here to bypass the fact that we can't set a value insind a struct, due to this version of C# (9), should be 10
        if (_PassiveCapacityProperties._CurrentLevel == 0)
            _PassiveCapacityProperties._CurrentLevel = 1;
    }
#endif
}