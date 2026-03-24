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
        [SerializeField] private Transform offset = default;
        [SerializeField] private Conversion[] conversions = default;
        private readonly Dictionary<TileType, Sprite> typeToSprite = new Dictionary<TileType, Sprite>();
        private readonly List<GameObject> spawned = new List<GameObject>();

        public void Setup()
        {
            for (int i = 0; i < conversions.Length; i++)
            {
                typeToSprite.Add(conversions[i].type, conversions[i].sprite);
            }
        }

        public void Visualize(TileType[,] tiles)
        {
            spawned.ForEach(x => Destroy(x));
            spawned.Clear();
            
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileType type = tiles[x, y];
                    if (type == null)
                        continue;

                    Transform tile = Instantiate(prefab).transform;
                    SpriteRenderer rend = tile.GetComponent<SpriteRenderer>();

                    tile.SetParent(offset);
                    tile.name = "tile";
                    tile.localPosition = new Vector3(size * spacing * x, size * spacing * y, 0f);
                    tile.localRotation = Quaternion.identity;
                    tile.localScale = Vector3.one * size;

                    rend.sprite = typeToSprite[type];
                    spawned.Add(tile.gameObject);
                }
            }

            float offsetX = size * spacing * width;
            float offsetY = size * spacing * height;
            offset.localPosition = new Vector3(offsetX, offsetY, 0f) * -0.5f;
        }

        [System.Serializable]
        private struct Conversion
        {
            public TileType type;
            public Sprite sprite;
        }
    }
}