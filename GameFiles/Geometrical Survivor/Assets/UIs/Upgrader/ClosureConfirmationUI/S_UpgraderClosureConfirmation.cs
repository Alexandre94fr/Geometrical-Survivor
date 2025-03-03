using UnityEngine;
using UnityEngine.UI;

public class S_UpgraderClosureConfirmation : MonoBehaviour
{
    [Header(" Internal references :")]
    public GameObject _ClosureConfirmationUIGameObject;

    [Space]
    public Button _CancelButton;
    public Button _ExitButton;

    [Header(" External references :")]
    [SerializeField] Button _firstButtonSelectedWhenClosed;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_ClosureConfirmationUIGameObject, nameof(_ClosureConfirmationUIGameObject)),

            (_CancelButton, nameof(_CancelButton)),
            (_ExitButton, nameof(_ExitButton)),

            (_firstButtonSelectedWhenClosed, nameof(_firstButtonSelectedWhenClosed))
        )) return;
    }

    public void OpenCloserConfirmationUI()
    {
        _ClosureConfirmationUIGameObject.SetActive(true);

        _CancelButton.Select();
    }

    public void CloseCloserConfirmationUI()
    {
        _ClosureConfirmationUIGameObject.SetActive(false);

        _firstButtonSelectedWhenClosed.Select();
    }
}