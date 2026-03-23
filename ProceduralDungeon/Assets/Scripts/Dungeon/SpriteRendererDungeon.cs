using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public class SpriteRendererDungeon : MonoBehaviour, IDungeonVisualizer
    {
        [SerializeField] private GameObject prefab = default;
        [SerializeField] private float size = default;
        [SerializeField] private float spacing = default;
        [SerializeField] private Transform parent = default;
        [SerializeField] private Conversion[] conversions = default;

        private Dictionary<TileType, Sprite> typeToSprite;
        private List<GameObject> spawned;

        public void Setup()
        {
            spawned = new List<GameObject>();
            typeToSprite = new Dictionary<TileType, Sprite>();
            for (int i = 0; i < conversions.Length; i++)
            {
                typeToSprite[conversions[i].type] = conversions[i].sprite;
            }
        }

        public void Visualize(TileType[,] tiles)
        {
            spawned.ForEach(x => Destroy(x));
            spawned.Clear();

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    TileType type = tiles[x, y];
                    Transform tile = Instantiate(prefab).transform;
                    SpriteRenderer rend = tile.GetComponent<SpriteRenderer>();
                    tile.SetParent(parent);
                    tile.localPosition = new Vector3(size * spacing * x, size * spacing * y, 0f);
                    tile.localRotation = Quaternion.identity;
                    tile.localScale = Vector3.one * size;

                    rend.sprite = typeToSprite[type];
                    spawned.Add(tile.gameObject);
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
