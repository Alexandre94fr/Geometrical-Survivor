using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_UpgraderMenu : MonoBehaviour
{
    [Header(" Properties :")]
    [SerializeField] [Range(0, 1)] float _activeCapacityApparitionFactor = 0.25f;

    [Space]
    [SerializeField] int _baseRepairCost = 10;
    [SerializeField] [Range(0, 2)] float _repairUseCostFactor = 1;
    [SerializeField] [Range(0, 1)] float _healthPointsHealFactorPerRepair = 0.1f;
    [SerializeField] int _costPerHealthPointsHealed = 1;

    [Header(" Internal references :")]
    [SerializeField] GameObject _upgraderMenuUIGameObject;
    [SerializeField] Button _firstButtonSelected;

    [Space]
    [SerializeField] List<S_CapacitySeller> _capacitySellers;

    [Space]
    [SerializeField] S_RepairHealthBar _repairHealthBar;
    [SerializeField] Button _repairButton;
    [SerializeField] TextMeshProUGUI _repairCostText;

    [Space]
    [SerializeField] TextMeshProUGUI _remainingCollectedNanomachinesText;

    [Header(" External references :")]
    [SerializeField] List<S_ActiveCapacityProperties> _activeCapacityProperties;
    [SerializeField] List<S_PassiveCapacityProperties> _passiveCapacityProperties;

    // Repair cost
    int _currentRepairCost;
    int _repairUseNumber = 0;

    int _healthPointsAfterHeal { 
        get { return (int)(_playerMaxHealthPoints * _healthPointsHealFactorPerRepair); } 
    }

    // Player properties
    S_PlayerAttributes _playerAttributes;

    int _playerHealthPoints;
    int _playerMaxHealthPoints;

    S_ActiveCapacityProperties _playerActiveCapacityProperties;
    List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct> _playerPassiveCapacityPropertiesStruct;

    int _playerCurrentNanomachine;
    int _playerLevel;


    void Start()
    {
        _playerAttributes = S_Player._Instance._PlayerAttributes;

        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_firstButtonSelected, nameof(_firstButtonSelected)),
            (_upgraderMenuUIGameObject, nameof(_upgraderMenuUIGameObject)),
            (_capacitySellers, nameof(_capacitySellers)),
            (_repairHealthBar, nameof(_repairHealthBar)),
            (_repairButton, nameof(_repairButton)),
            (_repairCostText, nameof(_repairCostText)),
            (_remainingCollectedNanomachinesText, nameof(_remainingCollectedNanomachinesText)),

            (_activeCapacityProperties, nameof(_activeCapacityProperties)),
            (_passiveCapacityProperties, nameof(_passiveCapacityProperties))
        )) return;

        S_PlayerAttributes._OnHealthPointsUpdateEvent += OnPlayerHealthPointsUpdate;
        S_PlayerAttributes._OnMaxHealthPointsUpdateEvent += OnPlayerMaxHealthPointsUpdate;

        S_PlayerAttributes._OnEquippedActiveCapacityUpdateEvent += OnPlayerActiveCapacityPropertiesUpdate;
        S_PlayerAttributes._OnEquippedPassiveCapacityUpdateEvent += OnPlayerPassiveCapacityPropertiesStructUpdate;

        S_PlayerAttributes._OnCollectedNanomachinesUpdateEvent += OnPlayerCurrentNanomachineUpdate;
        S_PlayerAttributes._OnTechnologicalLevelUpdateEvent += OnPlayerLevelUpdate;
    }

    void OnDestroy()
    {
        S_PlayerAttributes._OnHealthPointsUpdateEvent -= OnPlayerHealthPointsUpdate;
        S_PlayerAttributes._OnMaxHealthPointsUpdateEvent -= OnPlayerMaxHealthPointsUpdate;

        S_PlayerAttributes._OnEquippedActiveCapacityUpdateEvent -= OnPlayerActiveCapacityPropertiesUpdate;
        S_PlayerAttributes._OnEquippedPassiveCapacityUpdateEvent -= OnPlayerPassiveCapacityPropertiesStructUpdate;

        S_PlayerAttributes._OnCollectedNanomachinesUpdateEvent -= OnPlayerCurrentNanomachineUpdate;
        S_PlayerAttributes._OnTechnologicalLevelUpdateEvent -= OnPlayerLevelUpdate;
    }

    #region Player attributes update methods

    void OnPlayerHealthPointsUpdate(int p_newPlayerHealthPoints)
    {
        _playerHealthPoints = p_newPlayerHealthPoints;

        UpdateHealthBar(_playerHealthPoints, _playerMaxHealthPoints, _healthPointsAfterHeal, _playerMaxHealthPoints);
    }

    void OnPlayerMaxHealthPointsUpdate(int p_playerMaxHealthPoints)
    {
        _playerMaxHealthPoints = p_playerMaxHealthPoints;

        UpdateHealthBar(_playerHealthPoints, _playerMaxHealthPoints, _healthPointsAfterHeal, _playerMaxHealthPoints);

        // If the player take a passive capacity that changes the max health points, we need to update the repair button
        UpdateRepairButton(_repairButton);
    }

    void OnPlayerCurrentNanomachineUpdate(int p_newCurrentNanomachine)
    {
        _playerCurrentNanomachine = p_newCurrentNanomachine;

        UpdateRemainingNanomachineTextUI(p_newCurrentNanomachine);
        UpdateRepairButton(_repairButton);
    }

    void OnPlayerLevelUpdate(int p_newLevel)
    {
        _playerLevel = p_newLevel;

        if (_playerLevel > 1)
            OpenUpgraderMenu();
    }

    void OnPlayerActiveCapacityPropertiesUpdate(S_ActiveCapacityProperties p_newActiveCapacityProperties)
    {
        _playerActiveCapacityProperties = p_newActiveCapacityProperties;
    }

    void OnPlayerPassiveCapacityPropertiesStructUpdate(List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct> p_newPassiveCapacityPropertiesStruct)
    {
        _playerPassiveCapacityPropertiesStruct = p_newPassiveCapacityPropertiesStruct;
    }
    #endregion

    void OpenUpgraderMenu()
    {
        S_PauseMenuUI._OnCanPauseMenuBeShowedEvent?.Invoke(false);
        
        Time.timeScale = 0;
        _upgraderMenuUIGameObject.SetActive(true);

        _firstButtonSelected.Select();

        // Updating UI's values
        UpdateCapacitySellers(GetRandomCapacities(_capacitySellers.Count));

        UpdateHealthBar(_playerHealthPoints, _playerMaxHealthPoints, _healthPointsAfterHeal, _playerMaxHealthPoints);

        UpdateRepairCostText();

        UpdateRemainingNanomachineTextUI(_playerAttributes._CollectedNanomachines);
    }

    public void CloseUpgraderMenu()
    {
        S_PauseMenuUI._OnCanPauseMenuBeShowedEvent?.Invoke(true);

        Time.timeScale = 1;
        _upgraderMenuUIGameObject.SetActive(false);
    }

    #region OpenUpgraderMenu sub-methods

    /// <summary>
    /// Selects a random set of available capacities, ensuring that the player does not receive duplicates
    /// or capacities they have already maxed out. 
    /// 
    /// <para> <b> Beware ! </b> In certain case you can recieve less that wanted,
    /// it's because the player have almost all passive capacities maxed out. </para> </summary>
    /// 
    /// <param name = "p_numberOfCapacities"> The number of capacities to randomly select.</param>
    /// <returns> A tuple containing the selected active and passive capacities. </returns>
    (List<S_ActiveCapacityProperties>, List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct>) GetRandomCapacities(int p_numberOfCapacities)
    {
        if (p_numberOfCapacities <= 0)
        {
            Debug.LogError("ERROR! The number of requested capacities must be greater than 0.");
            return (null, null);
        }

        // Copying the available capacities list
        List<S_ActiveCapacityProperties> sortedActiveCapacityProperties = new(_activeCapacityProperties);
        List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct> sortedPassiveCapacityProperties = new();

        for (int i = 0; i < _passiveCapacityProperties.Count; i++)
        {
            sortedPassiveCapacityProperties.Add(_passiveCapacityProperties[i]._PassiveCapacityProperties);
        }

        // Removing already owned active capacity
        sortedActiveCapacityProperties.Remove(_playerActiveCapacityProperties);

        // Removing already owned and maxed out passive capacities
        for (int i = sortedPassiveCapacityProperties.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < _playerPassiveCapacityPropertiesStruct.Count; j++)
            {
                if (_playerPassiveCapacityPropertiesStruct[j]._Name == sortedPassiveCapacityProperties[i]._Name &&
                    _playerPassiveCapacityPropertiesStruct[j]._CurrentLevel >= _playerPassiveCapacityPropertiesStruct[j]._MaxLevel)
                {
                    sortedPassiveCapacityProperties.RemoveAt(i);
                    break;
                }
            }
        }

        // Picking random capacities ensuring the total count matches the required number
        List<S_ActiveCapacityProperties> selectedActiveCapacities = new();
        List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct> selectedPassiveCapacities = new();

        for (int i = 0; i < p_numberOfCapacities; i++)
        {
            // 25 % chance to pick a active capacity (check out _activeCapacityApparitionFactor for the real value)
            if (UnityEngine.Random.value > _activeCapacityApparitionFactor && sortedActiveCapacityProperties.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, sortedActiveCapacityProperties.Count);
                selectedActiveCapacities.Add(sortedActiveCapacityProperties[randomIndex]);
                sortedActiveCapacityProperties.RemoveAt(randomIndex);
            }
            else if (sortedPassiveCapacityProperties.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, sortedPassiveCapacityProperties.Count);
                selectedPassiveCapacities.Add(sortedPassiveCapacityProperties[randomIndex]);
                sortedPassiveCapacityProperties.RemoveAt(randomIndex);
            }
        }

        return (selectedActiveCapacities, selectedPassiveCapacities);
    }

    /// <summary>
    /// Updates the <see cref="_capacitySellers"/> with the given capacities' attributes </summary>
    void UpdateCapacitySellers((List<S_ActiveCapacityProperties> activeCapacity, List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct> passiveCapacity) p_soldCapacities )
    {
        if (p_soldCapacities.activeCapacity.Count + p_soldCapacities.passiveCapacity.Count > _capacitySellers.Count)
        {
            Debug.LogError(
                $"ERROR ! You have gave more capacities' attributes than CapacitySellers number : " +
                $"{p_soldCapacities.activeCapacity.Count + p_soldCapacities.passiveCapacity.Count} > {_capacitySellers.Count}. The sellers were not updated."
            );

            return;
        }

        int numberOfCapacitySellerUpdated = 0;

        for (int i = p_soldCapacities.activeCapacity.Count - 1; i >= 0; i--)
        {
            _capacitySellers[numberOfCapacitySellerUpdated].UpdateSoldCapacity(p_soldCapacities.activeCapacity[i]);
            numberOfCapacitySellerUpdated++;
        }

        for (int i = p_soldCapacities.passiveCapacity.Count - 1; i >= 0; i--)
        {
            _capacitySellers[numberOfCapacitySellerUpdated].UpdateSoldCapacity(p_soldCapacities.passiveCapacity[i]);
            numberOfCapacitySellerUpdated++;
        }
    }
    #endregion

    #region In upgrader menu methods

    int GetRepairCost()
    {
        return (int)(_baseRepairCost + ((_repairUseCostFactor * _repairUseNumber) + (_playerMaxHealthPoints * _healthPointsHealFactorPerRepair * _costPerHealthPointsHealed)));
    }

    public void OnRepairPlayer()
    {
        int repairCost = GetRepairCost();

        // Not enought nanomachines
        if (_playerAttributes._CollectedNanomachines < repairCost) 
            return;

        // Applying the cost
        _playerAttributes._CollectedNanomachines -= repairCost;
        _repairUseNumber++;

        // Healing the player
        _playerAttributes._HealthPoints += (int)(_playerAttributes._MaxHealthPoints * _healthPointsHealFactorPerRepair);

        // Update the UIs
        UpdateRepairCostText();
        UpdateRemainingNanomachineTextUI(_playerAttributes._CollectedNanomachines);
    }

    public void UpdateRepairButton(Button p_repairButton)
    {
        bool carRepair = true;

        if (_playerAttributes._CollectedNanomachines < GetRepairCost() || _playerAttributes._HealthPoints >= _playerAttributes._MaxHealthPoints)
        {
            carRepair = false;

            // Security
            if (p_repairButton == _firstButtonSelected)
            {
                Debug.LogError(
                    $"ERROR ! Beware the repair button has been set as not interactable, that means this button cannot be selected. \n" +
                    "This is an error because a player who is using a controller, will not be able a select new buttons in the upgrader menu, " +
                    "which will result to the player being stuck in the menu. \n" +
                    $"You should set '{nameof(_firstButtonSelected)}' to another button.\n"
                );
            }

            _firstButtonSelected.Select();
        }
            
        p_repairButton.interactable = carRepair;
    }

    void UpdateRepairCostText()
    {
        _repairCostText.text = $"Cost : {GetRepairCost()}";
    }

    void UpdateRemainingNanomachineTextUI(int p_newValue)
    {
        _remainingCollectedNanomachinesText.text = $"Remaining Nanomachines : \n{p_newValue}";
    }

    void UpdateHealthBar(int p_newHealthPoints, int p_newMaxHealthPoints, int p_newPreviewValue, int p_newPreviewMaxValue)
    {
        _repairHealthBar.UpdateBar(p_newHealthPoints, p_newMaxHealthPoints, p_newPreviewValue, p_newPreviewMaxValue);
    }
    #endregion
}