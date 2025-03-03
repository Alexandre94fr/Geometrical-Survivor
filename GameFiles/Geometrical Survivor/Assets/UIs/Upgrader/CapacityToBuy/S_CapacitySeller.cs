using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_CapacitySeller : MonoBehaviour
{
    [Header(" Internal references :")]
    [SerializeField] TextMeshProUGUI _capacityNameText;
    [SerializeField] Image _capacityIconImage;
    [SerializeField] TextMeshProUGUI _capacityPropertiesText;
    [SerializeField] TextMeshProUGUI _capacityPriceText;
    [SerializeField] Button _capacityBuyButton;

    int _capacityPrice;

    // Selling sold capacity
    bool _isSoldCapacityActive;

    S_ActiveCapacityProperties _soldActiveCapacityProperties;
    S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct _soldPassiveCapacityPropertiesStruct;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_capacityNameText, nameof(_capacityNameText)),
            (_capacityIconImage, nameof(_capacityIconImage)),
            (_capacityPropertiesText, nameof(_capacityPropertiesText)),
            (_capacityPriceText, nameof(_capacityPriceText)),
            (_capacityBuyButton, nameof(_capacityBuyButton))
        )) return;
    }

    public void UpdateSoldCapacity(S_ActiveCapacityProperties p_activeCapacityProperties, int p_nanomachineCollectedByPlayer)
    {
        // Changing UI's values to the given active capacity
        _capacityNameText.text = p_activeCapacityProperties._ActiveCapacityProperties._Name;
        _capacityIconImage.sprite = p_activeCapacityProperties._ActiveCapacityProperties._Sprite;

        _capacityPropertiesText.text = GetUsefulProperties(p_activeCapacityProperties._ActiveCapacityProperties, "_Price");

        _capacityPrice = p_activeCapacityProperties._ActiveCapacityProperties._Price;

        _capacityPriceText.text = $"Price : {_capacityPrice}";

        // Handling buy button interactability
        _capacityBuyButton.interactable = true;

        if (_capacityPrice > p_nanomachineCollectedByPlayer)
            _capacityBuyButton.interactable = false;

        // Saving which type of capacity we sells
        _soldActiveCapacityProperties = p_activeCapacityProperties;
        _isSoldCapacityActive = true;
    }

    public void UpdateSoldCapacity(S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct p_passiveCapacityPropertiesStruct, int p_nanomachineCollectedByPlayer)
    {
        // Changing UI's values to the given passive capacity
        _capacityNameText.text = p_passiveCapacityPropertiesStruct._Name;
        _capacityIconImage.sprite = p_passiveCapacityPropertiesStruct._Sprite;

        S_PassiveCapacityProperties.GamePropertiesStruct atNextLevelPassiveCapacityPropertiesStruct =
            p_passiveCapacityPropertiesStruct._UpgradesPerLevels[p_passiveCapacityPropertiesStruct._CurrentLevel + 1];

        _capacityPropertiesText.text = GetUsefulProperties(atNextLevelPassiveCapacityPropertiesStruct, "_Price");

        _capacityPrice = atNextLevelPassiveCapacityPropertiesStruct._Price;

        _capacityPriceText.text = $"Price : {_capacityPrice}";

        // Handling buy button interactability
        _capacityBuyButton.interactable = true;

        if (_capacityPrice > p_nanomachineCollectedByPlayer)
            _capacityBuyButton.interactable = false;

        // Saving which type of capacity we sells
        _soldPassiveCapacityPropertiesStruct = p_passiveCapacityPropertiesStruct;
        _isSoldCapacityActive = false;
    }


    /// <returns> A string with all capacity properties that are not equal to 0, wrote in this way : - AttributeName : AttributeValue </returns>
    string GetUsefulProperties<T>(T p_properties, string p_ignoredPropertyName)
    {
        List<string> propertiesList = new();

        foreach (FieldInfo field in typeof(T).GetFields())
        {
            object value = field.GetValue(p_properties);

            if (field.Name == p_ignoredPropertyName)
                continue;

            // If the field.Name has an _ for first character then we delete it, otherwise to name is untouched
            string fieldName = field.Name.StartsWith("_") ? field.Name[1..] : field.Name;

            if (value is int intValue && intValue != 0)
                propertiesList.Add($"{fieldName} : {intValue}");

            else if (value is float floatValue && Mathf.Abs(floatValue) > 0.0001f)
                propertiesList.Add($"{fieldName} : {floatValue:F2}"); // F2 convert a float like : 1.598874 into 1.59

            // You can add more type detection if you want
        }

        return "- " + string.Join("\n- ", propertiesList);
    }

    public void OnTryBuyCapacity()
    {
        if (_isSoldCapacityActive)
            S_UpgraderMenu._OnTryBuyActiveCapacityEvent?.Invoke(_soldActiveCapacityProperties, this);
        else
            S_UpgraderMenu._OnTryBuyPassiveCapacityEvent?.Invoke(_soldPassiveCapacityPropertiesStruct, this);
    }

    public void UpdateBuyButtonInteractability(int p_nanomachineCollectedByPlayer)
    {
        if (_capacityBuyButton.interactable == false)
            return;

        _capacityBuyButton.interactable = true;

        if (_capacityPrice > p_nanomachineCollectedByPlayer)
            _capacityBuyButton.interactable = false;
    }

    public void OnCapacityBought()
    {
        _capacityBuyButton.interactable = false;
    }
}