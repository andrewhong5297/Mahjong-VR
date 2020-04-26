using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBoxManager : MonoBehaviour
{
    public Scrollbar bar;
    public IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("scrolling to bottom");
        bar.value = 0f;
    }
}
