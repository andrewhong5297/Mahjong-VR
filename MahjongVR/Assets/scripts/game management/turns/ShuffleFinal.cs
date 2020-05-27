using Com.MyCompany.MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//functions are shown in order they are called/state is moved
//can we put clapping hands in as the dealer and to kind of bring life to AI?
public enum TurnManager { EASTTURN, SOUTHTURN, WESTTURN, NORTHTURN, START, WON, LOST } //how to manage four canvases? 

public class ShuffleFinal : MonoBehaviour
{
	public int state;

	//following two objects are only for testing connection with photon
	public Launcher launchstate;
	public GameObject testcube;

	public float tile_speed;
	int i = 4; //counting tiles for moving/distributing in state 2. Starting from 4 to include tiles 0-3 in first group
	GameObject setup_dist;
	Tiles tile = new Tiles();
	public List<GameObject> pai_obj;

	public float length_table, length_tile, height_tile, height_tile_double;

	//for show rotation
	public Quaternion EastRot = Quaternion.Euler(new Vector3(90f, 180f, 0));
	public Quaternion SouthRot = Quaternion.Euler(new Vector3(90f, 0, -90f));
	public Quaternion WestRot = Quaternion.Euler(new Vector3(90f, -180f, 0));
	public Quaternion NorthRot = Quaternion.Euler(new Vector3(90f, 0, 90f));

	public GameHandData[] PlayerHands; //contains playerchips[]

	public Text action;

	// Start is called before the first frame update
	void Awake()
	{
		action.text = "East Turn to start the game";
		pai_obj = tile.GetTiles();
		setup_dist = new GameObject("setup_dist"); //to deal tiles out in groups

		GameObject table = GameObject.Find("table");
		BoxCollider table_collider = table.GetComponent<BoxCollider>();
		length_table = table_collider.size.x / 35f; //change the divide to adjust positioning of tiles if scale was changed

		GameObject tile2 = GameObject.Find("psouzu_5"); //random tile to get size
		BoxCollider tile_collider = tile2.GetComponent<BoxCollider>();
		length_tile = tile_collider.size.x / 10f * 1.35f; //adjust if scale of board and pieces changed
		height_tile = tile_collider.size.y / 10f * 0.6f; //adjust if scale of board and pieces changed
		height_tile_double = height_tile * 2f; //adjust if scale of board and pieces changed

		//if(launchstate.joinedroom)
		//{
		//	testcube.GetComponent<Material>().SetColor("_Color", Color.green);
		//}
	}

	void shuffle()
	{
		for (int i = 0; i < pai_obj.Count; i++)
		{
			GameObject temp = pai_obj[i];
			int randomIndex = Random.Range(i, pai_obj.Count);
			pai_obj[i] = pai_obj[randomIndex]; //switch position after taking object into temp. Otherwise you have doubles. 
			pai_obj[randomIndex] = temp;
		}
		Debug.Log(pai_obj[0]);

		//assigning to hand
		for (int i = 0; i < 53; i++)
		{
			if ((i >= 0 && i <= 3) || (i >= 16 && i <= 19) || (i >= 32 && i <= 35) || (i >= 48 && i <= 49)) //East
			{ PlayerHands[0].playerchips.Add(pai_obj[i]); }

			if ((i >= 4 && i <= 7) || (i >= 20 && i <= 23) || (i >= 36 && i <= 39) || (i == 50)) //South
			{ PlayerHands[1].playerchips.Add(pai_obj[i]); }

			if ((i >= 8 && i <= 11) || (i >= 24 && i <= 27) || (i >= 40 && i <= 43) || (i == 51)) //West
			{ PlayerHands[2].playerchips.Add(pai_obj[i]); }

			if ((i >= 12 && i <= 15) || (i >= 28 && i <= 31) || (i >= 44 && i <= 47) || (i == 52)) //North
			{ PlayerHands[3].playerchips.Add(pai_obj[i]); }
		}
	}

	void set_tiles()
	{
		Vector3 end = new Vector3();
		//after shuffle, set tiles 2 tiles high and 17 long,four walls. Set the order to be 1,2 top bottom etc. 
		//technically shuffle should start from dice roll wall, can add that in as special feature later. may also need to reverse ordering on placement for animation consistency. 

		for (int a = 0; a < pai_obj.Count; a++)
		{
			if (a <= 33 && a >= 0) //first wall
			{
				if (a % 2 == 0) //even
				{
					end = new Vector3(-length_table / 1.3f + (a * length_tile) / 2, height_tile_double, -length_table * 0.9f);
					pai_obj[a].transform.position = end;
				}
				else //odd
				{
					end = new Vector3(-length_table / 1.3f + ((a - 1) * length_tile) / 2, height_tile, -length_table * 0.9f);
					pai_obj[a].transform.position = end;
				}
			}

			if (a <= 67 && a >= 34) //second wall
			{
				if (a % 2 == 0) //even
				{
					end = new Vector3(length_table, height_tile_double, -length_table * 0.8f + ((a - 33) * length_tile) / 2);
					pai_obj[a].transform.position = end;
					pai_obj[a].transform.rotation = Quaternion.Euler(new Vector3(pai_obj[a].transform.rotation.x, -90f, 180));
				}
				else //odd
				{
					end = new Vector3(length_table, height_tile, -length_table * 0.8f + ((a - 34) * length_tile) / 2);
					pai_obj[a].transform.position = end;
					pai_obj[a].transform.rotation = Quaternion.Euler(new Vector3(pai_obj[a].transform.rotation.x, -90f, 180));
				}
			}

			if (a <= 101 && a >= 68) //third wall
			{
				if (a % 2 == 0) //even
				{
					end = new Vector3(-length_table / 1.3f + ((a - 67) * length_tile) / 2, height_tile_double, length_table * 0.9f);
					pai_obj[a].transform.position = end;
				}
				else //odd
				{
					end = new Vector3(-length_table / 1.3f + ((a - 68) * length_tile) / 2, height_tile, length_table * 0.9f);
					pai_obj[a].transform.position = end;
				}
			}

			if (a <= 135 && a >= 102) //fourth wall
			{
				if (a % 2 == 0) //even
				{
					end = new Vector3(-length_table, height_tile_double, -length_table * 0.8f + ((a - 101) * length_tile) / 2);
					pai_obj[a].transform.position = end;
					pai_obj[a].transform.rotation = Quaternion.Euler(new Vector3(pai_obj[a].transform.rotation.x, -90f, 180));
				}
				else //odd
				{
					end = new Vector3(-length_table, height_tile, -length_table * 0.8f + ((a - 102) * length_tile) / 2);
					pai_obj[a].transform.position = end;
					pai_obj[a].transform.rotation = Quaternion.Euler(new Vector3(pai_obj[a].transform.rotation.x, -90f, 180));
				}
			}
		}
	}

	void set_player_tiles()
	{
		Vector3 end = new Vector3();
		//This function can be adapted later for animation

		int e = 0, s = 0, w = 0, n = 0;

		for (int a = 0; a < 53; a++)
		{
			if (PlayerHands[0].playerchips.Contains(pai_obj[a])) //east wall
			{
				end = new Vector3(-length_table / 1.5f + (e * 2.05f * length_tile) / 2, height_tile * 1.2f, -length_table * 1.3f);
				pai_obj[a].transform.position = end;
				e += 1;
			}

			if (PlayerHands[1].playerchips.Contains(pai_obj[a])) //south wall
			{
				end = new Vector3(length_table * 1.3f, height_tile * 1.2f, -length_table * 0.5f + (s * 2.05f * length_tile) / 2);
				pai_obj[a].transform.position = end;
				pai_obj[a].transform.rotation = Quaternion.Euler(new Vector3(pai_obj[a].transform.rotation.x, -90f, 180));
				s += 1;
			}

			if (PlayerHands[2].playerchips.Contains(pai_obj[a])) //west wall
			{
				end = new Vector3(length_table / 1.5f - (w * 2.05f * length_tile) / 2, height_tile * 1.2f, length_table * 1.3f);
				pai_obj[a].transform.position = end;
				w += 1;
			}

			if (PlayerHands[3].playerchips.Contains(pai_obj[a])) //north wall
			{
				end = new Vector3(-length_table * 1.3f, height_tile * 1.2f, length_table * 0.5f - (n * 2.05f * length_tile) / 2);
				pai_obj[a].transform.position = end;
				pai_obj[a].transform.rotation = Quaternion.Euler(new Vector3(pai_obj[a].transform.rotation.x, -90f, 180));
				n += 1;
			}
		}
	}

	void show_tiles()
	{

		foreach (GameObject go in PlayerHands[0].playerchips) //East
		{
			go.transform.rotation = EastRot;
		}

		foreach (GameObject go in PlayerHands[1].playerchips) //South
		{
			go.transform.rotation = SouthRot;
		}

		foreach (GameObject go in PlayerHands[2].playerchips) //West
		{
			go.transform.rotation = WestRot;
		}

		foreach (GameObject go in PlayerHands[3].playerchips) //North
		{
			go.transform.rotation = NorthRot;
		}

		/*
		put this in later to show all hands at once, requires hands to be stored in player gameobject parent, then rotate each parent. Can adjust this so in a different state, it rotates based on parent.transform.rotation
		Quaternion player1 = new Quaternion(90f, 180f, 0f, 1f);

		pai_obj[i].transform.rotation = Quaternion.Slerp(pai_obj[i].transform.rotation, player1, Time.deltaTime * tile_speed);//rotate tiles 90f along y axis to show? can we rotate along bottom edge so it is more realistic?
		i++;
		*/
		for (int i = 0; i < 53; i++)
		{
			pai_obj[i].GetComponent<BoxCollider>().enabled = true;
			pai_obj[i].AddComponent<Rigidbody>();
		}
	}

	private void Update()
	{
		if (state == 0)
		{
			shuffle();
			set_tiles();
			set_player_tiles(); //use this until movetiles is fixed
			show_tiles();
			state = 10;
		}
	}
}
