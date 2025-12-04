using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
// VARIABLES /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region variables

    // Randomization ===================================================================================================================
    /// <summary> Vector from OrderableText to SpawnSpherePivot </summary>
    public Vector3 vectorToSpawnSpherePivot; // This is needed to set the local position once the character is a child of OrdenableText

    /// <summary> Radius for the spawn area for the characters </summary>
    public float spawnSphereRadius = 10;

    /// <summary> Minimum value of the random rotation of the character when spawned </summary>
    public float rangeRandomRotation_min = -180;

    /// <summary> Maximum value of the random rotation of the character when spawned </summary>
    public float rangeRandomRotation_max = 180;

    /// <summary> Minimum values of the random rotation of the character when spawned </summary>
    public Vector3 rangeRandomScale_min = new Vector3(1, 1, 1);

    /// <summary> Maximum values of the random rotation of the character when spawned </summary>
    public Vector3 rangeRandomScale_max = new Vector3(1, 1, 1);

    // Movement options ================================================================================================================
    /// <summary> Desired duration of the movement </summary>
    public float movDuration;

    /// <summary> Maximum amplitude of the arc function</summary>
    public float amplitude = 10;

    /// <summary> Parameter to control the inclination of the function of the arc that characters follow.
    /// Tweak to customize the curve </summary>
    public float alpha = 2;

    /// <summary> Parameter to control the inclination of the function of the arc that characters follow.
    /// Tweak to customize the curve </summary>
    public float beta = 2;

    // Hidden in inspector =============================================================================================================
    /// <summary> Position that character is going to move to </summary>
    [HideInInspector]
    public Vector3 target_position;

    /// <summary> Rotation that character is going to rotate for </summary>
    [HideInInspector]
    public Vector3 target_eulerAngles;

    /// <summary> Scale that character is going to end with </summary>
    [HideInInspector]
    public Vector3 target_scale;

    /// <summary> Glyph in the text that this mesh game object is representing </summary>
    [HideInInspector]
    public char textCharacter;

    // Private ========================================================================================================================
    /// <summary> Stores position, rotation and scale at the beginning </summary>
    private Vector3 origin_position;
    private Vector3 origin_eulerAngles;
    private Vector3 origin_scale;

    /// <summary> Stores the time at the beginning of the movement </summary>
    private float startTime;

    /// <summary> Stores the vector that goes from the origin of the character to the target position </summary>
    private Vector3 PQvector;
    private float vPQ1;
    private float vPQ2;
    private float vPQ3;

    /// <summary> Stores a perpendicular vector to PQvector </summary>
    private Vector3 Vvector;
    private float vV1;
    private float vV2;
    private float vV3;

    /// <summary> Controls if the character values has been initialized </summary>
    private bool bInitialized = false;

    /// <summary> Controls if the character should move or not </summary>
    private bool bMove = false;
    #endregion

// PUBLIC FUNCTIONS ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region public functions
    /// <summary> Initialize and calculate parameters in order for the character to be able to move correctly </summary>
    public void init() {

        setTransformToRandom();

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------
        origin_position = transform.localPosition; // Save the position at the start
        origin_eulerAngles = transform.eulerAngles; //Save the rotation at the start
        origin_scale = transform.localScale; //Save the rotation at the start

        // Vector PQ is the vector that goes from P to Q. PQ = Q - P = (q1-p2, q2-p2, q3-p3)
        vPQ1 = target_position.x - origin_position.x;
        vPQ2 = target_position.y - origin_position.y;
        vPQ3 = target_position.z - origin_position.z;
        PQvector = new Vector3(vPQ1, vPQ2, vPQ3);  // Build the vector

        // Get a random Vector perpendicular to PQvector
        // any vector V = (v1, v2, v3) | v1(Q1 - P1) + v2(Q2 - P2) + v3(Q3 - P3) = 0 will be perpendicular to vector PQ
        vV1 = Random.Range(-100.0f, 100.0f)/100; // Get a random value
        vV2 = Random.Range(-100.0f, 100.0f)/100; // Get a random value
        vV3 = (-(vPQ1*vV1) -(vPQ2 * vV2)) / vPQ3;// Calculate what should be the value of v3 given the values for v1 and v2 for the equation to be equal to zero.
        Vvector = new Vector3(vV1, vV2, vV3); // Build the vector
        Vvector = Vvector.normalized; // Normalize the vector (makes its length equal to 1)
        
        // --------------------------------------------------------------------------------------------------------------------------------------------------------
        bInitialized = true;
    }

    /// <summary> Enable movement for the character </summary>
    public void startMoving() {
        startTime = Time.time; // Store the time at the start of the movement
        if (!bInitialized) { Debug.LogError("Error: Character not initialized. Aborting movement"); return; }

        bMove = true; // Enable movement (let the update function change position values)
    }


    #endregion

// PRIVATE FUNCTIONS //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region public functions
    /// <summary> Update triggers every frame, controlling the movement for the character </summary>
    private void Update() {
        if (bMove) {
            float currentTime = Time.time;

            if (currentTime - startTime < movDuration + 1) { // Give an additional second to let linear interpolation (lerp) reach its last value
                float timeElapsed = currentTime - startTime;
                if (timeElapsed > movDuration) timeElapsed = movDuration; // We do this to ensure that (timeElapsed / movDuration) always 
                                                                          // end up with a value of 1, so linear interpolation (lerp) 
                                                                          // works properly
                float t = timeElapsed / movDuration;

                // Set position ----------------------------------------------------------------------------------------------------------
                // V * curve(t)
                float fres = curve(t);
                Vector3 arcOffset = new Vector3(Vvector.normalized.x * fres, Vvector.normalized.y * fres, Vvector.normalized.z * fres);
                transform.localPosition = Vector3.Lerp(origin_position, target_position, t) + arcOffset;

                // Set rotation ----------------------------------------------------------------------------------------------------------
                transform.eulerAngles = Vector3.Lerp(origin_eulerAngles, target_eulerAngles, t);

                // Set scale -------------------------------------------------------------------------------------------------------------
                transform.localScale = Vector3.Lerp(origin_scale, target_scale, t);
            }
        }
    }

    /// <summary> Function for an arc. Customizable with 'amplitude', 'alpha' and 'beta' parameters </summary>
    /// <param name="x"> X variable for the curve function (f(x)). Must be between 0 and 1. 
    /// You can use it as a 'percentage', dividing time elapsed from the start of the movement by the desired movement duration </param>
    /// <returns> Image of the function for the given X, or the value of Y for the given X </returns>
    private float curve(float x) {
        return amplitude * Mathf.Pow((alpha + beta) / alpha, alpha) * Mathf.Pow((alpha + beta) / beta, beta) * Mathf.Pow(x, alpha) * Mathf.Pow(1 - x, beta);
    }

    /// <summary> Set transform of the character to random given the parameters for it </summary>
    private void setTransformToRandom() {
        // Set random position ----------------------------------------------------------------------------------------------
        Vector3 vR = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        float randomLength = Random.Range(-spawnSphereRadius, spawnSphereRadius);
        vR = new Vector3(vR.x * randomLength, vR.y * randomLength, vR.z * randomLength);

        transform.localPosition = vR + vectorToSpawnSpherePivot;

        // Set random rotation ----------------------------------------------------------------------------------------------
        transform.eulerAngles = new Vector3(Random.Range(rangeRandomRotation_min, rangeRandomRotation_max),
                                            Random.Range(rangeRandomRotation_min, rangeRandomRotation_max),
                                            Random.Range(rangeRandomRotation_min, rangeRandomRotation_max));

        // Set random scale -------------------------------------------------------------------------------------------------
        transform.localScale = new Vector3(Random.Range(rangeRandomScale_min.x, rangeRandomScale_max.x),
                                           Random.Range(rangeRandomScale_min.y, rangeRandomScale_max.y),
                                           Random.Range(rangeRandomScale_min.z, rangeRandomScale_max.z));
    }


    #endregion
}
