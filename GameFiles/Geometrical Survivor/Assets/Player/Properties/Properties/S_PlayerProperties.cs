using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerProperties", menuName = "ScriptableObject/PlayerProperties")]
public class S_PlayerProperties : ScriptableObject
{
    [Header(" Basic :")]
    public string _PlayerName = "Player";

    [Header(" Movement :")]
    public int _MovementSpeed = 5;

    [Header(" Combat :")]
    public int _MaxHealthPoints = 100;

    [Space]
    public S_ActiveCapacityProperties _EquippedActiveCapacity; 
    public List<S_PassiveCapacityProperties> _EquippedPassiveCapacities;

    [Header(" Experience :")]
    public int _NanomachinesNeededToLevelUp = 25;
    public float _NanomachinesNeededToLevelUpGrowthFactorPerLevelGain = 1.1f; // Not long at all
    public int _TechnologicalLevel = 1;

    [Header(" Economy :")]
    public int _CollectedNanomachines = 0;
    public int _NanomachineCollectionRadius = 1;
}