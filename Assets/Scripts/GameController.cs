using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("PlayerData")]
    [SerializeField] private MouseController mouseController;
    [SerializeField] private TypewriteEffect typewriter;
    [SerializeField] private GameObject player;


    [Header("Controls")]
    [SerializeField] private PlayerCamera playerCamera;

    [Header("LevelData")]
    [SerializeField] private GameObject[] rooms;
    private int currentLevel = 0;

    private GameObject room0;
    private GameObject room1;
    private GameObject room2;
    private GameObject room3;



    [Header("UI Stuff")]
    [SerializeField] private Button[] sensButtons;
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas mainOptions;
    [SerializeField] private Canvas fadeScreen;
    [SerializeField] private Canvas pausePage;
    [SerializeField] private Canvas tipsPage;
    [SerializeField] private Canvas gameOptions;
    [SerializeField] private Canvas deadScreen;
    [SerializeField] private Canvas gameOver;
    [SerializeField] private Texture[] tips;
    [SerializeField] private RawImage tooltip;

    [Header("AudioStuff")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip select;

    private bool fading = false;
    private int unlockedTips = 2;
    private int currentTip = 0;
    private bool paused = false;


    private int currentSens;
    private Coroutine activeCoroutine;

    public void sensButton(int button)
    {
        source.PlayOneShot(select);
        if (fading == false)
        {
            Vector3 baseSize = new Vector3(1.5f, 1.5f, 1.5f);
            Vector3 selectSize = new Vector3(2, 2, 2);


            GameObject oldButton = sensButtons[currentSens].gameObject;
            GameObject newButton = sensButtons[button].gameObject;

            oldButton.transform.localScale = baseSize;
            newButton.transform.localScale = selectSize;

            currentSens = button;

            playerCamera.SetSensitivity(button);
        }
    }


    public void nextTip(int nextIndex)
    {
        if (fading == false)
        {
            fading = true;
            GameObject tt = tipsPage.gameObject;
            Animation anim = tt.GetComponent<Animation>();
            AnimationClip clip = anim.GetClip("NextTipOut");
            float clipLength = clip.length;

            if (currentTip + nextIndex > currentTip)
            {

                if (currentTip == unlockedTips)
                {
                    currentTip = 0;
                }else
                {
                    currentTip += nextIndex;
                }
                    activeCoroutine = StartCoroutine(TipChange(currentTip, "next", clipLength));

            }
            else if (currentTip + nextIndex < currentTip)
            {
                if (currentTip + nextIndex < 0)
                {
                    currentTip = unlockedTips;
                }
                else
                {
                    currentTip += nextIndex;
                }
                    activeCoroutine = StartCoroutine(TipChange(currentTip, "previous", clipLength));
            }
        }
    }


    public void closeAllPages()
    {
        mainMenu.gameObject.SetActive(false);
        mainOptions.gameObject.SetActive(false);
        pausePage.gameObject.SetActive(false);
        tipsPage.gameObject.SetActive(false);
        gameOptions.gameObject.SetActive(false);
        deadScreen.gameObject.SetActive(false);
    }


    public void SetRoom(int setRoom)
    {
        currentLevel = setRoom;
    }
    public void ChangePage(string page)
    {
        if (fading == false)
        {
            if (page == "Pause")
            {
                activeCoroutine = StartCoroutine(Pause());




            }else if (page == "Continue")
            {
                activeCoroutine = StartCoroutine(UnPause());
            }
            else
            {
                fading = true;
                activeCoroutine = StartCoroutine(FadeOut(page));
            }
        }


    }

    private IEnumerator TipChange(int cTip, string dir, float time)
    {
        GameObject tt = tipsPage.gameObject;
        Animation anim = tt.GetComponent<Animation>();
        if (dir == "next")
        {
            anim.Play("NextTipOut");
        }else if (dir == "previous")
        {
            anim.Play("PreviousTipOut");
        }

            yield return new WaitForSeconds(time);
        tooltip.texture = tips[cTip];
        if (dir == "next")
        {
            anim.Play("NextTipIn");
        }
        else if (dir == "previous")
        {
            anim.Play("PreviousTipIn");
        }
        yield return new WaitForSeconds(time);
        fading = false;
        activeCoroutine = null;
    }

    private IEnumerator Pause()
    {
        paused = true;
        pausePage.gameObject.SetActive(true);
        player.SetActive(false);

        Animation anim = pausePage.GetComponent<Animation>();

        AnimationClip pause = anim.GetClip("Pause");

        anim.Play("Pause");
        yield return null;

        float pauseLength = pause.length;

        yield return new WaitForSeconds(pauseLength);

        playerCamera.MouseOn();
        activeCoroutine = null;
        

    }

    private IEnumerator UnPause()
    {
        playerCamera.MouseOff();
        

        Animation anim = pausePage.GetComponent<Animation>();

        AnimationClip pause = anim.GetClip("UnPause");

        anim.Play("UnPause");
        yield return null;

        float pauseLength = pause.length;

        yield return new WaitForSeconds(pauseLength);

        player.SetActive(true);
        activeCoroutine = null;
        pausePage.gameObject.SetActive(false);
        paused = false;

    }

    public void GameEnd()
    {
        closeAllPages();
        gameOver.gameObject.SetActive(true);
        playerCamera.MouseOn();
    }

    public IEnumerator FadeOut(string page)
    {
        source.PlayOneShot(select);
        Animation anim = fadeScreen.GetComponent<Animation>();

        AnimationClip fade = anim.GetClip("FadeOut");
        anim.Play("FadeOut");

        yield return null;

        float fadeLength = fade.length;
        yield return new WaitForSeconds(fadeLength);
        closeAllPages();


        if (page == "MainOptions")
        {
            mainOptions.gameObject.SetActive(true);
        } else if (page == "MainMenu")
        {
            mainMenu.gameObject.SetActive(true);
            
        } else if (page == "GameStart") {
            initializeAllRooms();
            paused = false;
        }
        else if (page == "Back")
        {
            pausePage.gameObject.SetActive(true);
        }else if (page == "Tips")
        {
            tipsPage.gameObject.SetActive(true);
        }else if (page == "GameOptions")
        {
            gameOptions.gameObject.SetActive(true);
        }else if( page == "Dead")
        {
            deadScreen.gameObject.SetActive(true);
        }else if (page == "Revive")
        {
            deadScreen.gameObject.SetActive(false);
            playerCamera.MouseOff();
            ResetRoom(currentLevel);
        }


            fade = anim.GetClip("FadeIn");
        anim.Play("FadeIn");
        yield return null;

        fadeLength = fade.length;
        yield return new WaitForSeconds(fadeLength);
        if (page == "GameStart")
        {
            player.SetActive(true);
            playerCamera.MouseOff();
        }
        fading = false;
        activeCoroutine = null;


    }

    public void Die()
    {
        player.SetActive(false);
        playerCamera.MouseOn();
        ChangePage("Dead");
    }

    public void AddTips(int number)
    {
        unlockedTips += number;
        if (unlockedTips > tips.Length)
        {
            unlockedTips = tips.Length-1;
        }
    }

    public void initializeGameStart()
    {
        playerCamera.MouseOn();
        player.SetActive(false);
        
    }
    void Start()
    {
        initializeGameStart();
    }

    public void ResetRoom(int cRoom)
    {
        RoomController roomController = rooms[cRoom].GetComponent<RoomController>();
        if (cRoom == 0)
        {
            Destroy(room0);
            room0 = Instantiate(rooms[cRoom]);
        } else if (cRoom == 1)
        {
            Destroy(room1);
            room1 = Instantiate(rooms[cRoom]);
        }
        else if (cRoom == 2)
        {
            Destroy(room2);
            room2 = Instantiate(rooms[cRoom]);
        }
        else if (cRoom == 3)
        {
            Destroy(room3);
            room3 = Instantiate(rooms[cRoom]);
        }

        player.transform.position = roomController.getSpawnPoint().position;
        player.SetActive(true);
        
    }

    public bool IsPaused()
    {
        return paused;
    }



    public void initializeAllRooms()
    {
        Destroy(room0);
        Destroy(room1);
        Destroy(room2);
        Destroy(room3);
        currentLevel = 0;
        RoomController roomController = rooms[0].GetComponent<RoomController>();
        room0 = Instantiate(rooms[0]);
        room1 = Instantiate(rooms[1]);
        room2 = Instantiate(rooms[2]);
        room3 = Instantiate(rooms[3]);
        player.transform.position = roomController.getSpawnPoint().position;
    }
    // Update is called once per frame
    void Update()
    {
        if (typewriter != null)
        {
            if (typewriter.checkActive() == true)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    source.PlayOneShot(select);
                    typewriter.SkipText(true);
                }

            }else if (typewriter.checkActive() == false)
            {
                if (player.activeSelf)
                {
                    if (activeCoroutine == null)
                    {
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            ChangePage("Pause");
                        }
                    }

                }
            }
        }

    }
}
