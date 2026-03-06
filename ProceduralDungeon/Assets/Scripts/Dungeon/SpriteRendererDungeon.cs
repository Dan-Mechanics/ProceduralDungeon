using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralDungeon
{
    public class SpriteRendererDungeon : MonoBehaviour, IDungeonVisualizer
    {
        [SerializeField] private GameObject prefab = default;
        [SerializeField] private float size = default;
        [SerializeField] private float spacing = default;
        [SerializeField] private Conversion[] conversions = default;
        private readonly Dictionary<TileType, Conversion> dict = new Dictionary<TileType, Conversion>();

        public void Setup() => conversions.ToList().ForEach(x => dict.Add(x.type, x));

        /// <summary>
        /// Make sure to call Setup().
        /// </summary>
        public void Visualize(TileType[,] tiles)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileType type = tiles[x, y];
                    Transform trans = Instantiate(prefab).transform;
                    SpriteRenderer rend = trans.GetComponent<SpriteRenderer>();

                    trans.name = $"{new string(' ', y)}{prefab.name}_[{x}_{y}]_{type.name}".ToLowerInvariant();
                    trans.localPosition = new Vector3(size * spacing * x, size * spacing * y, 0f);
                    trans.localRotation = Quaternion.identity;
                    trans.localScale = Vector3.one * size;

                    rend.sprite = dict[type].sprite;
                }
            }
        }

        [System.Serializable]
        private struct Conversion
        {
            public TileType type;
            public Sprite sprite;
        }
    }
}
