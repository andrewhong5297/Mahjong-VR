using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
list of gameobjects
four gamehands arrays?
keeps information of player hands
*/

[System.Serializable]
public class GameHandData
{
    public List<GameObject> playerchips;
    public List<GameObject> revealedchips;
    public List<GameObject> temporaryrevealedchips;
}
