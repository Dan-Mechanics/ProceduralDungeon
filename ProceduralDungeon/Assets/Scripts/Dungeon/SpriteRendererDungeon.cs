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
        [SerializeField] private Transform parent = default;
        [SerializeField] private Conversion[] conversions = default;
        private Dictionary<TileType, Sprite> tileTypeToSprite;

        private List<GameObject> previous = new List<GameObject>();

        public void Setup()
        {
            tileTypeToSprite = new Dictionary<TileType, Sprite>();
            for (int i = 0; i < conversions.Length; i++)
            {
                tileTypeToSprite[conversions[i].type] = conversions[i].sprite;
            }
        }

        /// <summary>
        /// TODO: make it so that it removes the old and adds the new .
        /// </summary>
        /// <param name="tiles"></param>
        public void Refresh(Dictionary<Vector2Int, TileType> tiles)
        {
            for (int i = 0; i < previous.Count; i++)
            {
                Destroy(previous[i]);
            }
            previous.Clear();
            
            foreach (KeyValuePair<Vector2Int, TileType> pair in tiles)
            {
                TileType type = pair.Value;
                int x = pair.Key.x;
                int y = pair.Key.y;

                Transform trans = Instantiate(prefab).transform;
                SpriteRenderer rend = trans.GetComponent<SpriteRenderer>();
                trans.SetParent(parent);
                trans.localPosition = new Vector3(size * spacing * x, size * spacing * y, 0f);
                trans.localRotation = Quaternion.identity;
                trans.localScale = Vector3.one * size;

                rend.sprite = tileTypeToSprite[type];
                previous.Add(trans.gameObject);
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
