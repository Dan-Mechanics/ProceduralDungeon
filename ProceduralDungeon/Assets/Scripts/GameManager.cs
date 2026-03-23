using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> prefabs = default;
        [SerializeField] private TextAsset defaults = default;

        private Blackboard blackboard;
        private PersistentBlackboard persistent;
        private SceneBoilerplate sceneBoilerplate;
        private List<Button> buttons;
        private CameraController cameraHandler;
        private SpriteRendererDungeon spriteRendererDungeon;
        private Dungeon dungeon;
        private List<Field> fields;

        private void Awake()
        {
            blackboard = new Blackboard();
            persistent = new PersistentBlackboard();
            prefabs.ForEach(x => Instantiate(x, x.transform.position, x.transform.rotation));

            sceneBoilerplate = FindAnyObjectByType<SceneBoilerplate>();
            fields = FindObjectsByType<Field>(FindObjectsSortMode.None).ToList();
            buttons = FindObjectsByType<Button>(FindObjectsSortMode.None).ToList();
            cameraHandler = FindAnyObjectByType<CameraController>();
            spriteRendererDungeon = FindAnyObjectByType<SpriteRendererDungeon>();
            dungeon = FindAnyObjectByType<Dungeon>();
        }

        private void Start()
        {
            sceneBoilerplate.Setup();
            blackboard.OnLog += Debug.Log;
            persistent.OnLog += Debug.Log;

            persistent.Setup(blackboard);
            persistent.LoadTextAsset(defaults);

            fields.ForEach(x => x.Setup(blackboard));
            cameraHandler.Setup(blackboard);

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

        private void Refresh()
        {
            dungeon.Refresh();
        }

        private void Save()
        {
            persistent.Save();
        }

        private void Load()
        {
            persistent.Load();
            MatchFieldsToBlackboard();
            Refresh();
        }
    }
}
