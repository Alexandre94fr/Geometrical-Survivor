using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_RepairHealthBar : MonoBehaviour
{
    [Serializable]
    struct RepairHealthBarComponentValues
    {
        [Header(" Bar colors :")]
        public Color _FileBarColor; // (0.75f, 0.75f, 0.75f, 1)
        public Color _PreviewFileBarColor; // (0.9, 0.9, 0.9, 1)
        public Color _FileBarBackgroundColor; // (0.35f, 0.35f, 0.35f, 1)

        [Space]
        public Color _BackgroundColor; // (0.65f, 0.65f, 0.65f, 1)

        [Header(" Bar sprites :")]
        public Sprite _BackgroundBarIconSprite;

        [Space]
        public Sprite _FileBarSprite;
        public Sprite _FileBarBackgroundSprite;
        public Sprite _PreviewFileBarSprite;

        [Space]
        public Sprite _BackgroundSprite;

        public RepairHealthBarComponentValues(Color p_fileBarColor, Color p_previewFileBarColor, Color p_fileBarBackgroundColor, Color p_backgroundColor)
        {
            _FileBarColor = p_fileBarColor;
            _PreviewFileBarColor = p_previewFileBarColor;
            _FileBarBackgroundColor = p_fileBarBackgroundColor;

            _BackgroundColor = p_backgroundColor;

            _BackgroundBarIconSprite = null;

            _FileBarSprite = null;
            _FileBarBackgroundSprite = null;
            _PreviewFileBarSprite = null;

            _BackgroundSprite = null;
        }
    }

    [Header(" Internal references :")]
    [SerializeField] TextMeshProUGUI _fileBarNumbers;

    [Space]
    [SerializeField] Image _fileBarImage;
    [SerializeField] Image _previewFileBarImage;
    [SerializeField] Image _fileBarBackgroundImage;

    [Space]
    [SerializeField] Image _backgroundImage;

    [Header(" Properties :")]
    [SerializeField] RepairHealthBarComponentValues _barComponentValues = new(
        new(0.75f, 0.75f, 0.75f, 1),
        new(0.90f, 0.90f, 0.90f, 1),
        new(0.35f, 0.35f, 0.35f, 1),

        new(0.65f, 0.65f, 0.65f, 1)
    );

#if UNITY_EDITOR

    void OnValidate()
    {
        // Security
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_fileBarNumbers, nameof(_fileBarNumbers)),

            (_barComponentValues._FileBarSprite, nameof(_barComponentValues._FileBarSprite)),

            (_fileBarImage, nameof(_fileBarImage)),
            (_previewFileBarImage, nameof(_previewFileBarImage)),
            (_fileBarBackgroundImage, nameof(_fileBarBackgroundImage)),

            (_backgroundImage, nameof(_backgroundImage))
        )) return;

        // Updating colors
        _fileBarImage.color = _barComponentValues._FileBarColor;
        _previewFileBarImage.color = _barComponentValues._PreviewFileBarColor;
        _fileBarBackgroundImage.color = _barComponentValues._FileBarBackgroundColor;

        _backgroundImage.color = _barComponentValues._BackgroundColor;

        // Updating sprites
        _fileBarImage.sprite = _barComponentValues._FileBarSprite;
        _previewFileBarImage.sprite = _barComponentValues._PreviewFileBarSprite;
        _fileBarBackgroundImage.sprite = _barComponentValues._FileBarBackgroundSprite;

        _backgroundImage.sprite = _barComponentValues._BackgroundSprite;
    }
#endif

    public void UpdateBar(float p_newValue, float p_newMaxValue, float p_newPreviewValue, float p_newPreviewMaxValue)
    {
        _fileBarImage.fillAmount = p_newValue / p_newMaxValue;
        _previewFileBarImage.fillAmount = (p_newValue + p_newPreviewValue) / p_newPreviewMaxValue;

        _fileBarNumbers.text = $"{p_newValue} / {p_newMaxValue}";
    }
}