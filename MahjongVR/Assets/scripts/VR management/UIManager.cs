using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas UI;

    // Update is called once per frame
    void Update()
    {
        if(UI.gameObject.activeSelf == false && OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            Debug.Log("Setting Canvas Enabled)");
            UI.gameObject.SetActive(true);
        }

        if(UI.gameObject.activeSelf == true && (OVRInput.GetDown(OVRInput.RawButton.B) || OVRInput.GetDown(OVRInput.RawButton.Start)))
        {
            Debug.Log("Setting Canvas Disabled)");
            UI.gameObject.SetActive(false); //B or menu button gets rid of menu
        }
    }
}
