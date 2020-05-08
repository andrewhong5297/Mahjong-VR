using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class TurnManagerMJ : MonoBehaviour
{
    //classes
    public ShuffleFinal hands;
    public Converters convert;
    public Buttons button;
    public Check check;

    //variables
    public TurnManager turn = TurnManager.START;
    public TurnManager previousturn = TurnManager.START;

    public int tile = 53; //start dealing from first tile not in hand

    public Text action; 

    //will need an or function for right or left hand (x,y), or don't use rawbutton 
    bool invokebutton, cancelbutton, invokegrab, invoketrigger, invokestick;

    //state of chow/pong/kong
    public bool chow = false, pong = false, kong = false;
    public bool InvokeButton { get { return OVRInput.GetDown(OVRInput.RawButton.A); } } //maybe call in virtual somehow

    void Start()
    {
        //maybe setup game here using shuffle methods. Will need method to reset game? maybe a button?
        turn = TurnManager.EASTTURN;
        //assign a delegate
        //button.OnChow += InvokeChow;//function InvokeChow will be listening for button.OnChow
    }

    void InvokeChow()
    {
        //launched at same time
    }

    private void Update()
    {
        //for testing only 
        if (hands.state == 11)
        {
            //checkmahjong test
            hands.PlayerHands[0].playerchips.Clear();
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[0]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[1]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[2]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[3]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[4]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[5]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[6]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[7]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[8]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[9]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[19]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[29]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[40]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[49]);

            check.CheckMahjong(true);
            Debug.Log("checked");
            hands.state = 11;
        }

        if (chow || pong || kong)
        {

            cancelbutton = OVRInput.GetDown(OVRInput.RawButton.B);
            invokegrab = OVRInput.GetDown(OVRInput.RawButton.RHandTrigger); //use an action? callback for invoking an event? 
            invoketrigger = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
            invokestick = OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown);
            //need one more for if three are possible for chow ;-;

            if (turn != TurnManager.EASTTURN)
            {
                invokebutton = true; //auto invoke if computer
                invokegrab = true; //auto take first part of chow
            }

            //have a timer, if timer = 10 second, invoke NoTake() and set all three to false. Pong and Kong need a way of sending the turn back to original order. 

            if (InvokeButton)
            {
                if (chow)
                {
                    if (hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.Count == 4)
                    {
                        if (invokegrab)
                        {
                            action.text = action.text + "\nConfirmed grab chow";
                            //keep only index 0 and 1
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(0, 2);
                            button.ButtonChow();
                        }
                        else if(invoketrigger)
                        {
                            action.text = action.text + "\nConfirmed trigger chow";
                            //keep only index 2 and 3
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(2, 2);
                            button.ButtonChow();
                        }
                    }
                    else if (hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.Count == 6)
                    {
                        if (invokegrab)
                        {
                            action.text = action.text + "\nConfirmed grab chow";
                            //keep only index 0 and 1
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(0, 2);
                            button.ButtonChow();
                        }
                        else if (invoketrigger)
                        {
                            action.text = action.text + "\nConfirmed trigger chow";
                            //keep only index 2 and 3
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(2, 2);
                            button.ButtonChow();
                        }
                        else if (invokestick)
                        {
                            action.text = action.text + "\nConfirmed stick chow";
                            //keep only index 4 and 5
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(4, 2);
                            button.ButtonChow();
                        }
                        StartCoroutine(Turn(1));
                    }
                    else
                    {
                        button.ButtonChow();
                        StartCoroutine(Turn(1));
                    }
                }
                if (pong)
                {
                    button.ButtonPong();
                    StartCoroutine(Turn(1));
                }
                if (kong)
                {
                    button.ButtonKong();
                    StartCoroutine(Turn(0)); //Kong requires a draw tile
                }
            }
            if(cancelbutton==true)
            {
                button.NoTake();
                StartCoroutine(Turn(0));
            }
        }
    }

    void DealTile(int player, int d_tile) //make tilemovement class that manages playing, dealing, snapback of tiles
    {
        Debug.Log("tile dealt "+ hands.pai_obj[d_tile]);
        hands.PlayerHands[player].playerchips.Add(hands.pai_obj[d_tile]); //adds tile to hand

        //turn off box collider so it doesn't trigger TilePlayed()
        hands.pai_obj[d_tile].GetComponent<BoxCollider>().enabled = false;

        //move tile to above 3rd tile
        hands.pai_obj[d_tile].transform.position = new Vector3(hands.PlayerHands[player].playerchips[3].transform.position.x, hands.height_tile * 1.2f * 2f, hands.PlayerHands[player].playerchips[3].transform.position.z);
        
        //rotating based on player
        if (player ==0)
        {
            hands.pai_obj[tile].transform.rotation = hands.EastRot;
            hands.pai_obj[tile].GetComponent<BoxCollider>().enabled = true;
            hands.pai_obj[tile].AddComponent<Rigidbody>();
        }
        if (player == 1)
        {
            hands.pai_obj[tile].transform.rotation = hands.SouthRot;
            hands.pai_obj[tile].GetComponent<BoxCollider>().enabled = true;
            hands.pai_obj[tile].AddComponent<Rigidbody>();
        }
        if (player == 2)
        {
            hands.pai_obj[tile].transform.rotation = hands.WestRot;
            hands.pai_obj[tile].GetComponent<BoxCollider>().enabled = true;
            hands.pai_obj[tile].AddComponent<Rigidbody>();
        }
        if (player == 3)
        {
            hands.pai_obj[tile].transform.rotation = hands.NorthRot;
            hands.pai_obj[tile].GetComponent<BoxCollider>().enabled = true;
            hands.pai_obj[tile].AddComponent<Rigidbody>();
        }
    }

    public void TilePlayed()
    {
        List<TurnManager> turnArray = Enum.GetValues(typeof(TurnManager)).Cast<TurnManager>().ToList();
        int playerTurn = 0;
        int playerNext = 1;

        check.CheckMahjong(false); 

        while (playerTurn < 4) //looping through just to make make sure playerNext is assigned correctly
        {
            if (turn == turnArray[playerTurn])
            {
                int interruptTurn = check.CheckPongKongForAllPlayers(); //this will assign a turn if pong/kong is available

                if (turn == convert.NumberConvPlayer(interruptTurn))
                {
                    interruptTurn = 10; //make sure you can't pong a tile you just played lmao
                }

                if (interruptTurn != 10) //10 is default return for no players have avail
                {
                    Debug.Log("turn interrupted to: " + convert.NumberConvPlayer(interruptTurn));
                    int nextturn = playerNext;
                    if(nextturn == 4)
                    {
                        nextturn = 0; //setting east to loop
                    }
                    previousturn = turnArray[nextturn]; //save who the nextturn should be
                    turn = convert.NumberConvPlayer(interruptTurn);
                    if (interruptTurn < 4)
                    {
                        pong = true; //going to need way to set turn backwards if invoke is no. Also how to set OVRInput to each player?
                        action.text = action.text + "\nConfirm | chow: " + chow + " | pong: " + pong + " | kong: " + kong;
                    }
                    if (interruptTurn >=4)
                    {
                        kong = true;
                        action.text = action.text + "\nConfirm | chow: " + chow + " | pong: " + pong + " | kong: " + kong;
                    }
                    return;
                }
                turn = turnArray[playerNext];

                if (check.CheckChow(playerNext)) //only next player can chow
                {
                    chow = true;
                    action.text = action.text + "\nConfirm | chow: " + chow + " | pong: " + pong + " | kong: " + kong;
                    return;
                }
                else
                {
                    StartCoroutine(Turn(0));
                }
                return;
            }

            playerTurn += 1;

            if (playerNext != 3)
            {
                playerNext += 1;
            }
            else
            {
                playerNext = 0; //so that when PlayerTurn is 3 (north), it sets PlayerNext to 0 (east) instead of 4 which is the center
            }
        }
    }

    public IEnumerator Turn(int interruption)
    {
        int player = convert.PlayerConvNumber(turn);

        if (interruption == 0)
        {
            DealTile(player, tile); //don't deal if chow or pong
            tile += 1;
        }

        check.CheckMahjong(true); //will check all players but only current player has 14 tiles. mathematically other players cannot mahjong. 

        if (player != 0) //AI code, lowest level random discard and invoke all
        {
            yield return new WaitForSeconds(1f);//computer delay
            GameObject discardtile = hands.PlayerHands[player].playerchips[UnityEngine.Random.Range(0, hands.PlayerHands[player].playerchips.Count - 1)];
            Debug.Log(convert.NumberConvPlayer(player) + " has played " + discardtile);
            discardtile.transform.position = convert.PlayerConvDiscardBox(turn).transform.position;
            yield break;
        }

        //action.text = action.text + "\n30 seconds to play!"; //need seperate UI element later

        //HOW TO RESET COUNTDOWN, since this is invoking too quickly if player doesn't take up full 30 seconds

        //yield return new WaitForSeconds(30f);//human only has 10 seconds to play
        //GameObject discardtileplayer = hands.PlayerHands[player].playerchips[UnityEngine.Random.Range(0, hands.PlayerHands[player].playerchips.Count - 1)];
        //Debug.Log(NumberConvPlayer(player) + " has played " + discardtileplayer);
        //discardtileplayer.transform.position = PlayerConvDiscardBox(turn).transform.position;
    }
}
