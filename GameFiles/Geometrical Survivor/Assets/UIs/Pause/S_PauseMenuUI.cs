using System;
using UnityEngine;
using UnityEngine.UI;

public class S_PauseMenuUI : MonoBehaviour
{
    public static Action _OnPlayerPauseEvent;
    public static Action<bool> _OnCanPauseMenuBeShowedEvent;

    [Header(" Internal references :")]
    [SerializeField] GameObject _pauseMenuUIGameObject;
    [SerializeField] Button _firstButtonSelected;

    bool _canPauseMenuBeShowed;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_pauseMenuUIGameObject, nameof(_pauseMenuUIGameObject)),
            (_firstButtonSelected, nameof(_firstButtonSelected))
        )) return;

        _OnPlayerPauseEvent += ChangePlayerPauseMenuVisibility;
        _OnCanPauseMenuBeShowedEvent += UpdateCanPauseMenuBeShowed;
    }

    void OnDestroy()
    {
        _OnPlayerPauseEvent -= ChangePlayerPauseMenuVisibility;
    }

    void UpdateCanPauseMenuBeShowed(bool p_canPauseMenuBeShowed)
    {
        _canPauseMenuBeShowed = p_canPauseMenuBeShowed;
    }

    public void ChangePlayerPauseMenuVisibility()
    {
        if (!_pauseMenuUIGameObject.activeSelf && _canPauseMenuBeShowed)
        {
            _pauseMenuUIGameObject.SetActive(true);
            Time.timeScale = 0f;

            _firstButtonSelected.Select();
        }
        else
        {
            _pauseMenuUIGameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}