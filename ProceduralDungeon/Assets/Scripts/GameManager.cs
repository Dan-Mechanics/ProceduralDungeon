using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> prefabs = default;

        private Blackboard blackboard;
        private PersistentBlackboard persistent;
        private SceneBoilerplate sceneBoilerplate;
        private List<Field> fields;
        private List<Button> buttons;
        private CameraMover cameraMover;
        private SpriteRendererDungeon spriteRendererDungeon;
        private Dungeon dungeon;

        /// <summary>
        /// Get / init the references, order not important.
        /// </summary>
        private void Awake()
        {
            blackboard = new Blackboard();
            persistent = new PersistentBlackboard();
            
            // SPAWN PREFABS.
            prefabs.ForEach(x => Instantiate(x, x.transform.position, x.transform.rotation));

            sceneBoilerplate = FindAnyObjectByType<SceneBoilerplate>();
            fields = FindObjectsByType<Field>(FindObjectsSortMode.None).ToList();
            buttons = FindObjectsByType<Button>(FindObjectsSortMode.None).ToList();
            cameraMover = FindAnyObjectByType<CameraMover>();
            spriteRendererDungeon = FindAnyObjectByType<SpriteRendererDungeon>();
            dungeon = FindAnyObjectByType<Dungeon>();
        }

        /// <summary>
        /// Use the references for setup / assign, order is important.
        /// </summary>
        private void Start()
        {
            sceneBoilerplate.Setup();
            blackboard.OnLog += Debug.Log;

            persistent.Setup(blackboard);
            persistent.LoadFromResources("defaults");

            fields.ForEach(x => x.Setup(blackboard));
            cameraMover.Setup(blackboard);

            spriteRendererDungeon.Setup();
            dungeon.Setup(blackboard);

            GetButtonByName(buttons, nameof(Refresh)).onClick.AddListener(Refresh);
            GetButtonByName(buttons, nameof(Save)).onClick.AddListener(Save);
            GetButtonByName(buttons, nameof(Load)).onClick.AddListener(Load);

            // ===

            MatchFieldsToBlackboard();
            blackboard.LogAll();
        }

        private Button GetButtonByName(List<Button> buttons, string methodName)
        {
            return buttons.
                Where(x => x.gameObject.name == methodName.ToLowerInvariant()).
                FirstOrDefault();
        }

        private void MatchFieldsToBlackboard()
        {
            List<Field> fields = FindObjectsByType<Field>(FindObjectsSortMode.None).ToList();
            fields.ForEach(x => x.GetFromBlackboard());
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
            MatchFieldsToBlackboard();
            // Refresh();
        }
    }
}
