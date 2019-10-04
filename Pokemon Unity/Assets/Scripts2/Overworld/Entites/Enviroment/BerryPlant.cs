﻿using PokemonUnity.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class BerryPlant : Entity
	{
		private int Phase = 0;
		private int Grow = 0;
		private int BerryIndex = 0;
		private int BerryGrowTime = 0;
		private Inventory.Item.Berry Berry;
		private int Berries = 0;
		private string PlantDate = "";
		private bool FullGrown = false;

		private List<bool> Watered = new List<bool>();

		private DateTime LastUpdateDate;

		public void Initialize(int BerryIndex, int BerriesYield, string Watered, string Time, bool FullGrown)
		{
			this.Berry = (Inventory.Item.Berry)Inventory.Item.GetItem((Inventory.Items)BerryIndex + 2000);
			this.Berries = BerriesYield;
			this.PlantDate = Time;
			this.BerryIndex = BerryIndex;
			this.FullGrown = FullGrown;

			InitBerry(Time);

			this.LastUpdateDate = DateTime.Now;

			if (Watered.CountSplits(",") != 4)
				this.Watered.AddRange(new bool[]
				{
					false,
					false,
					false,
					false
				});
			else
				foreach (string b in Watered.Split(System.Convert.ToChar(",")))
					this.Watered.Add(System.Convert.ToBoolean(b));

			this.NeedsUpdate = true;
			this.CreateWorldEveryFrame = true;
		}

		private void InitBerry(string oldDate)
		{
			string[] Data = oldDate.Split(System.Convert.ToChar(","));
			DateTime d = new DateTime(System.Convert.ToInt32(Data[0]), System.Convert.ToInt32(Data[1]), System.Convert.ToInt32(Data[2]), System.Convert.ToInt32(Data[3]), System.Convert.ToInt32(Data[4]), System.Convert.ToInt32(Data[5]));

			if (FullGrown)
			{
				this.Phase = 4;

				NewTexture();
			}
			else
			{
				var withBlock = DateTime.UtcNow;
				int diff = (DateTime.Now - d).Seconds;

				Grow += diff;

				while (Grow >= Berry.PhaseTime)
				{
					Grow -= Berry.PhaseTime;
					this.Phase += 1;

					if (this.Phase > 4)
						this.Phase = 0;
				}

				NewTexture();
			}
		}

		public override void Update()
		{
			if (this.LastUpdateDate.Year == 1)
				this.LastUpdateDate = DateTime.Now;

			int diff = (DateTime.Now - LastUpdateDate).Seconds;
			if (diff > 0)
			{
				this.Grow += diff;

				if (this.Grow >= this.Berry.PhaseTime)
				{
					while (Grow >= Berry.PhaseTime)
					{
						Grow -= Berry.PhaseTime;
						this.Phase += 1;

						if (this.Phase > 4)
							this.Phase = 0;
					}
					NewTexture();
				}
			}

			this.LastUpdateDate = DateTime.Now;
		}

		public override void UpdateEntity()
		{
			if (this.Rotation.y != Game.Camera.Yaw)
				this.Rotation.y = Game.Camera.Yaw;

			base.UpdateEntity();
		}

		private void NewTexture()
		{
			//Texture2D t;
			//if (this.Phase > 1)
			//{
			//    int x = this.Berry.BerryIndex * 128 + ((Phase - 1) * 32);
			//    int y = 0;
			//    while (x > 512)
			//    {
			//        x -= 512;
			//        y += 32;
			//    }
			//    Vector4 r = new Vector4(x, y, 32, 32);
			//    t = TextureManager.GetTexture(@"Textures\Berries", r, "");
			//}
			//else
			//{
			//    Vector4 r;
			//    switch (this.Phase)
			//    {
			//        case 0:
			//            {
			//                r = new Vector4(448, 480, 32, 32);
			//                break;
			//            }
			//
			//        case 1:
			//            {
			//                r = new Vector4(480, 480, 32, 32);
			//                break;
			//            }
			//    }
			//    t = TextureManager.GetTexture(@"Items\ItemSheet", r, "");
			//}

			//this.Textures[0] = t;
		}

		private int ResultIndex = 0;

		public override void ClickFunction()
		{
			string text = "";

			bool hasBottle = false;
			if (Game.Player.Bag.GetItemAmount((PokemonUnity.Inventory.Items)175) > 0)
				hasBottle = true;

			switch (this.Phase)
			{
				case 0:
					{
						this.ResultIndex = 1;
						if (hasBottle)
							text = "One " + this.Berry.Name + " Berry was~planted here.*Do you want to~water it?%Yes|No%";
						else
							text = "One " + this.Berry.Name + " Berry was~planted here.";
						break;
					}
				case 1:
					{
						this.ResultIndex = 1;
						if (hasBottle)
							text = Berry.Name + " has sprouted.*Do you want to~water it?%Yes|No%";
						else
							text = Berry.Name + " has sprouted.";
						break;
					}
				case 2:
					{
						this.ResultIndex = 1;
						if (hasBottle)
							text = "This " + Berry.Name + " plant is~growing taller.*Do you want to~water it?%Yes|No%";
						else
							text = "This " + Berry.Name + " plant is~growing taller.";
						break;
					}
				case 3:
					{
						this.ResultIndex = 1;
						if (hasBottle)
							text = "These " + Berry.Name + " flowers~are blooming.*Do you want to~water it?%Yes|No%";
						else
							text = "These " + Berry.Name + " flowers~are blooming.";
						break;
					}
				case 4:
					{
						this.ResultIndex = 0;
						if (this.Berries == 1)
							text = "There is a~" + Berry.Name + " Berry!*Do you want to pick~it?%Yes|No%";
						else
							text = "There are " + Berries + "~" + Berry.Name + " Berries!*Do you want to pick~them?%Yes|No%";
						break;
					}
			}

			Game.TextBox.Show(text, new Entity[] { this });
			SoundManager.PlaySound("select");
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				switch (this.ResultIndex)
				{
					case 0:
						{
							Game.Player.Bag.AddItem(this.Berry.ItemId, this.Berries);
							string Text = "";
							if (this.Berries == 1)
								Text = Game.Player.Name + " picked the~" + Berry.Name + " Berry.*" + Game.Player.Bag.GetMessageReceive(Berry, this.Berries);
							else
								Text = Game.Player.Name + " picked the " + Berries + "~" + Berry.Name + " Berries.*" + Game.Player.Bag.GetMessageReceive(Berry, this.Berries);

							//Game.Player.AddPoints(2, "Picked berries.");
							//PlayerStatistics.Track("[2006]Berries picked", this.Berries);

							SoundManager.PlaySound("item_found", true);
							//Game.TextBox.TextColor = TextBox.PlayerColor;
							Game.TextBox.Show(Text, new Entity[] { this });
							RemoveBerry();
							Game.Level.Entities.Remove(this);
							break;
						}
					case 1:
						{
							WaterBerry();
							string Text = Game.Player.Name + " watered~the " + Berry.Name + ".";
							Game.TextBox.Show(Text, new Entity[] { this });
							break;
						}
				}
			}
		}

		public override void Render()
		{
			//this.Draw(this.Model, Textures, false);
		}

		private void RemoveBerry()
		{
			string[] Data = Game.Player.BerryData.SplitAtNewline();
			string OutData = "";

			foreach (string Berry in Data)
			{
				if (Berry != "")
				{
					if (!Berry.ToLower().StartsWith("{" + Game.Level.LevelFile.ToLower() + "|" + (this.Position.x + "," + this.Position.y + "," + this.Position.z).ToLower() + "|"))
					{
						if (OutData != "")
							OutData += System.Environment.NewLine;
						OutData += Berry;
					}
				}
			}

			Game.Player.BerryData = OutData;
		}

		public static void AddBerryPlant(string LevelFile, Vector3 Position, int BerryIndex)
		{
			DateTime cD = DateTime.Now;
			string DateData = cD.Year + "," + cD.Month + "," + cD.Day + "," + cD.TimeOfDay.Hours + "," + cD.TimeOfDay.Minutes + "," + cD.TimeOfDay.Seconds;

			Inventory.Item.Berry Berry = (Inventory.Item.Berry)Inventory.Item.GetItem((Inventory.Items)BerryIndex + 2000);

			int BerryAmount = GetBerryAmount(Berry, 0);

			string WateredData = "0,0,0,0";

			string FullGrownData = "0";

			string Data = "{" + LevelFile + "|" + Position.x + "," + Position.y + "," + Position.z + "|" + BerryIndex + "|" + BerryAmount + "|" + WateredData + "|" + DateData + "|" + FullGrownData + "}";

			string OldData = Game.Player.BerryData;
			if (OldData != "")
				OldData += System.Environment.NewLine;
			OldData += Data;

			Game.Player.BerryData = OldData;

			Entity newEnt = Entity.GetNewEntity(Entities.BerryPlant, Position, new Texture2D[]
			{
				null/* TODO Change to default(_) if this is not a reference type */
			}, new int[]
			{
				0,
				0
			}, true, new Vector3(0,0,0), new Vector3(1,1,1)/*, UnityEngine.Mesh.BillModel*/, 0, "", true, new Vector3(1.0f,1,1), -1, "", "", new Vector3(0,0,0));
			((BerryPlant)newEnt).Initialize(BerryIndex, 0, "", DateData, false);
			Game.Level.Entities.Add(newEnt);

			Game.Player.Bag.RemoveItem((Inventory.Items)BerryIndex + 2000, 1);
		}

		private static int GetBerryAmount(Inventory.Item.Berry Berry, int Watered)
		{
			if (Watered > 0)
			{
				int a = Berry.maxBerries;
				int b = Berry.minBerries;
				int c = Core.Rand.Next(b, a + 1);
				int d = Watered;

				int amount = System.Convert.ToInt32((((a - b) * (d - 1) + c) / (double)4) + b);
				if (amount < Berry.minBerries)
					amount = Berry.minBerries;

				int seasonGrow = 0;
				switch (PokemonUnity.Overworld.World.CurrentSeason)
				{
					case PokemonUnity.Overworld.World.Seasons.Winter:
						{
							seasonGrow = Berry.WinterGrow;
							break;
						}
					case PokemonUnity.Overworld.World.Seasons.Spring:
						{
							seasonGrow = Berry.SpringGrow;
							break;
						}
					case PokemonUnity.Overworld.World.Seasons.Summer:
						{
							seasonGrow = Berry.SummerGrow;
							break;
						}
					case PokemonUnity.Overworld.World.Seasons.Fall:
						{
							seasonGrow = Berry.FallGrow;
							break;
						}
				}

				switch (seasonGrow)
				{
					case 0:
						{
							amount = Berry.minBerries;
							break;
						}
					case 1:
						{
							amount -= 1;
							break;
						}
					case 2:
						{
							break;
						}
					case 3:
						{
							amount += 1;
							break;
						}
				}

				amount = amount.Clamp(Berry.minBerries, Berry.maxBerries);

				return amount;
			}
			else
				return Berry.minBerries;
		}

		private void WaterBerry()
		{
			bool b = this.Watered[this.Phase];
			if (!b)
			{
				this.Watered[this.Phase] = true;

				RemoveBerry();

				string DateData = PlantDate;

				Inventory.Item.Berry Berry = (Inventory.Item.Berry)Inventory.Item.GetItem((Inventory.Items)BerryIndex + 2000);

				string WateredData = "";
				int wateredCount = 0;
				foreach (bool w in this.Watered)
				{
					if (WateredData != "")
						WateredData += ",";
					if (w)
					{
						WateredData += "1";
						wateredCount += 1;
					}
					else
						WateredData += "0";
				}

				int BerryAmount = GetBerryAmount(Berry, wateredCount);

				string Data = "{" + Game.Level.LevelFile + "|" + this.Position.x + "," + this.Position.y + "," + this.Position.z + "|" + BerryIndex + "|" + BerryAmount + "|" + WateredData + "|" + DateData + "}";

				string OldData = Game.Player.BerryData;
				if (OldData != "")
					OldData += System.Environment.NewLine;
				OldData += Data;

				Game.Player.BerryData = OldData;
			}
		}
	}
}