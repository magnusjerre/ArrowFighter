using Jerre.Utils;
using System;
using UnityEngine;

namespace Jerre.GameSettings
{
    public class KeyValueWithOptionsSetting
    {
        public string Name;
        public string DisplayName;
        public string Value;
        public string DefaultValue;

        public string[] PossibleValues;

        private int indexOfSelectedValue;

        public KeyValueWithOptionsSetting(string name, string displayName, string value, string defaultValue, string[] possibleValues)
        {
            Name = name;
            DisplayName = displayName;
            Value = value;
            DefaultValue = defaultValue;
            PossibleValues = possibleValues;

            for (var i = 0; i < possibleValues.Length; i++)
            {
                if (possibleValues[i] == value)
                {
                    indexOfSelectedValue = i;
                    break;
                }
            }
        }

        public static KeyValueWithOptionsSetting NumberedValue(string name, string displayName, int defaultValue, int min, int max, int step)
        {

            var stepsAboveDefaultValue = (max - defaultValue) / step;
            var stepAboveArray = new string[stepsAboveDefaultValue];
            var currentValue = defaultValue;
            for (var i = 0; i < stepAboveArray.Length; i++)
            {
                currentValue += step;
                stepAboveArray[i] = currentValue.ToString();
            }

            var stepsBelowDefaultValue = (defaultValue - min) / step;
            var stepBelowArray = new string[stepsBelowDefaultValue];
            currentValue = defaultValue;
            for (var i = stepBelowArray.Length - 1; i >= 0; i--)
            {
                currentValue -= step;
                stepBelowArray[i] = currentValue.ToString();
            }

            var totalNumberOfSteps = stepsAboveDefaultValue + stepsBelowDefaultValue + 1;
            var allPossibleValues = new string[totalNumberOfSteps];
            Array.Copy(stepBelowArray, allPossibleValues, stepBelowArray.Length);
            allPossibleValues[stepBelowArray.Length] = defaultValue.ToString();
            Array.Copy(stepAboveArray, 0, allPossibleValues, stepBelowArray.Length + 1, stepAboveArray.Length);

            Debug.Log("name: " + name + ", defaultValue: " + defaultValue + ", possibleValues: " + CollectionUtils.Stringify(allPossibleValues));
            return new KeyValueWithOptionsSetting(name, displayName, defaultValue.ToString(), defaultValue.ToString(), allPossibleValues);
        }

        public int GetIndexOfSelectedValue()
        {
            return indexOfSelectedValue;
        }

        public void SetNextValue()
        {
            indexOfSelectedValue = (indexOfSelectedValue + 1) % PossibleValues.Length;
            Value = PossibleValues[indexOfSelectedValue];
        }

        public void SetPreviousValue()
        {
            indexOfSelectedValue = indexOfSelectedValue - 1;
            if (indexOfSelectedValue < 0)
            {
                indexOfSelectedValue += PossibleValues.Length;
            }
            Value = PossibleValues[indexOfSelectedValue];
        }
    }
}
