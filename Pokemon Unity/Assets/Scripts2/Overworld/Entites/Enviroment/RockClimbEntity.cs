﻿using PokemonUnity.Pokemon;
using System;
using System.Linq;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class RockClimbEntity : Entity
{
    private ScriptBlock TempScriptEntity = null/* TODO Change to default(_) if this is not a reference type */;
    private bool TempClicked = false; // If true, walk up.

    public override void ClickFunction()
    {
        if (Badge.CanUseHMMove(Badge.HMMoves.RockClimb) == true | GameController.IS_DEBUG_ACTIVE == true | Core.Player.SandBoxMode == true)
        {
            TempClicked = true;
            if (GetRockClimbPokemon() == null)
                Screen.TextBox.Show("A Pokémon could~climb this rock...", this, true, true);
            else
                Screen.TextBox.Show("A Pokémon could~climb this rock.*Do you want to~use Rock Climb?%Yes|No%", this, true, true);
        }
    }

    public override void WalkOntoFunction()
    {
        if (Badge.CanUseHMMove(Badge.HMMoves.RockClimb) == true | GameController.IS_DEBUG_ACTIVE == true | Core.Player.SandBoxMode == true)
        {
            TempClicked = false;
            if (GetRockClimbPokemon() == null)
                Screen.TextBox.Show("A Pokémon could~climb this rock...", this, true, true);
            else
                Screen.TextBox.Show("A Pokémon could~climb this rock.*Do you want to~use Rock Climb?%Yes|No%", this, true, true);
            SoundManager.PlaySound("select");
        }
        else
            Screen.TextBox.Show("A path is engraved~into this rock...", this, true, true);
    }

    public override void ResultFunction(int Result)
    {
        if (Result == 0)
        {
            if (this.TempClicked == true)
                this.WalkUp();
            else
                this.WalkDown();
        }
    }

    private Pokemon GetRockClimbPokemon()
    {
        foreach (Pokemon teamPokemon in Core.Player.Pokemons)
        {
            if (teamPokemon.isEgg == false)
            {
                foreach (BattleSystem.Attack a in teamPokemon.Attacks)
                {
                    if (a.Name.ToLower() == "rock climb")
                        return teamPokemon;
                }
            }
        }

        // No rock climb in team:
        if (GameController.IS_DEBUG_ACTIVE == true | Core.Player.SandBoxMode == true)
        {
            if (Core.Player.Pokemons.Count > 0)
                return Core.Player.Pokemons(0);
            else
            {
                Pokemon p = Pokemon.GetPokemonByID(10);
                p.Generate(10, true);
                return p;
            }
        }
        else
            return null/* TODO Change to default(_) if this is not a reference type */;
    }

    private void WalkUp()
    {
        int facing = System.Convert.ToInt32(this.Rotation.Y / (double)MathHelper.PiOver2);
        facing -= 2;
        if (facing < 0)
            facing += 4;

        Screen.Camera.PlannedMovement = Vector3.Zero;

        if (Screen.Camera.GetPlayerFacingDirection() == facing & Screen.Camera.IsMoving == false)
        {
            int Steps = 0;

            Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
            checkPosition.Y = checkPosition.Y.ToInteger();

            bool foundSteps = true;
            while (foundSteps == true)
            {
                Entity e = GetEntity(Screen.Level.Entities, checkPosition, true, new System.Type[]
                {
                    typeof(RockClimbEntity),
                    typeof(ScriptBlock),
                    typeof(WarpBlock)
                });
                if (e != null)
                {
                    if (e.EntityID.ToLower() == "rockclimbentity")
                    {
                        Steps += 1;
                        checkPosition.X += Screen.Camera.GetMoveDirection().X;
                        checkPosition.Z += Screen.Camera.GetMoveDirection().Z;
                        checkPosition.Y += 1;
                    }
                    else
                    {
                        if (e.EntityID == "ScriptBlock")
                            TempScriptEntity = (ScriptBlock)e;
                        else if (e.EntityID == "WarpBlock")
                            (WarpBlock)e.WalkAgainstFunction();
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }

            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

            string tempSkin = Core.Player.Skin;

            Pokemon RockClimbPokemon = GetRockClimbPokemon();

            Screen.Level.OwnPlayer.Texture = RockClimbPokemon.GetOverworldTexture();
            Screen.Level.OwnPlayer.ChangeTexture();

            string s = "version=2" + Environment.NewLine + "@pokemon.cry(" + RockClimbPokemon.Number + ")" + Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().X + ",1," + Screen.Camera.GetMoveDirection().Z + ")" + Environment.NewLine + "@sound.play(destroy)" + Environment.NewLine + "@player.move(" + Steps + ")" + Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().X + ",0," + Screen.Camera.GetMoveDirection().Z + ")" + Environment.NewLine + "@pokemon.hide" + Environment.NewLine + "@player.move(1)" + Environment.NewLine + "@pokemon.hide" + Environment.NewLine + "@player.wearskin(" + tempSkin + ")" + Environment.NewLine;

            if (this.TempScriptEntity != null)
            {
                s += GetScriptStartLine(this.TempScriptEntity) + Environment.NewLine;
                this.TempScriptEntity = null;
            }

            s += ":end";

            // Reset the player's transparency:
            Screen.Level.OwnPlayer.Opacity = 1.0F;

            (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2, false);
        }

        facing = System.Convert.ToInt32(this.Rotation.Y / (double)MathHelper.PiOver2);
        if (facing < 0)
            facing += 4;
    }

    private void WalkDown()
    {
        int facing = System.Convert.ToInt32(this.Rotation.Y / (double)MathHelper.PiOver2);

        Screen.Camera.PlannedMovement = Vector3.Zero;

        if (Screen.Camera.GetPlayerFacingDirection() == facing)
        {
            int Steps = 0;

            Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
            checkPosition.Y = checkPosition.Y.ToInteger() - 1;

            bool foundSteps = true;
            while (foundSteps == true)
            {
                Entity e = GetEntity(Screen.Level.Entities, checkPosition, true, new System.Type[]
                {
                    typeof(RockClimbEntity),
                    typeof(ScriptBlock),
                    typeof(WarpBlock)
                });
                if (e != null)
                {
                    if (e.EntityID == "RockClimbEntity")
                    {
                        Steps += 1;
                        checkPosition.X += Screen.Camera.GetMoveDirection().X;
                        checkPosition.Z += Screen.Camera.GetMoveDirection().Z;
                        checkPosition.Y -= 1;
                    }
                    else
                    {
                        if (e.EntityID == "ScriptBlock")
                            this.TempScriptEntity = (ScriptBlock)e;
                        else if (e.EntityID == "WarpBlock")
                            (WarpBlock)e.WalkAgainstFunction();
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }

            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

            string tempSkin = Core.Player.Skin;

            Pokemon RockClimbPokemon = GetRockClimbPokemon();

            Screen.Level.OwnPlayer.Texture = RockClimbPokemon.GetOverworldTexture();
            Screen.Level.OwnPlayer.ChangeTexture();

            string s = "version=2" + Environment.NewLine + "@pokemon.cry(" + RockClimbPokemon.Number + ")" + Environment.NewLine + "@player.move(1)" + Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().X + ",-1," + Screen.Camera.GetMoveDirection().Z + ")" + Environment.NewLine + "@sound.play(destroy)" + Environment.NewLine + "@player.move(" + Steps + ")" + Environment.NewLine + "@pokemon.hide" + Environment.NewLine + "@player.wearskin(" + tempSkin + ")" + Environment.NewLine;

            if (this.TempScriptEntity != null)
            {
                s += GetScriptStartLine(this.TempScriptEntity) + Environment.NewLine;
                this.TempScriptEntity = null;
            }

            s += ":end";

            // Reset the player's transparency:
            Screen.Level.OwnPlayer.Opacity = 1.0F;

            (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2, false);
        }
    }

    private string GetScriptStartLine(ScriptBlock ScriptEntity)
    {
        if (!ScriptEntity == null)
        {
            if (ScriptEntity.CorrectRotation() == true)
            {
                switch (ScriptEntity.GetActivationID())
                {
                    case 0:
                        {
                            return "@script.start(" + ScriptEntity.ScriptID + ")";
                        }
                    case 1:
                        {
                            return "@script.text(" + ScriptEntity.ScriptID + ")";
                        }
                    case 2:
                        {
                            return "@script.run(" + ScriptEntity.ScriptID + ")";
                        }
                }
            }
        }

        return "";
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}
}