﻿using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Combat
{
// ===============================================================================
// This script modifies the battle system to implement battle rules.
// ===============================================================================

public partial class Battle {
  //unless (@__clauses__aliased) {
  //  alias __clauses__pbDecisionOnDraw pbDecisionOnDraw;
  //  alias __clauses__pbEndOfRoundPhase pbEndOfRoundPhase;
  //  @__clauses__aliased=true;
  //}

  public BattleResults pbDecisionOnDraw() {
    if (@rules.Contains("selfkoclause")) {
      if (this.lastMoveUser<0) {
        // in extreme cases there may be no last move user
        return BattleResults.DRAW; // game is a draw
      } else if (isOpposing(this.lastMoveUser)) {
        return BattleResults.LOST; // loss
      } else {
        return BattleResults.WON; // win
      }
    }
    return _pbDecisionOnDraw();
  }

  public void pbJudgeCheckpoint(Combat.Pokemon attacker,Combat.IMove move=null) {
    if (@rules.Contains("drawclause")) {		                                    // Note: Also includes Life Orb (not implemented)
      if (!(move.IsNotNullOrNone() && move.Effect==Attack.Data.Effects.x15A)) {		// Not a draw if fainting occurred due to Liquid Ooze
        if (pbAllFainted(@party1) && pbAllFainted(@party2)) {
          @decision=isOpposing(@attacker.Index) ? BattleResults.WON : BattleResults.LOST;
        }
      }
    } else if (@rules.Contains("modifiedselfdestructclause") && move.IsNotNullOrNone() && 
       move.Effect==Attack.Data.Effects.x008) { // Selfdestruct
      if (pbAllFainted(@party1) && pbAllFainted(@party2)) {
        @decision=isOpposing(@attacker.Index) ? BattleResults.WON : BattleResults.LOST;
      }
    }
  }

  public virtual void pbEndOfRoundPhase() {
    _pbEndOfRoundPhase();
    if (@rules.Contains("suddendeath") && @decision==BattleResults.ABORTED) {
      if (pbPokemonCount(@party1)>pbPokemonCount(@party2)) {
        @decision=BattleResults.LOST; // loss
      } else if (pbPokemonCount(@party1)<pbPokemonCount(@party2)) {
        @decision=BattleResults.WON; // win
      }
    }
  }
}



public partial class Pokemon {
  //unless (@__clauses__aliased) {
  //  alias __clauses__pbCanSleep? pbCanSleep?;
  //  alias __clauses__pbCanSleepYawn? pbCanSleepYawn?;
  //  alias __clauses__pbCanFreeze? pbCanFreeze?;
  //  alias __clauses__pbUseMove pbUseMove;
  //  @__clauses__aliased=true;
  //}

  public bool pbHasStatusPokemon (Status status) {
    int count=0;
    Monster.Pokemon[] party=@battle.pbParty(this.Index);
    for (int i = 0; i < party.Length; i++) {
      if (party[i].IsNotNullOrNone() && !party[i].isEgg &&
         party[i].Status==status) {
        count+=1;
      }
    }
    return (count>0);
  }

  public bool pbCanSleepYawn() {
    if ((@battle.rules.Contains("sleepclause") || @battle.rules.Contains("modifiedsleepclause")) && 
       pbHasStatusPokemon(Status.SLEEP)) {
      return false;
    }
    return _pbCanSleepYawn();
  }

  public bool pbCanFreeze (Pokemon attacker,bool showMessages,Move move=null) {
    if (@battle.rules.Contains("freezeclause") && pbHasStatusPokemon(Status.FROZEN)) {
      return false;
    }
    return _pbCanFreeze(attacker,showMessages,move);
  }

  public bool pbCanSleep (Pokemon attacker,bool showMessages,Combat.IMove move=null,bool ignorestatus=false) {
    bool selfsleep=(attacker.IsNotNullOrNone() && attacker.Index==this.Index);
    if (((@battle.rules.Contains("modifiedsleepclause")) || (!selfsleep && @battle.rules.Contains("sleepclause"))) && 
       pbHasStatusPokemon(Status.SLEEP)) {
      if (showMessages) {
        @battle.pbDisplay(Game._INTL("But {1} couldn't sleep!",this.ToString(true)));
      }
      return false;
    }
    return _pbCanSleep(attacker,showMessages,move,ignorestatus);
  }
}



public partial class PokeBattle_Move_022 { // Double Team
  public override bool pbMoveFailed(Pokemon attacker,Pokemon opponent) {
    if (@battle.rules.Contains("evasionclause")) return true;
    return false;
  }
}

public partial class PokeBattle_Move_034 { // Minimize
  public override bool pbMoveFailed(Pokemon attacker,Pokemon opponent) {
    if (@battle.rules.Contains("evasionclause")) return true;
    return false;
  }
}

public partial class PokeBattle_Move_067 { // Skill Swap
  public override bool pbMoveFailed(Pokemon attacker,Pokemon opponent) {
    if (@battle.rules.Contains("skillswapclause")) return true;
    return false;
  }
}

public partial class PokeBattle_Move_06A { // Sonicboom
  public override bool pbMoveFailed(Pokemon attacker,Pokemon opponent) {
    if (@battle.rules.Contains("sonicboomclause")) return true;
    return false;
  }
}

public partial class PokeBattle_Move_06B { // Dragon Rage
  public override bool pbMoveFailed(Pokemon attacker,Pokemon opponent) {
    if (@battle.rules.Contains("sonicboomclause")) return true;
    return false;
  }
}

public partial class PokeBattle_Move_070 { // OHKO moves
  public override bool pbMoveFailed(Pokemon attacker,Pokemon opponent) {
    if (@battle.rules.Contains("ohkoclause")) return true;
    return false;
  }
}

public partial class PokeBattle_Move_0E0 { // Selfdestruct
  //unless (@__clauses__aliased) {
  //  alias __clauses__pbOnStartUse pbOnStartUse;
  //  @__clauses__aliased=true;
  //}

  public override bool pbOnStartUse(Pokemon attacker) {
    if (@battle.rules.Contains("selfkoclause")) {
      // Check whether no unfainted Pokemon remain in either party
      int count=attacker.NonActivePokemonCount;
      count+=attacker.pbOppositeOpposing.NonActivePokemonCount;
      if (count==0) {
        @battle.pbDisplay("But it failed!");
        return false;
      }
    }
    if (@battle.rules.Contains("selfdestructclause")) {
      // Check whether no unfainted Pokemon remain in either party
      int count=attacker.NonActivePokemonCount;
      count+=attacker.pbOppositeOpposing.NonActivePokemonCount;
      if (count==0) {
        @battle.pbDisplay(Game._INTL("{1}'s team was disqualified!",attacker.ToString()));
        @battle.decision=@battle.isOpposing(attacker.Index) ? BattleResults.WON : BattleResults.LOST;
        return false;
      }
    }
    return _pbOnStartUse(attacker);
  }
}

public partial class PokeBattle_Move_0E5 { // Perish Song
  public override bool pbMoveFailed(Pokemon attacker,Pokemon opponent) {
    if (@battle.rules.Contains("perishsongclause") && attacker.NonActivePokemonCount==0) {
      return true;
    }
    return false;
  }
}

public partial class PokeBattle_Move_0E7 { // Destiny Bond
  public override bool pbMoveFailed(Pokemon attacker,Pokemon opponent) {
    if (@battle.rules.Contains("perishsongclause") && attacker.NonActivePokemonCount==0) {
      return true;
    }
    return false;
  }
}
}