using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectPlayed : MonoBehaviour
{
    public TurnManagerMJ turnmanagerMJ = new TurnManagerMJ();
    public ShuffleFinal hands = new ShuffleFinal();
    
    public GameObject lastplayed;
    public Text action;

    // Update is called once per frame
    void Update()
    {
        //check positioning for snapback, place under if statement to check current state and player turn? This might be expensive. 

        //how to highlight if tile is about to be grabbed? using OnTriggerEnter(other = hand?)

        //move lastplayed object on top of tile. This is probably an expensive update to be running. 
        if (hands.PlayerHands[4].playerchips.Count >= 1)
        {
            if (hands.PlayerHands[4].playerchips[hands.PlayerHands[4].playerchips.Count - 1] == gameObject)
            {
                lastplayed.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+0.5f*hands.height_tile, gameObject.transform.position.z);
                lastplayed.transform.rotation = gameObject.transform.rotation;
                lastplayed.transform.parent = gameObject.transform;
            }
        }
    }

    //may add option to sort by suit and value?
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "PlayArea")
        {
            action.text = action.text + "\n" + turnmanagerMJ.turn + "just played: " + name;

            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, gameObject.transform.rotation.y, 0));
            
            hands.PlayerHands[4].playerchips.Add(gameObject);//move to center array
            hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips.Remove(gameObject);//remove from player array

            int handsize = hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips.Count; //size of hand to reorder

            if (turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn) == 0)
            {
                for (int e = 0; e < handsize; e++)
                {
                    hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips[e].transform.position = new Vector3(-hands.length_table / 1.5f + (e * 2.05f * hands.length_tile) / 2, hands.height_tile * 1.2f, -hands.length_table * 1.3f);
                    hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips[e].transform.rotation = hands.EastRot;
                }
            }

            if (turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn) == 1)
            {
                for (int s = 0; s < handsize; s++)
                {
                    hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips[s].transform.position = new Vector3(hands.length_table * 1.3f, hands.height_tile * 1.2f, -hands.length_table * 0.5f + (s * 2.05f * hands.length_tile) / 2);
                    hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips[s].transform.rotation = hands.SouthRot;
                }
            }

            if (turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn) == 2)
            {
                for (int w = 0; w < handsize; w++)
                {
                    hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips[w].transform.position = new Vector3(hands.length_table / 1.5f - (w * 2.05f * hands.length_tile) / 2, hands.height_tile * 1.2f, hands.length_table * 1.3f);
                    hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips[w].transform.rotation = hands.WestRot;
                }
            }

            if (turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn) == 3)
            {
                for (int n = 0; n < handsize; n++)
                {
                    hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips[n].transform.position = new Vector3(-hands.length_table * 1.3f, hands.height_tile * 1.2f, hands.length_table * 0.5f - (n * 2.05f * hands.length_tile) / 2);
                    hands.PlayerHands[turnmanagerMJ.PlayerConvNumber(turnmanagerMJ.turn)].playerchips[n].transform.rotation = hands.NorthRot;
                }
            }
            turnmanagerMJ.TilePlayed(); //this goes last so that next tile dealed happens after tiles are moved
        }
    }
}
