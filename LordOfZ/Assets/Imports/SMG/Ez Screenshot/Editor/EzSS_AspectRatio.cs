using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.EzScreenshot
{
	public class EzSS_AspectRatio 
	{
		public enum AspectType
		{
			Free,
			Portrait,
			Landscape
		}
		public static string[] portraitAspects = new string[]
		{
			"1:1",
			"2:3",
			"3:4",
			"3:4.3",
			"3:5",
			"4:5",
			"9:16",
			"9:18",
			"9:18.5",
			"9:19.5",
			"9:21",
			"10:16",
			"13:16",
			// Special to wide mockups
			"16:9",
			"16:10",
		};
		
		public static string[] landscapeAspects = new string[]
		{
			"3:2",
			"4:3",
			"4.3:3",
			"5:3",
			"5:4",
			"16:9",
			"16:10",
			"16:13",
			"18:9",
			"18.5:9",
			"19.5:9",
			"21:9"
		};
		
		public static Dictionary<string, string> InvertAspectRatio = new Dictionary<string, string>()
		{
			// Portrait
			{"2:3", "3:2"},
			{"3:4", "4:3"},
			{"3:5", "5:3"},
			{"3:4.3", "4.3:3"},
			{"4:5", "5:4"},
			{"9:16", "16:9"},
			{"9:18", "18:9"},
			{"9:18.5", "18.5:9"},
			{"9:19.5", "19.5:9"},
			{"9:21", "21:9"},
			{"10:16", "16:10"},
			{"13:16", "16:13"},
			// Landscape
			{"3:2", "2:3"},
			{"4.3:3", "3:4.3"},
			{"4:3", "3:4"},
			{"5:3", "3:5"},
			{"5:4", "4:5"},
			{"16:9", "9:16"},
			{"16:10", "10:16"},
			{"16:13", "13:16"},
			{"18:9", "9:18"},
			{"18.5:9", "9:18.5"},
			{"19.5:9", "9:19.5"},
			{"21:9", "9:21"}
    };
    
		public static Dictionary<string, Vector2> AspectsVectors = new Dictionary<string, Vector2>()
		{
			// Portrait
			{"1:1", new Vector2(1, 1)},
			{"2:3", new Vector2(2, 3)},
			{"3:4", new Vector2(3, 4)},
			{"3:4.3", new Vector2(21, 30.1f)},
			{"3:5", new Vector2(3, 5)},
			{"4:5", new Vector2(4, 5)},
			{"9:16", new Vector2(9, 16)},
			{"9:18", new Vector2(9, 18)},
			{"9:18.5", new Vector2(18, 37)},
			{"9:19.5", new Vector2(18, 39)},
			{"9:21", new Vector2(9, 21)},
			{"10:16", new Vector2(10, 16)},
			{"13:16", new Vector2(13, 16)},
			// Landscape
			{"3:2", new Vector2(3, 2)},
			{"4:3", new Vector2(4, 3)},
			{"4.3:3", new Vector2(30.1f, 21)},
			{"5:3", new Vector2(5, 3)},
			{"5:4", new Vector2(5, 4)},
			{"16:9", new Vector2(16, 9)},
			{"16:10", new Vector2(16, 10)},
			{"16:13", new Vector2(16, 13)},
			{"18:9", new Vector2(18, 9)},
			{"18.5:9", new Vector2(37, 18)},
			{"19.5:9", new Vector2(39, 18)},
			{"21:9", new Vector2(21, 9)}
    };
		
		public static Dictionary<string, float> AspectRatioResults = new Dictionary<string, float>()
		{
			// Portrait
			{"1:1", 1f},
			{"2:3", .666f},
			{"3:4", .75f},
			{"3:4.3", .697f},
			{"3:5", .6f},
			{"4:5", .8f},
			{"9:16", .5625f},
			{"9:18", .5f},
			{"9:18.5", .486f},
			{"9:19.5", .4615f},
			{"9:21", .4285f},
			{"10:16", .625f},
			{"13:16", .8125f},
			// Landscape
			{"3:2", 1.5f},
			{"4:3", 1.333f},
			{"4.3:3", 1.433f},
			{"5:3", 1.666f},
			{"5:4", 1.25f},
			{"16:9", 1.777f},
			{"16:10", 1.6f},
			{"16:13", 1.230f},
			{"18:9", 2f},
			{"18.5:9", 2.05f},
			{"19.5:9", 2.16f},
			{"21:9", 2.33f}
		};
	}
}