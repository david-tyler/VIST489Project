using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MazeBehavior : MonoBehaviour
{
    #region Singleton
    public static MazeBehavior instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of MazeBehavior!");
            return;
        }

        instance = this;
    }
    #endregion
    public Material goodSquare;
    public Material badSquare;
    public Material PlatformOriginalMat;
    public Material falsePlatformMaterial;


    public GameObject firstMaze;
    public GameObject secondMaze;
   

    public AudioSource goalReachedAudio;
    public TMPro.TextMeshProUGUI timerText;

    public Button PopUpBoxButton;

    public GameObject Timer;
    public float remainingTime = 20.0f;

    // so you cant cheat the game and go to the end
    public int landGoodSquaresBeforeGoal = 5;
    private int goodSquaresLanedCurrent = 0;

    // ******* PopUp Boxes
    // --------------------------------------

    public Animator zoroarkStanding;
    public Animator leftKlefkiAnimation;
    public Animator rightKlefkiAnimation;

    public GameObject zoroark;
    public GameObject leftKlefki;
    public GameObject rightKlefki;
    public GameObject escapeButton;
    

    public string WrongSquareMessage = "Oh no! Looks like you stepped on the wrong platform. " +
        "You have to scan the key and start over again.";
    public string RanOutOfTimeMessage = "You ran out of time. Try scanning the key again to start over.";
    public string YouWonMessage = "Congrats! Looks like you solved the maze. Try scanning the key again, we should be able to pick it up now.";
    // --------------------------------------

    private List<Collider> platforms = new List<Collider>();
    private bool startTimer = false;
    private bool goalReached = false;
    private float originaltime;
    private bool failed = false;
    private bool won = false;
    private bool enteredMaze;

    public List<string> zoroarkAppearsLines = new List<string>();

    GameSystemBehavior GameSystem;
    ParaLensesButtonBehavior paraLenses;
    PopUpSystem popUp;
    PokemonWorld pokeWorld;
    AudioManager audioManagerScript;

    MazeSelector firstMazeScript;
    MazeSelector secondMazeScript;
    

    public AudioSource goodSquareSound;
    public AudioSource badSquareSound;

    public string ZoroarkEncounterAudioName;
    public string backgroundMusicName;

    void Start()
    {
        originaltime = remainingTime;
        enteredMaze = false;
        
    }

    void Update()
    {
        if (startTimer == true && failed == false && won == false)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                
            }
            else if (remainingTime < 0)
            {
                // Game over if we reach 0
                remainingTime = 0;

                if (goalReached == false)
                {
                    // you lose start over
                    timerText.color = Color.red;
                    FailedMaze(RanOutOfTimeMessage);
                    
                }
            }

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


        }


    }



    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        

        if (tag == "Maze Platform")
        {
            MazeSelector currentMazeScript = other.gameObject.GetComponentInParent<MazeSelector>();

            // once we choose to step on a maze platform for the first time set the other one to false
            if (enteredMaze == false)
            {
                if (currentMazeScript.GetMazeTag() == "First Maze")
                {
                    if (secondMaze.activeSelf == true)
                    {
                        secondMaze.SetActive(false);
                        rightKlefkiAnimation.SetTrigger("Disappear");
                    }
                    
                }
                else if (currentMazeScript.GetMazeTag() == "Second Maze")
                {
                    if (firstMaze.activeSelf == true)
                    {
                        firstMaze.SetActive(false);

                        leftKlefkiAnimation.SetTrigger("Disappear");
                    }
                }
                enteredMaze = true;
            }
            

            // if you havent failed yet can keep trying. Used so that if you hit a bad square you wont be able to keep colliding with other squares.
            if (failed == false && won == false)
            {
                // change the current platform to green or red
                if (other.gameObject.GetComponent<Platform>().isValidSquare == true)
                {
                    goodSquaresLanedCurrent += 1;
                    if (other.gameObject.GetComponent<Platform>().isGoal == true)
                    {
                        if (goodSquaresLanedCurrent >= landGoodSquaresBeforeGoal)
                        {
                            goalReached = true;
                            if (currentMazeScript.GetIsReal() == true)
                            {
                                
                                won = true;
                                // You win can play a sound and now can spawn the key and get it
                                goalReachedAudio.PlayOneShot(goalReachedAudio.clip);
                                WonMaze(YouWonMessage);
                                ResetMaze();
                            }
                            else if (currentMazeScript.GetIsReal() == false)
                            {
                                
                                FailedMaze(zoroarkAppearsLines[0]);
                            }
                            
                        }
                        else if (goodSquaresLanedCurrent < landGoodSquaresBeforeGoal)
                        {
                            startTimer = false;
                            FailedMaze("Nice Try.");
                            
                        }

                        return;

                    }
                    else if (other.gameObject.GetComponent<Platform>().isGoal == false)
                    {
                        goodSquareSound.PlayOneShot(goodSquareSound.clip);
                        platforms.Add(other);
                        other.transform.parent.gameObject.GetComponent<Renderer>().material = goodSquare;
                    }


                }
                else if (other.gameObject.GetComponent<Platform>().isValidSquare == false)
                {
                   
                    platforms.Add(other);
                    other.transform.parent.gameObject.GetComponent<Renderer>().material = badSquare;
                    startTimer = false;
                    
                    FailedMaze(WrongSquareMessage);
                    
                }
            }
        }
        
        

        
    }
    
    // add this method to button onclick for the popup button
    public void ResetMaze()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].transform.parent.gameObject.GetComponent<Renderer>().material = PlatformOriginalMat;
        }
        platforms.Clear();

        List<GameObject> firstMazePlatforms = GetChildren(firstMaze);
        List<GameObject> secondMazePlatforms = GetChildren(secondMaze);

        foreach (GameObject platform in firstMazePlatforms)
        {
            platform.gameObject.GetComponent<Renderer>().material = PlatformOriginalMat;
        }

        foreach (GameObject platform in secondMazePlatforms)
        {
            platform.gameObject.GetComponent<Renderer>().material = PlatformOriginalMat;
        }

        enteredMaze = false;
        firstMaze.SetActive(true);
        secondMaze.SetActive(true);
        
        remainingTime = originaltime;
        
        goodSquaresLanedCurrent = 0;
        AssignRandomMaze();

        if (zoroark.activeSelf == true)
        {
            zoroark.SetActive(false);
            zoroarkStanding.SetTrigger("Idle");
        }

        leftKlefki.SetActive(true);
        rightKlefki.SetActive(true);

    }

    public void MazeStarted()
    {
        if (failed == true)
        {
            ResetMaze();
        }
        Timer.SetActive(true);
        timerText.color = Color.black;
        startTimer = true;
        failed = false;
    }

    public void WonMaze(string text)
    {
        GameSystem = GameSystemBehavior.instance;
        paraLenses = ParaLensesButtonBehavior.instance;


        GameSystem.SetHaveMessage(true);
        GameSystem.SetMessageText(text);

        startTimer = false;
        
        pokeWorld = PokemonWorld.instance;

        pokeWorld.SolvedMaze();




        Timer.SetActive(false);
        firstMaze.SetActive(false);
        secondMaze.SetActive(false);
        leftKlefki.SetActive(false);
        rightKlefki.SetActive(false);
        
        
    }


    public void FailedMaze(string text)
    {
        badSquareSound.PlayOneShot(badSquareSound.clip);
        GameSystem = GameSystemBehavior.instance;
        popUp = PopUpSystem.instance;

        GameSystem.SetHaveMessage(true);
        

        failed = true;
        startTimer = false;
        Timer.SetActive(false);
        if ( goalReached == false)
        {
            GameSystem.SetMessageText(text);
            Timer.SetActive(false);
        }
        else if (goalReached == true)
        {
            popUp.PopUp(text);
            PopUpBoxButton.onClick.AddListener(ZoroarkAppear);
            
            

        }

    }

    private void ZoroarkAppear()
    {
        audioManagerScript = AudioManager.instance;
        audioManagerScript.PlayEventSound(ZoroarkEncounterAudioName);
        PopUpBoxButton.onClick.RemoveAllListeners();

        firstMazeScript = firstMaze.GetComponent<MazeSelector>();
        secondMazeScript = secondMaze.GetComponent<MazeSelector>();
        
        
        

        
        // tell user to click escape button so they can run away then prompt them to try tapping the key again.
        StartCoroutine(EscapeFromZoroark());


    }

    IEnumerator EscapeFromZoroark()
    {
        if (firstMazeScript.GetIsReal() == false)
        {
            leftKlefkiAnimation.SetTrigger("Disappear");
        }
        else if (secondMazeScript.GetIsReal() == false)
        {
            rightKlefkiAnimation.SetTrigger("Disappear");
        }
        yield return new WaitForSeconds(2);

        zoroark.SetActive(true);
        zoroarkStanding.SetTrigger("StartCrouch");
        GameSystem = GameSystemBehavior.instance;
        // maybe play an audio showing zoroark appears
        yield return new WaitForSeconds(2);

        GameSystem.SetHaveMessage(true);
        GameSystem.SetMessageText(zoroarkAppearsLines[1]);
        escapeButton.SetActive(true);


    }

    public void ClickedEscape()
    {
        audioManagerScript = AudioManager.instance;
        audioManagerScript.PlayEventSound(backgroundMusicName);
        escapeButton.SetActive(false);
        
        

        ResetMaze();
        leftKlefkiAnimation.SetTrigger("Appear");
    
        rightKlefkiAnimation.SetTrigger("Appear");
        
    }

    // personal preference think would be nice to have the timer still show if you fail before resetting it
    public bool getFailed()
    {
        return failed;

    }

    private void AssignRandomMaze()
    {

        int randomNumber = Random.Range(0, 2);
        firstMazeScript = firstMaze.GetComponent<MazeSelector>();
        secondMazeScript = secondMaze.GetComponent<MazeSelector>();
        if (randomNumber == 0)
        {
            firstMazeScript.SetIsReal(true);
            secondMazeScript.SetIsReal(false);
            setFalzeMazePlatformMaterial(secondMaze);

            zoroark.transform.localEulerAngles = new Vector3(0, 61.9f, 0);
            zoroark.transform.position = new Vector3(rightKlefki.transform.position.x, zoroark.transform.position.y, rightKlefki.transform.position.z);
           
        }
        else
        {
            firstMazeScript.SetIsReal(false);
            secondMazeScript.SetIsReal(true);
            setFalzeMazePlatformMaterial(firstMaze);
            zoroark.transform.localEulerAngles = new Vector3(0, 242.5f, 0);
            zoroark.transform.position = new Vector3(leftKlefki.transform.position.x, zoroark.transform.position.y, leftKlefki.transform.position.z);
            
        }
        Debug.Log($"First Maze is: {firstMazeScript.GetIsReal()} and Second Maze is: {secondMazeScript.GetIsReal()} ");
        

    }

    private void setFalzeMazePlatformMaterial(GameObject maze)
    {
        List<GameObject> falsePlatforms = GetChildren(maze);

        int randomNumber = Random.Range(0, falsePlatforms.Count - 1);
        
        falsePlatforms[randomNumber].gameObject.GetComponent<Renderer>().material = falsePlatformMaterial;
    }
    private List<GameObject> GetChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();

        // Iterate through all child transforms
        foreach (Transform child in parent.transform)
        {
            // Add the child GameObject to the list
            children.Add(child.gameObject);
        }

        return children;
    }

}
