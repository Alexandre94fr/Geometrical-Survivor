using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_UpgraderMenu : MonoBehaviour
{
    [Header(" Properties :")]
    [SerializeField] [Range(0, 1)] float _activeCapacityApparitionFactor = 0.25f;

    [Header(" Internal references :")]
    [SerializeField] GameObject _upgraderMenuUIGameObject;
    [SerializeField] Button _firstButtonSelected;

    [Space]
    [SerializeField] List<S_CapacitySeller> _capacitySellers;

    [Space]
    [SerializeField] TextMeshProUGUI _repairCostText;

    [Space]
    [SerializeField] TextMeshProUGUI _remainingCollectedNanomachines;

    [Header(" External references :")]
    [SerializeField] List<S_ActiveCapacityProperties> _activeCapacityProperties;
    [SerializeField] List<S_PassiveCapacityProperties> _passiveCapacityProperties;

    // Player properties
    int _playerCurrentNanomachine;
    int _playerLevel;

    S_ActiveCapacityProperties _playerActiveCapacityProperties;
    List<S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct> _playerPassiveCapacityPropertiesStruct;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_firstButtonSelected, nameof(_firstButtonSelected)),
            (_upgraderMenuUIGameObject, nameof(_upgraderMenuUIGameObject)),
            (_capacitySellers, nameof(_capacitySellers)),
            (_repairCostText, nameof(_repairCostText)),
            (_remainingCollectedNanomachines, nameof(_remainingCollectedNanomachines)),

            (_activeCapacityProperties, nameof(_activeCapacityProperties)),
            (_passiveCapacityProperties, nameof(_passiveCapacityProperties))
        )) return;

        S_PlayerAttributes._OnCollectedNanomachinesUpdateEvent += OnPlayerCurrentNanomachineUpdate;
        S_PlayerAttributes._OnTechnologicalLevelUpdateEvent += OnPlayerLevelUpdate;

        S_PlayerAttributes._OnEquippedActiveCapacityUpdateEvent += OnPlayerActiveCapacityPropertiesUpdate;
        S_PlayerAttributes._OnEquippedPassiveCapacityUpdateEvent += OnPlayerPassiveCapacityPropertiesStructUpdate;
    }

    #region Player attributes update methods

    void OnPlayerCurrentNanomachineUpdate(int p_newCurrentNanomachine)
    {
        _playerCurrentNanomachine = p_newCurrentNanomachine;
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

        UpdateCapacitySellers(GetRandomCapacities(_capacitySellers.Count));
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
            // 50 % chance to pick a active capacity
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
}