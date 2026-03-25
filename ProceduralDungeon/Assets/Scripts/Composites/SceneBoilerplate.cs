using System.Globalization;
using System.Threading;
using UnityEngine;

namespace ProceduralDungeon
{
    public class SceneBoilerplate : MonoBehaviour
    {
        [SerializeField] private EasyBinding escape = default;
        [SerializeField] private int fps = default;
        [SerializeField] private float physicsTicksPerSecond = default;

        public void Setup()
        {
            gameObject.name = "scene_boilerplate";
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            Application.targetFrameRate = fps;
            Time.fixedDeltaTime = 1f / physicsTicksPerSecond;

            QualitySettings.SetQualityLevel(0);
            QualitySettings.vSyncCount = 0;
        }

        private void Update()
        {
            if (escape.WasPressed)
                Application.Quit();
        }
    }
}