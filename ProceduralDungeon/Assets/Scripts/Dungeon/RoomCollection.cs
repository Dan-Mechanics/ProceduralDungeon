using UnityEngine;

namespace ProceduralDungeon
{
    [System.Serializable]
    public class RoomCollection
    {
        public Room[] rooms;
        [HideInInspector] public int minRequired;
        [HideInInspector] public int count;

        public void Clear() => count = 0;
        public Room GetRandomRoom() => rooms[Random.Range(0, rooms.Length)];
    }
}