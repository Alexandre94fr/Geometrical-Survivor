using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_NanomachinesManager : MonoBehaviour
{
    public static S_NanomachinesManager _Instance;

    [Header(" Properties :")]
    [SerializeField] GameObject _nanomachinePrefab;

    Transform _transform;

    void Awake()
    {
        _Instance = S_Instantiator.ReturnInstance(this, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_nanomachinePrefab, nameof(_nanomachinePrefab))
        )) return;
    }

    public void InstantiateNanomachineObject(Vector3 p_position, int p_nanomachineValue, Transform p_nanomachineObjectParent = null)
    {
        if (p_nanomachineObjectParent == null)
            p_nanomachineObjectParent = _transform;

        GameObject nanomachineGameObject = Instantiate(_nanomachinePrefab, p_position, Quaternion.identity, p_nanomachineObjectParent);

        S_Nanomachine nanomachineComponent = nanomachineGameObject.GetComponent<S_Nanomachine>();
        
        nanomachineComponent._NanomachineAmount = p_nanomachineValue;
    }
}