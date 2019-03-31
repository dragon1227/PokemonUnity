﻿using PokemonUnity.Inventory;
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
			//ChangeTexture();
		}

		//private void ChangeTexture()
		//{
		//	Vector4 r = new Vector4(16, 32, 16, 16);
		//
		//	if (hasApricorn)
		//	{
		//		int x = GetColorCode(ApricornColor);
		//		int y = 0;
		//
		//		while (x > 2)
		//		{
		//			x -= 3;
		//			y += 1;
		//		}
		//
		//		r = new Vector4(x * 16, y * 16, 16, 16);
		//	}
		//
		//	Textures[0] = TextureManager.GetTexture("Apricorn", r);
		//}

		private void CheckHasApricorn()
		{
			if (Game.Player.ApricornData == "")
				hasApricorn = true;
			else
			{
				List<string> ApricornsData = Game.Player.ApricornData.SplitAtNewline().ToList();

				bool hasRemoved = false;

				for (var i = 0; i <= ApricornsData.Count - 1; i++)
				{
					if (i < ApricornsData.Count)
					{
						string Apricorn = ApricornsData[i];

						Apricorn = Apricorn.Remove(0, 1);
						Apricorn = Apricorn.Remove(Apricorn.Length - 1, 1);

						string[] ApricornData = Apricorn.Split(System.Convert.ToChar("|"));

						if (ApricornData[0] == Game.Level.LevelFile)
						{
							string[] PositionData = ApricornData[1].Split(System.Convert.ToChar(","));
							if (Position.x == System.Convert.ToInt32(PositionData[0]) & Position.y == System.Convert.ToInt32(PositionData[1]) & Position.z == System.Convert.ToInt32(PositionData[2]))
							{
								string[] d = ApricornData[2].Split(System.Convert.ToChar(","));

								DateTime PickDate = new DateTime(System.Convert.ToInt32(d[0]), System.Convert.ToInt32(d[1]), System.Convert.ToInt32(d[2]), System.Convert.ToInt32(d[3]), System.Convert.ToInt32(d[4]), System.Convert.ToInt32(d[5]));

								int diff = (DateTime.Now - PickDate).Hours;

								int hasToDiff = 24;
								if (PokemonUnity.Overworld.World.CurrentSeason == PokemonUnity.Overworld.World.Seasons.Winter | PokemonUnity.Overworld.World.CurrentSeason == PokemonUnity.Overworld.World.Seasons.Fall)
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

				if (hasRemoved)
				{
					Game.Player.ApricornData = "";
					foreach (string Apricorn in ApricornsData)
					{
						if (Game.Player.ApricornData != "")
							Game.Player.ApricornData += System.Environment.NewLine;
						Game.Player.ApricornData += Apricorn;
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
			if (Rotation.y != Game.Camera.Yaw)
				Rotation.y = Game.Camera.Yaw;

			base.UpdateEntity();
		}

		public override void Render()
		{
			//Draw(Model, Textures, false);
		}

		public override void ClickFunction()
		{
			string text = "There are no apricorns~on this tree.*Better come back later...";

			if (hasApricorn)
			{
				Inventory.Item Item = new PokemonUnity.Inventory.Item(GetItem());

				text = "There is a " + Item.Name + "~on this tree.*Do you want to pick it?%Yes|No%";
			}

			Game.TextBox.Show(text, new Entity[] { this });
			SoundManager.PlaySound("select");
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				Inventory.Item Item = new PokemonUnity.Inventory.Item(GetItem());

				Game.Player.Bag.AddItem(Item.ItemId, 1);
				//Game.Player.Bag.AddItem(Item);
				//PlayerStatistics.Track("[85]Apricorns picked", 1);
				SoundManager.PlaySound("item_found", true);
				//Game.TextBox.TextColor = TextBox.PlayerColor;
				Game.TextBox.Show(Game.Player.Name + " picked the~" + Item.Name + ".*" + Game.Player.Bag.GetMessageReceive(Item, 1), new Entity[] { this });
				AddApriconSave();
				hasApricorn = false;
				//ChangeTexture();
			}
		}

		private void AddApriconSave()
		{
			string s = "{";

			DateTime d = DateTime.Now;
			s += Game.Level.LevelFile + "|" + System.Convert.ToInt32(Position.x) + "," + System.Convert.ToInt32(Position.y) + "," + System.Convert.ToInt32(Position.z) + "|" + d.Year + "," + d.Month + "," + d.Day + "," + d.Hour + "," + d.Minute + "," + d.Second + "}";

			if (Game.Player.ApricornData != "")
				Game.Player.ApricornData += System.Environment.NewLine;

			Game.Player.ApricornData += s;
		}

		private Items GetItem()
		{
			//int ItemID = 0;
			Items ItemID = 0;

			switch (ApricornColor)
			{
				case ApricornColors.Red:
					{
						//ItemID = 85;
						ItemID = Items.RED_APRICORN;
						break;
					}
				case ApricornColors.Blue:
					{
						//ItemID = 89;
						ItemID = Items.BLUE_APRICORN;
						break;
					}
				case ApricornColors.Yellow:
					{
						//ItemID = 92;
						ItemID = Items.YELLOW_APRICORN;
						break;
					}
				case ApricornColors.Green:
					{
						//ItemID = 93;
						ItemID = Items.GREEN_APRICORN;
						break;
					}
				case ApricornColors.White:
					{
						//ItemID = 97;
						ItemID = Items.WHITE_APRICORN;
						break;
					}
				case ApricornColors.Black:
					{
						//ItemID = 99;
						ItemID = Items.BLACK_APRICORN;
						break;
					}
				case ApricornColors.Pink:
					{
						//ItemID = 101;
						ItemID = Items.PINK_APRICORN;
						break;
					}
			}

				return (Items)ItemID; //Inventory.GetItem(ItemID);
		}
	}
}