using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRotate : MonoBehaviour
{
    [SerializeField] Transform cam;

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
