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
        public Color _SpriteColor;

        [Space]
        [Tooltip(
            "In order to set a custom collider, size, and more, to a specific enemy, we are forced to use a prefab. \n\n" +
            "You can copy paste the PB_DefaultEnemy prefab to be sure to have all the scripts needed for the enemy to work."
        )]
        public GameObject _Prefab;

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

#if UNITY_EDITOR

    void OnValidate()
    {
        // NOTE : It's here to bypass the fact that we can't set a value insind a struct, due to this version of C# (9), should be 10

        if (_EnemyProperties._Name == "")
            _EnemyProperties._Name = "Enemy";

        if (_EnemyProperties._SpriteColor == new Color(0, 0, 0, 0))
            _EnemyProperties._SpriteColor = new Color(0, 0, 0, 1);
    }
#endif

}