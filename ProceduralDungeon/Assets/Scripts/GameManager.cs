using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> prefabs = default;

        private readonly Blackboard blackboard = new Blackboard();
        private readonly PersistentBlackboard persistent = new PersistentBlackboard();
        private Dungeon dungeon;

        private void Start()
        {
            prefabs.ForEach(x => Instantiate(x, x.transform.position, x.transform.rotation));
            FindAnyObjectByType<SceneBoilerplate>().Setup();

            blackboard.OnLog += Debug.Log;
            persistent.Setup(blackboard);
            blackboard.LogAll();

            List<Field> fields = FindObjectsByType<Field>(FindObjectsSortMode.None).ToList();
            fields.ForEach(x => x.Setup(blackboard));

            FindAnyObjectByType<CameraMover>().Setup(blackboard);
            SendToFields();

            FindAnyObjectByType<SpriteRendererDungeon>().Setup();
            dungeon = FindAnyObjectByType<Dungeon>();
            dungeon.Setup(blackboard);
            dungeon.Refresh();
            
            // WE LOOVE PROGRAMMINNG !!! HAHAHAHA
            List<Button> buttons = FindObjectsByType<Button>(FindObjectsSortMode.None).ToList();
            FindButton(buttons, nameof(Refresh)).onClick.AddListener(Refresh);
            FindButton(buttons, nameof(Save)).onClick.AddListener(Save);
            FindButton(buttons, nameof(Load)).onClick.AddListener(Load);
        }

        private Button FindButton(List<Button> buttons, string methodName)
        {
            return buttons.
                Where(x => x.gameObject.name == methodName.ToLowerInvariant()).
                FirstOrDefault();
        }

        private void SendToFields()
        {
            List<Field> fields = FindObjectsByType<Field>(FindObjectsSortMode.None).ToList();
            fields.ForEach(x => x.SyncWithBlackboard());
        }

        [ContextMenu(nameof(Refresh))]
        private void Refresh()
        {
            print(nameof(Refresh));
            dungeon.Refresh();
        }

        [ContextMenu(nameof(Save))]
        private void Save()
        {
            print(nameof(Save));
            persistent.Save();
        }

        [ContextMenu(nameof(Load))]
        private void Load()
        {
            print(nameof(Load));
            persistent.Load();
            Refresh(); // ??
            SendToFields();
        }
    }
}
