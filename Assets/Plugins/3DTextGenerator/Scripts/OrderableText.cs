using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderableText : MonoBehaviour {

// VARIABLES //////////////////////////////////////////////////////////////////////////////////////////////////
    #region Variable region
    [Header("Text properties")]
    // Text ==============================================================================================
    [Tooltip("Text that characters will form")]
    public string Text = "hello world";

    [Tooltip("Will change all characters into CAPS")]
    public bool convertTextToAllUppercase = true;

    [Tooltip("Material for the characters")]
    public Material mat;

    [Tooltip("Wether this text should appear on Start or wait for other component to call\n")]
    public bool showAtStart;

    [Header("Format properties")]
    // Format ============================================================================================
    /// <summary> This variable let you set the space between 
    /// characters in the text </summary>
    [Tooltip("Set the distance between each characters in the text")]
    public float spaceBetweenCharacters = 1;

    /// <summary> This variable let you set the space between 
    /// characters in the text </summary>
    [Tooltip("Sets the distance between one line of text and the next one")]
    public float spaceBetweenLines = 2;

    /// <summary> The amount of character in one line of the
    /// text </summary>
    [Tooltip("Characters that fits in one line before changing to the next one.\n Incomplete words (Does not fit in the current line) will be written in the next line")]
    public int lineLenght = 25;

    [Header("Randomize parameters")]
    // Randomization =====================================================================================
    /// <summary> Pivot for the spawn sphere (spawn area) of the character </summary>
    [Tooltip("Transform of the spawn area for the characters")]
    public Transform spawnSpherePivot;
    
    [Tooltip("Radius of the spawn area for the characters")]
    /// <summary> Radius of the sphere </summary>
    public float spawnSphereRadius = 2;

    [Tooltip("Minimum random rotation for spawned chharacters")]
    /// <summary> minimum random value of rotation of characters when spawning </summary>
    public float RotationMin = -180;

    [Tooltip("Maximum random rotation for spawned chharacters")]
    /// <summary> maximum random value of rotation of characters when spawning </summary>
    public float RotationMax = 180;

    [Tooltip("Minimum random scale for spawned chharacters")]
    /// <summary> minimum random value of scale of characters when spawning </summary>
    public Vector3 ScaleMin = new Vector3(1, 1, 1);

    [Tooltip("Maximum random scale for spawned chharacters")]
    /// <summary> maximum random value of scale of characters when spawning </summary>
    public Vector3 ScaleMax = new Vector3(1, 1, 1);

    [Tooltip("Final Rotation of the character")]
    /// <summary> Final rotation of the character when movement ends </summary>
    public Vector3 finalRotation = new Vector3(0, -180, 0);

    [Tooltip("Final Scale of the character")]
    /// <summary> Final scale of the character when movement ends </summary>
    public Vector3 finalScale = new Vector3(1, 1, 1);

    [Header("Animation parameters")]
    // Ordering options =================================================================================
    /// <summary> The time that should pass before start moving the next letter </summary>
    [Tooltip("The time that should pass before start moving the next letter")]
    public float timeBetweenCharMov = 0.2f;

    [Tooltip("How long it takes for the letter to reach its correct position on the text")]
    /// <summary> How long it takes for the letter to reach its correct position on the text </summary>
    public float movementDuration = 0.2f;

    [Tooltip("Amplitude of the arc that characters follow")]
    /// <summary> Amplitude of the arc that characters follow </summary>
    public float arc_Amplitude = 10;

    [Tooltip("Parameter to control the inclination of the function of the arc that characters follow.\nTweak to customize the curve")]
    /// <summary> Parameter to control the inclination of the function of the arc that characters follow.
    /// Tweak to customize the curve </summary>
    public float arc_alpha = 2;

    [Tooltip("Parameter to control the inclination of the function of the arc that characters follow.\nTweak to customize the curve")]
    /// <summary> Parameter to control the inclination of the function of the arc that characters follow.
    /// Tweak to customize the curve </summary>
    public float arc_beta = 2;

    [Header("Debug")]
    // Debug ============================================================================================
    /// <summary> Decide if the scrip should show logs on the process </summary>
    [Tooltip("Decide if the scrip should show logs on the process")]
    public bool showLogs = false;

    public bool showCharacterBounds = false;

    public Transform CharacterBoundsVisualizerPrefab;

    public Transform SpaceBoundsVisualizerPrefab;

    // Private ==========================================================================================
    /// <summary> Hold array of instanced characters prefabs </summary>
    private Character[] characterArray;

    /// <summary> Reference to the Kerning pairs manager, which holds/load kerningPairs.json </summary>
    private KerningPairsManager kPManager;

    [Header("Alphabet References")]
    // Letter prefabs ===================================================================================
    /// <summary> All this variables contains a reference to the model of each character </summary>
    #region Letter prefabs region  
    public Transform space;
    public Transform exclam;
    public Transform quotedbl;
    public Transform numbersign;
    public Transform dollar;
    public Transform percent;
    public Transform ampersand;
    public Transform quotesingle;
    public Transform parentleft;
    public Transform parentright;
    public Transform asterisk;
    public Transform plus;
    public Transform comma;
    public Transform hyphen;
    public Transform period;
    public Transform slash;
    public Transform zero;
    public Transform one;
    public Transform two;
    public Transform three;
    public Transform four;
    public Transform five;
    public Transform six;
    public Transform seven;
    public Transform eight;
    public Transform nine;
    public Transform colon;
    public Transform semicolon;
    public Transform less;
    public Transform equal;
    public Transform greater;
    public Transform question;
    public Transform at;
    public Transform upper_A;
    public Transform upper_B;
    public Transform upper_C;
    public Transform upper_D;
    public Transform upper_E;
    public Transform upper_F;
    public Transform upper_G;
    public Transform upper_H;
    public Transform upper_I;
    public Transform upper_J;
    public Transform upper_K;
    public Transform upper_L;
    public Transform upper_M;
    public Transform upper_N;
    public Transform upper_O;
    public Transform upper_P;
    public Transform upper_Q;
    public Transform upper_R;
    public Transform upper_S;
    public Transform upper_T;
    public Transform upper_U;
    public Transform upper_V;
    public Transform upper_W;
    public Transform upper_X;
    public Transform upper_Y;
    public Transform upper_Z;
    public Transform bracketleft;
    public Transform backslash;
    public Transform bracketright;
    public Transform circumflex;
    public Transform underscore;
    public Transform grave;
    public Transform lower_A;
    public Transform lower_B;
    public Transform lower_C;
    public Transform lower_D;
    public Transform lower_E;
    public Transform lower_F;
    public Transform lower_G;
    public Transform lower_H;
    public Transform lower_I;
    public Transform lower_J;
    public Transform lower_K;
    public Transform lower_L;
    public Transform lower_M;
    public Transform lower_N;
    public Transform lower_O;
    public Transform lower_P;
    public Transform lower_Q;
    public Transform lower_R;
    public Transform lower_S;
    public Transform lower_T;
    public Transform lower_U;
    public Transform lower_V;
    public Transform lower_W;
    public Transform lower_X;
    public Transform lower_Y;
    public Transform lower_Z;
    public Transform braceleft;
    public Transform bar;
    public Transform braceright;
    public Transform tilde;
    public Transform upper_Ntilde;
    public Transform lower_Ntilde;
    public Transform upper_Ccedilla;
    public Transform lower_Ccedilla;
    public Transform acute;
    public Transform upper_Aacute;
    public Transform upper_Eacute;
    public Transform upper_Iacute;
    public Transform upper_Oacute;
    public Transform upper_Uacute;
    public Transform upper_Agrave;
    public Transform upper_Egrave;
    public Transform upper_Igrave;
    public Transform upper_Ograve;
    public Transform upper_Ugrave;
    public Transform upper_Acircum;
    public Transform upper_Ecircum;
    public Transform upper_Icircum;
    public Transform upper_Ocircum;
    public Transform upper_Ucircum;
    public Transform upper_Adiaere;
    public Transform upper_Ediaere;
    public Transform upper_Idiaere;
    public Transform upper_Odiaere;
    public Transform upper_Udiaere;
    public Transform lower_Aacute;
    public Transform lower_Eacute;
    public Transform lower_Iacute;
    public Transform lower_Oacute;
    public Transform lower_Uacute;
    public Transform lower_Agrave;
    public Transform lower_Egrave;
    public Transform lower_Igrave;
    public Transform lower_Ograve;
    public Transform lower_Ugrave;
    public Transform lower_Acircum;
    public Transform lower_Ecircum;
    public Transform lower_Icircum;
    public Transform lower_Ocircum;
    public Transform lower_Ucircum;
    public Transform lower_Adiaere;
    public Transform lower_Ediaere;
    public Transform lower_Idiaere;
    public Transform lower_Odiaere;
    public Transform lower_Udiaere;
    #endregion

    #endregion

// FUNCTIONS //////////////////////////////////////////////////////////////////////////////////////////////////
    #region Functions
    public void Start() {
        if(showAtStart)
            InitializeLetterCloud(Text);
    }

    /// <summary>
    /// Initializes all variables using the text written on the inspector: OrderableText.cs
    /// </summary>
    public void InitializeLetterCloudSavedText()
    {
        InitializeLetterCloud(Text);
    }
    /// <summary> Initialize all variables needed for the orderable text to work </summary>
    /// <param name="text"> Text that should be written</param>
    public void InitializeLetterCloud(string text) {

        kPManager = new KerningPairsManager(spaceBetweenCharacters, showLogs);
        InstantiateAllCharacters();
        FormatText();

        Order();
    }

    /// <summary> Start the text ordering process </summary>
    public void Order() {
        StartCoroutine(nameof(Order_corut));
    }
    #endregion

// PRIVATE FUNCTIONS //////////////////////////////////////////////////////////////////////////////////////////
    #region Private functions

    /// <summary> Instantiates all characters needed to formed the desired text </summary>
    private void InstantiateAllCharacters() {
        Text = Text.Trim(); // Removes white spaces at the beginning or end of the text
        if (convertTextToAllUppercase) Text = Text.ToUpper(); // Convert all text to capital letter

        char[] segmentedText = Text.ToCharArray(); // transform text into a char[] to access each position more easily

        // Instantiates array that holds all the character instances
        characterArray = new Character[Text.Length];

        for (int i = 0; i < segmentedText.Length; i++) {
            // Instantiates character, add character script to it and stores reference to character object
            GameObject go = InstantiateCharacter(segmentedText[i]).gameObject;
            Character character = go.AddComponent<Character>() as Character;
            Renderer rend = go.GetComponentInChildren<Renderer>();
            if (rend != null) {
                rend.material = mat;
            }

            // Spawn a box representing the bounds of the character
            if (showCharacterBounds) {
                Transform box;

                if (segmentedText[i] != ' ') box = Instantiate(CharacterBoundsVisualizerPrefab).transform;
                else box = Instantiate(SpaceBoundsVisualizerPrefab).transform;

                box.SetParent(character.transform);
                box.localPosition = new Vector3(0, 0.383f, 0);
            }

            if (character == null) Debug.LogError("Character couldn't be instantiated or does not have a Character script component");
            character.textCharacter = segmentedText[i]; // Set the glyph that the Character represent in the text
            characterArray[i] = character; // Add character to the array in this position
            // Note: spaces are also computed as instanced prefabs
        }
    }

    /// <summary> Calculate and assign the position of every character in the final text considering lines, spaces and kerning values </summary>
    private void FormatText() {
        if (characterArray == null) {
            Debug.LogError("Error: Array of character from text was null. Aborting. Did you use the method InstantiateAllCharacters() first?");
            return;
        }
        float currentPosition = 0;
        int currentLine = 0;
        int index = 0;

        while (index < characterArray.Length) {
            int aux = index;
            float previousPosition = currentPosition;

            while (aux < characterArray.Length && characterArray[aux].textCharacter != ' ') { // Read a whole word (until we finish reading array or find a space)
                currentPosition = calculateAndAssignNextPosition(aux, currentPosition, currentLine); // Calculate position of each letter and assign the value to it
                if(currentPosition - previousPosition > lineLenght) {
                    Debug.LogError("ERROR: Line length is too small!. At least one word is too long for the specified line length and does not fit in any line");
                    return;
                }
                aux++;

                if (aux == characterArray.Length) { // Last character (end of the text)
                    break; // We exit while loop and check if the last word fit in the current line
                }
            }

            if (currentPosition <= lineLenght) { // Word DOES fit in the current line, so we update the index and move to the next one
                // Mark the changes as 'permanent' updating index value;
                index = aux; // Update index to the last character set
                if (aux < characterArray.Length) { // The word has finished, but not the text, so this must be a space character
                    currentPosition = calculateAndAssignNextPosition(index, currentPosition, currentLine); // Set 'space' position and update current pos
                    index++; // Move to the next word
                }
            } else { // Word DOES NOT fit in the current line, so we move up to a new one and reassign its target position
                // Discard changes and start positioning the word on the next line
                currentLine++;
                currentPosition = 0;
            }
        }

        if(movementDuration <= 0) { // MovementDuration should never be 0 because we divide by it, so if it is equal 0, replace it for an small number instead
            movementDuration = 0.001f;
        }

        // Assign all configuration -----------------------------------------------------------------------
        foreach (Character c in characterArray) {
            c.movDuration = this.movementDuration;
            c.amplitude = this.arc_Amplitude;
            c.alpha = this.arc_alpha;
            c.beta = this.arc_beta;
            c.vectorToSpawnSpherePivot = spawnSpherePivot.localPosition;
            c.spawnSphereRadius = this.spawnSphereRadius; 
            c.rangeRandomRotation_min = this.RotationMin;
            c.rangeRandomRotation_max = this.RotationMax;
            c.rangeRandomScale_min = this.ScaleMin;
            c.rangeRandomScale_max = this.ScaleMin;
            c.target_eulerAngles = this.finalRotation;
            c.target_scale = this.finalScale;
            c.init(); // Initialize characters
        }
    }

    /// <summary> Calculate next position for a character in the text given the current state </summary>
    /// <param name="index"> Index of the current position in the characterArray </param>
    /// <param name="currentPosition"> Current position in the world </param>
    /// <param name="currentLine"> Current line in the text </param>
    /// <returns> Next position based on kerning, spacing and line occupation </returns>
    public float calculateAndAssignNextPosition(int index, float currentPosition, int currentLine) {
        // Set the target position to the current character
        characterArray[index].target_position = new Vector3(currentPosition, -currentLine * spaceBetweenLines, 0.0f);

        // If there is a valid next character, calculate what should be the next position
        if (index + 1 < characterArray.Length) {
            float scale = finalScale.x; // When scaling character horizontally, we also need to scale the distance between them
            // Calculate next position
            currentPosition += (spaceBetweenCharacters) * (kPManager.SearchValue(characterArray[index].textCharacter, characterArray[index + 1].textCharacter)) * (scale);
        }
        return currentPosition;
    }

    private IEnumerator Order_corut() {
        foreach (Character character in characterArray) {
            character.startMoving();
            yield return new WaitForSeconds(timeBetweenCharMov);
        }
    }

    /// <summary> Instantiate a prefab corresponding with the given character </summary>
    /// <param name="c"> Character to instantiate </param>
    /// <returns> Instanced transform of the given character. '@' for non implemented characters </returns>
    private Transform InstantiateCharacter(char c) {
        switch (c) {
            case ' ': return Instantiate(space, transform).transform;
            case '!': return Instantiate(exclam, transform).transform;
            case '\"': return Instantiate(quotedbl, transform).transform;
            case '#': return Instantiate(numbersign, transform).transform;
            case '$': return Instantiate(dollar, transform).transform;
            case '%': return Instantiate(percent, transform).transform;
            case '&': return Instantiate(ampersand, transform).transform;
            case '\'': return Instantiate(quotesingle, transform).transform;
            case '(': return Instantiate(parentleft, transform).transform;
            case ')': return Instantiate(parentright, transform).transform;
            case '*': return Instantiate(asterisk, transform).transform;
            case '+': return Instantiate(plus, transform).transform;
            case ',': return Instantiate(comma, transform).transform;
            case '-': return Instantiate(hyphen, transform).transform;
            case '.': return Instantiate(period, transform).transform;
            case '/': return Instantiate(slash, transform).transform;
            case '0': return Instantiate(zero, transform).transform;
            case '1': return Instantiate(one, transform).transform;
            case '2': return Instantiate(two, transform).transform;
            case '3': return Instantiate(three, transform).transform;
            case '4': return Instantiate(four, transform).transform;
            case '5': return Instantiate(five, transform).transform;
            case '6': return Instantiate(six, transform).transform;
            case '7': return Instantiate(seven, transform).transform;
            case '8': return Instantiate(eight, transform).transform;
            case '9': return Instantiate(nine, transform).transform;
            case ':': return Instantiate(colon, transform).transform;
            case ';': return Instantiate(semicolon, transform).transform;
            case '<': return Instantiate(less, transform).transform;
            case '=': return Instantiate(equal, transform).transform;
            case '>': return Instantiate(greater, transform).transform;
            case '?': return Instantiate(question, transform).transform;
            case '@': return Instantiate(at, transform).transform;
            case 'A': return Instantiate(upper_A, transform).transform;
            case 'B': return Instantiate(upper_B, transform).transform;
            case 'C': return Instantiate(upper_C, transform).transform;
            case 'D': return Instantiate(upper_D, transform).transform;
            case 'E': return Instantiate(upper_E, transform).transform;
            case 'F': return Instantiate(upper_F, transform).transform;
            case 'G': return Instantiate(upper_G, transform).transform;
            case 'H': return Instantiate(upper_H, transform).transform;
            case 'I': return Instantiate(upper_I, transform).transform;
            case 'J': return Instantiate(upper_J, transform).transform;
            case 'K': return Instantiate(upper_K, transform).transform;
            case 'L': return Instantiate(upper_L, transform).transform;
            case 'M': return Instantiate(upper_M, transform).transform;
            case 'N': return Instantiate(upper_N, transform).transform;
            case 'O': return Instantiate(upper_O, transform).transform;
            case 'P': return Instantiate(upper_P, transform).transform;
            case 'Q': return Instantiate(upper_Q, transform).transform;
            case 'R': return Instantiate(upper_R, transform).transform;
            case 'S': return Instantiate(upper_S, transform).transform;
            case 'T': return Instantiate(upper_T, transform).transform;
            case 'U': return Instantiate(upper_U, transform).transform;
            case 'V': return Instantiate(upper_V, transform).transform;
            case 'W': return Instantiate(upper_W, transform).transform;
            case 'X': return Instantiate(upper_X, transform).transform;
            case 'Y': return Instantiate(upper_Y, transform).transform;
            case 'Z': return Instantiate(upper_Z, transform).transform;
            case '[': return Instantiate(bracketleft, transform).transform;
            case '\\':return Instantiate(backslash, transform).transform;
            case ']': return Instantiate(bracketright, transform).transform;
            case '^': return Instantiate(circumflex, transform).transform;
            case '_': return Instantiate(underscore, transform).transform;
            case '`': return Instantiate(grave, transform).transform;
            case 'a': return Instantiate(lower_A, transform).transform;
            case 'b': return Instantiate(lower_B, transform).transform;
            case 'c': return Instantiate(lower_C, transform).transform;
            case 'd': return Instantiate(lower_D, transform).transform;
            case 'e': return Instantiate(lower_E, transform).transform;
            case 'f': return Instantiate(lower_F, transform).transform;
            case 'g': return Instantiate(lower_G, transform).transform;
            case 'h': return Instantiate(lower_H, transform).transform;
            case 'i': return Instantiate(lower_I, transform).transform;
            case 'j': return Instantiate(lower_J, transform).transform;
            case 'k': return Instantiate(lower_K, transform).transform;
            case 'l': return Instantiate(lower_L, transform).transform;
            case 'm': return Instantiate(lower_M, transform).transform;
            case 'n': return Instantiate(lower_N, transform).transform;
            case 'o': return Instantiate(lower_O, transform).transform;
            case 'p': return Instantiate(lower_P, transform).transform;
            case 'q': return Instantiate(lower_Q, transform).transform;
            case 'r': return Instantiate(lower_R, transform).transform;
            case 's': return Instantiate(lower_S, transform).transform;
            case 't': return Instantiate(lower_T, transform).transform;
            case 'u': return Instantiate(lower_U, transform).transform;
            case 'v': return Instantiate(lower_V, transform).transform;
            case 'w': return Instantiate(lower_W, transform).transform;
            case 'x': return Instantiate(lower_X, transform).transform;
            case 'y': return Instantiate(lower_Y, transform).transform;
            case 'z': return Instantiate(lower_Z, transform).transform;
            case '{': return Instantiate(braceleft, transform).transform;
            case '|': return Instantiate(bar, transform).transform;
            case '}': return Instantiate(braceright, transform).transform;
            case '~': return Instantiate(tilde, transform).transform;
            case 'ñ': return Instantiate(lower_Ntilde, transform).transform;
            case 'Ñ': return Instantiate(upper_Ntilde, transform).transform;
            case 'ç': return Instantiate(lower_Ccedilla, transform).transform;
            case 'Ç': return Instantiate(upper_Ccedilla, transform).transform;
            case '´': return Instantiate(acute, transform).transform;
            case 'Á': return Instantiate(upper_Aacute, transform).transform;
            case 'É': return Instantiate(upper_Eacute, transform).transform;
            case 'Í': return Instantiate(upper_Iacute, transform).transform;
            case 'Ó': return Instantiate(upper_Oacute, transform).transform;
            case 'Ú': return Instantiate(upper_Uacute, transform).transform;
            case 'À': return Instantiate(upper_Agrave, transform).transform;
            case 'È': return Instantiate(upper_Egrave, transform).transform;
            case 'Ì': return Instantiate(upper_Igrave, transform).transform;
            case 'Ò': return Instantiate(upper_Ograve, transform).transform;
            case 'Ù': return Instantiate(upper_Ugrave, transform).transform;
            case 'Â': return Instantiate(upper_Acircum, transform).transform;
            case 'Ê': return Instantiate(upper_Ecircum, transform).transform;
            case 'Î': return Instantiate(upper_Icircum, transform).transform;
            case 'Ô': return Instantiate(upper_Ocircum, transform).transform;
            case 'Û': return Instantiate(upper_Ucircum, transform).transform;
            case 'Ä': return Instantiate(upper_Adiaere, transform).transform;
            case 'Ë': return Instantiate(upper_Ediaere, transform).transform;
            case 'Ï': return Instantiate(upper_Idiaere, transform).transform;
            case 'Ö': return Instantiate(upper_Odiaere, transform).transform;
            case 'Ü': return Instantiate(upper_Udiaere, transform).transform;
            case 'á': return Instantiate(lower_Aacute, transform).transform;
            case 'é': return Instantiate(lower_Eacute, transform).transform;
            case 'í': return Instantiate(lower_Iacute, transform).transform;
            case 'ó': return Instantiate(lower_Oacute, transform).transform;
            case 'ú': return Instantiate(lower_Uacute, transform).transform;
            case 'à': return Instantiate(lower_Agrave, transform).transform;
            case 'è': return Instantiate(lower_Egrave, transform).transform;
            case 'ì': return Instantiate(lower_Igrave, transform).transform;
            case 'ò': return Instantiate(lower_Ograve, transform).transform;
            case 'ù': return Instantiate(lower_Ugrave, transform).transform;
            case 'â': return Instantiate(lower_Acircum, transform).transform;
            case 'ê': return Instantiate(lower_Ecircum, transform).transform;
            case 'î': return Instantiate(lower_Icircum, transform).transform;
            case 'ô': return Instantiate(lower_Ocircum, transform).transform;
            case 'û': return Instantiate(lower_Ucircum, transform).transform;
            case 'ä': return Instantiate(lower_Adiaere, transform).transform;
            case 'ë': return Instantiate(lower_Ediaere, transform).transform;
            case 'ï': return Instantiate(lower_Idiaere, transform).transform;
            case 'ö': return Instantiate(lower_Odiaere, transform).transform;
            case 'ü': return Instantiate(lower_Udiaere, transform).transform;
            default : return Instantiate(at, transform).transform;
        }
    }
    #endregion
}