using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class TurnManagerMJ : MonoBehaviour
{
    public ShuffleFinal hands;
    public Converters convert;
    public Buttons button;

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
        button.OnChow += InvokeChow;//function InvokeChow will be listening for button.OnChow
    }
    void InvokeChow()
    {
        //launched at same time
    }

    private void Update()
    {
        //for testing only 
        if(hands.state==11)
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

            CheckMahjong(true);
            Debug.Log("checked");
            hands.state = 11;
        }

        if(chow || pong || kong)
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
            Debug.Log("Confirm | chow: " + chow + "| pong: " + pong + "| kong: " + kong);
            action.text = action.text + "\nConfirm | chow: " + chow + " | pong: " + pong + " | kong: " + kong;

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
                            ButtonChow();
                        }
                        else if(invoketrigger)
                        {
                            action.text = action.text + "\nConfirmed trigger chow";
                            //keep only index 2 and 3
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(2, 2);
                            ButtonChow();
                        }
                    }
                    else if (hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.Count == 6)
                    {
                        if (invokegrab)
                        {
                            action.text = action.text + "\nConfirmed grab chow";
                            //keep only index 0 and 1
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(0, 2);
                            ButtonChow();
                        }
                        else if (invoketrigger)
                        {
                            action.text = action.text + "\nConfirmed trigger chow";
                            //keep only index 2 and 3
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(2, 2);
                            ButtonChow();
                        }
                        else if (invokestick)
                        {
                            action.text = action.text + "\nConfirmed stick chow";
                            //keep only index 4 and 5
                            hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips = hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.GetRange(4, 2);
                            ButtonChow();
                        }
                        StartCoroutine(Turn(1));
                    }
                    else
                    { 
                        ButtonChow();
                        StartCoroutine(Turn(1));
                    }
                }
                if (pong)
                {
                    ButtonPong();
                    StartCoroutine(Turn(1));
                }
                if (kong)
                {
                    ButtonKong();
                    StartCoroutine(Turn(0)); //Kong requires a draw tile
                }
            }
            if(cancelbutton==true)
            {
                NoTake();
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

    //I think these will need to be refactored into its own class 
    #region CheckAll 
    void CheckMahjong(bool ondraw) //final boss!!! need to check for Mahjong on the drawtile too, just switch don't add and remove last played tile if ondraw = true
    {
        for (int player = 0; player < 4; player++)
        {
            Debug.Log("checking" + player);
            bool avail = false;
            bool pairtaken = false; //this is just so we don't take two or three pairs and accidently count a Mahjong. Refreshes for each player.

            List<int> suitvalues = new List<int>();
            List<GameObject> mahjongcheck = new List<GameObject>(); //list to store mahjongcheck hand just so it is easier to clear after if false
            if (!ondraw)
            {
                mahjongcheck.Add(hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1]); //add last played tile if this check is not after dealtile()
            }
            mahjongcheck.AddRange(hands.PlayerHands[player].playerchips);
            List<GameObject> mahjongcheckmaster = new List<GameObject>();
            mahjongcheckmaster.AddRange(mahjongcheck);

            //moving melds and pairs out of mahjongcheck
            foreach (GameObject random in mahjongcheckmaster) //store this in a seperate list, mahjongcheckmaster, and remove from mahjongcheck and check if it is exists in the mahjongcheck before running the loop.
            {
                if (mahjongcheck.Contains(random))
                {
                    suitvalues.Clear();
                    Chip randomtile = random.GetComponent<Chip>();
                    ChipSuit SuitRandomTile = randomtile.chipsuit;
                    int ValueRandomTile = randomtile.chipvalue;

                    //all possible tiles, used to make sure only first occurance of tile is picked up. 
                    bool minusone = false;
                    bool minustwo = false;
                    bool plusone = false;
                    bool plustwo = false;

                    //filling suitvalues
                    foreach (GameObject tile in mahjongcheck) //getting values in the same suit in an array
                    {
                        Chip Tile = tile.GetComponent<Chip>();
                        if (Tile.chipsuit == SuitRandomTile)
                        {
                            int ValueTile = Tile.chipvalue;
                            suitvalues.Add(ValueTile);
                        }
                    }
                    suitvalues.Sort(); //see what max number in a row is, if we have 5 then just let algo do it's thing and rerun chow at the end. If it is 6 or 9 or 12 then remove all of them. 

                    //Chow must be removed first. otherwise pair or pong could break it. 
                    if (SuitRandomTile == ChipSuit.Winds || SuitRandomTile == ChipSuit.Dragons)
                    {
                        //don't check chow, do nothing 
                    }
                    else
                    {
                        //check chow, MUST only take first occurances of tile. Also check for a string, i.e. 3,6,9, or 12 tiles in a row and take all if true.

                        if (suitvalues.Contains(ValueRandomTile - 1) && suitvalues.Contains(ValueRandomTile - 2))
                        {
                            minusone = false;
                            minustwo = false;

                            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
                            {
                                Chip Tile = tile.GetComponent<Chip>();
                                if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile - 1 && !minusone)
                                {
                                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                                    minusone = true;
                                }

                                if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile - 2 && !minustwo)
                                {
                                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                                    minustwo = true;
                                }
                            }
                        }

                        if (suitvalues.Contains(ValueRandomTile - 1) && suitvalues.Contains(ValueRandomTile + 1))
                        {
                            plusone = false;
                            minusone = false;

                            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
                            {
                                Chip Tile = tile.GetComponent<Chip>();
                                if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile - 1 && !minusone)
                                {
                                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                                    minusone = true;
                                }

                                if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile + 1 && !plusone)
                                {
                                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                                    plusone = true;
                                }
                            }
                        }

                        if (suitvalues.Contains(ValueRandomTile + 1) && suitvalues.Contains(ValueRandomTile + 2))
                        {
                            plusone = false;
                            plustwo = false;

                            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
                            {
                                Chip Tile = tile.GetComponent<Chip>();
                                if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile + 2 && !plustwo)
                                {
                                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                                    plustwo = true;
                                }

                                if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile + 1 && !plusone)
                                {
                                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                                    plusone = true;
                                }
                            }
                        }

                        if (hands.PlayerHands[player].temporaryrevealedchips.Count == 2)
                        {
                            mahjongcheck.Remove(random); // remove the tile used as base. Only called if a single chow is found. 
                            //action.text = action.text + "\nRemoved chow " + random;
                            foreach (GameObject tile in hands.PlayerHands[player].temporaryrevealedchips)
                            {
                                mahjongcheck.Remove(tile); //remove other tiles in chow
                                //action.text = action.text + "\nRemoved chow " + tile;
                            }
                        }
                        hands.PlayerHands[player].temporaryrevealedchips.Clear();
                    }

                    int valuecount = 0;
                    foreach (int x in suitvalues)
                    {
                        if (x == ValueRandomTile)
                        {
                            valuecount += 1;
                        }
                    }

                    //remove pair if valuecount = 2 and this is first pair to be taken.
                    if (valuecount == 2 && pairtaken == false)
                    {
                        foreach (GameObject tile in hands.PlayerHands[player].playerchips)
                        {
                            Chip Tile = tile.GetComponent<Chip>();
                            if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile)
                            {
                                mahjongcheck.Remove(tile);
                                //action.text = action.text + "\nRemoved pair " + tile;
                            }
                        }
                        pairtaken = true;
                    }

                    //remove concealed pong if valuecount = 3
                    if (valuecount == 3)
                    {
                        foreach (GameObject tile in hands.PlayerHands[player].playerchips)
                        {
                            Chip Tile = tile.GetComponent<Chip>();
                            if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile)
                            {
                                mahjongcheck.Remove(tile);
                                //action.text = action.text + "\nRemoved pong " + tile;
                            }
                        }
                    }

                    //remove concealed kong if valuecount = 4
                    if (valuecount == 4)
                    {
                        foreach (GameObject tile in hands.PlayerHands[player].playerchips)
                        {
                            Chip Tile = tile.GetComponent<Chip>();
                            if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile)
                            {
                                mahjongcheck.Remove(tile);
                                //action.text = action.text + "\nRemoved kong " + tile;
                            }
                        }
                    }
                }
            }

            //if mahjongcheck size is 0, that means that all tiles are in a pair or a meld.
            if (mahjongcheck.Count == 0)
            {
                avail = true; //Mahjong!!!
            }

            if (avail == true)
            {
                if (player == convert.PlayerConvNumber(TurnManager.EASTTURN))
                {
                    turn = TurnManager.WON;
                    //enable a seperate textbox later
                    action.text = action.text + "\nYou Win! :)";
                    //won UI and sound
                    //move temporaryreveal into reveal, then revealedchips.transform.LookAt(PlayArea);
                    return;
                }
                else
                {
                    turn = TurnManager.LOST;
                    //enable a seperate textbox later
                    action.text = action.text + "\nYou Lose! :(";
                    //lost UI and sound
                    return;
                }
            }

            //action.text = action.text + "\n" + NumberConvPlayer(player) + " has tiles left until mahjong: " + mahjongcheck.Count();
            mahjongcheck.Clear();
            mahjongcheckmaster.Clear();
        }
    }

    bool CheckChow(int player)
    {
        bool ChowAvail = false;
        //three types of chows
        bool twosides = false;
        bool twolower = false;
        bool twohigher = false;

        //all possible tiles, used to make sure only first occurance of tile is picked up. 
        bool minusone = false;
        bool minustwo = false;
        bool plusone = false;
        bool plustwo = false;

        GameObject lastplayed = hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1];
        List<int> suitvalues = new List<int>();
        Chip PlayedTile = lastplayed.GetComponent<Chip>();
        ChipSuit SuitPlayedTile = PlayedTile.chipsuit;
        int ValuePlayedTile = PlayedTile.chipvalue;

        if(SuitPlayedTile == ChipSuit.Winds || SuitPlayedTile == ChipSuit.Dragons)
        {
            Debug.Log("Can't chow Wind/Honor tiles");
            return ChowAvail;
        }

        foreach (GameObject tile in hands.PlayerHands[player].playerchips) //taking value of all tiles in same suit as played tile suit
        {
            Chip Tile = tile.GetComponent<Chip>();
            if(Tile.chipsuit == SuitPlayedTile)
                {
                int ValueTile = Tile.chipvalue;
                suitvalues.Add(ValueTile);
                }
        }
        
        if (suitvalues.Contains(ValuePlayedTile-1) && suitvalues.Contains(ValuePlayedTile-2))
        {
            twolower = true;
            ChowAvail = true;
            minusone = false;
            minustwo = false; 

            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
            {
                Chip Tile = tile.GetComponent<Chip>();
                if (Tile.chipsuit == SuitPlayedTile && Tile.chipvalue == ValuePlayedTile - 1 && !minusone) 
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                    minusone = true;
                }

                if (Tile.chipsuit == SuitPlayedTile && Tile.chipvalue == ValuePlayedTile - 2 && !minustwo)
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                    minustwo = true;
                }
            }
        }

        if (suitvalues.Contains(ValuePlayedTile - 1) && suitvalues.Contains(ValuePlayedTile + 1))
        {
            twosides = true;
            ChowAvail = true;
            plusone = false;
            minusone = false;

            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
            {
                Chip Tile = tile.GetComponent<Chip>();
                if (Tile.chipsuit == SuitPlayedTile && Tile.chipvalue == ValuePlayedTile - 1 && !minusone)
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                    minusone = true;
                }

                if (Tile.chipsuit == SuitPlayedTile && Tile.chipvalue == ValuePlayedTile + 1 && !plusone)
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                    plusone = true;
                }
            }
        }

        if (suitvalues.Contains(ValuePlayedTile + 1) && suitvalues.Contains(ValuePlayedTile + 2))
        {
            twohigher = true;
            ChowAvail = true;
            plusone = false;
            plustwo = false;

            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
            {
                Chip Tile = tile.GetComponent<Chip>();
                if (Tile.chipsuit == SuitPlayedTile && Tile.chipvalue == ValuePlayedTile + 2 && !plustwo)
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                    plustwo = true;
                }

                if (Tile.chipsuit == SuitPlayedTile && Tile.chipvalue == ValuePlayedTile + 1 && !plusone)
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                    plusone = true;
                }
            }
        }

        if (ChowAvail)
        {
            Debug.Log("Can Chow of type twolower: " + twolower + " twohigher: " + twohigher + " twosides: " + twosides);
        }
        else
        {
            Debug.Log("Can't Chow");
        }
        return ChowAvail;
    }

    bool CheckPong(int player)
    {
        bool PongAvail = false;
        GameObject lastplayed = hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1];
        List<int> suitvalues = new List<int>();

        Chip PlayedTile = lastplayed.GetComponent<Chip>();
        ChipSuit SuitPlayedTile = PlayedTile.chipsuit;
        int ValuePlayedTile = PlayedTile.chipvalue;

        foreach (GameObject tile in hands.PlayerHands[player].playerchips) //taking value of all tiles in same suit as played tile suit
        {
            Chip Tile = tile.GetComponent<Chip>();
            if (Tile.chipsuit == SuitPlayedTile)
            {
                int ValueTile = Tile.chipvalue;
                suitvalues.Add(ValueTile);
            }
        }

        int valuecount = 0;
        foreach (int x in suitvalues)
        {
            if(x==ValuePlayedTile)
            {
                valuecount += 1;
            }
        }
        if(valuecount == 2) //two of the same tile
        {
            PongAvail = true;
            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
            {
                Chip Tile = tile.GetComponent<Chip>();
                if (Tile.chipsuit == SuitPlayedTile && Tile.chipvalue == ValuePlayedTile)
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                }
            }
        }

        if (PongAvail)
        {
            Debug.Log(player + " player can Pong");
        }
        return PongAvail;
    }

    bool CheckKong(int player)
    {
        bool KongAvail = false;
        GameObject lastplayed = hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1];
        List<int> suitvalues = new List<int>();

        Chip PlayedTile = lastplayed.GetComponent<Chip>();
        ChipSuit SuitPlayedTile = PlayedTile.chipsuit;
        int ValuePlayedTile = PlayedTile.chipvalue;

        foreach (GameObject tile in hands.PlayerHands[player].playerchips) //taking value of all tiles in same suit as played tile suit
        {
            Chip Tile = tile.GetComponent<Chip>();
            if (Tile.chipsuit == SuitPlayedTile)
            {
                int ValueTile = Tile.chipvalue;
                suitvalues.Add(ValueTile);
            }
        }

        foreach (GameObject tile in hands.PlayerHands[player].revealedchips) //kong can happen with revealedchips as well
        {
            Chip Tile = tile.GetComponent<Chip>();
            if (Tile.chipsuit == SuitPlayedTile)
            {
                int ValueTile = Tile.chipvalue;
                suitvalues.Add(ValueTile);
            }
        }


        int valuecount = 0;
        foreach (int x in suitvalues)
        {
            if (x == ValuePlayedTile)
            {
                valuecount += 1;
            }
        }

        if (valuecount == 3) //3 of the same tile
        {
            KongAvail = true;
            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
            {
                Chip Tile = tile.GetComponent<Chip>();
                if (Tile.chipsuit == SuitPlayedTile && Tile.chipvalue == ValuePlayedTile)
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                }
            }
        }

        if (KongAvail)
        {
            Debug.Log(player + " player Kong");
        }
        return KongAvail;
    }

    //shouldn't matter checking pong first, since it is strictly = not >3. 
    int CheckPongKongForAllPlayers()
    {
        for (int i = 0; i <= 3; i++)
        {
            if (CheckPong(i))
            {
                return i; 
            }
            if (CheckKong(i))
            {
                return i;
            }
        }
        return 5; //equiv of no pongs
    }

    #endregion

    public void RemoveFromHand()
    {
        int player = convert.PlayerConvNumber(turn);
        List<GameObject> movetiles = new List<GameObject>();
        movetiles.AddRange(hands.PlayerHands[player].temporaryrevealedchips);
        foreach (GameObject tile in movetiles)
        {
            tile.transform.position = convert.PlayerConvRemoveBox(turn).transform.position;
            hands.PlayerHands[player].revealedchips.Add(tile);
            hands.PlayerHands[player].playerchips.Remove(tile); 
        }
        hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count-1].transform.position = convert.PlayerConvRemoveBox(turn).transform.position; //move taken tile out of middle
        hands.PlayerHands[player].revealedchips.Add(hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1]); //add taken tile to revealchips array
        hands.PlayerHands[player].temporaryrevealedchips.Clear(); //clear temp after done
        
        //will want to make this drop more organized in the box later
    }

    #region buttons
    public void NoTake()
    {
        if (pong || kong)
        {
            turn = previousturn; //sends to the supposed next player
        }
        action.text = action.text + "\nDidn't Take going to " + turn;

        chow = false;
        kong = false;
        pong = false;
        hands.PlayerHands[convert.PlayerConvNumber(turn)].temporaryrevealedchips.Clear();
    }

    public void ButtonChow()
    {
        action.text = action.text + "\n" + turn + " Chow!";
        chow = false;

        //need something here to choose which chow if there are multiple hmmm
        RemoveFromHand();
    }

    public void ButtonPong()
    {
        action.text = action.text + "\n" + turn + " Pong!";
        pong = false;

        RemoveFromHand();
    }

    public void ButtonKong()
    {
        action.text = action.text + "\n" + turn + " Kong!";
        kong = false;

        RemoveFromHand();
    }
    #endregion

    public void TilePlayed()
    {
        List<TurnManager> turnArray = Enum.GetValues(typeof(TurnManager)).Cast<TurnManager>().ToList();
        int playerTurn = 0;
        int playerNext = 1;

        CheckMahjong(false); 

        while (playerTurn < 4)
        {
            if (turn == turnArray[playerTurn])
            {
                int interruptTurn = CheckPongKongForAllPlayers(); //this will assign a turn if pong/kong is available

                if (turn == convert.NumberConvPlayer(interruptTurn))
                {
                    interruptTurn = 5; //make sure you can't pong a tile you just played lmao
                }

                if (interruptTurn != 5) //5 is default return for no players have avail
                {
                    Debug.Log("turn interrupted to: " + convert.NumberConvPlayer(interruptTurn));
                    int nextturn = playerNext;
                    if(nextturn == 4)
                    {
                        nextturn = 0; //setting east to loop
                    }
                    previousturn = turnArray[nextturn]; //save who the nextturn should be
                    turn = convert.NumberConvPlayer(interruptTurn);
                    pong = true; //going to need way to set turn backwards if invoke is no. Also how to set OVRInput to each player?
                    return;
                }
                turn = turnArray[playerNext];

                if (CheckChow(playerNext)) //only next player can chow
                {
                    chow = true;
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

        CheckMahjong(true); //will check all players but only current player has 14 tiles. mathematically other players cannot mahjong. 

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
