using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBehavior : MonoBehaviour
{
    public Material goodSquare;
    public Material badSquare;
    public Material PlatformOriginalMat;
    public GameObject entireMaze;
    public AudioSource goalReachedAudio;
    public TMPro.TextMeshProUGUI timerText;
    public GameObject Timer;
    public float remainingTime = 20.0f;

    // so you cant cheat the game and go to the end
    public int landGoodSquaresBeforeGoal = 5;
    private int goodSquaresLanedCurrent = 0;

    // ******* PopUp Boxes
    // --------------------------------------
    PopUpSystem popUp;
    public GameObject PopUpBox; // Our dialogue box
    public Animator PopUpBoxAnimator;
    public TMPro.TextMeshProUGUI PopUpBoxText;
    
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


    GameSystemBehavior GameSystem;
    ParaLensesButtonBehavior paraLenses;

    void Start()
    {
        originaltime = remainingTime;
    }

    void Update()
    {
        if (startTimer == true)
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
                        won = true;
                        // You win can play a sound and now can spawn the key and get it
                        StartCoroutine(SolvedPuzzle());
                        WonMaze(YouWonMessage);
                        ResetMaze();
                    }
                    else if (goodSquaresLanedCurrent < landGoodSquaresBeforeGoal)
                    {
                        startTimer = false;
                        FailedMaze("Nice Try.");
                        ResetMaze();
                    }

                    return;

                }
                else if (other.gameObject.GetComponent<Platform>().isGoal == false)
                {
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

    IEnumerator SolvedPuzzle()
    {
        goalReachedAudio.Play();
        yield return new WaitForSeconds(3);
    }

    
    // add this method to button onclick for the popup button
    public void ResetMaze()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].transform.parent.gameObject.GetComponent<Renderer>().material = PlatformOriginalMat;
        }
        platforms.Clear();
        entireMaze.SetActive(true);
        
        startTimer = false;
        remainingTime = originaltime;
        Timer.SetActive(false);
        goodSquaresLanedCurrent = 0;
        failed = false;

    }

    public void MazeStarted()
    {
        Timer.SetActive(true);
        timerText.color = Color.black;
        startTimer = true;
    }

    public void WonMaze(string text)
    {
        GameSystem = GameSystemBehavior.instance;
        paraLenses = ParaLensesButtonBehavior.instance;
        

        GameSystem.SetHaveMessage(true);
        GameSystem.SetMessageText(text);

        startTimer = false;
        // popUp = PopUpSystem.instance;
        
        // popUp.PopUp(text);
        PokemonWorld pokeWorld = gameObject.GetComponent<PokemonWorld>();
        pokeWorld.SolvedMaze();
        
    }


    public void FailedMaze(string text)
    {
        GameSystem = GameSystemBehavior.instance;


        GameSystem.SetHaveMessage(true);
        GameSystem.SetMessageText(text);

        failed = true;

        // popUp = PopUpSystem.instance;
        
        // popUp.PopUp(text);

    }

}