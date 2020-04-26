using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class TurnManagerMJ : MonoBehaviour
{
    // Start is called before the first frame update
    public ShuffleFinal hands = new ShuffleFinal();
    public UpdateBoxManager scroll = new UpdateBoxManager();
    public Check check = new Check();
    public TurnManager turn;
    public TurnManager previousturn;

    public int tile = 53; //start dealing from first tile not in hand

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

    public Text action;

    void Start()
    {
        //maybe setup game here using shuffle methods. Will need method to reset game? maybe a button?
        //turn = TurnManager.EASTTURN;
    }

    private void Update()
    {
        //mahjongcheck testing
        if(hands.state==10)
        {
            //checkmahjong test
            hands.PlayerHands[0].playerchips.Clear();
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[3]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[1]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[2]);
            hands.PlayerHands[0].playerchips.Add(hands.pai_obj[0]);
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

        if(check.chow || check.pong || check.kong)
        {
            check.invokebutton = OVRInput.GetDown(OVRInput.RawButton.A);
            check.cancelbutton = OVRInput.GetDown(OVRInput.RawButton.B);
            check.invokegrab = OVRInput.GetDown(OVRInput.RawButton.RHandTrigger);
            check.invoketrigger = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
            check.invokestick = OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown);

            if (turn != TurnManager.EASTTURN)
            {
                check.invokebutton = true; //auto invoke if computer
                check.invokegrab = true; //auto take first part of chow
            }

            //have a timer, if timer = 10 second, invoke NoTake() and set all three to false. Pong and Kong need a way of sending the turn back to original order. 
            Debug.Log("Confirm | chow: " + check.chow + "| pong: " + check.pong + "| kong: " + check.kong);
            action.text = action.text + "\nConfirm | chow: " + check.chow + " | pong: " + check.pong + " | kong: " + check.kong;

            if (check.invokebutton ==true)
            {
                if (check.chow)
                {
                    if (hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.Count == 4)
                    {
                        if (check.invokegrab)
                        {
                            action.text = action.text + "\nConfirmed grab chow";
                            //keep only index 0 and 1
                            hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(0, 2);
                            ButtonChow();
                            check.chow = false;
                        }
                        else if(check.invoketrigger)
                        {
                            action.text = action.text + "\nConfirmed trigger chow";
                            //keep only index 2 and 3
                            hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(2, 2);
                            ButtonChow();
                            check.chow = false;
                        }
                    }
                    else if (hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.Count == 6)
                    {
                        if (check.invokegrab)
                        {
                            action.text = action.text + "\nConfirmed grab chow";
                            //keep only index 0 and 1
                            hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(0, 2);
                            ButtonChow();
                            check.chow = false;
                        }
                        else if (check.invoketrigger)
                        {
                            action.text = action.text + "\nConfirmed trigger chow";
                            //keep only index 2 and 3
                            hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(2, 2);
                            ButtonChow();
                            check.chow = false;
                        }
                        else if (check.invokestick)
                        {
                            action.text = action.text + "\nConfirmed stick chow";
                            //keep only index 4 and 5
                            hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(4, 2);
                            ButtonChow();
                            check.chow = false;
                        }
                    }
                    else
                    { 
                        ButtonChow();
                        check.chow = false;
                    }
                }
                if (check.pong)
                {
                    ButtonPong();
                    check.pong = false;
                }
                if (check.kong)
                {
                    ButtonKong();
                    check.kong = false;
                }
            }
            if(check.cancelbutton ==true)
            {
                NoTake();
                check.chow = false;
                check.kong = false;
                check.pong = false;
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

    #region buttons
    public void NoTake()
    {
        turn = previousturn; //sends to the supposed next player
        action.text = action.text + "\nDidn't Take going to " + turn;
        scroll.ScrollToBottom(); //this gets called after every text update

        hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.Clear();
        StartCoroutine(Turn(0, PlayerConvNumber(turn)));
    }

    public void ButtonChow()
    {
        action.text = action.text + "\n" + turn + " Chow!";
        scroll.ScrollToBottom(); //this gets called after every text update

        //need something here to choose which chow if there are multiple hmmm
        check.RemoveFromHand(PlayerConvNumber(turn));
        StartCoroutine(Turn(1, PlayerConvNumber(turn)));
        return;
    }

    public void ButtonPong()
    {
        action.text = action.text + "\n" + turn + " Pong!";
        scroll.ScrollToBottom(); //this gets called after every text update

        check.RemoveFromHand(PlayerConvNumber(turn));
        StartCoroutine(Turn(1, PlayerConvNumber(turn)));
        return;
    }

    public void ButtonKong()
    {
        action.text = action.text + "\n" + turn + " Kong!";
        scroll.ScrollToBottom(); //this gets called after every text update

        check.RemoveFromHand(PlayerConvNumber(turn));
        StartCoroutine(Turn(0, PlayerConvNumber(turn))); //Kong requires a draw tile
        return;
    }
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

    public void TilePlayed()
    {
        List<TurnManager> turnArray = Enum.GetValues(typeof(TurnManager)).Cast<TurnManager>().ToList();
        int playerTurn = 0;
        int playerNext = 1;

        check.CheckMahjong(false); //turn will be changed so that the bottom loop doesn't do anything

        while (playerTurn < 4)
        {
            if (turn == turnArray[playerTurn])
            {
                int interruptTurn = check.CheckPongKongForAllPlayers(); //this will assign a turn if pong/kong is available

                if (turn == NumberConvPlayer(interruptTurn))
                {
                    interruptTurn = 5; //make sure you can't pong a tile you just played lmao
                }

                if (interruptTurn != 5) //5 is default return for no players have avail
                {
                    Debug.Log("turn interrupted to: " + NumberConvPlayer(interruptTurn));
                    int nextturn = playerNext;
                    if(nextturn == 4)
                    {
                        nextturn = 0; //setting east to loop
                    }
                    previousturn = turnArray[nextturn]; //save who the nextturn should be
                    turn = NumberConvPlayer(interruptTurn);
                    check.pong = true; //going to need way to set turn backwards if invoke is no. Also how to set OVRInput to each player?
                    return;
                }
                turn = turnArray[playerNext];

                if (check.CheckChow(playerNext)) //only next player can chow
                {
                    check.chow = true;
                    return;
                }
                else
                {
                    StartCoroutine(Turn(0, playerNext));
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

    IEnumerator Turn(int interruption, int player)
    {
        //UI shift text, timer start, etc. 

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
            Debug.Log(NumberConvPlayer(player) + " has played " + discardtile);
            discardtile.transform.position = PlayerConvDiscardBox(turn).transform.position;
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
