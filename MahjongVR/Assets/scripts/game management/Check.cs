using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Check : MonoBehaviour
{

    //will need an or function for right or left hand (x,y), or don't use rawbutton 
    public bool invokebutton, cancelbutton, invokegrab, invoketrigger, invokestick;

    //state of chow/pong/kong
    public bool chow = false, pong = false, kong = false;

    public TurnManagerMJ turnMJ = new TurnManagerMJ();
    public ShuffleFinal hands = new ShuffleFinal();
    public Text action;

    public void CheckMahjong(bool ondraw) //final boss!!! need to check for Mahjong on the drawtile too, just switch don't add and remove last played tile if ondraw = true
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
                            action.text = action.text + "\nRemoved chow " + random;
                            foreach (GameObject tile in hands.PlayerHands[player].temporaryrevealedchips)
                            {
                                mahjongcheck.Remove(tile); //remove other tiles in chow
                                action.text = action.text + "\nRemoved chow " + tile;
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
                    action.text = action.text + "\n Tile " + random + " pair/pong/kong count: " + valuecount;

                    //remove pair if valuecount = 2 and this is first pair to be taken.
                    if (valuecount == 2 && pairtaken == false)
                    {
                        foreach (GameObject tile in hands.PlayerHands[player].playerchips)
                        {
                            Chip Tile = tile.GetComponent<Chip>();
                            if (Tile.chipsuit == SuitRandomTile && Tile.chipvalue == ValueRandomTile)
                            {
                                mahjongcheck.Remove(tile);
                                action.text = action.text + "\nRemoved pair " + tile;
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
                                action.text = action.text + "\nRemoved pong " + tile;
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
                                action.text = action.text + "\nRemoved kong " + tile;
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
                if (player == turnMJ.PlayerConvNumber(TurnManager.EASTTURN))
                {
                    turnMJ.turn = TurnManager.WON;
                    //enable a seperate textbox later
                    action.text = action.text + "\nYou Win! :)";
                    //won UI and sound
                    //move temporaryreveal into reveal, then revealedchips.transform.LookAt(PlayArea);
                    return;
                }
                else
                {
                    turnMJ.turn = TurnManager.LOST;
                    //enable a seperate textbox later
                    action.text = action.text + "\nYou Lose! :(";
                    //lost UI and sound
                    return;
                }
            }

            action.text = action.text + "\n" + turnMJ.NumberConvPlayer(player) + " has tiles left until mahjong: " + mahjongcheck.Count();
            mahjongcheck.Clear();
            mahjongcheckmaster.Clear();
        }
    }

    int CheckConcecutiveValues(List<int> suitvalues)
    {

        return 0;
    }

    public bool CheckChow(int player)
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

        if (SuitPlayedTile == ChipSuit.Winds || SuitPlayedTile == ChipSuit.Dragons)
        {
            Debug.Log("Can't chow Wind/Honor tiles");
            return ChowAvail;
        }

        foreach (GameObject tile in hands.PlayerHands[player].playerchips) //taking value of all tiles in same suit as played tile suit
        {
            Chip Tile = tile.GetComponent<Chip>();
            if (Tile.chipsuit == SuitPlayedTile)
            {
                int ValueTile = Tile.chipvalue;
                suitvalues.Add(ValueTile);
            }
        }

        if (suitvalues.Contains(ValuePlayedTile - 1) && suitvalues.Contains(ValuePlayedTile - 2))
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

    public bool CheckPong(int player)
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
            if (x == ValuePlayedTile)
            {
                valuecount += 1;
            }
        }
        if (valuecount == 2) //two of the same tile
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

    public bool CheckKong(int player)
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
    public int CheckPongKongForAllPlayers()
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

    public void RemoveFromHand(int player)
    {
        List<GameObject> movetiles = new List<GameObject>();
        movetiles.AddRange(hands.PlayerHands[player].temporaryrevealedchips);
        foreach (GameObject tile in movetiles)
        {
            tile.transform.position = turnMJ.PlayerConvRemoveBox(turnMJ.turn).transform.position;
            hands.PlayerHands[player].revealedchips.Add(tile);
            hands.PlayerHands[player].playerchips.Remove(tile);
        }
        hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1].transform.position = turnMJ.PlayerConvRemoveBox(turnMJ.turn).transform.position; //move taken tile out of middle
        hands.PlayerHands[player].revealedchips.Add(hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1]); //add taken tile to revealchips array
        hands.PlayerHands[player].temporaryrevealedchips.Clear(); //clear temp after done
                                                                  //will want to make this drop more organized later

        //function takes in chow to decide whether to further filter what gets removed. 
    }

}
