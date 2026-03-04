using UnityEngine;

namespace ProceduralDungeon
{
    [CreateAssetMenu(fileName = nameof(TileType), menuName = nameof(TileType))]
    public class TileType : ScriptableObject
    {
        public Sprite sprite;
        public Color color = Color.white;
    }
}
