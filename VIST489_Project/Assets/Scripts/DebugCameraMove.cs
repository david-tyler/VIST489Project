using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraMove : MonoBehaviour
{
    public GameObject ARCamera;

    public Transform positionToGo;
    //public Transform cameraOrigin;


    public void DebugMove()
    {
        //ARCamera.transform.position = positionToGo.position;
        //ARCamera.transform.rotation = positionToGo.rotation;

        // GameObject arCameraParent = new GameObject("ARCameraParent");
        //arCameraParent.transform.position = positionToGo.position; //desired position
        ///arCameraParent.transform.rotation = positionToGo.rotation; //desired position
        //ARCamera.transform.parent = arCameraParent.transform;




        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.enabled = false;
        ARCamera.transform.position = positionToGo.position; //- (ARCamera.transform.position - cameraOrigin.position); // Set desired position
        //cameraOrigin.position = positionToGo.position;
        ARCamera.transform.rotation = positionToGo.rotation; // Set desired position
                                                             // Re-enable tracking when done
       
        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.RecenterPose();
        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.Reset();// = true;
        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.enabled = true;

        //StartCoroutine(EnableTracking());
        
    }

    public IEnumerator EnableTracking()
    {
        yield return new WaitForSeconds(5);
        print("WaitAndPrint " + Time.time);
        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.Reset();// = true;
        Vuforia.VuforiaBehaviour.Instance.DevicePoseBehaviour.enabled = true;
    }
}
