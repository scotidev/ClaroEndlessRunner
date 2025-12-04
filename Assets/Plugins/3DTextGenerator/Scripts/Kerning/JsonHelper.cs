using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonHelper {

    /// <summary> Loads the kerning pairs json into and array given in argument </summary>
    /// <param name="filePath"> the path of the kerningPairs.json file </param>
    /// <param name="kpArray"> Reference to the array where KerningPairs objects should be loaded </param>
    /// <returns> true if file loaded correctly, false if not </returns>
    public static bool LoadKerningPairs(string filePath, ref KerningPairs[] kpArray) {
        Debug.Log("Loading kerning pairs data json file from " + filePath);
        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);

            kpArray = getJsonArray<KerningPairs>(dataAsJson);
            return true;
        } else {
            Debug.Log("Error loading kerningPairs.json file.");
            return false;
        }
    }

    /// <summary> As JsonUtility.FromJson do not support having an array as a parent object, this function
    /// purpose is to add an 'array' item to wrap the array of items in the json </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns> List of the objects contained in the json file </returns>
    public static T[] getJsonArray<T>(string json) {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    /// <summary> A simple class to load an array of item from json file </summary>
    /// <typeparam name="T"> Generic type to cast to differents objects </typeparam>
    [System.Serializable]
    private class Wrapper<T> {
        public T[] array;
    }
}
