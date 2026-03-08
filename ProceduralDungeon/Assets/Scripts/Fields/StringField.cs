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

        public override void Setup(Blackboard blackboard)
        {
            base.Setup(blackboard);
            inputField.onEndEdit.AddListener(SendToBlackboard);
            placeholder.text = key;
            text.text = key;
        }

        private void SendToBlackboard(string str)
        {
            if (Utils.IsStringValid(str) && str.Length <= MAX_STRING_LENGTH)
            {
                blackboard.SetValue(key, str);
            }
            else
            {
                GetFromBlackboard();
            }
        }

        public override void GetFromBlackboard()
        {
            string str = blackboard.GetValue<string>(key);
            if (!Utils.IsStringValid(str) || str.Length > MAX_STRING_LENGTH)
                str = "default";

            inputField.text = str;
            SendToBlackboard(str);
        }
    }
}
