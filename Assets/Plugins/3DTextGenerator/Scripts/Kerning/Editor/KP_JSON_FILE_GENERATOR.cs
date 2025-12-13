using UnityEditor;
using UnityEngine;

// O código de editor deve estar em um arquivo e pasta separados!
[CustomEditor(typeof(generate_kerningPairs_file))]
public class KP_JSON_FILE_GENERATOR : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        generate_kerningPairs_file script = (generate_kerningPairs_file)target;
        if (GUILayout.Button("Generate kerningPairs.json"))
        {
            script.GenerateJson(Application.dataPath + "/Plugins/OrderableText/kerningPairs.json");
        }
    }
}
