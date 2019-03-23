using System;
using System.Linq;
using System.Runtime.CompilerServices;
using PokemonUnity.Pokemon;

namespace PokemonUnity.Overworld.Entity.Misc
{
public class OverworldPokemon : Entity
{
	public OverworldPokemon(float X, float Y, float Z) : base(X, Y, Z, "OverworldPokemon", P3D.TextureManager.DefaultTexture,
		new int[]{
			0,
			0
		}, false, 0, new Vector3(1.0F), BaseModel.BillModel, 0, "", new Vector3(1))
    {
        PokemonReference = null/* TODO Change to default(_) if this is not a reference type */;
        this.Respawn();
        if (GameVariables.playerTrainer.LastPokemonPosition == new Vector3(999, 999, 999))
        {
            this.Position = new Vector3(Screen.Camera.Position.x, Screen.Camera.Position.y, Screen.Camera.Position.z);
            this.Visible = false;
            this.warped = false;
        }
        else
			this.Position = GameVariables.playerTrainer.LastPokemonPosition;

        this.Position = new Vector3(System.Convert.ToInt32(this.Position.x), this.GetYPosition(), System.Convert.ToInt32(this.Position.z));
        this.NeedsUpdate = true;
        this.CreateWorldEveryFrame = true;

        this.DropUpdateUnlessDrawn = false;

	}

	public int PokemonID = 0;
	private Pokemon _PokemonReference;

	public Pokemon PokemonReference
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		get
		{
			return _PokemonReference;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_PokemonReference != null)
			{
				_PokemonReference.TexturesCleared -= PokemonReference_TexturesCleared;
			}

			_PokemonReference = value;
			if (_PokemonReference != null)
			{
				_PokemonReference.TexturesCleared += PokemonReference_TexturesCleared;
			}
		}
	}

	public Texture2D Texture;
	private Rectangle lastRectangle = new Rectangle(0, 0, 0, 0);
	public int faceRotation = 0;
	public float MoveSpeed = 0.04F;
	public bool warped = true;

	private int AnimationX = 1;
	private float AnimationDelayLenght = 2.2F;
	private float AnimationDelay = AnimationDelayLenght;

	private void ChangeTexture()
	{
		if (this.Texture == null)
			this.Texture = PokemonReference.GetOverworldTexture();

		Rectangle r = new Rectangle(0, 0, 0, 0);
		int cameraRotation = Screen.Camera.GetFacingDirection();
		int spriteIndex = this.faceRotation - cameraRotation;

		spriteIndex = this.faceRotation - cameraRotation;
		if (spriteIndex < 0)
			spriteIndex += 4;

		int width = System.Convert.ToInt32(this.Texture.Width / (double)3);

		int x = 0;
		x = AnimationX * width;

		int height = System.Convert.ToInt32(this.Texture.Height / (double)4);

		int y = height * spriteIndex;

		r = new Rectangle(x, y, width, height);

		if (r != lastRectangle)
		{
			lastRectangle = r;

			Texture2D t = TextureManager.GetTexture(this.Texture, r, 1);
			Textures(0) = t;
		}
	}

	public override void Update()
	{
		if (GameVariables.playerTrainer.GetWalkPokemon() != null)
		{
			bool differentAdditionalData = false;
			bool differentShinyState = false;
			if (this.PokemonReference != null)
			{
				differentAdditionalData = (this.PokemonReference.AdditionalData != GameVariables.playerTrainer.GetWalkPokemon().AdditionalData);
				differentShinyState = (this.PokemonReference.IsShiny != GameVariables.playerTrainer.GetWalkPokemon().IsShiny);
			}

			if (this.PokemonID != GameVariables.playerTrainer.GetWalkPokemon().Number | differentAdditionalData == true | differentShinyState == true)
			{
				this.Texture = null;
				this.PokemonID = GameVariables.playerTrainer.GetWalkPokemon().Number;
				this.PokemonReference = GameVariables.playerTrainer.GetWalkPokemon();
			}

			this.ChangeTexture();

			this.AnimationDelay -= 0.1F;
			if (AnimationDelay <= 0.0F)
			{
				AnimationDelay = AnimationDelayLenght;
				AnimationX += 1;
				if (AnimationX > 2)
					AnimationX = 1;
			}

			ChangePosition();
		}
	}

	protected override float CalculateCameraDistance(Vector3 CPosition)
	{
		return base.CalculateCameraDistance(CPosition) - 0.2F;
	}

	public override void UpdateEntity()
	{
		if (this.Rotation.y != Screen.Camera.Yaw)
			this.Rotation.y = Screen.Camera.Yaw;
		this.Scale = new Vector3(1.0F);
		this.Position.y = this.GetYPosition();

		base.UpdateEntity();
	}

	public override void Render()
	{
		if (this.IsVisible() == true)
		{
			var state = GraphicsDevice.DepthStencilState;
			GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
			Draw(this.Model, this.Textures(0), false);
			GraphicsDevice.DepthStencilState = state;
		}
	}

	/// <summary>
	/// If the OverworldPokémon should be rendered.
	/// </summary>
	public bool IsVisible()
	{
		if (System.Convert.ToBoolean(GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")) == true)
		{
			if (Screen.Level.ShowOverworldPokemon == true)
			{
				if (IsCorrectScreen() == true)
				{
					if (GameVariables.playerTrainer.GetWalkPokemon() != null)
					{
						if (Screen.Level.Surfing == false & Screen.Level.Riding == false)
						{
							if (this.PokemonID > 0)
							{
								if (this.Textures != null)
									return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public void ChangeRotation()
	{
		this.Position = new Vector3(System.Convert.ToInt32(this.Position.x), System.Convert.ToSingle(this.Position.y) + 0.001F, System.Convert.ToInt32(this.Position.z));
		if (Screen.Camera.Position.x == System.Convert.ToInt32(this.Position.x) | Screen.Camera.Position.z == System.Convert.ToInt32(this.Position.z))
		{
			if (this.Position.x < Screen.Camera.Position.x)
				this.faceRotation = 3;
			else if (this.Position.x > Screen.Camera.Position.x)
				this.faceRotation = 1;
			if (this.Position.z < Screen.Camera.Position.z)
				this.faceRotation = 2;
			else if (this.Position.z > Screen.Camera.Position.z)
				this.faceRotation = 0;
		}
	}

	private void ChangePosition()
	{
		if (Screen.Camera.IsMoving() == true)
		{
			if (System.Convert.ToInt32(this.Position.x) != System.Convert.ToInt32(Screen.Camera.Position.x) | System.Convert.ToInt32(this.Position.z) != System.Convert.ToInt32(Screen.Camera.Position.z))
			{
				this.Position += GetMove();
				this.AnimationDelayLenght = 1.1F;
			}
		}
		else
			this.AnimationDelayLenght = 2.2F;
	}

	private Vector3 GetMove()
	{
		Vector3 moveVector;
		switch (this.faceRotation)
		{
			case 0:
				{
					moveVector = new Vector3(0, 0, -1) * MoveSpeed;
					break;
				}
			case 1:
				{
					moveVector = new Vector3(-1, 0, 0) * MoveSpeed;
					break;
				}
			case 2:
				{
					moveVector = new Vector3(0, 0, 1) * MoveSpeed;
					break;
				}
			case 3:
				{
					moveVector = new Vector3(1, 0, 0) * MoveSpeed;
					break;
				}
		}
		return moveVector;
	}

	private bool IsCorrectScreen()
	{
		Screen.Identifications[] screens = new[] { Screen.Identifications.BattleCatchScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleGrowStatsScreen, Screen.Identifications.BattleScreen, Screen.Identifications.CreditsScreen, Screen.Identifications.BattleAnimationScreen, Screen.Identifications.ViewModelScreen, Screen.Identifications.HallofFameScreen };
		if (screens.Contains(Core.CurrentScreen.Identification) == true)
			return false;
		else if (Core.CurrentScreen.Identification == Screen.Identifications.TransitionScreen)
		{
			if (screens.Contains((TransitionScreen)Core.CurrentScreen.OldScreen.Identification) == true | screens.Contains((TransitionScreen)Core.CurrentScreen.NewScreen.Identification) == true)
				return false;
		}
		else
		{
			Screen c = Core.CurrentScreen;
			while (c.PreScreen != null)
				c = c.PreScreen;
			if (screens.Contains(c.Identification) == true)
				return false;
		}
		return true;
	}

	public void MakeVisible()
	{
		if (warped == true)
			warped = false;
		else if (this.Visible == false)
		{
			this.Visible = true;
			this.Respawn();
		}
	}

	public void Respawn()
	{
		Vector3 newPosition = new Vector3(0, -2, 0);
		if (Screen.Camera.Name == "Overworld")
			newPosition = (OverworldCamera)Screen.Camera.LastStepPosition;
		if (newPosition != new Vector3(0, -2, 0))
			this.Position = newPosition;
		else
			switch (Screen.Camera.GetPlayerFacingDirection())
			{
				case 0:
					{
						this.Position = new Vector3(Screen.Camera.Position.x, this.GetYPosition(), Screen.Camera.Position.z + 1);
						break;
					}
				case 1:
					{
						this.Position = new Vector3(Screen.Camera.Position.x + 1, this.GetYPosition(), Screen.Camera.Position.z);
						break;
					}
				case 2:
					{
						this.Position = new Vector3(Screen.Camera.Position.x, this.GetYPosition(), Screen.Camera.Position.z - 1);
						break;
					}
				case 3:
					{
						this.Position = new Vector3(Screen.Camera.Position.x - 1, this.GetYPosition(), Screen.Camera.Position.z);
						break;
					}
			}

		ChangeRotation();
	}

	public override void ClickFunction()
	{
		if (System.Convert.ToBoolean(GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")) == true)
		{
			if (this.Visible == true & GameVariables.playerTrainer.GetWalkPokemon() != null & Screen.Level.Surfing == false & Screen.Level.Riding == false & Screen.Level.ShowOverworldPokemon == true)
			{
				Pokemon p = GameVariables.playerTrainer.GetWalkPokemon();
				string scriptString = PokemonInteractions.GetScriptString(p, this.Position, this.faceRotation);

				if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
				{
					if ((OverworldScreen)Core.CurrentScreen.ActionScript.IsReady == true)
						(OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(scriptString, 2);
				}
			}
		}
	}

	public void ApplyShaders()
	{
		this.Shaders.Clear();
		foreach (Shader Shader in Screen.Level.Shaders)
			Shader.ApplyShader(this);
	}

	private void PokemonReference_TexturesCleared(object sender, EventArgs e)
	{
		this.Texture = null;
		this.ForceTextureChange();
	}

	private float GetYPosition()
	{
		return System.Convert.ToSingle(Screen.Camera.Position.y);
	}

	public void ForceTextureChange()
	{
		this.lastRectangle = new Rectangle(0, 0, 0, 0);
		this.ChangeTexture();
	}
}
}