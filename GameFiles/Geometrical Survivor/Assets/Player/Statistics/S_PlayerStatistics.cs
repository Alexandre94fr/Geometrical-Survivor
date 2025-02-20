using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerStatistics", menuName = "ScripableObject/PlayerStatistics")]
public class S_PlayerStatistics : ScriptableObject
{
    [Header(" Basic :")]
    public string _PlayerName = "Player";

    [Header(" Movement :")]
    public int _MovementSpeed = 5;

    [Header(" Combat :")]
    public int _MaxHealthPoints = 100;

    // TODO : Equipped active capacity
    //public S_ActiveCapacity _EquippedActiveCapacity; 

    // TODO : Equipped passive capacity
    //public List<S_PassiveCapacity> _EquippedPassiveCapacities;

    [Header(" Experience :")]
    public int _NanomachinesNeededToLevelUp = 25;
    public float _NanomachinesNeededToLevelUpGrowthFactorPerLevelGain = 1.1f; // Not long at all
    public int _TechnologicalLevel = 1;

    [Header(" Economy :")]
    public int _CollectedNanomachines = 0;
}