﻿using PokemonUnity.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class AnimatedBlock : Entity
	{
		private static Dictionary<string, Texture2D> BlockTexturesTemp = new Dictionary<string, Texture2D>();


		private List<string> AnimationNames;

		private List<Animation> Animations;
		private List<Vector4> currentRectangle;

		private List<int> X, Y, width, height, rows, columns, animationSpeed, startRow, startColumn;

		private int AnimCount = 0;


		public AnimatedBlock()
		{
			X = new List<int>();
			Y = new List<int>();
			width = new List<int>();
			height = new List<int>();
			rows = new List<int>();
			columns = new List<int>();
			animationSpeed = new List<int>();
			startRow = new List<int>();
			startColumn = new List<int>();

			AnimationNames = new List<string>();
			currentRectangle = new List<Vector4>();
			Animations = new List<Animation>();
		}

		public new void Initialize(List<List<int>> AnimationData)
		{
			base.Initialize();
			for (var i = 0; i <= AnimationData.Count - 1; i++)
			{
				X.Add(AnimationData[i][0]);
				Y.Add(AnimationData[i][1]);
				width.Add(AnimationData[i][2]);
				height.Add(AnimationData[i][3]);
				rows.Add(AnimationData[i][4]);
				columns.Add(AnimationData[i][5]);
				animationSpeed.Add(AnimationData[i][6]);
				startRow.Add(AnimationData[i][7]);
				startColumn.Add(AnimationData[i][8]);

				AnimationNames.Add("");
				currentRectangle.Add(new Vector4(0, 0, 0, 0));

				//Animations.Add(new Animation(TextureManager.GetTexture(@"Textures\Routes"), rows[i], columns[i], 16, 16, animationSpeed[i], startRow[i], startColumn[i]));

				AnimCount += 1;
			}

			CreateBlockTextureTemp();
		}

		public static void ClearAnimationResources()
		{
			BlockTexturesTemp.Clear();
		}

		private void CreateBlockTextureTemp()
		{
			// If Core.GameOptions.GraphicStyle = 1 Then

			for (var n = 0; n <= Animations.Count - 1; n++)
			{
				Vector4 r = new Vector4(X[n], Y[n], width[n], height[n]);
				this.AnimationNames[n] = AdditionalValue + "," + X[n] + "," + Y[n] + "," + height[n] + "," + width[n];
				if (!BlockTexturesTemp.ContainsKey(AnimationNames[n] + "_0"))
				{
					for (var i = 0; i <= this.rows[n] - 1; i++)
					{
						//for (var j = 0; j <= this.columns[n] - 1; j++)
						//	BlockTexturesTemp.Add(AnimationNames[n] + "_" + (j + columns[n] * i).ToString(), TextureManager.GetTexture(AdditionalValue, new Vector4(r.x + r.width * j, r.y + r.height * i, r.width, r.height)));
					}
				}
			}
		}

		public override void ClickFunction()
		{
			this.Surf();
		}

		public override bool WalkAgainstFunction()
		{
			WalkOntoFunction();
			return base.WalkAgainstFunction();
		}

		public override bool WalkIntoFunction()
		{
			WalkOntoFunction();
			return base.WalkIntoFunction();
		}

		public override void WalkOntoFunction()
		{
			if (Game.Level.Surfing)
			{
				bool canSurf = false;

				foreach (Entity Entity in Game.Level.Entities)
				{
					if (Entity.boundingBox.Contains(Game.Camera.GetForwardMovedPosition()))// == ContainmentType.Contains
					{
						if (Entity.ActionValue == 0 && (Entity.EntityID == Entities.AnimatedBlock || Entity.EntityID == Entities.Water))
							canSurf = true;
						else if (Entity.Collision)
						{
							canSurf = false;
							break;
						}
					}
				}

				if (canSurf)
				{
					Game.Camera.Move(1);

					//Game.Level.PokemonEncounter.TryEncounterWildPokemon(this.Position, EncounterTypes.Surfing, "");
				}
			}
		}

		private void Surf()
		{
			if (!Game.Camera.Turning)
			{
				if (!Game.Level.Surfing)
				{
					if (Badge.CanUseHMMove(Badge.HMMoves.Surf) | Game.IS_DEBUG_ACTIVE | Game.Player.SandBoxMode)
					{
						//if (!Screen.ChooseBox.Showing)
						//{
							bool canSurf = false;

							if (this.ActionValue == 0)
							{
								foreach (Entity Entity in Game.Level.Entities)
								{
									if (Entity.boundingBox.Contains(Game.Camera.GetForwardMovedPosition()))// == ContainmentType.Contains
									{
										if (Entity.EntityID == Entities.AnimatedBlock)
										{
											if (Game.Player.SurfPokemon > -1)
												canSurf = true;
										}
										else if (Entity.Collision)
										{
											canSurf = false;
											break;
										}
									}
								}
							}

							if (Game.Level.Riding)
								canSurf = false;

							if (canSurf)
							{
								string message = "Do you want to Surf?%Yes|No%";
								Game.TextBox.Show(message, new Entity[] { this }, true, true);
								SoundManager.PlaySound("select");
							}
						//}
					}
				}
			}
		}


		protected override float CalculateCameraDistance(Vector3 CPosition)
		{
			return base.CalculateCameraDistance(CPosition) - 0.25f;
		}

		public override void UpdateEntity()
		{
			if (Animations != null)
			{
				for (var n = 0; n <= Animations.Count - 1; n++)
				{
					//Animations[n].Update(0.01f);
					//if (currentRectangle[n] != Animations[n].TextureRectangle)
					//{
					//	ChangeTexture(n);
					//
					//	currentRectangle[n] = Animations[n].TextureRectangle;
					//}
				}
			}
			base.UpdateEntity();
		}

		private void ChangeTexture(int n)
		{
			// If Core.GameOptions.GraphicStyle = 1 Then

			if (BlockTexturesTemp.Count == 0)
			{
				ClearAnimationResources();
				CreateBlockTextureTemp();
			}
			//var i = Animations[n].CurrentRow;
			//var j = Animations[n].CurrentColumn;
			//this.Textures(n) = AnimatedBlock.BlockTexturesTemp[AnimationNames[n] + "_" + (j + columns[n] * i)];
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				Game.TextBox.Show(Game.Player.Party[Game.Player.SurfPokemon].Name + " used~Surf!", new Entity[] { this });
				Game.Level.Surfing = true;
				Game.Camera.Move(1);
				//PlayerStatistics.Track("Surf used", 1);

				{
					var withBlock = Game.Level.OwnPlayer;
					//Game.Player.TempSurfSkin = withBlock.SkinName;

					int pokemonNumber = (int)Game.Player.Party[Game.Player.SurfPokemon].Species;
					//string SkinName = "[POKEMON|N]" + pokemonNumber + PokemonForms.GetOverworldAddition(Game.Player.Party[Game.Player.SurfPokemon]);
					//if (Game.Player.Party[Game.Player.SurfPokemon].IsShiny)
					//	SkinName = "[POKEMON|S]" + pokemonNumber + PokemonForms.GetOverworldAddition(Game.Player.Party[Game.Player.SurfPokemon]);

					//withBlock.SetTexture(SkinName, false);

					withBlock.UpdateEntity();

					SoundManager.PlayPokemonCry(pokemonNumber);

					//if (!Game.Level.IsRadioOn || !GameJolt.PokegearScreen.StationCanPlay(Game.Level.SelectedRadioStation))
					//	MusicManager.Play("surf", true);
				}
			}
		}

		public override void Render()
		{
			//bool setRasterizerState = this.Model.ID != 0;
			//
			//this.Draw(this.Model, Textures, setRasterizerState);
		}
	}
}