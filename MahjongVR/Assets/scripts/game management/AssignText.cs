using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignText : MonoBehaviour
{
    public Text action;
    public delegate void AssignTextEvent(Text text);
    public AssignTextEvent OnAssignText;
    private Check checkScript;

    // Start is called before the first frame update
    void Awake()
    {
        action = GetComponent<Text>();
        checkScript = FindObjectOfType<Check>();
        checkScript.assignTextScript = this; //object inspector assigning
        checkScript.DoAssigning();
        OnAssignText(action);
    }
}
