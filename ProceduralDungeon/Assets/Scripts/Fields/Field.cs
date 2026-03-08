using UnityEngine;

namespace ProceduralDungeon
{
    public abstract class Field : MonoBehaviour
    {
        public string Key => gameObject.name;
        protected Blackboard blackboard;

        public virtual void Setup(Blackboard blackboard) => this.blackboard = blackboard;
        public abstract void GetFromBlackboard();
    }
}
