using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralDungeon
{
    /// <summary>
    /// De opbouw is een beetje zo:
    /// je hebt een game dat procedural generation roguleike achtig is
    /// die ga je analyseren, welke layout welke enemies waar, hoe werkt dat?
    /// volgens moet dat spels proc gen met een bestaand ding gemaakt zijn
    /// dat bestaand ding ga je dan onderzoeken en programmeren,
    /// je gebruikt je reference game als standby voor waar enemies
    /// komen en shit, het belangrijke hier is dat je kan spelen met
    /// met welke criteria er zijn voor de solve algorithm te determine the shit
    /// en ik denk dat dus de ideale opdracht is dat 
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> prefabs = default;

        private Blackboard blackboard;
        private PersistentBlackboard persistent;
        private SceneBoilerplate sceneBoilerplate;
        private List<Button> buttons;
        private CameraHandler cameraHandler;
        private SpriteRendererDungeon spriteRendererDungeon;
        private Dungeon dungeon;
        private List<Field> fields;

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
            cameraHandler = FindAnyObjectByType<CameraHandler>();
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
            persistent.OnLog += Debug.Log;

            persistent.Setup(blackboard);
            persistent.LoadFromResources("defaults");

            fields.ForEach(x => x.Setup(blackboard));
            cameraHandler.Setup(blackboard);

            spriteRendererDungeon.Setup();
            dungeon.Setup(blackboard);

            GetButtonByName(buttons, nameof(Show)).onClick.AddListener(Show);
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

        [ContextMenu(nameof(Show))]
        private void Show()
        {
            print(nameof(Show));
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
