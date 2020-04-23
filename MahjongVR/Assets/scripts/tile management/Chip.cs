using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public ChipSuit chipsuit;

    public string chipname;
    public int chipvalue;

    private void Awake()
    {
        chipname = this.gameObject.name;
    }

    //add this.gameobject to static tiles list
}
