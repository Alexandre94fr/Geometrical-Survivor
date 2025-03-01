using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_CapacitySeller : MonoBehaviour
{
    [Header(" Internal references :")]
    [SerializeField] TextMeshProUGUI _CapacityNameText;
    [SerializeField] Image _CapacityIconImage;
    [SerializeField] TextMeshProUGUI _CapacityPropertiesText;
    [SerializeField] TextMeshProUGUI _CapacityPriceText;
    [SerializeField] Button _CapacityBuyButton;


    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_CapacityNameText, nameof(_CapacityNameText)),
            (_CapacityIconImage, nameof(_CapacityIconImage)),
            (_CapacityPropertiesText, nameof(_CapacityPropertiesText)),
            (_CapacityPriceText, nameof(_CapacityPriceText)),
            (_CapacityBuyButton, nameof(_CapacityBuyButton))
        )) return;
    }

    public void UpdateSoldCapacity(S_ActiveCapacityProperties p_activeCapacityProperties)
    {
        _CapacityNameText.text = p_activeCapacityProperties._ActiveCapacityProperties._Name;
        _CapacityIconImage.sprite = p_activeCapacityProperties._ActiveCapacityProperties._Sprite;

        _CapacityPropertiesText.text = GetUsefulProperties(p_activeCapacityProperties._ActiveCapacityProperties, "_Price");

        _CapacityPriceText.text = $"Price : {p_activeCapacityProperties._ActiveCapacityProperties._Price}";
    }

    public void UpdateSoldCapacity(S_PassiveCapacityProperties.PassiveCapacityPropertiesStruct p_passiveCapacityPropertiesStruct)
    {
        _CapacityNameText.text = p_passiveCapacityPropertiesStruct._Name;
        _CapacityIconImage.sprite = p_passiveCapacityPropertiesStruct._Sprite;

        S_PassiveCapacityProperties.GamePropertiesStruct atNextLevelPassiveCapacityPropertiesStruct =
            p_passiveCapacityPropertiesStruct._UpgradesPerLevels[p_passiveCapacityPropertiesStruct._CurrentLevel + 1];

        _CapacityPropertiesText.text = GetUsefulProperties(atNextLevelPassiveCapacityPropertiesStruct, "_Price");

        _CapacityPriceText.text = $"Price : {atNextLevelPassiveCapacityPropertiesStruct._Price}";
    }


    /// <returns> A string with all capacity properties that are not equal to 0, wrote in this way : AttributeName : AttributeValue
    /// <para> <b> Beware ! </b> The '_Price' name attribute will never be returned. </para> </returns>
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
}