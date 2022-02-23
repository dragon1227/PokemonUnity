﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Misc
{
	public class NetworkPlayer : Entity
	{
		private static readonly string[] FallbackSkins = new string[] { "0", "1", "2", "5", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "32", "49", "61", "63", "oldhatman", "PinkShirtGirl", "bugcatcher" };
		private static Dictionary<int, string> FallBack = new Dictionary<int, string>();

		public string Name = "";

		/// <summary>
		/// The Network ID of the player
		/// </summary>
		public int NetworkID = 0;

		public int faceRotation;
		public string MapFile = "";

		private string GameJoltID = "";
		private bool RotatedSprite = false;

		public string TextureID;
		public Texture2D Texture;

		public bool moving = false;
		private Vector4 lastRectangle = new Vector4(0, 0, 0, 0);
		private int AnimationX = 1;
		const float AnimationDelayLenght = 1.1f;
		private float AnimationDelay = AnimationDelayLenght;
		public bool HasPokemonTexture = false;

		private Texture2D NameTexture;

		private string LastName = "";

		private string LevelFile = "";

		public string BusyType = "0";

		private bool DownloadingSprite = false;
		private bool CheckForOnlineSprite = false;

		//public NetworkPlayer(float X, float Y, float Z, Texture2D[] Textures, string TextureID, int Rotation, int ActionValue, string AdditionalValue, Vector3 Scale, string Name, int ID) : base(X, Y, Z, "NetworkPlayer", Textures,
		//	new int[]
		//	{
		//		0,
		//		0
		//	}, true, Rotation, Scale/*, UnityEngine.Mesh.BillModel*/, 0, "", new Vector3(1.0f,1,1))
		//{
		//    this.Name = Name;
		//    this.NetworkID = ID;
		//    this.faceRotation = Rotation;
		//    this.TextureID = TextureID;
		//    this.Collision = false;
		//    this.NeedsUpdate = true;
		//
		//
		//	AssignFallback(ID);
		//
		//
		//	SetTexture(TextureID);
		//
		//	ChangeTexture();
		//    this.CreateWorldEveryFrame = true;
		//
		//    this.DropUpdateUnlessDrawn = false;
		//}

		private void AssignFallback(int ID)
		{
			if (!FallBack.ContainsKey(ID))
				FallBack.Add(ID, FallbackSkins[Core.Rand.Next(0, FallbackSkins.Length)]);
		}

		public void SetTexture(string TextureID)
		{
			this.TextureID = TextureID;

			string texturePath = GetTexturePath(TextureID);

			Texture2D OnlineSprite = null;//* TODO Change to default(_) if this is not a reference type
			if (this.GameJoltID != "")
			{
				//if (GameJolt.Emblem.HasDownloadedSprite(this.GameJoltID))
				//	OnlineSprite = GameJolt.Emblem.GetOnlineSprite(this.GameJoltID);
				//else
				//{
				//	System.Threading.Thread t = new System.Threading.Thread(DownloadOnlineSprite);
				//	t.IsBackground = true;
				//	t.Start();
				//	DownloadingSprite = true;
				//}
			}

			if (OnlineSprite != null)
				this.Texture = OnlineSprite;
			else if (TextureManager.TextureExist(texturePath))
			{
				Game.DebugLog("Change network texture to [" + texturePath + "]");

				if (texturePath.StartsWith(@"Pokemon\"))
					this.HasPokemonTexture = true;
				else
					this.HasPokemonTexture = false;

				this.Texture = TextureManager.GetTexture(texturePath);
			}
			else
			{
				Game.DebugLog("Texture fallback!");
				this.Texture = TextureManager.GetTexture(@"Textures\NPC\" + FallBack[this.NetworkID]);
			}
		}

		//private void DownloadOnlineSprite()
		//{
		//	Texture2D t = GameJolt.Emblem.GetOnlineSprite(this.GameJoltID);
		//
		//	if (t != null)
		//		this.Texture = t;
		//}

		public static string GetTexturePath(string TextureID)
		{
			string texturePath = @"Textures\NPC\";
			bool isPokemon = false;
			if (TextureID.StartsWith("[POKEMON|N]") | TextureID.StartsWith("[Pokémon|N]"))
			{
				TextureID = TextureID.Remove(0, 11);
				isPokemon = true;
				texturePath = @"Pokemon\Overworld\Normal\";
			}
			else if (TextureID.StartsWith("[POKEMON|S]") | TextureID.StartsWith("[Pokémon|S]"))
			{
				TextureID = TextureID.Remove(0, 11);
				isPokemon = true;
				texturePath = @"Pokemon\Overworld\Shiny\";
			}
			return texturePath + TextureID;
		}

		private void ChangeTexture()
		{
			if (this.Texture != null)
			{
				Vector4 r = new Vector4(0, 0, 0, 0);
				int cameraRotation = Game.Camera.GetFacingDirection();
				int spriteIndex = this.faceRotation - cameraRotation;

				spriteIndex = this.faceRotation - cameraRotation;
				if (spriteIndex < 0)
					spriteIndex += 4;

				if (RotatedSprite)
				{
					switch (spriteIndex)
					{
						case 1:
							{
								spriteIndex = 3;
								break;
							}
						case 3:
							{
								spriteIndex = 1;
								break;
							}
					}
				}

				//Size spriteSize = new Size(System.Convert.ToInt32(this.Texture.width / (double)3), System.Convert.ToInt32(this.Texture.height / (double)4));
				//
				//int x = 0;
				//if (this.moving)
				//	x = GetAnimationX() * spriteSize.width;
				//
				//r = new Vector4(x, spriteSize.height * spriteIndex, spriteSize.width, spriteSize.height);
				//
				//if (r != lastRectangle)
				//{
				//	lastRectangle = r;
				//
				//	Textures[0] = TextureManager.GetTexture(this.Texture, r, 1);
				//}
			}
		}

		private int GetAnimationX()
		{
			if (this.HasPokemonTexture)
			{
				switch (AnimationX)
				{
					case 1:
						{
							return 0;
						}
					case 2:
						{
							return 1;
						}
					case 3:
						{
							return 0;
						}
					case 4:
						{
							return 1;
						}
				}
			}
			switch (AnimationX)
			{
				case 1:
					{
						return 0;
					}
				case 2:
					{
						return 1;
					}
				case 3:
					{
						return 0;
					}
				case 4:
					{
						return 2;
					}
			}
			return 1;
		}

		private void Move()
		{
			if (this.moving)
			{
				this.AnimationDelay -= 0.1f;
				if (this.AnimationDelay <= 0.0f)
				{
					this.AnimationDelay = AnimationDelayLenght;
					AnimationX += 1;
					if (AnimationX > 4)
						AnimationX = 1;
				}
			}
		}

		protected override float CalculateCameraDistance(Vector3 CPosition)
		{
			return base.CalculateCameraDistance(CPosition) - 0.2f;
		}

		public override void UpdateEntity()
		{
			if (this.Rotation.y != Game.Camera.Yaw)
				this.Rotation.y = Game.Camera.Yaw;
			if (this.TextureID != null && this.TextureID.ToLower() == "nilllzz" & this.GameJoltID == "17441")
			{
				this.Rotation.z = MathHelper.Pi;
				RotatedSprite = true;
			}
			else
			{
				RotatedSprite = false;
				this.Rotation.z = 0;
			}

			ChangeTexture();

			base.UpdateEntity();
		}

		public override void Update()
		{
			if (this.Name != this.LastName)
			{
				this.LastName = this.Name;
				//this.NameTexture = SpriteFontTextToTexture(FontManager.InGameFont, this.Name);
			}

			Move();

			//if (DownloadingSprite && GameJolt.Emblem.HasDownloadedSprite(GameJoltID))
			//{
			//	SetTexture(TextureID);
			//	ChangeTexture();
			//	DownloadingSprite = false;
			//}

			base.Update();
		}

		public override void Render()
		{
			//if (ConnectScreen.Connected)
			//{
			//	if (IsCorrectScreen())
			//	{
			//		this.Draw(this.Model, Textures, false);
			//		if (Core.GameOptions.ShowGUI)
			//		{
			//			if (this.NameTexture != null)
			//			{
			//				var state = GraphicsDevice.DepthStencilState;
			//				GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
			//				Draw(UnityEngine.Mesh.BillModel,
			//					new Texture2D[]
			//					{
			//						this.NameTexture
			//					}, false);
			//				GraphicsDevice.DepthStencilState = state;
			//			}
			//			if (this.BusyType != "0")
			//				RenderBubble();
			//		}
			//	}
			//}
		}

		private void RenderBubble()
		{
			MessageBulb b = null/* TODO Change to default(_) if this is not a reference type */;
			switch (BusyType)
			{
				case "1":
					{
						b = new MessageBulb(new Vector3(this.Position.x, this.Position.y + 1, this.Position.z), MessageBulb.NotifcationTypes.Battle);
						break;
					}
				case "2":
					{
						b = new MessageBulb(new Vector3(this.Position.x, this.Position.y + 1, this.Position.z), MessageBulb.NotifcationTypes.Waiting);
						break;
					}
				case "3":
					{
						b = new MessageBulb(new Vector3(this.Position.x, this.Position.y + 1, this.Position.z), MessageBulb.NotifcationTypes.AFK);
						break;
					}
			}
			if (b != null)
			{
				b.Visible = this.Visible;
				b.Render();
			}
		}

		//private bool IsCorrectScreen()
		//{
		//	Screen.Identifications[] screens = new[] { Screen.Identifications.BattleCatchScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleGrowStatsScreen, Screen.Identifications.BattleScreen, Screen.Identifications.CreditsScreen, Screen.Identifications.BattleAnimationScreen, Screen.Identifications.ViewModelScreen, Screen.Identifications.HallofFameScreen };
		//	if (screens.Contains(Core.CurrentScreen.Identification))
		//		return false;
		//	else if (Core.CurrentScreen.Identification == Screen.Identifications.TransitionScreen)
		//	{
		//		if (screens.Contains((TransitionScreen)Core.CurrentScreen.OldScreen.Identification) | screens.Contains((TransitionScreen)Core.CurrentScreen.NewScreen.Identification))
		//			return false;
		//	}
		//	return true;
		//}

		//public void ApplyPlayerData(Servers.Player p)
		//{
		//	try
		//	{
		//		this.NetworkID = p.ServersID;
		//
		//		this.Position = p.Position;
		//
		//		this.Name = p.Name;
		//
		//		if (!p.Skin.StartsWith("[POKEMON|N]") && !p.Skin.StartsWith("[Pokémon|N]") && !p.Skin.StartsWith("[POKEMON|S]") && !p.Skin.StartsWith("[Pokémon|S]"))
		//		{
		//			if (!string.IsNullOrEmpty(GameJoltID) && !CheckForOnlineSprite)
		//			{
		//				CheckForOnlineSprite = true;
		//				this.SetTexture(p.Skin);
		//			}
		//		}
		//
		//		if (this.TextureID != p.Skin)
		//			this.SetTexture(p.Skin);
		//		this.ChangeTexture();
		//
		//		this.GameJoltID = p.GameJoltId;
		//		this.faceRotation = p.Facing;
		//		this.FaceDirection = p.Facing;
		//		this.moving = p.Moving;
		//		this.LevelFile = p.LevelFile;
		//		this.BusyType = p.BusyType.ToString();
		//		this.Visible = false;
		//
		//		if (Game.Level.LevelFile.ToLower() == p.LevelFile.ToLower())
		//			this.Visible = true;
		//		else if (LevelLoader.LoadedOffsetMapNames.Contains(p.LevelFile))
		//		{
		//			Offset = LevelLoader.LoadedOffsetMapOffsets(LevelLoader.LoadedOffsetMapNames.IndexOf(p.LevelFile));
		//			this.Position.x += Offset.x;
		//			this.Position.y += Offset.y;
		//			this.Position.z += Offset.z;
		//			this.Visible = true;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Game.DebugLog("NetworkPlayer.vb: Error while assigning player data over network: " + ex.Message);
		//	}
		//}

		public override void ClickFunction()
		{
			object[] Data = new object[5];
			Data[0] = this.NetworkID;
			Data[1] = this.GameJoltID;
			Data[2] = this.Name;
			Data[3] = this.Texture;
		}

		public static void ScreenRegionChanged()
		{
			if (/*Core.CurrentScreen != null &&*/ Game.Level != null)
			{
				foreach (NetworkPlayer netPlayer in Game.Level.NetworkPlayers)
					netPlayer.LastName = "";
			}
		}

		private static Dictionary<string, Texture2D> SpriteTextStorage = new Dictionary<string, Texture2D>();

		//private static Texture2D SpriteFontTextToTexture(SpriteFont font, string text)
		//{
		//	if (text.Length > 0)
		//	{
		//		if (SpriteTextStorage.ContainsKey(text))
		//			return SpriteTextStorage[text];
		//		else
		//		{
		//			Vector2 size = font.MeasureString(text);
		//			RenderTarget2D renderTarget = new RenderTarget2D(Core.GraphicsDevice, System.Convert.ToInt32(size.x), System.Convert.ToInt32(size.y * 3));
		//			Core.GraphicsDevice.SetRenderTarget(renderTarget);
		//
		//			Core.GraphicsDevice.Clear(new UnityEngine.Color(0,0,0,0));//ToDo: Transparent Color, check alpha
		//
		//			Core.SpriteBatch.Begin();
		//			Canvas.DrawRectangle(new Vector4(0, 0, System.Convert.ToInt32(size.x), System.Convert.ToInt32(size.y)), new UnityEngine.Color(0, 0, 0, 150));
		//			Core.SpriteBatch.DrawString(font, text, Vector2.Zero, UnityEngine.Color.white);
		//			Core.SpriteBatch.End();
		//
		//			Core.GraphicsDevice.SetRenderTarget(null);// TODO Change to default(_) if this is not a reference type 
		//			SpriteTextStorage.Add(text, renderTarget);
		//
		//			return renderTarget;
		//		}
		//	}
		//	return null;// TODO Change to default(_) if this is not a reference type
		//}
	}
}