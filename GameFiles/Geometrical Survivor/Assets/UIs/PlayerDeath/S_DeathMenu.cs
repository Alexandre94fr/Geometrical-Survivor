using System;
using UnityEngine;
using UnityEngine.UI;

public class S_DeathMenu : MonoBehaviour
{
    [Header(" Internal references :")]
    [SerializeField] GameObject _deathMenuGameObject;
    [SerializeField] Button _firstButtonSelected;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_deathMenuGameObject, nameof(_deathMenuGameObject)),
            (_firstButtonSelected, nameof(_firstButtonSelected))
        )) return;

        S_PlayerAttributes._OnHealthPointsUpdateEvent += CheckPlayerDeath;

        _deathMenuGameObject.SetActive(false);
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
        S_PauseMenuUI._OnCanPauseMenuBeShowedEvent?.Invoke(false);

        Time.timeScale = 0;

        _deathMenuGameObject.SetActive(true);

        _firstButtonSelected.Select();
    }
}