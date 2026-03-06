using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    [RequireComponent(typeof(Slider))]
    public class SliderField : Field
    {
        [SerializeField] private Slider slider = default;

        public override void Setup(Blackboard blackboard)
        {
            base.Setup(blackboard);
            slider.onValueChanged.AddListener(SendToBlackboard);
        }

        private void SendToBlackboard(float value)
        {
          //  Debug.Log("SendToBlackboard");
            blackboard.SetValue(key, value);
        }

        public override void GetFromBlackboard()
        {
          //  Debug.Log("GetFromBlackboard");
            slider.value = blackboard.GetValue<float>(key);
        }

        private void OnValidate()
        {
            slider = GetComponent<Slider>();
            gameObject.name = key;
        }
    }
}
