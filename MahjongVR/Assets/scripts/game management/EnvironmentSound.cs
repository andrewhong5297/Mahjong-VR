using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSound : MonoBehaviour
{
    public Audiom audiom;

    // Start is called before the first frame update
    void Start()
    {
        audiom.Play("guzheng");
        audiom.Play("wind");
        //audio.Play("water"); swimming fish
    }
}
