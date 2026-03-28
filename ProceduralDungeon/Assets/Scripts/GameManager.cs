using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextAsset defaults = default;

        private Blackboard blackboard;
        private PersistentBlackboard persistent;
        private SceneBoilerplate sceneBoilerplate;
        private List<Button> buttons;
        private CameraController cameraHandler;
        private Dungeon dungeon;
        private List<Field> fields;
        private ILayoutGenerator generator;
        private ILayoutAnalyzer analyzer;
        private IContentPlacer contentPlacer;
        private IDungeonVisualizer visualizer;

        private void Awake()
        {
            blackboard = new Blackboard();
            persistent = new PersistentBlackboard();

            sceneBoilerplate = FindAnyObjectByType<SceneBoilerplate>();
            fields = FindObjectsByType<Field>(FindObjectsSortMode.None).ToList();
            buttons = FindObjectsByType<Button>(FindObjectsSortMode.None).ToList();
            cameraHandler = FindAnyObjectByType<CameraController>();

            generator = FindAnyObjectByType<RandomWalkWithRooms>();
            analyzer = FindAnyObjectByType<FloodFill>();
            contentPlacer = FindAnyObjectByType<ContentPlacer>();
            visualizer = FindAnyObjectByType<SpriteRendererDungeon>();
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
            dungeon.Setup(generator, analyzer, contentPlacer, visualizer);

            GetButtonByName(buttons, nameof(Generate)).onClick.AddListener(Generate);
            GetButtonByName(buttons, nameof(GenerateRandom)).onClick.AddListener(GenerateRandom);
            GetButtonByName(buttons, nameof(Save)).onClick.AddListener(Save);
            GetButtonByName(buttons, nameof(Load)).onClick.AddListener(Load);

            MatchFieldsToBlackboard();
            blackboard.LogAll();

            Generate();
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

        private void GenerateRandom()
        {
            blackboard.SetValue("seed", Time.time.ToString());
            MatchFieldsToBlackboard();
            Generate();
        }

        private void Generate() => dungeon.Generate(blackboard);
        private void Save() => persistent.Save();

        private void Load()
        {
            persistent.Load();
            MatchFieldsToBlackboard();
            Generate();
        }
    }
}
