using System;
using UnityEngine;

public class S_PauseMenuUI : MonoBehaviour
{
    public static Action _OnPlayerPauseEvent;

    [Header(" Internal references :")]
    [SerializeField] GameObject _pauseMenuUIGameObject;

    bool _isPlayerPauseMenuVisible;

    void Start()
    {
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
        }
        else
        {
            _pauseMenuUIGameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}