using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class ResetScenePositionScript : MonoBehaviour
{
    public Transform MoviePoster;
    public Transform SpaceCalibrator;

    private ObserverBehaviour observerBehaviour;

    private Vector3 offset;

    private bool enabledScan = false;
    private Quaternion initialSceneRotation;

    void Awake()
    {
        // Get the ObserverBehaviour component (e.g., Image Target)
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            // Register event handlers
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
            observerBehaviour.OnBehaviourDestroyed += OnObserverDestroyed;
        }
    }

    void Start()
    {
        
        initialSceneRotation = SpaceCalibrator.rotation;
        offset = SpaceCalibrator.position - MoviePoster.position;
        initialSceneRotation = SpaceCalibrator.rotation;
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
            ResetScenePosition();
        }
        else
        {
            // The image target is not being tracked
            // Optionally handle this case if needed
        }
    }


    private void ResetScenePosition()
    {
        MoviePoster.position = observerBehaviour.transform.position;
        //MoviePoster.rotation = observerBehaviour.transform.rotation;

        SpaceCalibrator.position = MoviePoster.position + offset;
        SpaceCalibrator.rotation = initialSceneRotation;

        
        enabledScan = false;
    }

    private void SetEnabledScan(bool status)
    {
        enabledScan = status;
    }
}
