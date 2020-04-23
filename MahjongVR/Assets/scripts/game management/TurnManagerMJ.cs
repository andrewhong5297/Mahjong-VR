using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerMJ : MonoBehaviour
{
    // Start is called before the first frame update
    public ShuffleFinal hands = new ShuffleFinal();
    public TurnManager turn;
    public int tile = 53;

    public GameObject chowbuttonEast;
    public GameObject pongbuttonEast;
    public GameObject kongbuttonEast;
    public GameObject container;

    void Start()
    {
        //maybe setup game here using shuffle methods
        turn = TurnManager.EASTTURN; //default always first player to start, will need to make it so east just plays a tile and doesn't get dealt one. Maybe freeze unfreeze all here.
        chowbuttonEast.SetActive(false);
        pongbuttonEast.SetActive(false);
        kongbuttonEast.SetActive(false);
        container.SetActive(false);
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
    bool CheckMahjong(int player) 
    {
        //check next player
        //hands.pai_obj[1].GetComponent<Chip>
        bool avail = false;
        return avail;
        //check for all winstates in player hand given current card, run it once for each hand. 
        //remove from hand
    }
    bool CheckChow(int player)
    {
        bool ChowAvail = false;
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

        //three types of chows
        bool twosides = false;
        bool twolower = false;
        bool twohigher = false;
        
        if (suitvalues.Contains(ValuePlayedTile-1) && suitvalues.Contains(ValuePlayedTile-2))
        {
            twolower = true;
            ChowAvail = true;
        }

        if (suitvalues.Contains(ValuePlayedTile - 1) && suitvalues.Contains(ValuePlayedTile + 1))
        {
            twosides = true;
            ChowAvail = true;
        }

        if (suitvalues.Contains(ValuePlayedTile + 1) && suitvalues.Contains(ValuePlayedTile + 2))
        {
            twohigher = true;
            ChowAvail = true;
        }

        //sort suitvalues
        //Check for three consecutive values -> sort, then remove duplicates. Then check if there are three one's in a row when a difference is taken. 
        if (ChowAvail)
        {
            Debug.Log("Can Chow of type twolower: " + twolower + " twohigher: " + twohigher + " twosides: " + twosides);
            //If there is, reverse find gameobject using foreach if(value && suit) give UI button option to chow. move these tiles to revealedchips and then move them physically to the right corner.
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
        }

        if (PongAvail)
        {
            Debug.Log(player + " player can Pong");
            //If there is, reverse find gameobject and give UI button option to chow. move these tiles to revealedchips and then move them physically to the right corner.
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
        }

        if (KongAvail)
        {
            Debug.Log(player + " player Kong");
            //If there is, reverse find gameobject and give UI button option to chow. move these tiles to revealedchips and then move them physically to the right corner.
        }
        return KongAvail;
    }

    //currently checks in playerhand that just played too, though that shouldn't be a problem unless someone delibrately ponged/konged themself just to reveal the tiles? 
    //currently only returns first player to have a pong, should make it an array of size 8 later to assign buttons for all
    int CheckPongKongForAllPlayers()
    {
        for (int i = 0; i <= 3; i++)
        {
            if (CheckPong(i))
            {
                pongbuttonEast.SetActive(true);
                container.SetActive(true);
                return i; 
            }
            if (CheckKong(i))
            {
                kongbuttonEast.SetActive(true);
                container.SetActive(true);
                return i;
            }
        }
        return 5;
    }

    #endregion

    void RemoveFromHand()
    {
        //how do we select tiles? maybe move all tiles to a revealedtiles array? what to do about multiple chows? If check chow is multiple what to do? 

        //move ToMove to playercorner[i]
        //this function removes chow,kong, or pong from hand when called. 
        //will need to resort and move all tiles in hand
    }

    #region buttons
    public void ButtonChow()
    {
        Debug.Log("Chow!");
        RemoveFromHand();
        if (turn == TurnManager.EASTTURN)
        {
            chowbuttonEast.SetActive(false);
            container.SetActive(false);
            EastTurn(1);
            return;
        }
        if (turn == TurnManager.SOUTHTURN)
        {
            chowbuttonEast.SetActive(false);
            container.SetActive(false);
            SouthTurn(1);
            return;
        }
        if (turn == TurnManager.WESTTURN)
        {
            chowbuttonEast.SetActive(false);
            container.SetActive(false);
            WestTurn(1);
            return;
        }
        if (turn == TurnManager.NORTHTURN)
        {
            chowbuttonEast.SetActive(false);
            container.SetActive(false);
            NorthTurn(1);
            return;
        }
    }

    public void ButtonPong()
    {
        Debug.Log("Pong!");
        RemoveFromHand();
        if (turn == TurnManager.EASTTURN)
        {
            pongbuttonEast.SetActive(false);
            container.SetActive(false);
            EastTurn(1);
            return;
        }
        if (turn == TurnManager.SOUTHTURN)
        {
            pongbuttonEast.SetActive(false);
            container.SetActive(false);
            SouthTurn(1);
            return;
        }
        if (turn == TurnManager.WESTTURN)
        {
            pongbuttonEast.SetActive(false);
            container.SetActive(false);
            WestTurn(1);
            return;
        }
        if (turn == TurnManager.NORTHTURN)
        {
            pongbuttonEast.SetActive(false);
            container.SetActive(false);
            NorthTurn(1);
            return;
        }
    }

    public void ButtonKong()
    {
        Debug.Log("Kong!");
        RemoveFromHand();
        if (turn == TurnManager.EASTTURN)
        {
            kongbuttonEast.SetActive(false);
            container.SetActive(false);
            EastTurn(1);
            return;
        }
        if (turn == TurnManager.SOUTHTURN)
        {
            kongbuttonEast.SetActive(false);
            container.SetActive(false);
            SouthTurn(1);
            return;
        }
        if (turn == TurnManager.WESTTURN)
        {
            kongbuttonEast.SetActive(false);
            container.SetActive(false);
            WestTurn(1);
            return;
        }
        if (turn == TurnManager.NORTHTURN)
        {
            kongbuttonEast.SetActive(false);
            container.SetActive(false);
            NorthTurn(1);
            return;
        }
    }
    #endregion

    TurnManager NumberConvPlayer(int input)
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

    public void TilePlayed()
    {
        Debug.Log("Tile was played, now assessing for chow, pong, or kong");

        if (turn == TurnManager.EASTTURN)
        {
            /*
            int interruptTurn = CheckPongKongForAllPlayers(); //this will assign a turn 
            if(interruptTurn != 5)
            {
                //turn = NumberConvPlayer(interruptTurn); //will need to make option to ignore kong. Need to refactor this function and turn function below.
                //move box to player? 
                //return;
            }
            */
            turn = TurnManager.SOUTHTURN;

            if (CheckChow(1))
            {
                SouthTurn(0);
                //chowbuttonEast.SetActive(true);
                //container.SetActive(true);
                return; //should just be able to return out afterwards
            }
            else
            {
                SouthTurn(0);
            }
            return;
        }

        if (turn == TurnManager.SOUTHTURN)
        {
            //CheckPongKongForAllPlayers();
            turn = TurnManager.WESTTURN;
            if (CheckChow(2))
            {
                WestTurn(0);
                return;
            }
            else
            {
                WestTurn(0);
            }
            return;
        }

        if (turn == TurnManager.WESTTURN)
        {
            //CheckPongKongForAllPlayers();
            turn = TurnManager.NORTHTURN;
            if (CheckChow(3))
            {
                NorthTurn(0);
                return;
            }
            else
            {
                 NorthTurn(0);
            }
            return;
        }

        if (turn == TurnManager.NORTHTURN)
        {
            //CheckPongKongForAllPlayers();
            turn = TurnManager.EASTTURN;
            if (CheckChow(0))
            {
                EastTurn(0);
                return;
            }
            else
            {
                EastTurn(0);
            }
            return;
        }
    }

    #region Turns
    void EastTurn(int interruption)
    {
        if (interruption == 0)
        {
            DealTile(0, tile); //don't deal if chow or pong
            tile += 1;
        }
        Debug.Log("Currently East Turn");
        //code for random discard 
        //UI shift text, timer start, etc. 
        //CheckMahjong
    }

    void SouthTurn(int interruption)
    {
        if (interruption == 0)
        {
            DealTile(1, tile);
            tile += 1;
        }
        Debug.Log("Currently South Turn");
        //code for random discard 
        //UI shift text, timer start, etc. 
        //CheckMahJong()
    }

    void WestTurn(int interruption)
    {
        if (interruption == 0)
        {
            DealTile(2, tile); 
            tile += 1;
        }
        Debug.Log("Currently West Turn");
        //code for random discard 
        //UI shift text, timer start, etc. 
        //CheckMahJong()
    }

    void NorthTurn(int interruption)
    {
        if (interruption == 0)
        {
            DealTile(3, tile); 
            tile += 1;
        }
        Debug.Log("Currently North Turn");
        //code for random discard 
        //UI shift text, timer start, etc. 
        //CheckMahJong()
    }

    #endregion
}
