using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class generate_kerningPairs_file : MonoBehaviour {

    //public void Awake() {
    //    filePath = System.IO.Path.Combine(Application.dataPath /*Path to the /Assets folder*/, "OrderableText/kerningPairs.json");
    //}

    public void GenerateJson(string filePath) {
        //!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~´
        string chars = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~´";
        string combinations = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~´";
        string text = "[";

        char[] charArray = chars.ToCharArray();
        char[] charArrayComb = combinations.ToCharArray();
        Debug.Log("Generating " + charArray.Length * charArrayComb.Length + " (" + charArray.Length + " x " + charArrayComb.Length + ") combinations of kerning pairs");

        for (int i = 0; i<charArray.Length; i++) {
            for (int j = 0; j < charArray.Length; j++) {
                //Debug.Log("combination: " + charArrayComb[j]);
                string first = ReplaceEspecialCharacteres(charArray[i]);
                string second = ReplaceEspecialCharacteres(charArrayComb[j]);
                float value = ValueControl(first, second);

                // EXPANDED FORMAT
                //text += "\n\t{\n\t\t\"first\": \"" + first + "\",\n\t\t\"second\": \"" + second + "\",\n\t\t\"value\": \""+ value +"\"\n\t},";

                // COMPACT FORMAT
                text += "\n\t{\"first\": \"" + first + "\", \"second\": \"" + second + "\", \"value\": \"" + value.ToString().Replace(',','.') + "\"},";
            }
        }

        text = text.Remove(text.Length - 1, 1); //Remove last comma
        text += "\n]";

        System.IO.File.WriteAllText(filePath, text);
    }

    public string ReplaceEspecialCharacteres(char c) {
        if (c == '\\') return "\\\\";
        if (c == '\"') return "\\\"";
        return c.ToString();
    }

    public float ValueControl(string first, string second) {

        //Exact combination control
        if (first == " " && second == "f") return 0.63f;
        if (first == " " && second == "i") return 0.55f;
        if (first == " " && second == "j") return 0.45f;
        if (first == " " && second == "l") return 0.5f;
        if (first == " " && second == "m") return 1.1f;
        if (first == " " && second == "r") return 0.7f;
        if (first == " " && second == "s") return 0.78f;
        if (first == " " && second == "t") return 0.6f;
        //if (first == " " && second == "w") return 1.3f;

        if (first == "f" && second == " ") return 0.65f;
        if (first == "f" && second == "f") return 0.65f;
        if (first == "f" && second == "i") return 0.45f;  
        if (first == "f" && second == "j") return 0.35f;  
        if (first == "f" && second == "l") return 0.4f;  
        if (first == "f" && second == "r") return 0.6f; 
        if (first == "f" && second == "t") return 0.5f;
        if (first == "f" && IsUpperCase(second)) return 1f;

        if (first == "i" && second == " ") return 0.55f;
        if (first == "i" && second == "f") return 0.5f;
        if (first == "i" && second == "i") return 0.4f;
        if (first == "i" && second == "j") return 0.3f;
        if (first == "i" && second == "l") return 0.4f;
        if (first == "i" && second == "r") return 0.5f;
        if (first == "i" && second == "t") return 0.5f;  

        if (first == "j" && second == " ") return 0.65f; 
        if (first == "j" && second == "f") return 0.6f;
        if (first == "j" && second == "i") return 0.5f;
        if (first == "j" && second == "j") return 0.4f;
        if (first == "j" && second == "l") return 0.5f;
        if (first == "j" && second == "r") return 0.65f;
        if (first == "j" && second == "t") return 0.55f;

        if (first == "l" && second == " ") return 0.5f; 
        if (first == "l" && second == "f") return 0.5f;
        if (first == "l" && second == "i") return 0.4f;
        if (first == "l" && second == "j") return 0.3f;
        if (first == "l" && second == "l") return 0.4f;
        if (first == "l" && second == "r") return 0.5f;
        if (first == "l" && second == "t") return 0.45f; 

        if (first == "m" && second == " ") return 1.1f; 
        if (first == "m" && second == "i") return 0.9f; 

        if (first == "r" && second == " ") return 0.63f; 
        if (first == "r" && second == "f") return 0.55f;
        if (first == "r" && second == "i") return 0.45f;
        if (first == "r" && second == "j") return 0.35f;
        if (first == "r" && second == "l") return 0.45f;
        if (first == "r" && second == "r") return 0.6f;
        if (first == "r" && second == "t") return 0.5f; 

        if (first == "t" && second == " ") return 0.55f; 
        if (first == "t" && second == "f") return 0.55f;
        if (first == "t" && second == "i") return 0.45f;
        if (first == "t" && second == "j") return 0.35f;
        if (first == "t" && second == "l") return 0.45f;
        if (first == "t" && second == "r") return 0.63f;
        if (first == "t" && second == "t") return 0.5f; 

        if (first == "s" && second == " ") return 0.8f;

        if (IsUpperCase(first) && !IsUpperCase(second)) return 1.1f;
        if (!IsUpperCase(first) && IsUpperCase(second)) return 1.1f;
        if (IsUpperCase(first) && IsUpperCase(second)) return 1.25f;

        // General first or second control

        if (first == "@" || second == "@") return 1.5f;
        if (first == "#" || second == "#") return 1.3f;

        if (first == "m" && !IsUpperCase(second)) return 1.3f;
        if (!IsUpperCase(second) && second == "m") return 1.3f;

        if (first == "i" && !IsUpperCase(second)) return 0.7f;
        if (!IsUpperCase(second) && second == "i") return 0.7f;

        if (first == "j" && !IsUpperCase(second)) return 0.8f;
        if (!IsUpperCase(second) && second == "j") return 0.7f;

        if (first == "l" && !IsUpperCase(second)) return 0.8f;
        if (!IsUpperCase(second) && second == "l") return 0.8f;

        if (first == "r" && !IsUpperCase(second)) return 0.8f;
        if (!IsUpperCase(second) && second == "r") return 0.9f;

        if (first == "t" && !IsUpperCase(second)) return 0.8f;
        if (!IsUpperCase(second) && second == "t") return 0.8f;

        if (first == "f" && !IsUpperCase(second)) return 0.75f;
        if (!IsUpperCase(second) && second == "f") return 0.8f;

        if (first == "w" && !IsUpperCase(second)) return 1.3f;
        if (!IsUpperCase(second) && second == "w") return 1.3f;

        if (first == "P" && !IsUpperCase(second)) return 1.1f;
        if (!IsUpperCase(second) && second == "P") return 1.1f;

        if (second == "Q") return 1.35f;

        // General control
        
        if (first == " " || second == " ") return 0.85f;

        

        return 1;
    }

    public bool IsUpperCase(string str) {
        char c = str.ToCharArray()[0];
        Debug.Log("CHAR" + str.ToCharArray()[0] + " -> " + char.IsUpper(c));
        return char.IsUpper(c);
    }
}

[CustomEditor(typeof(generate_kerningPairs_file))]
public class KP_JSON_FILE_GENERATOR : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        generate_kerningPairs_file script = (generate_kerningPairs_file)target;
        if (GUILayout.Button("Generate kerningPairs.json")) {
            script.GenerateJson(Application.dataPath + "/Plugins/OrderableText/kerningPairs.json");
        }
    }
}