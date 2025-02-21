using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_Bar : MonoBehaviour
{
    [Serializable]
    struct BarComponentValues
    {
        [Header(" Bar colors :")]
        public Color _FileBarColor; // (1, 1, 1, 1)
        public Color _FileBarBackgroundColor; // (0.35f, 0.35f, 0.35f, 1)
        public Color _BackgroundColor; // (0.65f, 0.65f, 0.65f, 1)

        [Header(" Bar sprites :")]
        public Sprite _BarIconSprite;
        public Sprite _BackgroundBarIconSprite;

        [Space]
        public Sprite _FileBarSprite;
        public Sprite _FileBarBackgroundSprite;
        public Sprite _BackgroundSprite;

        public BarComponentValues(Color p_backgroundColor, Color p_fileBarBackgroundColor, Color p_fileBarColor)
        {
            _FileBarColor = p_fileBarColor;
            _FileBarBackgroundColor = p_fileBarBackgroundColor;
            _BackgroundColor = p_backgroundColor;

            _BarIconSprite = null;
            _BackgroundBarIconSprite = null;

            _FileBarSprite = null;
            _FileBarBackgroundSprite = null;
            _BackgroundSprite = null;
        }
    }

    [Header(" Internal references :")]
    [SerializeField] TextMeshProUGUI _fileBarNumbers;

    [Space]
    [SerializeField] Image _fileBarImage;
    [SerializeField] Image _fileBarBackgroundImage;
    [SerializeField] Image _backgroundImage;

    [Space]
    [SerializeField] Image _barIconImage;
    [SerializeField] Image _barIconBackgroundImage;

    [Header(" Properties :")]
    [SerializeField] S_BarHandler.BarTypes _barType;

    [Space]
    [SerializeField] BarComponentValues _barComponentValues = new(
        new(0.65f, 0.65f, 0.65f, 1),
        new(0.35f, 0.35f, 0.35f, 1),
        new(0.85f, 0.85f, 0.85f, 1)
    );

    void Start()
    {
        S_BarHandler._OnBarValueUpdateEvent += UpdateBar;
    }

    void OnDestroy()
    {
        S_BarHandler._OnBarValueUpdateEvent -= UpdateBar;
    }

    void OnValidate()
    {
        // Security
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_fileBarNumbers, nameof(_fileBarNumbers)),

            (_fileBarImage, nameof(_fileBarImage)),
            (_fileBarBackgroundImage, nameof(_fileBarBackgroundImage)),
            (_backgroundImage, nameof(_backgroundImage)),

            (_barIconImage, nameof(_barIconImage)),
            (_barIconBackgroundImage, nameof(_barIconBackgroundImage))
        )) return;

        // Updating colors
        _fileBarImage.color = _barComponentValues._FileBarColor;
        _fileBarBackgroundImage.color = _barComponentValues._FileBarBackgroundColor;
        _backgroundImage.color = _barComponentValues._BackgroundColor;

        // Updating sprites
        _fileBarImage.sprite = _barComponentValues._FileBarSprite;
        _fileBarBackgroundImage.sprite = _barComponentValues._FileBarBackgroundSprite;
        _backgroundImage.sprite = _barComponentValues._BackgroundSprite;

        _barIconImage.sprite = _barComponentValues._BarIconSprite;
        _barIconBackgroundImage.sprite = _barComponentValues._BackgroundBarIconSprite;
    }


    void UpdateBar(S_BarHandler.BarTypes p_barType, float p_newValue, float p_newMaxValue)
    {
        if (p_barType != _barType)
            return;

        _fileBarImage.fillAmount = p_newValue / p_newMaxValue;
        _fileBarNumbers.text = $"{p_newValue} / {p_newMaxValue}";
    }
}