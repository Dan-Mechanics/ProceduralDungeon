using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    public class NumberField : Field
    {
        private const int MAX_STRING_LENGTH = 25;

        [SerializeField] private Slider slider = default;
        [SerializeField] private InputField inputField = default;
        [SerializeField] private Text text = default;
        [SerializeField] private Text placeholder = default;

        public override void Setup(Blackboard blackboard)
        {
            base.Setup(blackboard);
            slider.onValueChanged.AddListener(SendToBlackboard);
            inputField.onEndEdit.AddListener(OnEdit);

            text.text = key;
            placeholder.text = $"{(slider.wholeNumbers ? "int" : "float")}: {slider.minValue}_{slider.maxValue}";
        }

        private void SendToBlackboard(float value)
        {
            inputField.text = value.ToString();
            if (slider.wholeNumbers)
            {
                blackboard.SetValue(key, (int)value);
            }
            else
            {
                blackboard.SetValue(key, value);
            }
        }

        private void OnEdit(string str)
        {
            if (!Utils.IsStringValid(str) || str.Length > MAX_STRING_LENGTH)
                return;

            str = str.Replace(',', '.');
            if (!float.TryParse(str, out float value))
            {
                SendToBlackboard(slider.value);
                return;
            }

            slider.value = value;
        }

        public override void GetFromBlackboard()
        {
            if (slider.wholeNumbers)
            {
                slider.value = blackboard.GetValue<int>(key);
            }
            else
            {
                slider.value = blackboard.GetValue<float>(key);
            }
        }
    }
}
