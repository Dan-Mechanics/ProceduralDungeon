using UnityEngine;

namespace ProceduralDungeon
{
    public class SpriteRendererDungeon : MonoBehaviour, IDungeonVisualizer
    {
        [SerializeField] private GameObject prefab = default;
        [SerializeField] private Transform parent = default;
        [SerializeField] private float spacing = default;
        [SerializeField] private Vector2 offset = default;

        public void Visualize(Tile[,] tiles)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = tiles[x, y];
                    Transform trans = Instantiate(prefab).transform;
                    SpriteRenderer rend = trans.GetComponent<SpriteRenderer>();

                    trans.SetParent(parent);
                    trans.name = $"{prefab.name}_[{x}_{y}]_{tile.type.name}".ToLowerInvariant();
                    trans.localPosition = new Vector3(x * tile.width, y * tile.height, 0f) * spacing;
                    trans.localRotation = Quaternion.identity;
                    trans.localScale = new Vector3(tile.width, tile.height, 1f);

                    rend.sprite = tile.type.sprite;
                    rend.color = tile.type.color;
                }
            }
        }

        private void OnValidate() => parent.localPosition = offset;
    }
}
