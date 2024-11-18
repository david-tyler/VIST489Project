using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Vuforia;
using static System.Net.Mime.MediaTypeNames;


public class PokemonWorld : MonoBehaviour
{
    #region Singleton
    public static PokemonWorld instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of PokemonWorld!");
            return;
        }

        instance = this;
    }
    #endregion

    public GameObject enitreMaze;

    private bool canGetKey = false;
    private bool unlockedDoor = false;
    private bool pickedUpKey = false;
    private bool firstTimeUnlockingDoor = true;


    public string NeedToCompleteMazeText = "It seems we can't get the key, but look there! On the ground to your left, there seems to be a maze. Maybe you need to complete it to obtain the key.";
    public string YouNeedToFindTheKeyText = "This door seems to be locked in the pokemon world. The key should be somewhere on this floor! Perhaps the book has more clues?";
    public string MoveLikeCharizardLine = "We can't fly like Charizard, better jump to those platforms";

    public List<GameObject> objectsToSetActiveAfterDoor = new List<GameObject>();
    public GameObject charizard;
    public GameObject targetCharizardPlatform1;
    public GameObject targetCharizardPlatform2;
    public Camera mainCamera;
    public GameObject pit;
    

    // Script instances
    GameSystemBehavior gameSystem;
    ParaLensesButtonBehavior paraLenses;
    PopUpSystem popUp;
    MazeBehavior mazeScript;
    AudioManager audioManagerScript;
    TriggerZones triggerZonesScript;
    MessageBehavior messageBehavior;

    private int count = 0; // Used to limit in update how many times the pop up is called if we tap on an object that has a pop up box appear after
    // Start is called before the first frame update

    public float duration = 5.0f;
    public Animator CharizardAnim;

    // Make sure the player is across the pit and in range to tap charizard
    private bool canTapCharizard = false;

    public string freedAshAudioName;
    public string crossPitAudioName;

    public List<GameObject> removeGameObjects;

    private void Update()
    {
        
        // *******Touch Interactions
        // --------------------------------------
        // Check if there is at least one touch.
        if (Input.touchCount > 0)
        {
            // Debug.Log("TOUCHING SCREEN");
            Touch touch = Input.GetTouch(0);

            // Check if the touch is just beginning.
            if (touch.phase == TouchPhase.Began)
            {
                // Debug.Log("TOUCHING Begin");

                // Create a ray from the screen point where the touch occurred.
                Ray ray = mainCamera.ScreenPointToRay(touch.position);

                // Variable to store the hit information.
                RaycastHit hit;

                // Perform the raycast.
                if (Physics.Raycast(ray, out hit))
                {
                   hit.collider.gameObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);
                   if (hit.collider.gameObject.tag == "Charizard")
                   {

                        if (canTapCharizard)
                        {
                            audioManagerScript = AudioManager.instance;
                            audioManagerScript.PlayEventSound(crossPitAudioName);
                            pit.SetActive(true);
                            StartCoroutine(MoveCharizard(targetCharizardPlatform1));
                            
                        }
                        
                   }

                }
                

                if (Physics.Raycast(ray, out hit))
                {
                    // Try to find the IRaycastHitHandler interface on the hit object
                    IRaycastHitHandler hitHandler = hit.collider.GetComponent<IRaycastHitHandler>();

                    // If the object has the interface, fire the event
                    if (hitHandler != null)
                    {
                        hitHandler.OnRaycastHit();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) // 0 is for left-click
        {

            // Create a ray from the screen point where the touch occurred.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Variable to store the hit information.
            RaycastHit hit;

            // Perform the raycast.
            if (Physics.Raycast(ray, out hit))
            {
                hit.collider.gameObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);

                if (hit.collider.gameObject.tag == "Charizard")
                {
                    pit.SetActive(true);
                    if (canTapCharizard)
                    {
                        pit.SetActive(true);
                        StartCoroutine(MoveCharizard(targetCharizardPlatform1));
                    }

                }
            }

        }

    }

    public IEnumerator MoveCharizard(GameObject targetPlatform)
    {
        float elapsedTime = 0f;

        Vector3 startPosition = charizard.transform.position;

        pit.SetActive(true);

        while (elapsedTime < duration)
        {
            // Calculate how far along the duration we are
            elapsedTime += Time.deltaTime;

            // Interpolate between start and target positions
            charizard.transform.position = Vector3.Lerp(startPosition, targetPlatform.transform.position, elapsedTime / duration);
            Debug.Log(Vector3.Lerp(startPosition, targetPlatform.transform.position, elapsedTime / duration));
            // Wait for the next frame before continuing
            yield return new WaitForEndOfFrame();
        }

        // Ensure the object is exactly at the target position at the end
        transform.position = targetPlatform.transform.position;
        yield return new WaitForSeconds(1.0f);

        messageBehavior = MessageBehavior.instance;
        messageBehavior.SetHaveMessage(true);
        messageBehavior.SetMessageText(MoveLikeCharizardLine);
    }

    public void SolvedMaze()
    {
        canGetKey = true;

    }

    public bool CanPickUpKey()
    {
        return canGetKey;
    }

    public void CannotPickUpKey()
    {
        gameSystem = GameSystemBehavior.instance;
        paraLenses = ParaLensesButtonBehavior.instance;
        mazeScript = MazeBehavior.instance;
        triggerZonesScript = TriggerZones.instance;
        messageBehavior = MessageBehavior.instance;

        
        if (count == 0 || mazeScript.getFailed() == true)
        {
            count = 0;
            enitreMaze.SetActive(true);
            
            messageBehavior.SetHaveMessage(true);
            messageBehavior.SetMessageText(NeedToCompleteMazeText);
            // popUp = PopUpSystem.instance;

            // popUp.PopUp(NeedToCompleteMazeText);
            
            
            mazeScript.MazeStarted();
            mazeScript.ResetMaze();
            
            count += 1;
            //PopUpBoxButton.onClick.AddListener(SetCountForPopUpKey);

            triggerZonesScript.ModifyLists(null, false, false, false, true);
        }
        
    }
    

    public void SetUnlockedDoor(bool status)
    {
        unlockedDoor = status;
        gameSystem = GameSystemBehavior.instance;
        audioManagerScript = AudioManager.instance;
        if (status == true && firstTimeUnlockingDoor == true)
        {
            firstTimeUnlockingDoor = false;
            gameSystem.SetNarrativeEvent(GameSystemBehavior.NarrativeEvent.FreedAsh, true);
            audioManagerScript.PlayEventSound(freedAshAudioName);
            triggerZonesScript = TriggerZones.instance;
            

            foreach (GameObject item in objectsToSetActiveAfterDoor)
            {
                item.SetActive(true);
            }

            triggerZonesScript.ModifyLists(removeGameObjects);
        }
    }

    public bool GetUnlockedDoor()
    {
        return unlockedDoor;
    }

    public void SetPickedUpKey(bool status)
    {
        pickedUpKey = status;
    }

    // Check if we have the key or not
    public bool GetPickedUpKey()
    {
        return pickedUpKey;
    }

    public void SetCanTapCharizard(bool status)
    {
        canTapCharizard = status;
    }

    public bool GetCanTapCharizard()
    {
        return canTapCharizard;
    }
}
