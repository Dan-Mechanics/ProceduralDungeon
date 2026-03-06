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
        private CameraMover mover;

        private void Start()
        {
            prefabs.ForEach(x => Instantiate(x, x.transform.position, x.transform.rotation));
            FindAnyObjectByType<SceneBoilerplate>().Setup();
            mover = FindAnyObjectByType<CameraMover>();

            blackboard.OnLog += Debug.Log;
            persistent.Setup(blackboard);
            blackboard.LogAll();

            mover.Setup(blackboard);
            List<Field> fields = FindObjectsByType<Field>(FindObjectsSortMode.None).ToList();
            fields.ForEach(x => x.Setup(blackboard));

            SyncFields();


            dungeon = FindAnyObjectByType<Dungeon>();
            dungeon.Setup(blackboard);

            List<Button> buttons = FindObjectsByType<Button>(FindObjectsSortMode.None).ToList();

            // WE LOOVE PROGRAMMINNG !!! HAHAHAHA
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

        private void SyncFields()
        {
            List<Field> fields = FindObjectsByType<Field>(FindObjectsSortMode.None).ToList();
            fields.ForEach(x => x.SyncWithBlackboard());

            mover.Pull();
        }

        [ContextMenu(nameof(Refresh))]
        private void Refresh()
        {
            print(nameof(Refresh));
            dungeon.Show();
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
            SyncFields();
        }
    }
}
