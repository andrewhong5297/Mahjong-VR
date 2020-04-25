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
    public TurnManager turn;
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

    #region UI
    public Text action;
    #endregion

    //will need an or function for right or left hand (x,y), or don't use rawbutton 
    bool invokebutton, cancelbutton;

    //state of chow/pong/kong
    bool chow = false, pong = false, kong = false;

    void Start()
    {
        //maybe setup game here using shuffle methods
        turn = TurnManager.EASTTURN; //default always first player to start, will need to make it so east just plays a tile and doesn't get dealt one. Maybe freeze unfreeze all here.
    }

    private void Update()
    {
        if(chow || pong || kong)
        {
            invokebutton = OVRInput.GetDown(OVRInput.RawButton.A);
            cancelbutton = OVRInput.GetDown(OVRInput.RawButton.B);

            if(turn != TurnManager.EASTTURN)
            {
                invokebutton = true; //auto invoke if computer
            }

            //have a timer, if timer = 10 second, invoke NoTake() and set all three to false. Pong and Kong need a way of sending the turn back to original order. 
            Debug.Log("Waiting for input... chow: " + chow + " pong: " + pong + " kong: " + kong);
            action.text = "Waiting for input... chow: " + chow + " pong: " + pong + " kong: " + kong;

            Debug.Log(invokebutton);
            if (invokebutton==true)
            {
                if (chow)
                {
                    ButtonChow();
                    chow = false;
                }
                if (pong)
                {
                    ButtonPong();
                    pong = false;
                }
                if (kong)
                {
                    ButtonKong();
                    kong = false;
                }
            }
            if(cancelbutton==true)
            {
                NoTake();
                chow = false;
                kong = false;
                pong = false;
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
    bool CheckMahjong() //final boss!!!
    {
        bool avail = false;
        for (int player = 0; player < 3; player++)
        {
            
            hands.PlayerHands[player].playerchips.Add(hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1]);

            if (avail == false)
            {
                hands.PlayerHands[player].playerchips.Remove(hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1]);
            }
        }
        return avail;
    }

    bool CheckChow(int player)
    {
        bool ChowAvail = false;
        //three types of chows
        bool twosides = false;
        bool twolower = false;
        bool twohigher = false;

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
            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
            {
                Chip Tile = tile.GetComponent<Chip>();
                if (Tile.chipsuit == SuitPlayedTile && (Tile.chipvalue == ValuePlayedTile - 1 || Tile.chipvalue == ValuePlayedTile - 2))
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                }
            }
        }

        if (suitvalues.Contains(ValuePlayedTile - 1) && suitvalues.Contains(ValuePlayedTile + 1))
        {
            twosides = true;
            ChowAvail = true;
            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
            {
                Chip Tile = tile.GetComponent<Chip>();
                if (Tile.chipsuit == SuitPlayedTile && (Tile.chipvalue == ValuePlayedTile - 1 || Tile.chipvalue == ValuePlayedTile + 2))
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                }
            }
        }

        if (suitvalues.Contains(ValuePlayedTile + 1) && suitvalues.Contains(ValuePlayedTile + 2))
        {
            twohigher = true;
            ChowAvail = true;
            foreach (GameObject tile in hands.PlayerHands[player].playerchips)
            {
                Chip Tile = tile.GetComponent<Chip>();
                if (Tile.chipsuit == SuitPlayedTile && (Tile.chipvalue == ValuePlayedTile + 1 || Tile.chipvalue == ValuePlayedTile + 2))
                {
                    hands.PlayerHands[player].temporaryrevealedchips.Add(tile);
                }
            }
        }

        //sort suitvalues
        //Check for three consecutive values -> sort, then remove duplicates. Then check if there are three one's in a row when a difference is taken. 
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
        Debug.Log("Count of same tile for player " +player + ": " + valuecount);
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

    void RemoveFromHand(int player)
    {
        List<GameObject> movetiles = new List<GameObject>();
        movetiles.AddRange(hands.PlayerHands[player].temporaryrevealedchips);
        foreach (GameObject tile in movetiles)
        {
            tile.transform.position = PlayerConvRemoveBox(turn).transform.position;
            hands.PlayerHands[player].revealedchips.Add(tile);
            hands.PlayerHands[player].playerchips.Remove(tile);
        }
        hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count-1].transform.position = PlayerConvRemoveBox(turn).transform.position; //move taken tile out of middle
        hands.PlayerHands[player].temporaryrevealedchips.Clear(); //clear temp after done
        //will want to make this drop more organized later
        //happens before discard tile, so these won't be pulled back in
    }

    #region buttons
    public void NoTake()
    {
        //how do we send back a turn?
        Debug.Log("Didn't Take");
        action.text = "Didn't Take";
        hands.PlayerHands[PlayerConvNumber(turn)].temporaryrevealedchips.Clear();
        StartCoroutine(Turn(0, PlayerConvNumber(turn)));
    }

    public void ButtonChow()
    {
        Debug.Log("Chow!");
        action.text = "Chow!";
        //need something here to choose which chow if there are multiple
        RemoveFromHand(PlayerConvNumber(turn));
        StartCoroutine(Turn(1, PlayerConvNumber(turn)));
        return;
    }

    public void ButtonPong()
    {
        Debug.Log("Pong!");
        action.text = "Pong!";
        RemoveFromHand(PlayerConvNumber(turn));
        StartCoroutine(Turn(1, PlayerConvNumber(turn)));
        return;
    }

    public void ButtonKong()
    {
        Debug.Log("Kong!");
        action.text = "Kong!";
        RemoveFromHand(PlayerConvNumber(turn));
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
        Debug.Log("Tile was played, now assessing for chow, pong, or kong");
        List<TurnManager> turnArray = Enum.GetValues(typeof(TurnManager)).Cast<TurnManager>().ToList();
        int playerTurn = 0;
        int playerNext = 1;

        while (playerTurn < 4)
        {
            if (turn == turnArray[playerTurn])
            {
                /*
                if(playerNext!=0)
                {
                    invokebutton = true; //autoinvoke button press for non-east player
                }
                */

                int interruptTurn = CheckPongKongForAllPlayers(); //this will assign a turn if pong/kong is available

                if (turn == NumberConvPlayer(interruptTurn))
                {
                    interruptTurn = 5; //make sure you can't pong a tile you just played lmao
                }

                if (interruptTurn != 5) //5 is default return for no players have avail
                {
                    Debug.Log("turn interrupted to: " + NumberConvPlayer(interruptTurn));
                    turn = NumberConvPlayer(interruptTurn);
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
        action.text = "no actions";

        if (interruption == 0)
        {
            DealTile(player, tile); //don't deal if chow or pong
            tile += 1;
        }

        //if(CheckMahjong) { Debug.Log(player + " has won!"); Win(player); the function which displays TurnManager.WIN to player if player = 0 and LOST if not. 
        Debug.Log("Currently " + NumberConvPlayer(player) + " Turn");
        
        if(player != 0) //AI code, lowest level random discard and invoke all
        {
            yield return new WaitForSeconds(2f);//computer delay
            GameObject discardtile = hands.PlayerHands[player].playerchips[UnityEngine.Random.Range(0, hands.PlayerHands[player].playerchips.Count - 1)];
            Debug.Log(NumberConvPlayer(player) + " has played " + discardtile);
            discardtile.transform.position = PlayerConvDiscardBox(turn).transform.position;
        }
    }
}
