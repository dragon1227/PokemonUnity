﻿using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// Global metadata not specific to a map.  This class holds field state data that
	/// span multiple maps.
	/// </summary>
	public interface IGlobalMetadata
	{
		bool bicycle { get; set; }
		bool surfing { get; set; }
		bool diving { get; set; }
		bool sliding { get; set; }
		bool fishing { get; set; }
		bool runtoggle { get; set; }
		/// <summary>
		/// </summary>
		/// Should not stack (encourage users to deplete excessive money); 
		/// reset count based on repel used.
		///ToDo: Missing Variables for RepelType, Swarm
		int repel { get; set; }
		bool flashUsed { get; set; }
		float bridge { get; set; }
		bool runningShoes { get; set; }
		bool snagMachine { get; set; }
		bool seenStorageCreator { get; set; }
		DateTime startTime { get; set; }
		/// <summary>
		/// Has player beaten the game already and viewed credits from start to end
		/// </summary>
		/// New Game Plus
		bool creditsPlayed { get; set; }
		int playerID { get; set; }
		int coins { get; set; }
		int sootsack { get; set; }
		IList<IMail> mailbox { get; set; }
		IPCItemStorage pcItemStorage	{ get; set; }
		int stepcount { get; set; }
		int happinessSteps { get; set; }
		int? pokerusTime { get; set; }
		IDayCare daycare { get; set; }
		bool daycareEgg { get; set; } //ToDo: int?...
		int daycareEggSteps { get; set; }
		bool[] pokedexUnlocked { get; set; } // Array storing which Dexes are unlocked
		IList<int> pokedexViable { get; set; } // All Dexes of non-zero length and unlocked
		int pokedexDex { get; set; } // Dex currently looking at (-1 is National Dex)
		int[] pokedexIndex { get; set; } // Last species viewed per Dex
		int pokedexMode { get; set; } // Search mode
		int? healingSpot { get; set; }
		float[] escapePoint { get; set; }
		int pokecenterMapId { get; set; }
		float pokecenterX { get; set; }
		float pokecenterY { get; set; }
		int pokecenterDirection { get; set; }
		ITilePosition pokecenter			{ get; set; }
		IList<int> visitedMaps { get; set; }
		IList<int> mapTrail { get; set; }
		IAudioBGM nextBattleBGM { get; set; }
		IAudioME nextBattleME { get; set; }
		IAudioObject nextBattleBack { get; set; }
		ISafariState safariState { get; set; }
		IBugContestState bugContestState			{ get; set; }
		ITrainer partner { get; set; }
		int? challenge { get; set; }
		IBattleRecordData lastbattle { get; set; }
		IList<IPhoneContact> phoneNumbers { get; set; }
		/// <summary>
		/// The time between successive received phone calls is set to a random amount of time between 20 and 40 minutes, 
		/// and is counted down except when messages are being displayed or the player is being forced to move by a move route. 
		/// When this time hits 0, a call from a trainer will be generated.
		/// </summary>
		int phoneTime { get; set; }
		bool safesave { get; set; }
		IDictionary<KeyValuePair<int, int>, int> eventvars { get; set; }


		//IGlobalMetadata initialize();
		Pokemons[] roamPokemonCaught { get; }
	}
}