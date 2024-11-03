using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class ResetScenePositions : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;
    public GameObject imageTargetRecalibrate;
    public GameObject sceneObject;

    private Vector3 initialOffset;
    public Transform initialTransform;
    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private Vector3 imageTargetPosition;
    private Quaternion imageTargetRotation;
    private Vector3 initialImagePosition;

    private PopUpSystem popUp;

    // Threshold for detecting significant drift
    public float driftThreshold = 2.0f; // Adjust as needed
    public string message = "The world seems to have shifted. Scan the Inside Out movie poster in the main hallway to fix it.";

    private bool shownMessage;

    // first scan to store the position of the image
    bool firstScanToStore = true;
    private Vector3 intendedSceneObjectPosition;
    bool firstShift = true;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
             // Register this script as an event handler
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
            observerBehaviour.OnBehaviourDestroyed += OnObserverDestroyed;
        }
        
        
        initialPosition = initialTransform.position;
        initialRotation = initialTransform.rotation;

        initialImagePosition = imageTargetRecalibrate.transform.position;

        shownMessage = false;
        popUp = PopUpSystem.instance;
        if (firstScanToStore)
        {
            popUp.PopUp("Scan the Inside Out movie poster for world calibration checks");
        }
       

    }

    // Update is called once per frame
    void Update()
    {
        if (firstScanToStore == false)
        {
            Vector3 currentOffset = imageTargetPosition - sceneObject.transform.position;
            float driftDistance = Vector3.Distance(initialOffset, currentOffset);

            // Threshold for detecting significant drift
            if (driftDistance > driftThreshold && shownMessage == false)
            {
                
                popUp.PopUp(message);
                shownMessage = true;

            }
        }
       
    }

    private void OnObserverDestroyed(ObserverBehaviour behaviour)
    {
        // Unregister event handlers to prevent memory leaks
        observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        observerBehaviour.OnBehaviourDestroyed -= OnObserverDestroyed;
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED ||
            targetStatus.Status == Status.EXTENDED_TRACKED ||
            targetStatus.Status == Status.LIMITED)
        {
            
            // The image target is being tracked
            if (firstScanToStore)
            {
                imageTargetPosition = imageTargetRecalibrate.transform.position;
                imageTargetRotation = imageTargetRecalibrate.transform.rotation;

                initialOffset = imageTargetPosition - initialPosition;

                Vector3 initialDelta = imageTargetPosition - initialImagePosition;
                firstScanToStore = false;
                

            }
            else
            {
                ResetVirtualObjectPosition();
            }
            
            
           
        }
    }
    private void ResetVirtualObjectPosition()
    {

        Vector3 virtualImageTargetPosition = imageTargetPosition;
        Vector3 realWorldImageTargetPosition = imageTargetRecalibrate.transform.position;
        
        Vector3 positionDelta = realWorldImageTargetPosition - virtualImageTargetPosition;

        if ( firstShift )
        {
            intendedSceneObjectPosition = sceneObject.transform.position - positionDelta;
            firstShift = false;
        }
        

        popUp.PopUp($"real image: {realWorldImageTargetPosition} virtual image: {virtualImageTargetPosition} \n Scene: {sceneObject.transform.position } Calculation: {intendedSceneObjectPosition}");
        sceneObject.transform.position = intendedSceneObjectPosition;

        

        Vector3 currentOffset = realWorldImageTargetPosition - sceneObject.transform.position;
        float driftDistance = Vector3.Distance(initialOffset, currentOffset);
        
        
        if (driftDistance > driftThreshold)
        {
            
            shownMessage = false;

            Debug.Log($"Drift detected: {driftDistance} units");
            
        }
        
        
    }
}
