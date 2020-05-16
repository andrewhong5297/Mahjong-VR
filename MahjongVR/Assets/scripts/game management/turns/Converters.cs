using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converters : MonoBehaviour
{

    #region boxes gameobjects
    public GameObject discardeast;
    public GameObject discardsouth;
    public GameObject discardwest;
    public GameObject discardnorth;

    public GameObject removeeast;
    public GameObject removesouth;
    public GameObject removewest;
    public GameObject removenorth;
    #endregion

    //converter methods
    public int PlayerConvNumber(TurnManager turn)
    {
        if (turn == TurnManager.EASTTURN)
        {
            return 0;
        }
        if (turn == TurnManager.SOUTHTURN)
        {
            return 1;
        }
        if (turn == TurnManager.WESTTURN)
        {
            return 2;
        }
        if (turn == TurnManager.NORTHTURN)
        {
            return 3;
        }
        return 0; //will never return this
    }
    public TurnManager NumberConvPlayer(int input)
    {
        if (input == 0)
        {
            return TurnManager.EASTTURN;
        }
        if (input == 1)
        {
            return TurnManager.SOUTHTURN;
        }
        if (input == 2)
        {
            return TurnManager.WESTTURN;
        }
        if (input == 3)
        {
            return TurnManager.NORTHTURN;
        }
        return TurnManager.EASTTURN; //will never return this
    }
    public GameObject PlayerConvRemoveBox(TurnManager turn)
    {
        if (turn == TurnManager.EASTTURN)
        {
            return removeeast;
        }
        if (turn == TurnManager.SOUTHTURN)
        {
            return removesouth;
        }
        if (turn == TurnManager.WESTTURN)
        {
            return removewest;
        }
        if (turn == TurnManager.NORTHTURN)
        {
            return removenorth;
        }
        return removeeast; //will never return this
    }
    public GameObject PlayerConvDiscardBox(TurnManager turn)
    {
        if (turn == TurnManager.EASTTURN)
        {
            return discardeast;
        }
        if (turn == TurnManager.SOUTHTURN)
        {
            return discardsouth;
        }
        if (turn == TurnManager.WESTTURN)
        {
            return discardwest;
        }
        if (turn == TurnManager.NORTHTURN)
        {
            return discardnorth;
        }
        return removeeast; //will never return this
    }

}
