using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_EnemyHealthBar : MonoBehaviour
{
    [Header(" External references :")]
    [SerializeField] S_Enemy _enemy;

    [Header(" Internal references :")]
    [SerializeField] GameObject _healthBarBackground;
    [SerializeField] Image _fileBarImage;
    [SerializeField] TextMeshProUGUI _fileBarNumbers;

    int _currentHealthPoints;
    int _currentMaxHealthPoints;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_enemy, nameof(_enemy)),

            (_healthBarBackground, nameof(_healthBarBackground)),
            (_fileBarImage, nameof(_fileBarImage)),
            (_fileBarNumbers, nameof(_fileBarNumbers))
        )) return;

        S_EnemyAttributes._OnHealthPointsUpdateEvent += OnHealthPointsUpdate;
        S_EnemyAttributes._OnMaxHealthPointsUpdateEvent += OnMaxHealthPointsUpdate;
    }

    void OnHealthPointsUpdate(S_Enemy p_enemy, int p_healthPoints)
    {
        if (p_enemy != _enemy)
            return;

        _currentHealthPoints = p_healthPoints;

        UpdateBar(_currentHealthPoints, _currentMaxHealthPoints);
    }

    void OnMaxHealthPointsUpdate(S_Enemy p_enemy, int p_maxHealthPoints)
    {
        if (p_enemy != _enemy)
            return;

        _currentMaxHealthPoints = p_maxHealthPoints;

        UpdateBar(_currentHealthPoints, _currentMaxHealthPoints);
    }

    void UpdateBar(int p_newValue, int p_newMaxValue)
    {
        // NOTE : We cast p_newMaxValue into a float because without it the result of the operation will be an int.
        //        Getting an int is a problem because _fileBarImage.fillAmount is a float beetween 0 and 1. 

        float fillAmount = p_newValue / (float)p_newMaxValue;

        if (fillAmount >= 1)
            _healthBarBackground.SetActive(false);
        else
            _healthBarBackground.SetActive(true);

        _fileBarImage.fillAmount = fillAmount;
        _fileBarNumbers.text = $"{p_newValue} / {p_newMaxValue}";
    }
}