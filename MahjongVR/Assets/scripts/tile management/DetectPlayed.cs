using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayed : MonoBehaviour
{
    public TurnManagerMJ turnmanagerMJ = new TurnManagerMJ();
    public ShuffleFinal hands = new ShuffleFinal();
    public GameObject lastplayed;
    // Update is called once per frame
    void Update()
    {
        //check positioning for snapback, place under if statement to check current state and player turn? This might be expensive. 

        //how to highlight if tile is about to be grabbed? using OnTriggerEnter(other = hand?)

        //highlights edge of tile and pulses it
    }

    int CurrentPlayer()
    {
        int i = 0;
        if(turnmanagerMJ.turn == TurnManager.EASTTURN)
        {
            i = 0;
        }

        if (turnmanagerMJ.turn == TurnManager.SOUTHTURN)
        {
            i = 1;
        }

        if (turnmanagerMJ.turn == TurnManager.WESTTURN)
        {
            i = 2;
        }

        if (turnmanagerMJ.turn == TurnManager.NORTHTURN)
        {
            i = 3;
        }
        return i;
    }

    //update below to just reorder everything by suit and value and replace all tiles for each player. 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayArea")
        {
            hands.PlayerHands[4].playerchips.Add(gameObject);
            Debug.Log(name + " was played");
            int position = hands.PlayerHands[CurrentPlayer()].playerchips.IndexOf(gameObject); //getting index to move dealed tile to
            if (turnmanagerMJ.tile == 53) //move 0th tile in list, since there is no deal tile to move
            {
                Debug.Log("moving tile first turn");
                hands.PlayerHands[0].playerchips[0].transform.position = new Vector3(-hands.length_table / 1.5f + (position * 2.05f * hands.length_tile) / 2, hands.height_tile * 1.2f, -hands.length_table * 1.3f);
                hands.PlayerHands[0].playerchips.Insert(position, hands.PlayerHands[0].playerchips[0]);
                hands.PlayerHands[0].playerchips.Remove(hands.PlayerHands[0].playerchips[0]);
            }
            else
            {
                if (hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1] != hands.pai_obj[turnmanagerMJ.tile-1]) //if played tile is not the same as dealt tile then have to move dealed tile into played tile spot
                {
                    Debug.Log("different tile");
                    if (CurrentPlayer() == 0)
                    {
                        //there is a bug here with placing tiles for east player for some reason. 
                        hands.pai_obj[turnmanagerMJ.tile-1].transform.position = new Vector3(-hands.length_table / 1.5f + (position * 2.05f * hands.length_tile + hands.length_tile*0.95f) / 2, hands.height_tile * 1.2f, -hands.length_table * 1.3f);
                        Debug.Log("moving deal tile to played tile position " + hands.pai_obj[turnmanagerMJ.tile] + hands.pai_obj[turnmanagerMJ.tile].transform.position);
                        hands.PlayerHands[0].playerchips.Insert(position, hands.pai_obj[turnmanagerMJ.tile - 1]);
                        hands.PlayerHands[0].playerchips.RemoveAt(hands.PlayerHands[0].playerchips.Count-1);
                    }
                    if (CurrentPlayer() == 1)
                    {
                        hands.pai_obj[turnmanagerMJ.tile-1].transform.position = new Vector3(hands.length_table * 1.3f, hands.height_tile * 1.2f, -hands.length_table * 0.5f + (position * 2.05f * hands.length_tile) / 2);
                        Debug.Log("moving deal tile to played tile position " + hands.pai_obj[turnmanagerMJ.tile] + hands.pai_obj[turnmanagerMJ.tile].transform.position);
                        hands.PlayerHands[1].playerchips.Insert(position, hands.pai_obj[turnmanagerMJ.tile - 1]);
                        hands.PlayerHands[1].playerchips.RemoveAt(hands.PlayerHands[1].playerchips.Count - 1);
                    }
                    if (CurrentPlayer() == 2)
                    {
                       hands.pai_obj[turnmanagerMJ.tile-1].transform.position = new Vector3(hands.length_table / 1.5f - (position * 2.05f * hands.length_tile) / 2, hands.height_tile * 1.2f, hands.length_table * 1.3f);
                       Debug.Log("moving deal tile to played tile position " + hands.pai_obj[turnmanagerMJ.tile] + hands.pai_obj[turnmanagerMJ.tile].transform.position);
                        hands.PlayerHands[2].playerchips.Insert(position, hands.pai_obj[turnmanagerMJ.tile - 1]);
                        hands.PlayerHands[2].playerchips.RemoveAt(hands.PlayerHands[2].playerchips.Count - 1);
                    }
                    if (CurrentPlayer() == 3)
                    {
                        hands.pai_obj[turnmanagerMJ.tile-1].transform.position = new Vector3(-hands.length_table * 1.3f, hands.height_tile * 1.2f, hands.length_table * 0.5f - (position * 2.05f * hands.length_tile) / 2);
                        Debug.Log("moving deal tile to played tile position " + hands.pai_obj[turnmanagerMJ.tile] + hands.pai_obj[turnmanagerMJ.tile].transform.position);
                        hands.PlayerHands[3].playerchips.Insert(position, hands.pai_obj[turnmanagerMJ.tile - 1]);
                        hands.PlayerHands[3].playerchips.RemoveAt(hands.PlayerHands[3].playerchips.Count - 1);
                    }
                }
            }
            hands.PlayerHands[CurrentPlayer()].playerchips.Remove(gameObject);
            turnmanagerMJ.TilePlayed(); //this goes last so that next tile dealed happens after tiles are moved
        }
    }
}
