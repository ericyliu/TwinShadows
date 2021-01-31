using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class Dialogue : MonoBehaviour
{

    #region Speakers
    // Pitch value is out of 100
    public enum Speaker
    {
        Meta = 20,
        Dad = 50,
        Son = 51,
        Ghost = 70,

        Count = -1
    }

    /*
    // not in use
    private static readonly Dictionary<Speaker, float> speakerPitchBends = new Dictionary<Speaker, float> {
        // { Name, average pitch }
        { Speaker.A, 1 },
        { Speaker.B, 2 }
    };
    */
    #endregion


    #region Conversation
    [System.Serializable]
    public class Conversation
    {

        [System.Serializable]
        public struct Line
        {
            [XmlAttribute("speaker")]
            public Speaker speaker;

            [XmlAttribute("nameAlignment")]
            public TextAnchor nameAlignment;

            [XmlAttribute("nameHidden")]
            public bool nameHidden;

            [XmlAttribute("waits")]
            public bool waits;

            [XmlAttribute("text")]
            public string text;
        }

        [SerializeField]
        [XmlArray("Speakers")]
        [XmlArrayItem("Speaker")]
        public List<Speaker> speaker;

        [SerializeField]
        [XmlArray("Lines")]
        [XmlArrayItem("Line")]
        public List<Line> lines;

        private int index = 0;

        public bool GetNextLine(out Line line) {
            bool isValid = (lines.Count > index);
            line = isValid ? lines[index] : new Line();
            index++;
            return isValid;
        }

        public void Reset() {
            index = 0;
        }

        public static Conversation LoadXml(string path) {

            XmlSerializer serializer = new XmlSerializer(typeof(Conversation));
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                return (Conversation)serializer.Deserialize(stream);
            }
        }
    }
#endregion
    public Stack<Conversation> conversationStack;

    // Control
    public string file = "test";

    public enum State
    {
        Idle,
        Speaking, // Open widget while line is going
        WaitingForResponse, // The player selecting a response will trigger the next line
        WaitingForEvent, // Something external (like animation) will trigger the next line
        WaitingForNextLine, // Pressing the continue button will trigger the next line
        Finished // Signal to UI that we area ready to close
    }
    public State state;// { get; private set; }

    // Text speed
    public const float textSpeedSlow = 0.5f;
    public const float textSpeedNormal = 1.0f;
    public const float textSpeedFast = 2.0f;
    public static float globalTextSpeed = textSpeedNormal;

    public float charPerSec = 1.0f;
    private float currSpeed = 1.0f;
    public float realCharPerSec = 1;
    private float nextCharTimer = 0;
    private bool fastForward = false;

    public Conversation.Line pendingLine;
    private StringBuilder readyLine = new StringBuilder();
    private Stack<string> unclosedTags = new Stack<string>();


    // Audio
    private AudioSource audioSource;
    private float wordPitchBend = 0;
    public List<AudioClip> vowelClips = new List<AudioClip>();
    public List<AudioClip> consonantClips = new List<AudioClip>();

    private void Start() {
        audioSource = GetComponent<AudioSource>();

        if (vowelClips.Count > 0 && consonantClips.Count > 0)
            Debug.Log("no consonants or maybe vowels!");

        UpdateTextSpeed(5);
        LoadJSON(file);
    }


    private void LoadJSON(string file) {
        string path = Path.Combine(Application.dataPath, "Dialogue/" + file + ".xml");
        conversationStack = new Stack<Conversation>();
        Conversation c = Conversation.LoadXml(path);

        conversationStack.Push(c);
    }

    private void Update() {

        if (state != State.Speaking)
            return;

        if (fastForward) {
            while (pendingLine.text.Length > 0) {
                ParseNextCharacter();
            }
            state = State.WaitingForNextLine;
            return;
        }

        if (nextCharTimer > 0) {
            nextCharTimer -= Time.deltaTime;
        }
        else if (pendingLine.text.Length == 0) {
            state = State.WaitingForNextLine;
        }
        else {
            nextCharTimer += 1.0f / realCharPerSec;

            char next = ParseNextCharacter();

            float perlinPitchBend = Mathf.PerlinNoise(1, Time.time);
            if (audioSource)
                audioSource.pitch = 0.3f + ((int)pendingLine.speaker / 100.0f) + perlinPitchBend + wordPitchBend;

            if (char.IsLetter(next)) {
                switch (char.ToLower(next)) {
                    case 'a':
                    case 'e':
                    case 'i':
                    case 'o':
                    case 'u':
                    case 'y':
                        if (vowelClips.Count > 0)
                            audioSource.PlayOneShot(vowelClips[Random.Range(0, vowelClips.Count)]);
                        break;

                    default:
                        if (consonantClips.Count > 0)
                            audioSource.PlayOneShot(consonantClips[Random.Range(0, consonantClips.Count)]);
                        break;
                }
            }
            else {
                wordPitchBend = Random.Range(-0.2f, 0.2f);
            }
        }
    }

    private char ParseNextCharacter() {

        char next = pendingLine.text[0];
        while (next == '\\' || next == '<') {
            next = pendingLine.text[0];
            switch (next) {
                case '\\':
                    if (pendingLine.text[1] == 'n') {
                        readyLine.Append('\n');
                    }
                    else {
                        UpdateTextSpeed((pendingLine.text[1]) - '0');
                    }
                    pendingLine.text = pendingLine.text.Remove(0, 2);
                    break;

                case '<':
                    int index = pendingLine.text.IndexOf('>');
                    string tag = pendingLine.text.Substring(0, index + 1);
                    readyLine.Append(tag);
                    pendingLine.text = pendingLine.text.Remove(0, index + 1);

                    // Deal with closing tags
                    if (tag[1] == '/') {
                        unclosedTags.Pop();
                    }
                    else {
                        int equals = tag.IndexOf('=');
                        if (equals != -1) {
                            int closeChevronIndex = tag.IndexOf('>');
                            tag = tag.Remove(equals, closeChevronIndex - equals);
                        }
                        tag = tag.Insert(1, "/");
                        unclosedTags.Push(tag);
                    }
                    break;
            }
        }

        // Add next char to readyLine
        readyLine.Append(next);
        pendingLine.text = pendingLine.text.Remove(0, 1);

        return next;
    }

    // speed = 0-9, 2xMulti?
    private void UpdateTextSpeed(int speed) {
        currSpeed = speed;

        realCharPerSec = globalTextSpeed * charPerSec * speed;
        nextCharTimer = 1.0f / realCharPerSec;
    }

    public void Interact() {
        switch (state) {

            case State.Speaking:
                fastForward = true;
                break;

            case State.Idle:
            case State.WaitingForNextLine:
                fastForward = false;
                SpeakNextLine();
                break;

            case State.Finished:
                break;
        }
    }

    public void Reset() {
        conversationStack.Peek().Reset();
        state = State.Idle;
    }

    public void SpeakNextLine() {
        if (conversationStack.Peek().GetNextLine(out pendingLine)) {
            state = State.Speaking;
            readyLine = new StringBuilder();
        }
        else {
            state = State.Finished;
        }
    }

    public string Say() {
        string line = "";
        line = readyLine.ToString();
        foreach (string s in unclosedTags) {
            line += s;
        }
        return line;
    }

}
