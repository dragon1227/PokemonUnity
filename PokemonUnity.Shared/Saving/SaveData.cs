﻿using System;
using System.Collections.Generic;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;

namespace PokemonUnity.Saving
{

    [System.Serializable]
    public class SaveData
    {
        #region ImportantInfo
        public string BuildVersion { get; private set; } //SaveManager.GetBuildVersion();
        public string SaveName { get; private set; }
		public int ActiveScene { get; private set; }
		public DateTime TimeCreated { get; private set; }
		#endregion

		#region User Settings 
		//GameSettings is different from UserSettings, and should be loaded first before save state is even reached...
		//public int Language { get; private set; }
		//public byte WindowBorder { get; private set; }
		//public byte DialogBorder { get; private set; }
		//public byte TextSpeed { get; private set; }
		//public bool FullScreen { get; private set; }
		//public float mVol { get; private set; }
		//public float sVol { get; private set; }
		#endregion

		#region Player
		public string PlayerName { get; private set; }		
        //public int playerID { get { return (TrainerID + SecretID) * 65536; } }
        public int TrainerID { get; private set; }
        public int SecretID { get; private set; }
        public bool IsMale { get; private set; }
        //public Dictionary<GymBadges, System.DateTime?> GymBadges;
        #endregion

        #region PlayerInfo
        public bool?[] Pokedex { get; private set; }
        public byte[,] Pokedex2 { get; private set; }
		/// <summary>
		/// Duration of time accumulated on save file
		/// </summary>
		public System.TimeSpan PlayerTime { get; private set; }
		public int PlayerMoney { get; private set; }
		public int PlayerCoins { get; private set; }

		//Player
		public SeriV3 PlayerPosition { get; private set; }
		public float PlayerDirection { get; private set; }
		//Follower
		/// <summary>
		/// </summary>
		/// i would really just make this an offset of player position, i.e. 1-back, 1-left. 
		/// don't think it's necessary to store full on x,y,z position of something that's always xy-distance away from player
		public SeriV3 FollowerPosition { get; private set; }
		/// <summary>
		/// </summary>
		/// Don't need to save follower direction... should just be the same direction player is facing...
		public int FollowerDirection { get; private set; }
		/// <summary>
		/// Which pokemon from player party is out in overworld with player
		/// </summary>
		public int FollowerPokemon { get; private set; }
		#endregion

		#region Pokemon Center
		/// <summary>
		/// When the Player blacks out they need to return to the last visited Pokemon Center
		/// </summary>
		public int pCenterScene { get; private set; }
		//private SeriV3 pCenterPosition;
		//private int pCenterDirection;

		//private SeriV3 pCenterFposition;
		//private int pCenterFdirection;
		#endregion

		#region SeriClasses
		public SeriPokemon[] PlayerParty { get; private set; }
		public SeriPC PC { get; private set; }
		public List<Items> PlayerBag { get; private set; }
		public Dictionary<GymBadges,DateTime?> GymsChallenged { get; private set; }
		public List<SaveEvent> EventList { get; private set; }
		#endregion

		//public void AddPokemonCenter(int scene, SeriV3 position, int direction, SeriV3 fPosition, int fDirection)
		//{
		//    pCenterScene = scene;
		//
		//    pCenterPosition = position;
		//    pCenterDirection = direction;
		//
		//    pCenterFposition = fPosition;
		//    pCenterFdirection = fDirection;
		//}

		public SaveData (Player player, int? money = null, int? coin = null, byte[,] pokedex = null, 
			TimeSpan? time = null, SeriV3 position = null, float? direction = null, int? scene = null, 
			int? pokecenter = null, Dictionary<GymBadges, DateTime?> gym = null, List<Items> bag = null, 
			SeriPC pc = null, List<SaveEvent> eventList = null) 
				: this(name: player.Name, money: money, coin: coin, trainer: player.Trainer.TrainerID,
					  secret: player.Trainer.SecretID, gender: player.Trainer.Gender, pokedex: pokedex,
					  time: time, position: position, direction: direction, scene: scene, pokecenter: pokecenter,
					  gym: gym, bag: bag, party: player.Trainer.Party.Serialize(), pc: pc, eventList: eventList)
		{
			// Just made this one for fun... might be useful in future, than doing things the long way, like below
		}

		public SaveData (string name = null, int? money = null, int? coin = null, int? trainer = null, int? secret = null, 
			bool? gender = null, byte[,] pokedex = null, TimeSpan? time = null, SeriV3 position = null, float? direction = null, 
			int? scene = null, int? pokecenter = null, Dictionary<GymBadges, DateTime?> gym = null, List<Items> bag = null, 
			SeriPokemon[] party = null, SeriPC pc = null, List<SaveEvent> eventList = null)
        {
			//SaveName = saveName;
			BuildVersion = SaveManager.BuildVersion;//.GetBuildVersion();
			TimeCreated = DateTime.UtcNow;

			PlayerName			= name			?? Game.Player.Name;
			PlayerMoney			= money			?? Game.Player.Money;
			PlayerCoins			= coin			?? Game.Player.Coins;
			GymsChallenged		= gym			?? Game.Player.GymsBeatTime;
			PlayerBag			= bag			?? Game.Bag_Items;//Player.Bag; //playerBag;
			TrainerID			= trainer		?? Game.Player.Trainer.TrainerID;
			SecretID			= secret		?? Game.Player.Trainer.SecretID;
			IsMale				= gender		?? Game.Player.isMale;
			Pokedex2			= pokedex		?? Game.Player.Pokedex;
			PlayerTime			= time			?? Game.Player.PlayTime;
			PlayerPosition		= position		?? Game.Player.playerPosition;
			PlayerDirection		= direction		?? Game.Player.playerDirection;
			//FollowerPokemon	= Game.Player.followerPokemon;
			//FollowerPosition	= Game.Player.followerPosition;
			//FollowerDirection	= Game.Player.followerDirection;
			ActiveScene			= scene			?? Game.Player.mapName;//.activeScene;
			pCenterScene		= pokecenter	?? Game.Player.respawnScene;//pkmnCenter;
			PlayerParty			= party			?? Game.Player.Trainer.Party.Serialize();
			//	new SeriPokemon[Game.Player.Trainer.Party.Length];
			//if(party != null)
			//	for (int i = 0; i < Game.Player.Trainer.Party.Length; i++)
			//	{
			//		PlayerParty[i]	= Game.Player.Trainer.Party[i];
			//	}
				
			//ToDo: Store user's Active PC
			PC = pc ?? new SeriPC(Game.PC_Poke, Game.PC_boxNames, Game.PC_boxTexture, Game.PC_Items);
			EventList			= eventList; //Game.EventList;
        }

		//public SaveData
        //    (
		//		string saveName,
		//
		//		Languages language,
		//		byte windowBorder,
		//		byte dialogBorder,
		//		byte textSpeed,
		//		float mvol,
		//		float svol,
		//
		//		string playerName,
		//		int trainerID,
		//		int secretID,
		//		bool isMale,
		//
		//		bool?[] pokedex,
		//		TimeSpan PlayTime,
		//
		//		SeriV3 playerPosition,
		//		int playerDirection,
		//		SeriV3 followerPosition,
		//		int followerDirection,
		//
		//		int activeScene,
		//
		//		Pokemon.Pokemon[] playerParty,
		//		SeriPC pc,
		//		List<Items> playerBag,
		//
		//		List<SaveEvent> eventList
        //    )
        //{
        //    SaveName = saveName;
		//
        //    //Language = (int)language;
        //    //WindowBorder = windowBorder;
        //    //DialogBorder = dialogBorder;
        //    //TextSpeed = textSpeed;
        //    //mVol = mvol;
        //    //sVol = svol;
		//
        //    Name = playerName;
        //    TrainerID = trainerID;
        //    SecretID = secretID;
        //    IsMale = isMale;
		//
        //    Pokedex = pokedex;
        //    PlayerTime = PlayTime;
		//
        //    PlayerPosition = playerPosition;
        //    PlayerDirection = playerDirection;
        //    FollowerPosition = followerPosition;
        //    FollowerDirection = followerDirection;
		//
        //    ActiveScene = activeScene;
		//
        //    PlayerParty = new SeriPokemon[playerParty.Length];
        //    for (int i = 0; i < playerParty.Length; i++)
        //    {
        //        PlayerParty[i] = playerParty[i];
        //    }
        //    PC = pc;
        //    PlayerBag = playerBag;
		//
        //    EventList = eventList;
        //}
    }
}