using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;

public class SmallWorldSearcher
{
	// 카드 이미지 링크 : https://images.ygoprodeck.com/images/cards/{카드번호}.jpg

	private readonly string dbAddress = @"Data Source=cards.db";
	private SQLiteConnection connect;

	private readonly Dictionary<string, CardData> CardDict = new();

	private readonly Dictionary<int, List<CardData>> LevelDict  = new();
	private readonly Dictionary<int, List<CardData>> AttributeDict = new();
	private readonly Dictionary<int, List<CardData>> RaceDict = new();
	private readonly Dictionary<int, List<CardData>> AtkDict = new();
	private readonly Dictionary<int, List<CardData>> DefDict = new();

	public SmallWorldSearcher()
	{
		connect = new SQLiteConnection(dbAddress);
		connect.Open();

		FillCardList(connect);
		connect.Close();
	}

	private void FillCardList(SQLiteConnection connect)
	{
		var sql = "SELECT * FROM swdb";
		var command = new SQLiteCommand(sql, connect);
		SQLiteDataReader reader = command.ExecuteReader();

		while (reader.Read())
		{
			if((reader.GetInt32(2) & CardData.TYPE_MASK) == 0)
			{
				var data = new CardData(reader.GetString(1), reader.GetInt32(5) & 0xF, reader.GetInt32(7), reader.GetInt32(6), reader.GetInt32(3), reader.GetInt32(4));
				if(!CardDict.ContainsKey(data.Name))
				{
					CardDict.Add(data.Name, data);

					if(!LevelDict.ContainsKey(data.Level))
						LevelDict.Add(data.Level, new List<CardData>());
					if(!AttributeDict.ContainsKey(data.Attribute))
						AttributeDict.Add(data.Attribute, new List<CardData>());
					if(!RaceDict.ContainsKey(data.Race))
						RaceDict.Add(data.Race, new List<CardData>());
					if(!AtkDict.ContainsKey(data.Atk))
						AtkDict.Add(data.Atk, new List<CardData>());
					if(!DefDict.ContainsKey(data.Def))
						DefDict.Add(data.Def, new List<CardData>());

					LevelDict[data.Level].Add(data);
					AttributeDict[data.Attribute].Add(data);
					RaceDict[data.Race].Add(data);
					AtkDict[data.Atk].Add(data);
					DefDict[data.Def].Add(data);
				}
			}
		}
	}

	private bool HasEdge(string card1, string card2) => HasEdge(CardDict[card1], CardDict[card2]);
	private bool HasEdge(string card1, CardData card2) => HasEdge(CardDict[card1], card2);
	private bool HasEdge(CardData card1, string card2) => HasEdge(card1, CardDict[card2]);
	private bool HasEdge(CardData card1, CardData card2)
	{
		int count = 0;
		if(card1.Race == card2.Race) ++count;
		if(card1.Level == card2.Level) ++count;
		if(card1.Attribute == card2.Attribute) ++count;
		if(card1.Atk == card2.Atk) ++count;
		if(card1.Def == card2.Def) ++count;

		return count == 1;
	}

	public CardData GetCardData(string cardName) => CardDict[cardName];

	public List<string> CardNameSearch(string cardName)
		=> CardDict.Keys.Where(name => name.Contains(cardName)).OrderBy(name => name.IndexOf(cardName)).ThenBy(name => name).ToList();

	public List<CardData> GetWaypoints(string dept, string dest)
	{
		List<CardData> filtered = new();
		List<CardData> waypoints = new();

		foreach(CardData card in RaceDict[CardDict[dept].Race])
			if(HasEdge(dept, card))
				filtered.Add(card);
		foreach(CardData card in LevelDict[CardDict[dept].Level])
			if(HasEdge(dept, card))
				filtered.Add(card);
		foreach(CardData card in AttributeDict[CardDict[dept].Attribute])
			if(HasEdge(dept, card))
				filtered.Add(card);
		foreach(CardData card in AtkDict[CardDict[dept].Atk])
			if(HasEdge(dept, card))
				filtered.Add(card);
		foreach(CardData card in DefDict[CardDict[dept].Def])
			if(HasEdge(dept, card))
				filtered.Add(card);

		foreach(CardData card in filtered)
			if(HasEdge(dest, card))
				waypoints.Add(card);

		return waypoints;
	}

	public List<Tuple<CardData, CardData, CardData>> GetDeckMap(List<string> deckList)
	{
		List<Tuple<CardData, CardData, CardData>> deckMap = new();
		List<CardData> tmpList = new();

		foreach(string card1 in deckList)
		{
			if(!CardDict.ContainsKey(card1)) continue;

			tmpList.Clear();
			foreach(string card2 in deckList)
			{
				if(HasEdge(card1, card2))
					tmpList.Add(CardDict[card2]);
			}

			foreach(CardData card2 in tmpList)
			{
				foreach(string card3 in deckList)
				{
					if(!CardDict.ContainsKey(card3)) continue;

					if(HasEdge(card2, card3) && card1 != card3)
						deckMap.Add(new Tuple<CardData, CardData, CardData>(CardDict[card1], card2, CardDict[card3]));
				}
			}
		}

		return deckMap;
	}
}
