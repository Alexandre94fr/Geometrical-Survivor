using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class S_PlayerAttributes : MonoBehaviour
{
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

                print("Health : Player die");
                // TODO : Launch a death event (Health)
            }

            if (_healthPoints > _MaxHealthPoints)
                _healthPoints = _MaxHealthPoints;

            // Launch HUD update
            S_BarHandler.NotifyBarValueChange(S_BarHandler.BarTypes.Health, _healthPoints, _MaxHealthPoints);
        }
    }
    #endregion

    #region Capacities

    // TODO : Equipped active capacity
    //public S_ActiveCapacity _EquippedActiveCapacity; 

    // TODO : Equipped passive capacity
    //public List<S_PassiveCapacity> _EquippedPassiveCapacities;
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
        }
    }
    #endregion

    #endregion

    [Header(" Debugging :")]
    [SerializeField] bool _isDebuggingKeysEnabled;

    [Header(" Player's statistics :")]
    [SerializeField] S_PlayerStatistics _playerStatistics;
    [Space]

    #region -= Attributes - Private variables =-

    [Header(" Basic :")]
    [ReadOnlyInInspector] [SerializeField] string _playerName;

    [Header(" Movement :")]
    [ReadOnlyInInspector] [SerializeField] int _movementSpeed;

    [Header(" Combat :")]
    [ReadOnlyInInspector] [SerializeField] int _maxHealthPoints;
    [ReadOnlyInInspector] [SerializeField] int _healthPoints;

    // [ReadOnlyInInspector] [SerializeField] S_ActiveCapacity _equippedActiveCapacity; 
    // [ReadOnlyInInspector] [SerializeField] List<S_PassiveCapacity> _equippedPassiveCapacities;

    [Header(" Experience :")]
    [ReadOnlyInInspector] [SerializeField] int _nanomachinesNeededToLevelUp;
    [ReadOnlyInInspector] [SerializeField] int _collectedNanomachinesSinceLevelUp;
    [ReadOnlyInInspector] [SerializeField] float _nanomachinesNeededToLevelUpGrowthFactorPerLevelGain; // Not long at all
    [ReadOnlyInInspector] [SerializeField] int _technologicalLevel;

    [Header(" Economy :")]
    [ReadOnlyInInspector] [SerializeField] int _collectedNanomachines;

    #endregion

    int _firstNanomachinesNeededToLevelUp;


    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_playerStatistics, nameof(_playerStatistics))
        )) return;

        InitializingPlayerAttributes(_playerStatistics);
    }

    void InitializingPlayerAttributes(S_PlayerStatistics p_playerStatistics)
    {
        // Basic
        _PlayerName = p_playerStatistics._PlayerName;

        // Movement
        _MovementSpeed = p_playerStatistics._MovementSpeed;

        // Combat
        _MaxHealthPoints = p_playerStatistics._MaxHealthPoints;
        _HealthPoints = _MaxHealthPoints;

        // TODO : When created, un-comment
        // _EquippedActiveCapacity = p_playerStatistics._EquippedActiveCapacity;
        // _EquippedPassiveCapacities = p_playerStatistics._EquippedPassiveCapacities;

        // Experience
        _NanomachinesNeededToLevelUp = p_playerStatistics._NanomachinesNeededToLevelUp;
        _firstNanomachinesNeededToLevelUp = _NanomachinesNeededToLevelUp;

        _nanomachinesNeededToLevelUpGrowthFactorPerLevelGain = p_playerStatistics._NanomachinesNeededToLevelUpGrowthFactorPerLevelGain;
        _TechnologicalLevel = p_playerStatistics._TechnologicalLevel;

        // Economy
        _CollectedNanomachines = p_playerStatistics._CollectedNanomachines;
        // Will also update '_CollectedNanomachinesSinceLevelUp' automaticly and '_TechnologicalLevel' when there is the need to level up
    }

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

    private void Update()
    {
        // TESTING

        if (!_isDebuggingKeysEnabled)
            return;

        #region Test cases

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
}