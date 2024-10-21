using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraMove : MonoBehaviour
{
    public GameObject ARCamera;

    public Transform positionToGo;
    

    public void DebugMove()
    {
        ARCamera.transform.position = positionToGo.position;
        ARCamera.transform.rotation = positionToGo.rotation;
    }
}
