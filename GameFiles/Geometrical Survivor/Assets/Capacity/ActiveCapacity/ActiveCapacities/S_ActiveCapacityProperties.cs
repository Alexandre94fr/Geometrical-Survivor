using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ActiveCapactityProperties", menuName = "ScriptableObject/ActiveCapactityProperties")]
public class S_ActiveCapacityProperties : ScriptableObject
{
    [Serializable]
    public struct ActiveCapacityStruct
    {
        [Header(" Economy :")]
        public int _Price;
        public int _NanomachineCostPerUse;

        [Header(" Basic :")]
        public string _Name;

        [Space]
        public float _ArmingTime;
        public int _InvulnerabilityTime;
        public float _AttackLifetime;
        public float _CooldownTime;

        [Space]
        public float _AttackStunningTime;
        public float _AttackKnockback;
        public float _SelfStunningTimeWhenSucceed;
        public float _SelfStunningTimeWhenFailed;


        [Header(" Attack :")]
        public int _AttackDamage;
        [Tooltip("Correspond for example to the attack's animation speed for a classic attack. On the other hand, the variable is NOT used for a projectile attack.")]
        public float _AttackSpeed;
        [Tooltip("Correspond for classic attacks, to the reach of damage of the attack. On the other hand, for projectile attacks it correspond to the distance the projectile will do before disappearing.")]
        public float _AttackReach;
        [Vector2Range(-1, -1, 1, 1)] public Vector2 _AttackDirection; // Forward = (0, 1), Backward = (0, -1)

        [Header(" Projectile :")]
        [Tooltip("Making a projectile spawn, will transfert the capacity's attributs to the projectile.")]
        public bool _DoesInstantiateProjectile;
        public GameObject _ProjectilePrefab;
    }

    public ActiveCapacityStruct _ActiveCapacityProperties;
}