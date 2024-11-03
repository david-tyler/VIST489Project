using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSelector : MonoBehaviour
{

    private bool realMaze;
    private string mazeTag; 
    
    void Start()
    {
        mazeTag = this.gameObject.tag;
    }
    public void SetIsReal(bool isReal)
    {
        realMaze = isReal;
    }

    public bool GetIsReal()
    {
        return realMaze;
    }

    public string GetMazeTag()
    {
        return mazeTag;
    }
}
