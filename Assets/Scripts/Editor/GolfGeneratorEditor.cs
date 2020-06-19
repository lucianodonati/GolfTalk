using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GolfGenerator))]
    public class GolfGeneratorEditor : UnityEditor.Editor
    {
        private GolfGenerator generator = null;

        private void OnEnable()
        {
            if (null ==  generator)
            {
                generator = target as GolfGenerator;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Generate Course"))
                generator.GenerateCourse();
        }
    }
}