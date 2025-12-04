using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

[System.Serializable]
public class KerningPairs {

// VARIABLES ///////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary> Character on the left </summary>
    public string first; // this should be a char type, but Unity Json serialization doesn't like that
    /// <summary> Character on the Right </summary>
    public string second; // this should be a char type, but Unity Json serialization doesn't like that
    /// <summary> Kerning value between the two characters </summary>
    public float value;


// FUNCTIONS ///////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary> Check if a kerning object is equal to this one </summary>
    /// <param name="kp"> KerningPair object to compare with </param>
    /// <returns> true if equal, false if not </returns>
    public bool Equals(KerningPairs kp) {
        return (first == kp.first) && (second == kp.second);
    }

    /// <summary> Check if a pair characters is equal to this one </summary>
    /// <param name="first"> Character on the left </param>
    /// <param name="second"> Character on the right </param>
    /// <returns> true if equal, false if not </returns>
    public bool Equals(char first, char second) {
        return (this.first[0] == first) && (this.second[0] == second);
    }

    /// <summary> Allow to format a string with data of the object </summary>
    /// <returns> String with data of the object </returns>
    public string toString() {
        return "[" + first + ", " + second + "] -> " + value;
    }
}



