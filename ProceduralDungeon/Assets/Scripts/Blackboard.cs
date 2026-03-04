using UnityEngine;

namespace ProceduralDungeon 
{ 
    public class Blackboard 
    {
        public T GetValue<T>(string key)
        {
            Debug.Log(key);
            return default(T);
        }

        /*public override string ToString()
        {
            return base.ToString();
        }*/
    }
}
