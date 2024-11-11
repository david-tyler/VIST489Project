using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokemon", menuName = "Pokemon")]
public class Pokemon : ScriptableObject
{
    public new string name;
    public string shortDescription;
    public string longDescription;
    public List<string> type;
    public Sprite image;
 



}
