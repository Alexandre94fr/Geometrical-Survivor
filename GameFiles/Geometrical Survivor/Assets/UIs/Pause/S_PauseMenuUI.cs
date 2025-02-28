using System;
using UnityEngine;

public class S_PauseMenuUI : MonoBehaviour
{
    public static Action _OnPlayerPauseEvent;

    [Header(" Internal references :")]
    [SerializeField] GameObject _pauseMenuUIGameObject;

    void Start()
    {
        _OnPlayerPauseEvent += ChangePlayerPauseMenuVisibility;
    }

    void OnDestroy()
    {
        _OnPlayerPauseEvent -= ChangePlayerPauseMenuVisibility;
    }

    public void ChangePlayerPauseMenuVisibility()
    {
        if (!_pauseMenuUIGameObject.activeSelf)
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