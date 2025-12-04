using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KerningPairsManager {
// VARIABLES ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Variables
    /// <summary> Path where Json is stored </summary>
    private string filePath = Path.Combine(Application.dataPath /*Path to the /Assets folder*/, "Plugins/3DTextGenerator/kerningPairs.json");

    /// <summary> Holds all the KerningPairs objects </summary>
    private KerningPairs[] kpArray;

    /// <summary> Controls if kerning pairs Json have been loaded or not</summary>
    private bool jsonLoaded;

    /// <summary> Amount of space between characters in the text </summary>
    float spaceBetweenCharacters = 1;

    /// <summary> Control if methods should print data in unity log as they read it </summary>
    bool bDebug = false;
    #endregion

// CONSTRUCTOR //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Con structor
    /// <summary> Constructor of the KerningPairsManager objects </summary>
    /// <param name="spaceBetweenCharacters"> Amount of space between characters in the text </param>
    /// <param name="bDebug"> Control if methods should print data in unity log as they read it. Set true for printing data </param>
    public KerningPairsManager(float spaceBetweenCharacters, bool bDebug) {
        this.spaceBetweenCharacters = spaceBetweenCharacters;
        this.bDebug = bDebug;
    }
    #endregion

// PUBLIC FUNCTIONS /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Con structor
    /// <summary> Load kerning pairs Json when game starts </summary>
    public void Start() {
        jsonLoaded = JsonHelper.LoadKerningPairs(filePath, ref kpArray);
    }

    /// <summary> Check the kerning value between two characters </summary>
    /// <param name="first"> Character on the left </param>
    /// <param name="second"> Character on the right </param>
    /// <returns></returns>
    public float SearchValue(char first, char second) {
        if (!jsonLoaded) jsonLoaded = JsonHelper.LoadKerningPairs(filePath, ref kpArray);
        char firstNoMod = deleteMods(first);
        char secondNoMod = deleteMods(second);
        foreach (KerningPairs kp in kpArray) {
            if (kp.Equals(firstNoMod, secondNoMod)) {
                if (bDebug) Debug.Log("Reading kerning pair [" + first + " ," + second + "] -> " + kp.value);
                return kp.value;
            }
        }

        if (bDebug) Debug.Log("Reading kerning pair [" + first + " ," + second + "] -> not existent");
        return 1;
    }

    public char deleteMods(char c) {
        switch (c) {
            case 'Ñ': return 'N'; case 'ñ': return 'n';
            case 'Ç': return 'C'; case 'ç': return 'ç';
            case 'Á': return 'A'; case 'á': return 'a';
            case 'À': return 'A'; case 'à': return 'a';
            case 'Ä': return 'A'; case 'ä': return 'a';
            case 'Â': return 'A'; case 'â': return 'a';
            case 'É': return 'E'; case 'é': return 'e';
            case 'È': return 'E'; case 'è': return 'e';
            case 'Ë': return 'E'; case 'ë': return 'e';
            case 'Ê': return 'E'; case 'ê': return 'e';
            case 'Í': return 'I'; case 'í': return 'i';
            case 'Ì': return 'I'; case 'ì': return 'i';
            case 'Ï': return 'I'; case 'ï': return 'i';
            case 'Î': return 'I'; case 'î': return 'i';
            case 'Ó': return 'O'; case 'ó': return 'o';
            case 'Ò': return 'O'; case 'ò': return 'o';
            case 'Ö': return 'O'; case 'ö': return 'o';
            case 'Ô': return 'O'; case 'ô': return 'o';
            case 'Ú': return 'U'; case 'ú': return 'o';
            case 'Ù': return 'U'; case 'ù': return 'o';
            case 'Ü': return 'U'; case 'ü': return 'o';
            case 'Û': return 'U'; case 'û': return 'o';
            default : return c;
        }
     }

    /// <summary> Debugs and print in unity log the content of the array of kerning values </summary>
    public void DebugContent() {
        if (!jsonLoaded) jsonLoaded = JsonHelper.LoadKerningPairs(filePath, ref kpArray);
        foreach (KerningPairs kp in kpArray) {
            Debug.Log("Debugging kerning pairs objects: ");
            Debug.Log(" [" + kp.first + ", " + kp.second + "] -> " + kp.value);
        }
    }
    #endregion
}
