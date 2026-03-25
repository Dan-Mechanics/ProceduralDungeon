using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    public class StringField : Field
    {
        private const int MAX_STRING_LENGTH = 25;

        [SerializeField] private InputField inputField = default;
        [SerializeField] private Text placeholder = default;
        [SerializeField] private Text text = default;
        [SerializeField] private List<char> bannedCharacters = default;

        public override void Setup(Blackboard blackboard)
        {
            base.Setup(blackboard);
            inputField.onEndEdit.AddListener(SendToBlackboard);
            placeholder.text = Key;
            UpdateText();
        }

        private void UpdateText()
        {
            if (text != null)
                text.text = Key;
        }

        private void SendToBlackboard(string str)
        {
            bannedCharacters.ForEach(x => str.Replace(x.ToString(), string.Empty));
            if (Utils.IsStringValid(str) && str.Length <= MAX_STRING_LENGTH)
            {
                blackboard.SetValue(Key, str);
            }
            else
            {
                GetFromBlackboard();
            }
        }

        public override void GetFromBlackboard()
        {
            string str = blackboard.GetValue<string>(Key);
            if (!Utils.IsStringValid(str) || str.Length > MAX_STRING_LENGTH)
                str = "default";

            inputField.text = str;
            SendToBlackboard(str);
        }

        private void OnValidate() => UpdateText();
    }
}
