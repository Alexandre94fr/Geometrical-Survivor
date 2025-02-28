using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_CollectedNanomachineUI : MonoBehaviour
{
    [Header(" Internal references :")]
    [SerializeField] TextMeshProUGUI _collectedNanomachineText;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_collectedNanomachineText, nameof(_collectedNanomachineText))
        )) return;

        S_PlayerAttributes._OnCollectedNanomachinesUpdateEvent += UpdateCollectedNanomachines;
    }

    void OnDestroy()
    {
        S_PlayerAttributes._OnCollectedNanomachinesUpdateEvent -= UpdateCollectedNanomachines;
    }

    void UpdateCollectedNanomachines(int p_newCollectedNanomachines)
    {
        _collectedNanomachineText.text = p_newCollectedNanomachines.ToString();
    }
}