using UnityEditor;
using UnityEngine;

namespace ProceduralDungeon
{
    [CustomEditor(typeof(Dungeon))]
    public class DungeonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Dungeon dungeon = target as Dungeon;
            if (GUILayout.Button(nameof(Dungeon.Show)))
                dungeon.Show();

            if (GUILayout.Button(nameof(Dungeon.Hide)))
                dungeon.Hide();

        }
    }
}
