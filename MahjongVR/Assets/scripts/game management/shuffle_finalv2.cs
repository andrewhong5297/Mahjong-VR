﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//functions are shown in order they are called/state is moved
//can we put clapping hands in as the dealer and to kind of bring life to AI?

//public enum TurnManager { START, EASTTURN, SOUTHTURN, WESTTURN, NORTHTURN, TEMPTURN, WON, LOST } //how to manage four canvases? 

public class shuffle_finalv2 : MonoBehaviour
{
	private int state = 0;
	private string[] pai = new string[140]; //missing bonus tiles for now, ignoring red tiles
	private List<GameObject> pai_obj = new List<GameObject>();
	public float tile_speed = 0.5f;

	int i = 4; //counting tiles for moving/distributing in state 2. Starting from 4 to include tiles 0-3 in first group
	GameObject setup_dist;

	float length_table, length_tile, height_tile, height_tile_double;

	private List<GameObject> EastHand = new List<GameObject>();
	private List<GameObject> SouthHand = new List<GameObject>();
	private List<GameObject> NorthHand = new List<GameObject>();
	private List<GameObject> WestHand = new List<GameObject>();

	// Start is called before the first frame update
	void Start()
	{
		#region Tiles
		//souzu
		pai[0] = "psouzu_5r";
		pai[1] = "psouzu_1";
		pai[2] = "psouzu_2";
		pai[3] = "psouzu_3";
		pai[4] = "psouzu_4";
		pai[5] = "psouzu_5";
		pai[6] = "psouzu_6";
		pai[7] = "psouzu_7";
		pai[8] = "psouzu_8";
		pai[9] = "psouzu_9";

		pai[10] = "pjihai_chun";
		pai[11] = "psouzu_1 (1)";
		pai[12] = "psouzu_2 (1)";
		pai[13] = "psouzu_3 (1)";
		pai[14] = "psouzu_4 (1)";
		pai[15] = "psouzu_5 (1)";
		pai[16] = "psouzu_6 (1)";
		pai[17] = "psouzu_7 (1)";
		pai[18] = "psouzu_8 (1)";
		pai[19] = "psouzu_9 (1)";

		pai[20] = "pjihai_chun (1)";
		pai[21] = "psouzu_1 (2)";
		pai[22] = "psouzu_2 (2)";
		pai[23] = "psouzu_3 (2)";
		pai[24] = "psouzu_4 (2)";
		pai[25] = "psouzu_5 (2)";
		pai[26] = "psouzu_6 (2)";
		pai[27] = "psouzu_7 (2)";
		pai[28] = "psouzu_8 (2)";
		pai[29] = "psouzu_9 (2)";

		pai[30] = "pjihai_chun (2)";
		pai[31] = "psouzu_1 (3)";
		pai[32] = "psouzu_2 (3)";
		pai[33] = "psouzu_3 (3)";
		pai[34] = "psouzu_4 (3)";
		pai[35] = "psouzu_5 (3)";
		pai[36] = "psouzu_6 (3)";
		pai[37] = "psouzu_7 (3)";
		pai[38] = "psouzu_8 (3)";
		pai[39] = "psouzu_9 (3)";

		//pinzu
		pai[40] = "ppinzu_5r";
		pai[41] = "ppinzu_1";
		pai[42] = "ppinzu_2";
		pai[43] = "ppinzu_3";
		pai[44] = "ppinzu_4";
		pai[45] = "ppinzu_5";
		pai[46] = "ppinzu_6";
		pai[47] = "ppinzu_7";
		pai[48] = "ppinzu_8";
		pai[49] = "ppinzu_9";

		pai[50] = "ppinzu_5r (1)";
		pai[51] = "ppinzu_1 (1)";
		pai[52] = "ppinzu_2 (1)";
		pai[53] = "ppinzu_3 (1)";
		pai[54] = "ppinzu_4 (1)";
		pai[55] = "ppinzu_5 (1)";
		pai[56] = "ppinzu_6 (1)";
		pai[57] = "ppinzu_7 (1)";
		pai[58] = "ppinzu_8 (1)";
		pai[59] = "ppinzu_9 (1)";

		pai[60] = "pjihai_chun (3)";
		pai[61] = "ppinzu_1 (2)";
		pai[62] = "ppinzu_2 (2)";
		pai[63] = "ppinzu_3 (2)";
		pai[64] = "ppinzu_4 (2)";
		pai[65] = "ppinzu_5 (2)";
		pai[66] = "ppinzu_6 (2)";
		pai[67] = "ppinzu_7 (2)";
		pai[68] = "ppinzu_8 (2)";
		pai[69] = "ppinzu_9 (2)";

		pai[70] = "pjihai_hatsu";
		pai[71] = "ppinzu_1 (3)";
		pai[72] = "ppinzu_2 (3)";
		pai[73] = "ppinzu_3 (3)";
		pai[74] = "ppinzu_4 (3)";
		pai[75] = "ppinzu_5 (3)";
		pai[76] = "ppinzu_6 (3)";
		pai[77] = "ppinzu_7 (3)";
		pai[78] = "ppinzu_8 (3)";
		pai[79] = "ppinzu_9 (3)";

		//manzu
		pai[80] = "pmanzu_5r";
		pai[81] = "pmanzu_1";
		pai[82] = "pmanzu_2";
		pai[83] = "pmanzu_3";
		pai[84] = "pmanzu_4";
		pai[85] = "pmanzu_5";
		pai[86] = "pmanzu_6";
		pai[87] = "pmanzu_7";
		pai[88] = "pmanzu_8";
		pai[89] = "pmanzu_9";

		pai[90] = "pjihai_hatsu (1)";
		pai[91] = "pmanzu_1 (1)";
		pai[92] = "pmanzu_2 (1)";
		pai[93] = "pmanzu_3 (1)";
		pai[94] = "pmanzu_4 (1)";
		pai[95] = "pmanzu_5 (1)";
		pai[96] = "pmanzu_6 (1)";
		pai[97] = "pmanzu_7 (1)";
		pai[98] = "pmanzu_8 (1)";
		pai[99] = "pmanzu_9 (1)";

		pai[100] = "pjihai_hatsu (2)";
		pai[101] = "pmanzu_1 (2)";
		pai[102] = "pmanzu_2 (2)";
		pai[103] = "pmanzu_3 (2)";
		pai[104] = "pmanzu_4 (2)";
		pai[105] = "pmanzu_5 (2)";
		pai[106] = "pmanzu_6 (2)";
		pai[107] = "pmanzu_7 (2)";
		pai[108] = "pmanzu_8 (2)";
		pai[109] = "pmanzu_9 (2)";

		pai[110] = "pjihai_hatsu (3)";
		pai[111] = "pmanzu_1 (3)";
		pai[112] = "pmanzu_2 (3)";
		pai[113] = "pmanzu_3 (3)";
		pai[114] = "pmanzu_4 (3)";
		pai[115] = "pmanzu_5 (3)";
		pai[116] = "pmanzu_6 (3)";
		pai[117] = "pmanzu_7 (3)";
		pai[118] = "pmanzu_8 (3)";
		pai[119] = "pmanzu_9 (3)";

		pai[120] = "pjihai_haku";
		pai[121] = "pjihai_haku (1)";
		pai[122] = "pjihai_haku (2)";
		pai[123] = "pjihai_haku (3)";

		pai[124] = "pjihai_pe";
		pai[125] = "pjihai_pe (1)";
		pai[126] = "pjihai_pe (2)";
		pai[127] = "pjihai_pe (3)";

		pai[128] = "pjihai_sha";
		pai[129] = "pjihai_sha (1)";
		pai[130] = "pjihai_sha (2)";
		pai[131] = "pjihai_sha (3)";

		pai[132] = "pjihai_nan";
		pai[133] = "pjihai_nan (1)";
		pai[134] = "pjihai_nan (2)";
		pai[135] = "pjihai_nan (3)";

		pai[136] = "pjihai_ton";
		pai[137] = "pjihai_ton (1)";
		pai[138] = "pjihai_ton (2)";
		pai[139] = "pjihai_ton (3)";
		#endregion
		foreach (string tile_name in pai)
		{
			if (tile_name != "psouzu_5r" && tile_name != "pmanzu_5r" && tile_name != "ppinzu_5r" && tile_name != "ppinzu_5r (1)") //removing (r) for now, add in bonus tiles later. 
			{
				GameObject tile = GameObject.Find(tile_name);
				pai_obj.Add(tile);
			}
		}
		setup_dist = new GameObject("setup_dist"); //to deal tiles out in groups

		GameObject table = GameObject.Find("table");
		BoxCollider table_collider = table.GetComponent<BoxCollider>();
		length_table = table_collider.size.x / 35f; //change the divide to adjust positioning of tiles if scale was changed

		GameObject tile2 = GameObject.Find("psouzu_5"); //random tile to get size
		BoxCollider tile_collider = tile2.GetComponent<BoxCollider>();
		length_tile = tile_collider.size.x / 10f;
		height_tile = tile_collider.size.y / 10f;
		height_tile_double = height_tile * 2f;
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

	bool movetiles(int i) // 4 out 12 times, east then ccw to s,w,n. first and 3rd top tiles go to east, then bottom first to south, top to west and last bottom to north. 
	{
		bool done = false;
		setup_dist.transform.position = pai_obj[i - 4].transform.position; //set it to position of first in group

		if (i < 48)
		{
			for (int x = i - 4; x < i; x++)
			{
				pai_obj[x].transform.parent = setup_dist.transform; //create parent to hold (i-4,i) from list.
			}
		}

		//first wall
		if (i == 4 || i == 20 || i == 36)
		{
			//first wall
			if (i == 4)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3].transform.position = Vector3.MoveTowards(pai_obj[i - 3].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1].transform.position = Vector3.MoveTowards(pai_obj[i - 1].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3].transform.position == end1 && pai_obj[i - 1].transform.position == end2;
				}
			}
			if (i == 20)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4 - 12) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2 - 12) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1 - 12) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3 - 12].transform.position = Vector3.MoveTowards(pai_obj[i - 3 - 12].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1 - 12].transform.position = Vector3.MoveTowards(pai_obj[i - 1 - 12].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3 - 12].transform.position == end1 && pai_obj[i - 1 - 12].transform.position == end2;
				}
			}

			if (i == 36)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4 - 24) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2 - 24) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1 - 24) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3 - 24].transform.position = Vector3.MoveTowards(pai_obj[i - 3 - 24].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1 - 24].transform.position = Vector3.MoveTowards(pai_obj[i - 1 - 24].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3 - 24].transform.position == end1 && pai_obj[i - 1 - 24].transform.position == end2;
				}
			}
		}

		#region other walls spaghetti code
		/*
		//second wall
		if (i == 8 || i == 24 || i == 40)
		{
			if (i == 8)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3].transform.position = Vector3.MoveTowards(pai_obj[i - 3].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1].transform.position = Vector3.MoveTowards(pai_obj[i - 1].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3].transform.position == end1 && pai_obj[i - 1].transform.position == end2;
				}
			}
			if (i == 24)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4 - 12) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2 - 12) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1 - 12) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3 - 12].transform.position = Vector3.MoveTowards(pai_obj[i - 3 - 12].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1 - 12].transform.position = Vector3.MoveTowards(pai_obj[i - 1 - 12].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3 - 12].transform.position == end1 && pai_obj[i - 1 - 12].transform.position == end2;
				}
			}

			if (i == 40)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4 - 24) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2 - 24) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1 - 24) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3 - 24].transform.position = Vector3.MoveTowards(pai_obj[i - 3 - 24].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1 - 24].transform.position = Vector3.MoveTowards(pai_obj[i - 1 - 24].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3 - 24].transform.position == end1 && pai_obj[i - 1 - 24].transform.position == end2;
				}
			}
		}

		//third wall
		if (i == 12 || i == 28 || i == 44)
		{
			if (i == 4)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3].transform.position = Vector3.MoveTowards(pai_obj[i - 3].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1].transform.position = Vector3.MoveTowards(pai_obj[i - 1].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3].transform.position == end1 && pai_obj[i - 1].transform.position == end2;
				}
			}
			if (i == 20)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4 - 12) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2 - 12) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1 - 12) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3 - 12].transform.position = Vector3.MoveTowards(pai_obj[i - 3 - 12].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1 - 12].transform.position = Vector3.MoveTowards(pai_obj[i - 1 - 12].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3 - 12].transform.position == end1 && pai_obj[i - 1 - 12].transform.position == end2;
				}
			}

			if (i == 36)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4 - 24) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2 - 24) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1 - 24) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3 - 24].transform.position = Vector3.MoveTowards(pai_obj[i - 3 - 24].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1 - 24].transform.position = Vector3.MoveTowards(pai_obj[i - 1 - 24].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3 - 24].transform.position == end1 && pai_obj[i - 1 - 24].transform.position == end2;
				}
			}
		}

		//fourth wall
		if (i == 16 || i == 32 || i == 48)
		{
			if (i == 4)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3].transform.position = Vector3.MoveTowards(pai_obj[i - 3].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1].transform.position = Vector3.MoveTowards(pai_obj[i - 1].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3].transform.position == end1 && pai_obj[i - 1].transform.position == end2;
				}
			}
			if (i == 20)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4 - 12) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2 - 12) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1 - 12) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3 - 12].transform.position = Vector3.MoveTowards(pai_obj[i - 3 - 12].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1 - 12].transform.position = Vector3.MoveTowards(pai_obj[i - 1 - 12].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3 - 12].transform.position == end1 && pai_obj[i - 1 - 12].transform.position == end2;
				}
			}

			if (i == 36)
			{
				//Move parent to player spread out by length_tile on x.
				Vector3 end = new Vector3(-length_table * 0.6f + (i - 4 - 24) * length_tile * 1.01f, height_tile_double, -length_table * 1.3f);
				setup_dist.transform.position = Vector3.MoveTowards(setup_dist.transform.position, end, tile_speed * Time.deltaTime);

				done = setup_dist.transform.position == end; //spread out children after it reaches end
				Debug.Log(done);

				if (done)
				{
					setup_dist.transform.DetachChildren(); //moving top tiles down
					Vector3 end1 = new Vector3(-length_table * 0.6f + (i - 2 - 24) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					Vector3 end2 = new Vector3(-length_table * 0.6f + (i - 1 - 24) * length_tile * 1.01f, height_tile, -length_table * 1.3f);
					pai_obj[i - 3 - 24].transform.position = Vector3.MoveTowards(pai_obj[i - 3 - 24].transform.position, end1, tile_speed * Time.deltaTime);
					pai_obj[i - 1 - 24].transform.position = Vector3.MoveTowards(pai_obj[i - 1 - 24].transform.position, end2, tile_speed * Time.deltaTime);
					done = pai_obj[i - 3 - 24].transform.position == end1 && pai_obj[i - 1 - 24].transform.position == end2;
				}
			}
		}
		*/
		#endregion

		Debug.Log(done);

		int PlaceInHand = i - 4;
		while (PlaceInHand < i)
		{
			if (i == 4 || i == 20 || i == 36)
				EastHand.Add(pai_obj[PlaceInHand]);
			if (i == 8 || i == 24 || i == 40)
				SouthHand.Add(pai_obj[PlaceInHand]);
			if (i == 12 || i == 28 || i == 44)
				WestHand.Add(pai_obj[PlaceInHand]);
			if (i == 16 || i == 32 || i == 48)
				NorthHand.Add(pai_obj[PlaceInHand]);
		}

		// send tiles to list<gameobject> for each player each pass. 
		if (i > 48)
		{
			//move double tile to east
			//EastHand.Add(pai_obj[i]);
			if (i > 50)
			{
				//move single tiles to south, west and north
			}
		}
		return done;
	}

	void show_tiles()
	{
		int i = 0;
		while (i < 48) // should flip by gameobject in Hand, instead of looping through again
		{
			pai_obj[i].transform.rotation = Quaternion.Euler(new Vector3(90f, 180f, 0));//rotate tiles 90f along y axis to show? can we rotate along bottom edge so it is more realistic?
			i++;
		}

		/*
		put this in later to show all hands at once, requires hands to be stored in player gameobject parent, then rotate each parent. Can adjust this so in a different state, it rotates based on parent.transform.rotation
		Quaternion player1 = new Quaternion(90f, 180f, 0f, 1f);

		pai_obj[i].transform.rotation = Quaternion.Slerp(pai_obj[i].transform.rotation, player1, Time.deltaTime * tile_speed);//rotate tiles 90f along y axis to show? can we rotate along bottom edge so it is more realistic?
		i++;
		*/
	}

	private void Update()
	{
		if (state == 0)
		{
			shuffle();
			set_tiles();
			state = 1;
		}

		if (state == 1)
		{
			if (i < 13) //removing count for now to test
			{
				movetiles(i);
				if (movetiles(i))
				{
					for (int a = 0; a < i; a++)
					{
						pai_obj[a].GetComponent<BoxCollider>().isTrigger = false;
						pai_obj[a].AddComponent<Rigidbody>();
					}
					i += 4;
				}
			}
			else
			{
				state = 2;
			}
		}

		if (state == 2)
		{
			show_tiles();
			Debug.Log("hello");
			state = 3;
		}

		if (state == 3)
		{
			//start_game(); do I put methods in a seperate script for organization purposes?
			//don't move to next state until object is let go inside of box
			//state pause for if can chow, kong, or pong (turn changing??? watch turn based by Brackey first)
		}
	}
}
