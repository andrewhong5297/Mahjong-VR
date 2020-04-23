using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
	private string[] pai = new string[140]; //missing bonus tiles for now, ignoring red tiles
	public List<GameObject> pai_obj = new List<GameObject>();

	//list of gameobjects that store all chips
	//souzu
	public List<GameObject> GetTiles()
	{
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

		foreach (string tile_name in pai)
		{
			if (tile_name != "psouzu_5r" && tile_name != "pmanzu_5r" && tile_name != "ppinzu_5r" && tile_name != "ppinzu_5r (1)") //removing (r) for now, add in bonus tiles later. 
			{
				GameObject tile = GameObject.Find(tile_name);
				pai_obj.Add(tile);
			}
		}
		return pai_obj;
	}
}
