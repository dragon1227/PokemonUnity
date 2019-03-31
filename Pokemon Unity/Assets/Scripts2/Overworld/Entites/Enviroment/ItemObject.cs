﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class ItemObject : Entity
	{
		private static Dictionary<string, Texture2D> AnimationTexturesTemp = new Dictionary<string, Texture2D>();
		private string AnimationName = "";
		private Animation Animation = null;// TODO Change to default(_) if this is not a reference type

		private Inventory.Item Item;
		private int ItemID = 0;
		private bool checkedExistence = false;
		private string AnimationPath = "";
		private int X, Y, width, height, rows, columns, animationSpeed, startRow, startColumn;
		private Vector4 CurrentRectangle = new Vector4(0, 0, 0, 0);
		private bool CanInteractWith = true;

		public new void Initialize(List<List<int>> AnimationData = null)
		{
			base.Initialize();

			this.Item = new Inventory.Item((Inventory.Items)System.Convert.ToInt32(this.AdditionalValue.GetSplit(1)));
			this.ItemID = System.Convert.ToInt32(this.AdditionalValue.GetSplit(0));

			//this.Textures[0] = this.Item.Texture;
			if (this.ActionValue == 0)
				this.Visible = Visible;
			else if (this.ActionValue == 1)
			{
				this.Visible = false;
				this.Collision = false;
			}
			else if (this.ActionValue == 2)
			{
				if (Game.Player.Bag.HasMegaBracelet())
				{
					this.Visible = Visible;
					// sparkles
					if (AnimationData != null)
					{
						X = AnimationData[0][0];
						Y = AnimationData[0][1];
						width = AnimationData[0][2];
						height = AnimationData[0][3];
						rows = AnimationData[0][4];
						columns = AnimationData[0][5];
						animationSpeed = AnimationData[0][6];
						startRow = AnimationData[0][7];
						startColumn = AnimationData[0][8];
						AnimationPath = "ItemAnimations";
					}
					else
					{
						X = 0;
						Y = 0;
						width = 48;
						height = 48;
						rows = 5;
						columns = 10;
						animationSpeed = 60;
						startRow = 0;
						startColumn = 0;
						AnimationPath = "SparkleAnimation";
					}
					CreateAnimationTextureTemp();

					//this.Animation = new Animation(TextureManager.GetTexture(@"Textures\Routes"), rows, columns, 16, 16, animationSpeed, startRow, startColumn);
				}
				else
				{
					this.Visible = false;
					this.Collision = false;
					CanInteractWith = false;
				}
			}

			this.NeedsUpdate = true;
		}

		public static void ClearAnimationResources()
		{
			AnimationTexturesTemp.Clear();
		}

		private void CreateAnimationTextureTemp()
		{
			// If Core.GameOptions.GraphicStyle = 1 Then
			Vector4 r = new Vector4(X, Y, width, height);
			this.AnimationName = AnimationPath + "," + X + "," + Y + "," + height + "," + width;
			if (!AnimationTexturesTemp.ContainsKey(AnimationName + "_0"))
			{
				for (var i = 0; i <= this.rows - 1; i++)
				{
					//for (var j = 0; j <= this.columns - 1; j++)
					//    AnimationTexturesTemp.Add(AnimationName + "_" + (j + columns * i).ToString(), TextureManager.GetTexture(AnimationPath, new Vector4(r.x + r.width * j, r.y + r.height * i, r.width, r.height)));
				}
			}
		}

		public override void Update()
		{
			if (!checkedExistence)
			{
				checkedExistence = true;

				if (ItemExists(this))
					RemoveItem(this);
			}

			if (this.IsHiddenItem())
			{
				if (this.Opacity > 0.0f)
				{
					this.Opacity -= 0.01f;
					if (this.Opacity <= 0.0f)
					{
						this.Opacity = 1.0f;
						this.Visible = false;
					}
				}
			}

			base.Update();
		}

		public override void UpdateEntity()
		{
			if (this.Rotation.y != Game.Camera.Yaw)
			{
				this.Rotation.y = Game.Camera.Yaw;
				this.CreatedWorld = false;
			}

			if (Animation != null)
			{
				//Animation.Update(0.01f);
				//if (CurrentRectangle != Animation.TextureRectangle)
				//{
				//    ChangeTexture();
				//    CurrentRectangle = Animation.TextureRectangle;
				//}
			}

			base.UpdateEntity();
		}

		private void ChangeTexture()
		{
			// If Core.GameOptions.GraphicStyle = 1 Then

			if (AnimationTexturesTemp.Count == 0)
			{
				ClearAnimationResources();
				CreateAnimationTextureTemp();
			}
			//var i = Animation.CurrentRow;
			//var j = Animation.CurrentColumn;
			//this.Textures[0] = ItemObject.AnimationTexturesTemp[AnimationName + "_" + (j + columns * i)];
		}

		public override void ClickFunction()
		{
			if (CanInteractWith)
			{
				RemoveItem(this);
				SoundManager.PlaySound("item_found", true);
				//Game.TextBox.TextColor = TextBox.PlayerColor;
				Game.TextBox.Show(Game.Player.Name + " found~" + this.Item.Name + "!*" + Game.Player.Bag.GetMessageReceive(this.Item, 1), new Entity[] { this });
				Game.Player.Bag.AddItem(this.Item.ItemId, 1);
				//PlayerStatistics.Track("Items found", 1);

				//Game.Player.AddPoints(1, "Found an item.");
			}
		}

		public override void Render()
		{
			//this.Draw(this.Model, Textures, false);
		}

		public static bool ItemExists(ItemObject ItemObject)
		{
			if (Game.Player.ItemData != "")
			{
				if (Game.Player.ItemData.Contains(","))
				{
					string[] IDs = Game.Player.ItemData.ToLower().Split(System.Convert.ToChar(","));

					if (IDs.Contains((Game.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower()))
						return true;
					else
						return false;
				}
				else if (Game.Player.ItemData.ToLower() == (Game.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower())
					return true;
				else
					return false;
			}
			else
				return false;
		}

		public static void RemoveItem(ItemObject ItemObject)
		{
			Game.Level.Entities.Remove(ItemObject);

			if (Game.Player.ItemData == "")
				Game.Player.ItemData = (Game.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower();
			else
			{
				string[] IDs = Game.Player.ItemData.Split(System.Convert.ToChar(","));
				if (!IDs.Contains((Game.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower()))
					Game.Player.ItemData += "," + (Game.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower();
			}
		}

		public bool IsHiddenItem()
		{
			if (!this.Collision & this.ActionValue == 1)
				return true;
			else
				return false;
		}
	}
}