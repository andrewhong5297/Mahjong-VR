using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public Converters convert = new Converters();
    public TurnManagerMJ TurnMJ = new TurnManagerMJ();
    
    public void NoTake()
    {
        if (TurnMJ.pong || TurnMJ.kong)
        {
            TurnMJ.turn = TurnMJ.previousturn; //sends to the supposed next player
        }
        TurnMJ.action.text = TurnMJ.action.text + "\nDidn't Take going to " + TurnMJ.turn;

        TurnMJ.chow = false;
        TurnMJ.kong = false;
        TurnMJ.pong = false;
        TurnMJ.hands.PlayerHands[convert.PlayerConvNumber(TurnMJ.turn)].temporaryrevealedchips.Clear();
        StartCoroutine(TurnMJ.Turn(0));
    }

    public void ButtonChow()
    {
        TurnMJ.action.text = TurnMJ.action.text + "\n" + TurnMJ.turn + " Chow!";
        TurnMJ.chow = false;

        //need something here to choose which chow if there are multiple hmmm
        TurnMJ.RemoveFromHand();
        StartCoroutine(TurnMJ.Turn(1));
        return;
    }

    public void ButtonPong()
    {
        TurnMJ.action.text = TurnMJ.action.text + "\n" + TurnMJ.turn + " Pong!";
        TurnMJ.pong = false;

        TurnMJ.RemoveFromHand();
        StartCoroutine(TurnMJ.Turn(1));
        return;
    }

    public void ButtonKong()
    {
        TurnMJ.action.text = TurnMJ.action.text + "\n" + TurnMJ.turn + " Kong!";
        TurnMJ.kong = false;

        TurnMJ.RemoveFromHand();
        StartCoroutine(TurnMJ.Turn(0)); //Kong requires a draw tile
        return;
    }
}
