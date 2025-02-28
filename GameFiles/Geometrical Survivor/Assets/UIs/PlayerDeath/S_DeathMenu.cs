using UnityEngine;

public class S_DeathMenu : MonoBehaviour
{
    [Header(" Internal references :")]
    [SerializeField] GameObject _deathMenuGameObject;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_deathMenuGameObject, nameof(_deathMenuGameObject))
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
        Time.timeScale = 0;

        _deathMenuGameObject.SetActive(true);
    }
}