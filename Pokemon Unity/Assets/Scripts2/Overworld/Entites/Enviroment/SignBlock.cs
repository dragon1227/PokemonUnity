﻿namespace PokemonUnity.Overworld.Entity.Environment
{
public class SignBlock : Entity
{
    // Action value:  0=normal text in additional value
    // 1=script path in additional value
    // 2=direct script input in additional value
    // 3=normal text in additional value, block not resized

    public override void Initialize()
    {
        base.Initialize();

        this.Scale = new Vector3(0.7);

        if (ActionValue < 3)
            this.Position.y -= 0.15F;

        this.CreatedWorld = false;
    }

    public override void ClickFunction()
    {
        bool canRead = false;

        switch (GameVariables.Camera.GetPlayerFacingDirection())
        {
            case 1:
            case 3:
                {
                    if (this.Rotation.y == MathHelper.Pi * 1.5F | this.Rotation.y == MathHelper.Pi * 0.5F)
                        canRead = true;
                    break;
                }
            case 0:
            case 2:
                {
                    if (this.Rotation.y == MathHelper.Pi | this.Rotation.y == MathHelper.TwoPi | this.Rotation.y == 0)
                        canRead = true;
                    break;
                }
        }

        if (canRead == true)
        {
            OverworldScreen oScreen = (OverworldScreen)Core.CurrentScreen;
            if (oScreen.ActionScript.IsReady == true)
            {
                SoundManager.PlaySound("select");
                switch (this.ActionValue)
                {
                    case 0:
                    case 3:
                        {
                            oScreen.ActionScript.StartScript(this.AdditionalValue, 1);
                            break;
                        }
                    case 1:
                        {
                            oScreen.ActionScript.StartScript(this.AdditionalValue, 0);
                            break;
                        }
                    case 2:
                        {
                            oScreen.ActionScript.StartScript(this.AdditionalValue.Replace("<br>", System.Environment.NewLine), 2);
                            break;
                        }
                    default:
                        {
                            oScreen.ActionScript.StartScript(this.AdditionalValue, 1);
                            break;
                        }
                }
            }
        }
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, true);
    }
}
}