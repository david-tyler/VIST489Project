using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class ResetScenePositionScript : MonoBehaviour
{
    public Transform MoviePoster;
    public Transform sceneTransform;
    public Transform ARCamera;

    private ObserverBehaviour observerBehaviour;

    private Vector3 offset;

    private bool enabledScan = false;
    private Quaternion initialRelativeRotation;

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
        
        offset = sceneTransform.position - MoviePoster.position;
        initialRelativeRotation =  Quaternion.Inverse(MoviePoster.rotation) * sceneTransform.rotation;
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
        // not the actual image target in unity it is the gameobject representing it
        MoviePoster.position = observerBehaviour.transform.position;
        MoviePoster.rotation = observerBehaviour.transform.rotation;


        // Object A's new position and rotation
        Vector3 newPositionA = MoviePoster.position;
        Quaternion newRotationA = MoviePoster.rotation;

        // Rotate the initial offset by Object A's new rotation
        Vector3 rotatedOffset = newRotationA * offset;

        Quaternion rotationX = Quaternion.Euler(-90, 0, 0);
        sceneTransform.position = newPositionA + rotatedOffset;

        // sceneTransform.position = MoviePoster.position + offset;
        // sceneTransform.rotation = initialRelativeRotation;

        
        enabledScan = false;
    }

    private void SetEnabledScan(bool status)
    {
        enabledScan = status;
    }
}
