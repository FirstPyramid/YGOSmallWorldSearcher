using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CardData
{
	public static Dictionary<int, string> ATTRIBUTES = new Dictionary<int, string>()
	{
		{0x1, "땅"},
		{0x2, "물"},
		{0x4, "화염"},
		{0x8, "바람"},
		{0x10, "빛"},
		{0x20, "어둠"},
		{0x40, "신"}
	};

	public static Dictionary<int, string> RACES = new Dictionary<int, string>()
	{
		{0x1, "전사족"},
		{0x2, "마법사족"},
		{0x4, "천사족"},
		{0x8, "악마족"},
		{0x10, "언데드족"},
		{0x20, "기계족"},
		{0x40, "물족"},
		{0x80, "화염족"},
		{0x100, "암석족"},
		{0x200, "비행야수족"},
		{0x400, "식물족"},
		{0x800, "곤충족"},
		{0x1000, "번개족"},
		{0x2000, "드래곤족"},
		{0x4000, "야수족"},
		{0x8000, "야수전사족"},
		{0x10000, "공룡족"},
		{0x20000, "어류족"},
		{0x40000, "해룡족"},
		{0x80000, "파충류족"},
		{0x100000, "사이킥족"},
		{0x200000, "환신야수족"},
		{0x400000, "창조신족"},
		{0x800000, "환룡족"},
		{0x1000000, "사이버스족"}
	};

	public static int TYPE_MASK = 0x1C9FE14E;

	public string Name { get; set; } = "";
	public int Level { get; set; }
	public int Attribute { get; set; }
	public int Race { get; set; }
	public int Atk { get; set; }
	public int Def { get; set; }

	public CardData(string name, int level, int attribute, int race, int atk, int def)
	{
		Name = name;
		Level = level;
		Attribute = attribute;
		Race = race;
		Atk = atk;
		Def = def;
	}
}
