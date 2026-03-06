using UnityEngine;

namespace ProceduralDungeon
{
    public abstract class Field : MonoBehaviour
    {
        [SerializeField] protected string key = default;
        protected Blackboard blackboard;

        public void Setup(Blackboard blackboard) => this.blackboard = blackboard;
        public abstract void SyncWithBlackboard();
    }
}
