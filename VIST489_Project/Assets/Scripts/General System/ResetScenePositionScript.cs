using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class ResetScenePositionScript : MonoBehaviour
{
    public Transform MoviePoster;
    public Transform sceneTransform;

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
        initialRelativeRotation =  Quaternion.Inverse(MoviePoster.rotation) * sceneTransform.rotation;;
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
        MoviePoster.rotation = observerBehaviour.transform.rotation;

        // Object A's new position and rotation
        Vector3 newPositionA = MoviePoster.position;
        Quaternion newRotationA = MoviePoster.rotation;
        newRotationA = Quaternion.Euler(newRotationA.eulerAngles.x - 90f, newRotationA.eulerAngles.y + 90f, newRotationA.eulerAngles.z);
        // Rotate the initial offset by Object A's new rotation
        Vector3 rotatedOffset = newRotationA * offset;

         // Update Object B's position
        sceneTransform.position = newPositionA + rotatedOffset;

        // Update Object B's rotation
        sceneTransform.rotation = newRotationA * initialRelativeRotation;

        // sceneTransform.position = MoviePoster.position + offset;
        // sceneTransform.rotation = initialRelativeRotation;

        
        enabledScan = false;
    }

    private void SetEnabledScan(bool status)
    {
        enabledScan = status;
    }
}
