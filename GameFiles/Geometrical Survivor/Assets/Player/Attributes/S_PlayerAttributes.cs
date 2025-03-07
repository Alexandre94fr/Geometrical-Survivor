using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class S_PlayerAttributes : MonoBehaviour
{
    #region -= Events =-

    // Basic
    public static event Action<string> _OnPlayerNameUpdateEvent;

    // Movement
    public static event Action<int> _OnMovementSpeedUpdateEvent;

    // Combat
    public static event Action<int> _OnMaxHealthPointsUpdateEvent;
    public static event Action<int> _OnHealthPointsUpdateEvent;

    public static event Action<S_ActiveCapacityProperties> _OnEquippedActiveCapacityUpdateEvent;
    public static event Action<List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct>> _OnEquippedPassiveCapacityUpdateEvent;

    // Experience
    public static event Action<int> _OnNanomachinesNeededToLevelUpUpdateEvent;
    public static event Action<int> _OnCollectedNanomachinesSinceLevelUpUpdateEvent;
    public static event Action<int> _OnTechnologicalLevelUpdateEvent;

    // Economy
    public static event Action<int> _OnCollectedNanomachinesUpdateEvent;
    public static event Action<int> _OnNanomachineCollectionRadiusUpdateEvent;

    #endregion

    #region -= Getters / Setters =-

    #region - Basic -

    public string _PlayerName
    {
        get { return _playerName; }
        set
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_PlayerName)}' to null or '' or ' '. The variable's value has NOT been changed.");
                return;
            }

            _playerName = value;

            _OnPlayerNameUpdateEvent?.Invoke(_playerName);
        }
    }
    #endregion

    #region - Movement -

    public int _MovementSpeed
    {
        get { return _movementSpeed; }
        set
        {
            if (value < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_MovementSpeed)}' under 0. The variable's value has NOT been changed.");
                return;
            }

            _movementSpeed = value;

            _OnMovementSpeedUpdateEvent?.Invoke(_movementSpeed);
        }
    }
    #endregion

    #region - Combat -

    #region Health points

    public int _MaxHealthPoints
    {
        get { return _maxHealthPoints; }
        set
        {
            if (value < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_MaxHealthPoints)}' under 0. The variable's value has NOT been changed.");
                return;
            }

            // To avoid having more HealthPoints than the MaxHealthPoints we set HealthPoints at MaxHealthPoints
            if (value < _HealthPoints)
                _HealthPoints = value;

            _maxHealthPoints = value;

            // Launch HUD update
            S_BarHandler.NotifyBarValueChange(S_BarHandler.BarTypes.Health, _HealthPoints, _maxHealthPoints);

            _OnMaxHealthPointsUpdateEvent?.Invoke(_maxHealthPoints);
        }
    }

    public int _HealthPoints
    { 
        get { return _healthPoints; }
        set
        {
            _healthPoints = value;

            if (_healthPoints <= 0)
            {
                _healthPoints = 0;
            }

            if (_healthPoints > _MaxHealthPoints)
                _healthPoints = _MaxHealthPoints;

            // Launch HUD update
            S_BarHandler.NotifyBarValueChange(S_BarHandler.BarTypes.Health, _healthPoints, _MaxHealthPoints);

            _OnHealthPointsUpdateEvent?.Invoke(_healthPoints);
        }
    }
    #endregion

    #region Capacities

    public S_ActiveCapacityProperties _EquippedActiveCapacity
    {
        get { return _equippedActiveCapacity; }
        set
        {
            if (value == null)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_EquippedActiveCapacity)}' to 'null'. That should not append.");
                return;
            }

            _equippedActiveCapacity = value;

            // TODO : Add HUD event (change icon)

            _OnEquippedActiveCapacityUpdateEvent?.Invoke(_equippedActiveCapacity);
        }
    }

    /// <summary>
    /// <b> BEWARE : </b> If you want to use .Add() or .Remove(), please use the 
    /// <see cref = "AddEquippedPassiveCapacity(S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct)"/>, or 
    /// <see cref = "RemoveEquippedPassiveCapacity(S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct)"/> methods instead.
    /// </summary>
    public List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct> _EquippedPassiveCapacities
    {
        get { return _equippedPassiveCapacities; }
        set
        {
            if (value == null)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_EquippedPassiveCapacities)}' to 'null'. That should not append.");
                return;
            }

            _equippedPassiveCapacities = value;

            LaunchEquippedPassiveCapacityInvokes();
        }
    }
    #endregion

    #endregion

    #region - Experience -

    public int _NanomachinesNeededToLevelUp
    {
        get { return _nanomachinesNeededToLevelUp; }
        private set
        {
            _nanomachinesNeededToLevelUp = value;

            if (_nanomachinesNeededToLevelUp < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_NanomachinesNeededToLevelUp)}' under 0. The variable's value has been set to 0.");
                _nanomachinesNeededToLevelUp = 0;
            }

            // Launch HUD update
            S_BarHandler.NotifyBarValueChange(S_BarHandler.BarTypes.Nanomachine, _CollectedNanomachinesSinceLevelUp, _nanomachinesNeededToLevelUp);

            _OnNanomachinesNeededToLevelUpUpdateEvent?.Invoke(_nanomachinesNeededToLevelUp);
        }
    }

    public int _CollectedNanomachinesSinceLevelUp
    {
        get { return _collectedNanomachinesSinceLevelUp; }
        set
        {
            _collectedNanomachinesSinceLevelUp = value;

            if (_collectedNanomachinesSinceLevelUp < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_CollectedNanomachinesSinceLevelUp)}' under 0. The variable's value has been set to 0.");
                _collectedNanomachinesSinceLevelUp = 0;
            }

            // Check if the player character can level up

            // NOTE : We also check if '_collectedNanomachinesSinceLevelUp' is not 0 because
            //        if it is and it enters insind the if, it will cause a DividedByZeroException
            if (_collectedNanomachinesSinceLevelUp >= _NanomachinesNeededToLevelUp && _collectedNanomachinesSinceLevelUp != 0)
            {
                // Adding the remaining collected nanomachines
                int remainingNanomachinesAfterLevelUp = _collectedNanomachinesSinceLevelUp % _NanomachinesNeededToLevelUp;

                _TechnologicalLevel += _collectedNanomachinesSinceLevelUp / _NanomachinesNeededToLevelUp;

                _collectedNanomachinesSinceLevelUp = remainingNanomachinesAfterLevelUp;

                // TODO : Launch event open Upgrader menu
            }

            // Launch HUD update
            S_BarHandler.NotifyBarValueChange(S_BarHandler.BarTypes.Nanomachine, _collectedNanomachinesSinceLevelUp, _NanomachinesNeededToLevelUp);

            _OnCollectedNanomachinesSinceLevelUpUpdateEvent?.Invoke(_collectedNanomachinesSinceLevelUp);
        }
    }

    public int _TechnologicalLevel
    {
        get { return _technologicalLevel; }
        private set
        {
            int lastTechnologicalLevel = _technologicalLevel;

            // To handle the case where the last _technologicalLevel was 0, we set it at 1 to avoid setting _NanomachinesNeededToLevelUp to 0
            if (lastTechnologicalLevel <= 0)
                lastTechnologicalLevel = 1;

            _technologicalLevel = value;

            if (_technologicalLevel < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_TechnologicalLevel)}' under 0. The variable's value has been set to {lastTechnologicalLevel}.");
                _technologicalLevel = lastTechnologicalLevel;
            }

            if (_technologicalLevel == 0)
            {
                Debug.LogError(
                    $"ERROR ! Someone tryied to set '{nameof(_TechnologicalLevel)}' to 0, " +
                    $"this can cause DividedByZeroException in '{nameof(_CollectedNanomachinesSinceLevelUp)}' logic. \n" +
                    $"The variable's value has been set to {lastTechnologicalLevel}."
                );
                _technologicalLevel = lastTechnologicalLevel;
            }

            _NanomachinesNeededToLevelUp = Mathf.RoundToInt(
                _firstNanomachinesNeededToLevelUp * Mathf.Pow(_nanomachinesNeededToLevelUpGrowthFactorPerLevelGain, _technologicalLevel - 1)
            );

            // TODO : Launch event update HUD (_TechnologicalLevel)

            _OnTechnologicalLevelUpdateEvent?.Invoke(_technologicalLevel);
        }
    }
    #endregion

    #region - Economy -

    /// <summary>
    /// Represents the total number of nanomachines collected by the player character throw the game. </summary>
    public int _CollectedNanomachines
    {
        get { return _collectedNanomachines; }
        set
        {
            _collectedNanomachines = value;

            if (_collectedNanomachines < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_CollectedNanomachines)}' under 0. The variable's value has been set to 0.");
                _collectedNanomachines = 0;
            }

            // TODO : Launch event update HUD (CollectedNanomachine)

            _OnCollectedNanomachinesUpdateEvent?.Invoke(_collectedNanomachines);
        }
    }

    public int _NanomachineCollectionRadius
    {
        get { return _nanomachineCollectionRadius; }
        set
        {
            _nanomachineCollectionRadius = value;

            if (_nanomachineCollectionRadius < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_nanomachineCollectionRadius)}' under 0. The variable's value has been set to 0.");
                _nanomachineCollectionRadius = 0;
            }

            _OnNanomachineCollectionRadiusUpdateEvent?.Invoke(_nanomachineCollectionRadius);
        }
    }
    #endregion

    #endregion

    [HideInInspector] public S_PassiveCapacityProperties.GamePropertiesStruct _SumPassiveCapacityProperties = new();

    [Header(" Debugging :")]
    [SerializeField] bool _isDebuggingKeysEnabled;

    [Header(" Player's statistics :")]
    [SerializeField] S_PlayerProperties _playerStatistics;
    [Space]

    #region -= Attributes - Private variables =-

    [Header(" Basic :")]
    [ReadOnlyInInspector] [SerializeField] string _playerName;

    [Header(" Movement :")]
    [ReadOnlyInInspector] [SerializeField] int _movementSpeed;

    [Header(" Combat :")]
    [ReadOnlyInInspector] [SerializeField] int _maxHealthPoints;
    [ReadOnlyInInspector] [SerializeField] int _healthPoints;

    [Space]
    [ReadOnlyInInspector] [SerializeField] S_ActiveCapacityProperties _equippedActiveCapacity;
    [Tooltip("In inspector only : For the sake of the program, please do not add, remove, or modify any elements from the list. \nIt's only there to debug more easily.")]
    [SerializeField] List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct> _equippedPassiveCapacities;

    [Header(" Experience :")]
    [ReadOnlyInInspector] [SerializeField] int _nanomachinesNeededToLevelUp;
    [ReadOnlyInInspector] [SerializeField] int _collectedNanomachinesSinceLevelUp;
    [ReadOnlyInInspector] [SerializeField] float _nanomachinesNeededToLevelUpGrowthFactorPerLevelGain; // Not long at all
    [ReadOnlyInInspector] [SerializeField] int _technologicalLevel;

    [Header(" Economy :")]
    [ReadOnlyInInspector] [SerializeField] int _collectedNanomachines;
    [ReadOnlyInInspector] [SerializeField] int _nanomachineCollectionRadius;
    #endregion

    int _firstNanomachinesNeededToLevelUp;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_playerStatistics, nameof(_playerStatistics))
        )) return;

        InitializingPlayerAttributes(_playerStatistics);
    }

    void InitializingPlayerAttributes(S_PlayerProperties p_playerProperties)
    {
        // Basic
        _PlayerName = p_playerProperties._PlayerName;

        // Movement
        _MovementSpeed = p_playerProperties._MovementSpeed;

        // Combat
        _MaxHealthPoints = p_playerProperties._MaxHealthPoints;
        _HealthPoints = _MaxHealthPoints;

        _EquippedActiveCapacity = p_playerProperties._EquippedActiveCapacity;

        // Passive capacities (conversion of ScripableObject into PassiveCapacityPropertiesStruct)
        _EquippedPassiveCapacities.Clear();

        for (int i = 0; i < p_playerProperties._EquippedPassiveCapacities.Count; i++)
        {
            AddEquippedPassiveCapacity(ref p_playerProperties._EquippedPassiveCapacities[i]._PassiveCapacityProperties);
        }

        // Experience
        _NanomachinesNeededToLevelUp = p_playerProperties._NanomachinesNeededToLevelUp;
        _firstNanomachinesNeededToLevelUp = _NanomachinesNeededToLevelUp;

        _nanomachinesNeededToLevelUpGrowthFactorPerLevelGain = p_playerProperties._NanomachinesNeededToLevelUpGrowthFactorPerLevelGain;
        _TechnologicalLevel = p_playerProperties._TechnologicalLevel;

        // Economy
        _CollectedNanomachines = p_playerProperties._CollectedNanomachines;
        _NanomachineCollectionRadius = p_playerProperties._NanomachineCollectionRadius;

        // NOTE : In order to update the possible UIs linked to the _CollectedNanomachinesSinceLevelUp variable, we call it
        _CollectedNanomachinesSinceLevelUp += 0;

        UpdatePlayerAttributes();
    }

    private void Update()
    {
        // TESTING

        if (!_isDebuggingKeysEnabled)
            return;

        #region Test cases

        // Capacity
        if (Input.GetKeyDown(KeyCode.Y))
        {
            S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct passiveCapacityPropertiesStruct = _EquippedPassiveCapacities[0];

            TryLevelUpPassiveCapacity(ref passiveCapacityPropertiesStruct);
        }

        // Nanomachine
        if (Input.GetKeyDown(KeyCode.R))
        {
            _CollectedNanomachinesSinceLevelUp -= 15;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            AddNanomachine(10);
        }

        // Health
        if (Input.GetKeyDown(KeyCode.F))
        {
            _HealthPoints += 15;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _HealthPoints -= 15;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            _MaxHealthPoints += 15;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            _MaxHealthPoints -= 15;
        }

        // Player name
        if (Input.GetKeyDown(KeyCode.K))
        {
            _PlayerName = "MyCorrectorIsTheBest <3"; // Should NOT cause error
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _PlayerName = "    "; // Should cause error
        }
        #endregion
    }

    #region - Capacity - 

    #region Active capacity


    #endregion

    #region Passive capacity

    public void AddEquippedPassiveCapacity(ref S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct p_passiveCapacityPropertiesStruct)
    {
        _equippedPassiveCapacities.Add(p_passiveCapacityPropertiesStruct);

        LaunchEquippedPassiveCapacityInvokes();
    }

    public void RemoveEquippedPassiveCapacity(ref S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct p_passiveCapacityPropertiesStruct)
    {
        _equippedPassiveCapacities.Remove(p_passiveCapacityPropertiesStruct);

        LaunchEquippedPassiveCapacityInvokes();
    }

    void LaunchEquippedPassiveCapacityInvokes()
    {
        UpdatePlayerAttributes();

        // TODO : Add HUD event (change icon / order)

        _OnEquippedPassiveCapacityUpdateEvent?.Invoke(_equippedPassiveCapacities);
    }

    public void TryLevelUpPassiveCapacity(ref S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct p_passiveCapacityPropertiesStruct, bool p_doesPrintsError = true)
    {
        int equippedPassiveCapacityIndex = TryGetPassiveCapacityIndex(ref p_passiveCapacityPropertiesStruct, false);

        if (equippedPassiveCapacityIndex == -1)
        {
            if (p_doesPrintsError)
            {
                Debug.LogError($"ERROR ! The given '{p_passiveCapacityPropertiesStruct._Name}' is not equiped by the player.");
            }

            return;
        }

        // Explanation :
        // In order to be able to modify the struct value,
        // we are forced to completly replace the equipped passive capacity by a new one with new values

        S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct equippedPassiveCapacity = _EquippedPassiveCapacities[equippedPassiveCapacityIndex];

        // Security
        if (equippedPassiveCapacity._CurrentLevel >= equippedPassiveCapacity._MaxLevel)
        {
            if (p_doesPrintsError)
            {
                Debug.LogError($"ERROR ! The passive capacity you tryied to level up '{p_passiveCapacityPropertiesStruct._Name}' is already maxed out.");
            }

            return;
        }

        equippedPassiveCapacity._CurrentLevel++;

        _EquippedPassiveCapacities[equippedPassiveCapacityIndex] = equippedPassiveCapacity;

        UpdatePlayerAttributes();
    }

    public bool DoesPlayerAlreadyHaveThisPassiveCapacity(ref S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct p_passiveCapacityPropertiesStruct)
    {
        for (int i = 0; i < _EquippedPassiveCapacities.Count; i++)
        {
            if (_EquippedPassiveCapacities[i]._Name == p_passiveCapacityPropertiesStruct._Name)
            {
                return true;
            }
        }

        return false;
    }

    /// <returns> The passive capacity index in '<see cref="_EquippedPassiveCapacities"/>' but <b> ONLY </b> if the player has it.
    /// 
    /// <para> Otherwise it returns -1 and prints out a warning (if activated, he is by default). </para>
    /// 
    /// <para> <b> Note : </b> You can use the returned -1 to know if the capacity is equiped by the player,
    /// just think to disable the warning by setting the parameter '<see cref="p_doesPrintWarning"/>' to false. </para> </returns>
    public int TryGetPassiveCapacityIndex(ref S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct p_passiveCapacityPropertiesStruct, bool p_doesPrintWarning = true)
    {
        for (int i = 0; i < _EquippedPassiveCapacities.Count; i++)
        {
            if (_EquippedPassiveCapacities[i]._Name == p_passiveCapacityPropertiesStruct._Name)
            {
                return i;
            }
        }

        if (p_doesPrintWarning)
        {
            Debug.LogWarning(
                $"WARNING ! The method '{nameof(TryGetPassiveCapacityIndex)}' has not found the given '{p_passiveCapacityPropertiesStruct._Name}' " +
                $"passive capacity in the player capacities attributes. \n" +
                "The method will return -1."
            );
        }

        return -1;
    }
    #endregion

    #endregion

    #region - Nanomachine -

    /// <summary>
    /// Will add the given value to the '<see cref="_CollectedNanomachines"/>' and the '<see cref="_CollectedNanomachinesSinceLevelUp"/>'.
    /// 
    /// <para> To <b>  subtract value, </b> just give a negative value. </para>
    /// 
    /// <para> To <b> set value, </b> please use the '<see cref="_CollectedNanomachines"/>' or/and the '<see cref="_CollectedNanomachinesSinceLevelUp"/>'. </para>
    /// </summary>
    public void AddNanomachine(int p_nanomachines)
    {
        _CollectedNanomachines += p_nanomachines;
        _CollectedNanomachinesSinceLevelUp += p_nanomachines;
    }
    #endregion

    void UpdatePlayerAttributes()
    {
        _SumPassiveCapacityProperties = new();

        for (int i = 0; i < _EquippedPassiveCapacities.Count; i++)
        {
            // We get the passive capacity properties of the good level
            S_PassiveCapacityProperties.GamePropertiesStruct passiveCapacityProperties = _EquippedPassiveCapacities[i]._UpgradesPerLevels[_EquippedPassiveCapacities[i]._CurrentLevel - 1];

            // Player properties
            _SumPassiveCapacityProperties._MovementSpeed += passiveCapacityProperties._MovementSpeed;

            _SumPassiveCapacityProperties._MaxHealthPoints += passiveCapacityProperties._MaxHealthPoints;

            _SumPassiveCapacityProperties._NanomachineCollectionRadius += passiveCapacityProperties._NanomachineCollectionRadius;

            // Capacity properties
            _SumPassiveCapacityProperties._Damage += passiveCapacityProperties._Damage;
            _SumPassiveCapacityProperties._AttackReach += passiveCapacityProperties._AttackReach;

            _SumPassiveCapacityProperties._ArmingTime += passiveCapacityProperties._ArmingTime;
            _SumPassiveCapacityProperties._InvulnerabilityTime += passiveCapacityProperties._InvulnerabilityTime;
            _SumPassiveCapacityProperties._AttackLifetime += passiveCapacityProperties._AttackLifetime;
            _SumPassiveCapacityProperties._CooldownTime += passiveCapacityProperties._CooldownTime;

            _SumPassiveCapacityProperties._StunningTime += passiveCapacityProperties._StunningTime;
            _SumPassiveCapacityProperties._AttackKnockback += passiveCapacityProperties._AttackKnockback;
            _SumPassiveCapacityProperties._SelfStunningTimeWhenSucceed += passiveCapacityProperties._SelfStunningTimeWhenSucceed;
            _SumPassiveCapacityProperties._SelfStunningTimeWhenFailed += passiveCapacityProperties._SelfStunningTimeWhenFailed;
        }

        // Updating player's properties
        _MovementSpeed = _playerStatistics._MovementSpeed + _SumPassiveCapacityProperties._MovementSpeed;

        _MaxHealthPoints = _playerStatistics._MaxHealthPoints + _SumPassiveCapacityProperties._MaxHealthPoints;

        _NanomachineCollectionRadius = _playerStatistics._NanomachineCollectionRadius + _SumPassiveCapacityProperties._NanomachineCollectionRadius;
    }
}