using UnityEngine;

public class S_Player : MonoBehaviour
{
    public static S_Player _Instance;

    [Header(" Internal references :")]
    public S_PlayerAttributes _PlayerAttributes;

    void Awake()
    {
        _Instance = S_Instantiator.ReturnInstance(this, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOneParent);
    }

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_PlayerAttributes, nameof(_PlayerAttributes))
        )) return;
    }
}