using System;
using UnityEngine;

public class S_DeathMenu : MonoBehaviour
{
    public static event Action<bool> _OnPlayerDeathMenuVisibiltyChangeEvent;

    [Header(" Internal references :")]
    [SerializeField] GameObject _deathMenuGameObject;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_deathMenuGameObject, nameof(_deathMenuGameObject))
        )) return;

        S_PlayerAttributes._OnHealthPointsUpdateEvent += CheckPlayerDeath;

        _deathMenuGameObject.SetActive(false);

        _OnPlayerDeathMenuVisibiltyChangeEvent?.Invoke(false);
    }

    private void OnDestroy()
    {
        S_PlayerAttributes._OnHealthPointsUpdateEvent -= CheckPlayerDeath;
    }

    void CheckPlayerDeath(int p_playerHealth)
    {
        if (p_playerHealth <= 0)
            ShowDeathMenu();
    }

    void ShowDeathMenu()
    {
        _OnPlayerDeathMenuVisibiltyChangeEvent?.Invoke(true);

        Time.timeScale = 0;

        _deathMenuGameObject.SetActive(true);
    }
}