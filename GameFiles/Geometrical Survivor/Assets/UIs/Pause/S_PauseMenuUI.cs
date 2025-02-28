using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class S_PauseMenuUI : MonoBehaviour
{
    public static Action _OnPlayerPauseEvent;

    [Header(" Internal references :")]
    [SerializeField] GameObject _pauseMenuUIGameObject;
    [SerializeField] Button _firstButtonSelected;

    bool _isPlayerPauseMenuVisible;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_pauseMenuUIGameObject, nameof(_pauseMenuUIGameObject)),
            (_firstButtonSelected, nameof(_firstButtonSelected))
        )) return;

        S_DeathMenu._OnPlayerDeathMenuVisibiltyChangeEvent += UpdatePlayerPauseMenuVisibility;
        _OnPlayerPauseEvent += ChangePlayerPauseMenuVisibility;
    }

    void OnDestroy()
    {
        S_DeathMenu._OnPlayerDeathMenuVisibiltyChangeEvent -= UpdatePlayerPauseMenuVisibility;
        _OnPlayerPauseEvent -= ChangePlayerPauseMenuVisibility;
    }

    void UpdatePlayerPauseMenuVisibility(bool p_newVisibility)
    {
        _isPlayerPauseMenuVisible = p_newVisibility;
    }

    public void ChangePlayerPauseMenuVisibility()
    {
        if (!_pauseMenuUIGameObject.activeSelf && !_isPlayerPauseMenuVisible)
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