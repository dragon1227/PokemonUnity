﻿using PokemonUnity.Item;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class ApricornPlant : Entity
{
	public enum ApricornColors
	{
		White = 0,
		Black = 1,
		Yellow = 6,
		Green = 5,
		Red = 4,
		Blue = 3,
		Pink = 2
	}

	private ApricornColors ApricornColor = ApricornColors.White;
	private bool hasApricorn = true;

	public override void Initialize()
	{
		base.Initialize();

		CreateWorldEveryFrame = true;

		ApricornColor = GetApricornColor(System.Convert.ToInt32(AdditionalValue));
		CheckHasApricorn();
		ChangeTexture();
	}

	private void ChangeTexture()
	{
		Rectangle r = new Rectangle(16, 32, 16, 16);

		if (hasApricorn == true)
		{
			int x = GetColorCode(ApricornColor);
			int y = 0;

			while (x > 2)
			{
				x -= 3;
				y += 1;
			}

			r = new Rectangle(x * 16, y * 16, 16, 16);
		}

		Textures(0) = TextureManager.GetTexture("Apricorn", r);
	}

	private void CheckHasApricorn()
	{
		if (Core.Player.ApricornData == "")
			hasApricorn = true;
		else
		{
			List<string> ApricornsData = Core.Player.ApricornData.SplitAtNewline().ToList();

			bool hasRemoved = false;

			for (var i = 0; i <= ApricornsData.Count - 1; i++)
			{
				if (i < ApricornsData.Count)
				{
					string Apricorn = ApricornsData[i];

					Apricorn = Apricorn.Remove(0, 1);
					Apricorn = Apricorn.Remove(Apricorn.Length - 1, 1);

					string[] ApricornData = Apricorn.Split(System.Convert.ToChar("|"));

					if (ApricornData[0] == Screen.Level.LevelFile)
					{
						string[] PositionData = ApricornData[1].Split(System.Convert.ToChar(","));
						if (Position.x == System.Convert.ToInt32(PositionData[0]) & Position.y == System.Convert.ToInt32(PositionData[1]) & Position.z == System.Convert.ToInt32(PositionData[2]))
						{
							string[] d = ApricornData[2].Split(System.Convert.ToChar(","));

							DateTime PickDate = new DateTime(System.Convert.ToInt32(d[0]), System.Convert.ToInt32(d[1]), System.Convert.ToInt32(d[2]), System.Convert.ToInt32(d[3]), System.Convert.ToInt32(d[4]), System.Convert.ToInt32(d[5]));

							int diff = (DateTime.Now - PickDate).Hours;

							int hasToDiff = 24;
							if (P3D.World.CurrentSeason == P3D.World.Seasons.Winter | P3D.World.CurrentSeason == P3D.World.Seasons.Fall)
								hasToDiff = 12;

							if (diff >= hasToDiff)
							{
								ApricornsData.RemoveAt(i);
								i -= 1;
								hasApricorn = true;
								hasRemoved = true;
							}
							else
								hasApricorn = false;
						}
					}
				}
			}

			if (hasRemoved == true)
			{
				Core.Player.ApricornData = "";
				foreach (string Apricorn in ApricornsData)
				{
					if (Core.Player.ApricornData != "")
						Core.Player.ApricornData += Environment.NewLine;
					Core.Player.ApricornData += Apricorn;
				}
			}
		}
	}

	public ApricornColors GetApricornColor(int ColorCode)
	{
		return (ApricornColors)ColorCode;
	}

	public int GetColorCode(ApricornColors ApricornColor)
	{
		return System.Convert.ToInt32(ApricornColor);
	}

	public override void UpdateEntity()
	{
		if (Rotation.y != Screen.Camera.Yaw)
			Rotation.y = Screen.Camera.Yaw;

		base.UpdateEntity();
	}

	public override void Render()
	{
		Draw(Model, Textures, false);
	}

	public override void ClickFunction()
	{
		string text = "There are no apricorns~on this tree.*Better come back later...";

		if (hasApricorn == true)
		{
			Item Item = GetItem();

			text = "There is a " + Item.Name + "~on this tree.*Do you want to pick it?%Yes|No%";
		}

		Screen.TextBox.Show(text, this);
		SoundManager.PlaySound("select");
	}

	public override void ResultFunction(int Result)
	{
		if (Result == 0)
		{
			Item Item = GetItem();

			Core.Player.Inventory.AddItem(Item.ID, 1);
			PlayerStatistics.Track("[85]Apricorns picked", 1);
			SoundManager.PlaySound("item_found", true);
			Screen.TextBox.TextColor = TextBox.PlayerColor;
			Screen.TextBox.Show(Core.Player.Name + " picked the~" + Item.Name + ".*" + Core.Player.Inventory.GetMessageReceive(Item, 1), this);
			AddApriconSave();
			hasApricorn = false;
			ChangeTexture();
		}
	}

	private void AddApriconSave()
	{
		string s = "{";

		DateTime d = DateTime.Now;
		s += Screen.Level.LevelFile + "|" + System.Convert.ToInt32(Position.x) + "," + System.Convert.ToInt32(Position.y) + "," + System.Convert.ToInt32(Position.z) + "|" + d.Year + "," + d.Month + "," + d.Day + "," + d.Hour + "," + d.Minute + "," + d.Second + "}";

		if (Core.Player.ApricornData != "")
			Core.Player.ApricornData += Environment.NewLine;

		Core.Player.ApricornData += s;
	}

	private Items GetItem()
	{
		int ItemID = 0;

		switch (ApricornColor)
		{
			case ApricornColors.Red:
				{
					ItemID = 85;
					break;
				}

			case ApricornColors.Blue:
				{
					ItemID = 89;
					break;
				}

			case ApricornColors.Yellow:
				{
					ItemID = 92;
					break;
				}

			case ApricornColors.Green:
				{
					ItemID = 93;
					break;
				}

			case ApricornColors.White:
				{
					ItemID = 97;
					break;
				}

			case ApricornColors.Black:
				{
					ItemID = 99;
					break;
				}

			case ApricornColors.Pink:
				{
					ItemID = 101;
					break;
				}
		}

		return Item.GetItemByID(ItemID);
	}
}
}