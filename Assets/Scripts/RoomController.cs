using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] GameObject[] targets;
    [SerializeField] GameObject[] bombs;
    [SerializeField] TextController scriptController;
    [SerializeField] private string[] script;
    [SerializeField] private Animation cutscenes;
    private CutsceneController cutsceneController;
    public TypewriteEffect typewriter;
    private bool typing;
    private bool speechStarted = false;
    private int speechIndex = 0;
    private float speechTimer = 0;
    private int goal;
    private int points;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        typewriter = GameObject.FindWithTag("Typewriter").GetComponent<TypewriteEffect>();
        goal = targets.Length;

        for (int i = 0; i < bombs.Length; i++)
        {
            BombController bc = bombs[i].GetComponent<BombController>();
            if (bc != null)
            {
                bc.roomOwner(gameObject);
            }
        }
    }

    public void initializeCutscene(CutsceneController cc)
    {
        cutsceneController = cc;
        string newCutscene = cutsceneController.getCutsceneName();
        CutsceneController thiscc = GetComponent<CutsceneController>();

        thiscc.setupCutscene(cc.getCutsceneName(), cc.getAllCameras(), cc.getPlayerCamera());
        cutscenes.Play(newCutscene);

    }

    public void endGame()
    {
        GameController gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        gc.GameEnd();
    }

    public Transform getSpawnPoint()
    {
        return spawnPoint;
    }
    public void initializeSpeech(TextController speech)
    {
        if (speechStarted)
        {
            speechStarted = false;
            typing = false;
            script = null;
            typewriter.DropBubble();
        }
        script = speech.getMessages();
        startSpeech();
    }
    
    public bool isTalking()
    {
        return speechStarted;
    }

    public int checkPoints()
    {
        return points;
    }

    public void startSpeech()
    {

            speechStarted = true;
            typing = false;
            speechIndex = 0;
    }

    public void addPoint()
    {
        points += 1;

        //if (points >= goal)
        //{
        //    GameObject door = GameObject.FindWithTag("Door");
        //    CutsceneController cc = door.GetComponent<CutsceneController>();


        //    initializeCutscene(cc);
        //}
    }

    public int getIndex()
    {
        return speechIndex;
    }

    public int checkGoal()
    {
        return goal;
    }

    // Update is called once per frame
    void Update()
    {
        if (script != null)
        {
            if (speechStarted == true)
            {
                if (typewriter.checkSkip() == true)
                {

                    if (speechIndex >= script.Length)
                    {
                        typewriter.StopText();
                        typewriter.DropBubble();
                        speechStarted = false;
                        
                    }
                    else
                    {

                        typewriter.ShowText(script[speechIndex]);
                        speechIndex++;

                        typing = true;

                    }
                    typewriter.SkipText(false);
                    speechTimer = 0;
                }
                if (!typing)
                {
                    if (speechTimer <= 0)
                    {
                        if (speechIndex >= script.Length)
                        {
                            typewriter.DropBubble();
                            speechStarted = false;
                            typewriter.StopText();
                        }
                        else
                        {

                            typewriter.ShowText(script[speechIndex]);
                            speechIndex++;

                            typing = true;

                        }

                    }
                    else if (speechTimer > 0)
                    {
                        speechTimer -= Time.deltaTime;
                    }
                }
                else if (typing)
                {
                    if (typewriter.checkTyping() == false)
                    {
                        speechTimer = 3f;
                        typing = false;
                    }
                }
            } else if (speechStarted == false)
            {
                script = null;
            }

        }
    }
}
