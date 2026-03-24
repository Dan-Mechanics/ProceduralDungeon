using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    public class NumberField : Field
    {
        private const int MAX_STRING_LENGTH = 25;

        [SerializeField] private Slider slider = default;
        [SerializeField] private float min = default;
        [SerializeField] private float max = default;
        [SerializeField] private bool wholeNumbers = default;
        [SerializeField] private InputField inputField = default;
        [SerializeField] private Text text = default;
        [SerializeField] private Text placeholder = default;

        public override void Setup(Blackboard blackboard)
        {
            base.Setup(blackboard);
            slider.wholeNumbers = wholeNumbers;
            slider.minValue = min;
            slider.maxValue = max;

            slider.onValueChanged.AddListener(SendToBlackboard);
            inputField.onEndEdit.AddListener(OnEndEdit);

            text.text = Key.ToUpperInvariant();
            placeholder.text = $"{(slider.wholeNumbers ? "int" : "float")}: {slider.minValue}_{slider.maxValue}";
        }

        private void OnEndEdit(string str)
        {
            if (!Utils.IsStringValid(str) || str.Length > MAX_STRING_LENGTH)
            {
                GetFromBlackboard();
                return;
            }

            str = str.Replace(',', '.');
            if (!float.TryParse(str, out float value))
            {
                GetFromBlackboard();
                return;
            }

            SendToBlackboard(value);
        }

        private void SendToBlackboard(float value)
        {
            value = Mathf.Clamp(value, min, max);

            inputField.text = value.ToString();
            slider.value = value;

            if (wholeNumbers)
            {
                blackboard.SetValue(Key, (int)value);
            }
            else
            {
                blackboard.SetValue(Key, value);
            }
        }

        public override void GetFromBlackboard()
        {
            if (wholeNumbers)
            {
                slider.value = blackboard.GetValue<int>(Key);
            }
            else
            {
                slider.value = blackboard.GetValue<float>(Key);
            }

            inputField.text = slider.value.ToString();
            SendToBlackboard(slider.value);
        }

        private void OnValidate()
        {
            if (!wholeNumbers)
                return;

            min = Mathf.Round(min);
            max = Mathf.Round(max);
        }
    }
}