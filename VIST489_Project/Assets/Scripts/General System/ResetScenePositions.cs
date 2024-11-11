using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class ResetScenePositions : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;

    public Transform MoviePoster;
    public Transform SpaceCalibrator;
    public Transform ARCamera;

    private Vector3 offset;

    private bool enabledScan = false;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
             // Register this script as an event handler
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
            observerBehaviour.OnBehaviourDestroyed += OnObserverDestroyed;
        }

        offset = SpaceCalibrator.position - MoviePoster.position;

    }

    // Update is called once per frame
    void Update()
    {
        
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
            if (enabledScan == true)
            {
                ResetScenePosition();
            }
            
            
            
            
           
        }
    }

    void ResetScenePosition()
    {
        MoviePoster.transform.position = this.transform.position;
        
        SpaceCalibrator.position = MoviePoster.transform.position + offset;
        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.enabled = false;
        
        ARCamera.position = new Vector3(MoviePoster.position.x, ARCamera.position.y, MoviePoster.position.z);
        ARCamera.rotation = MoviePoster.transform.rotation;
        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.RecenterPose();
        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.enabled = true;
        enabledScan = false;
    }

    private void SetEnabledScan(bool status)
    {
        enabledScan = status;
    }
}
