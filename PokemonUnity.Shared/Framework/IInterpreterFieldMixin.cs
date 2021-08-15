﻿using PokemonUnity.Monster;

namespace PokemonUnity
{
	/// <summary>
	/// Logic that's used by Game while player is in `Overworld_Field`
	/// </summary>
	public interface IInterpreterFieldMixin
	{
		object[] pbParams { get; }

		object getVariable(params int[] arg);
		Pokemon pbGetPokemon(int id);
		bool pbHeadbutt();
		bool pbPushThisBoulder();
		void pbPushThisEvent();
		void pbSetEventTime(params int[] arg);
		/// <summary>
		/// Used when player encounters a trainer, and battle logic get triggered
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns>Plays TrainerType Audio Theme and returns true</returns>
		bool pbTrainerIntro(TrainerTypes symbol);
		void pbTrainerEnd();
		void setTempSwitchOff(string c);
		void setTempSwitchOn(string c);
		void setVariable(params int[] arg);
		bool tsOff(string c);
		bool tsOn(string c);
	}
}