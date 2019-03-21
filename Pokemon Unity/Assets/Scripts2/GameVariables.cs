﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using Localization;
//using Localization.Dictionaries;
//using Localization.Dictionaries.Xml;
//using System.IO;
using System.Xml;
using System.Collections;
//using System.Collections.Immutable;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Item;
using PokemonUnity.Saving;

/// <summary>
/// Variables that are stored when game is saved, and other temp values used for gameplay.
/// This class should be called once, when the game boots-up.
/// During boot-up, game will check directory for save files and load data.
/// GameVariables class will overwrite all the other class default values when player triggers a load state.
/// </summary>
/// This class should be static...
public partial class GameVariables : UnityUtilityIntegration//: UnityEngine.MonoBehaviour//, UnityEngine.EventSystems.
{
	#region Player and Overworld Data
	//ToDo: Missing Variables for RepelSteps, RepelType, Swarm
	public static Player playerTrainer { get; set; }
	//public GameVariables.TrainerPC PC { get { return new GameVariables.TrainerPC(playerTrainer); } }
	#endregion

	#region Private Records of Player Storage Data
	//ToDo: Berry Field Data (0x18 per tree, 36 trees)
	//ToDo: Honey Tree, smearing honey on tree will spawn pokemon in 6hrs, for 24hrs (21 trees)
	//Honey tree timer is done in minutes (1440, spawns at 1080), only goes down while playing...
	//ToDo: Missing Variable for DayCare, maybe `Pokemon[,]` for multipe locations?
	//Daycare Data
	//(Slot 1) Occupied Flag 
	//(Slot 1) Steps Taken Since Depositing 
	//(Slot 1) Box EK6 1 
	//(Slot 2) Occupied Flag 
	//(Slot 2) Steps Taken Since Depositing2 
	//(Slot 2) Box EK6 2 
	//Flag (egg available) 
	//RNG Seed
	//ToDo: a bool variable for PC background (if texture is unlocked) `bool[]`
	public static Pokemon[,] PC_Poke { get; set; }
	public static string[] PC_boxNames { get; set; }
	public static int[] PC_boxTexture { get; set; }
	public static List<Item> PC_Items { get; set; }
	public static List<Items> Bag_Items { get; set; }
	#endregion

	#region Constructor
	static GameVariables()
	{
		UserLanguage  = Settings.Languages.English;
		PC_Poke = new Pokemon[Settings.STORAGEBOXES, 30];
		PC_boxNames = new string[Settings.STORAGEBOXES];
		PC_boxTexture = new int[Settings.STORAGEBOXES];
		for (int i = 0; i < Settings.STORAGEBOXES; i++)
		{
			//Initialize the PC storage so pokemons arent null (in value)
			for (int j = 0; j < PC_Poke.GetLength(1); j++)
			{
				//All default values must be `NONE`
				PC_Poke[i, j] = new Pokemon(Pokemons.NONE);//pokemons[i, j];
			}
			//ToDo: Using string from translator here
			PC_boxNames[i] = string.Format("Box {0}", (i + 1).ToString());
			//ToDo: Make sure there's enough texture in library for array size
			PC_boxTexture[i] = i; 
		}

		PC_Items = new List<Item>();
		Bag_Items = new List<Items>();
	}
	#endregion

	#region Unity Canvas UI
	//public static Translator.Languages UserLanguage = Translator.Languages.English;
	public static Settings.Languages UserLanguage { get; private set; }// = Settings.Languages.English;
    //public GlobalVariables.Language playerLanguage = GlobalVariables.Language.English;
	/// <summary>
	/// Frame Style for all System Prompts and Text Displays
	/// </summary>
	public static byte WindowSkin { get; private set; }
	/// <summary>
	/// Frame Style for all player and non-playable characters Speech bubbles
	/// </summary>
	public static byte DialogSkin { get; private set; }
	public static byte mvol { get; private set; }
	/// <summary>
	/// Music Volume
	/// </summary>
	public static float mVol {
		set
		{
			if (value > 20f) mvol = (byte)20;
			if (value < 0f) mvol = (byte)0;
			if (value < 20f && value > 0f) mvol = (byte)value;
		}
		get { return (mvol / 20f) * (mvol / 20f); }
	}
	public static byte svol { get; private set; }
    /// <summary>
    /// SFX (Sound Effects) Volume 
    /// </summary>
    public static float sVol {
		set
		{
			if (value > 20f) svol = (byte)20;
			if (value < 0f)  svol = (byte)0;
			if (value < 20f && value > 0f) svol = (byte)value;
		}
		get { return (svol / 20f) * (svol / 20f); }
	}
    public static bool battleScene = true;
    public static bool fullscreen;
    public static byte textSpeed = 2;

	#region Global and map metadata
	//ToDo: Each time map changes, new values are loaded/replaced below
	public class Global
	{
		/// <summary>
		/// Location you return to when you respawn
		/// </summary>
		public string MetadataHome;
		/// <summary>
		/// 
		/// </summary>
		/// String below should point to Audio/Sound files
		public string MetadataWildBattleBGM;
		public string MetadataTrainerBattleBGM;
		public string MetadataWildVictoryME;
		public string MetadataTrainerVictoryME;
		public string MetadataSurfBGM;
		public string MetadataBicycleBGM;
		/* TrainerClass
		Trainer MetadataPlayerA          ;
		Trainer MetadataPlayerB          ;
		Trainer MetadataPlayerC          ;
		Trainer MetadataPlayerD          ;
		Trainer MetadataPlayerE          ;
		Trainer MetadataPlayerF          ;
		Trainer MetadataPlayerG          ;
		Trainer MetadataPlayerH;*/
	}

	public class NonGlobalTypes : Global
	{
		bool MetadataOutdoor;
		bool MetadataShowArea;
		bool MetadataBicycle;
		bool MetadataBicycleAlways;
		/// <summary>
		/// 
		/// </summary>
		/// "uuu"
		int[,] MetadataHealingSpot; 
		/// <summary>
		/// 
		/// </summary>
		/// return WeatherType
		bool MetadataWeather;
		/// <summary>
		/// 
		/// </summary>
		/// "uuu"
		int[] MetadataMapPosition; 
		int MetadataDiveMap;
		bool MetadataDarkMap;
		bool MetadataSafariMap;
		bool MetadataSnapEdges;
		bool MetadataDungeon;
		/// <summary>
		/// 
		/// </summary>
		/// String below should point to Audio/Sound files
		public string MetadataBattleBack;
		//public string MetadataMapWildBattleBGM;
		//public string MetadataMapTrainerBattleBGM;
		//public string MetadataMapWildVictoryME;
		//public string MetadataMapTrainerVictoryME;
		int[,] MetadataMapSize;
	}
	#endregion
	#endregion

	#region Unity Scene Manager
	public static CanvasUIHandler CanvasManager { get; private set; }
	public static DialogHandler DialogScene { get; private set; }
	public static StartupSceneHandler StartScene { get; private set; }
	public static BattlePokemonHandler BattleScene { get; private set; }
	//public static ItemHandler ItemScene { get; private set; }
	//public static SummaryHandler SummaryScene { get; private set; }
	//public static SettingsHandler SettingsScene { get; private set; }
	#region Scene Manager Methods
	public static void SetCanvasManager(CanvasUIHandler canvas) { CanvasManager = canvas; }
	public static void SetStartScene(StartupSceneHandler start) { StartScene = start; }
	#endregion
	#endregion

	#region Save/Load Data
	private static byte slotIndex { get; set; }
	//private int fileIndex { get; set; }
	/// <summary>
	/// Bool used to tell Start-Up screen whether or not to display "Continue" option
	/// </summary>
    public static bool SaveFileFound { get; private set; }
    //public System.DateTimeOffset fileCreationDate { get; set; }
	//public System.DateTimeOffset? lastSave { get; set; }
	//public System.DateTimeOffset startTime { get; set; }

	/// <summary
	/// Preload before any of the other scenes are loaded...
	/// </summary>
	///ToDo: Temp Save Profiles to be used and displayed on Start-Up screen
	public static void Load()
	{
		//Load player settings (language, full screen, vol...)
		//Load continue/new game/"choose load slots" options...
		//Load temp profile data (Party, pokedex seen/caught, hours played...)
	}
	/// <summary>
	/// Loads saved game data from memory slot
	/// </summary>
	/// <param name="i">Array int from binary stream</param>
	public static void Load(byte i)
    {
		slotIndex = i > 0 && i < 3 ? i : slotIndex;
        //GameVariables.SaveLoad.Load();
		PokemonUnity.Saving.SaveData data = PokemonUnity.Saving.SaveManager.GetSave(i);
		GameVariables.playerTrainer = new Player();

		switch (data.BuildVersion)
		{
			case "0.0.1":
			//Next one gets added to list, and default is copied above, and modified below...
			default:
				GameVariables.playerTrainer.LoadTrainer(data); 
				GameVariables.PC_Poke = data.PC.GetPokemonsFromSeri();
				GameVariables.PC_boxNames = data.PC.BoxNames;
				GameVariables.PC_boxTexture = data.PC.BoxTextures;
				GameVariables.PC_Items = new List<Item>(data.PC.GetItemsFromSeri());
				GameVariables.Bag_Items = data.PlayerBag;
				break;
		}
	}
    public static void Save()
    {
		//using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(System.IO.File.Open(FILE_NAME,)))
		//GameVariables.SaveLoad.Save();

		//PokemonUnity.Saving.SaveManager.Overwrite(new PokemonUnity.Saving.SaveData(), slotIndex);
		SaveData[] save = SaveManager.GetSaves();
		if (save == null)
			save = new SaveData[] { null, null, null };
		save[slotIndex] = new SaveData();
		SaveManager.CreateSaveFileAndSerialize(save);
    }
	/// <summary>
	/// For Debug Use Purposes;
	/// Used Unit Tester...
	/// </summary>
    public static void Save(SaveData test, int slot)
    {
		SaveData[] save = SaveManager.GetSaves();
		if (save == null)
			save = new SaveData[] { null, null, null };
		save[slot] = test;
		SaveManager.CreateSaveFileAndSerialize(save);
    }

    /*private class SaveLoad {
        #region Variables
        //int DatabaseEntryStringWidth = 100;
        System.IO.FileStream fs;
        //BinaryWriter w; //= new BinaryWriter(fs);
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //GameVariables data = new GameVariables();
        GameVariables[] gamesaves = new GameVariables[3];
        Translator.Languages userpreflanguage = Translator.Languages.English;
#if DEBUG
		private const string FILE_NAME = @"Test.pkud"; //TestProject\bin\Debug
		//private const string FILE_NAME = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Test.data"; //TestProject\bin\Debug
		//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
		//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
		private const string FILE_NAME = UnityEngine.Application.persistentDataPath + "/Test.pkud";
		//string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
#endif
        #endregion

        public static void Save(System.IO.BinaryWriter writer) { }
        
        /// <summary>
        /// When initially boots up, this will be all the application data stored
        /// on user's PersonalComputer (PC). If first time running game, naturally
        /// stored data would not exist, and the game will produce one by default.
        /// </summary>
        void OldMe()
        {
            //UnityEngine.Debug.Log("Checking to see if BinaryText exist...");
            if (System.IO.File.Exists(FILE_NAME))
            {
                //UnityEngine.Debug.Log(FILE_NAME + " already exists!");

                //UnityEngine.Debug.Log("Loading Old Info from BinaryText...");
                // Create the reader for data.
                //fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                fs = System.IO.File.Open(FILE_NAME, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //UnityEngine.Debug.Log("Information Loaded.");
                //UnityEngine.Debug.Log("Deserializing Information...");
                //data = (GameVariables)bf.Deserialize(fs);
                SaveLoad d = (SaveLoad)bf.Deserialize(fs);
                //UnityEngine.Debug.Log("Rewriting TextField/Variables...");

                //username = data.username;
                //rememberme = data.rememberme;
                Settings.UserLanguage = d.userpreflanguage;

                //An Array[3] of GameVariables representing GameSaves
                gamesaves = d.gamesaves;//(GameVariables[])bf.Deserialize(fs);
                SaveFileFound = true;

                //UnityEngine.Debug.Log("Information Loaded and Updated.");
                //UnityEngine.Debug.Log("Closing BinaryText...");
                //UnityEngine.Debug.Log("Closing FileStream...");
                fs.Close();
                //UnityEngine.Debug.Log("BinaryText Closed.");
                return;
            }
            else
            {
                SaveFileFound = false;
                System.IO.File.Open(FILE_NAME, System.IO.FileMode.Create).Close();
            }
		}

		//public static SaveDataOld[] savedGames = new SaveDataOld[]
		//{
		//	null, null, null
		//};

		public static void Save()
		{
			//if (SaveDataOld.currentSave != null)
			//{
			//	if (SaveDataOld.currentSave.getFileIndex() >= 0 && SaveDataOld.currentSave.getFileIndex() < savedGames.Length)
			//	{
			//		SaveDataOld.currentSave.playerTime += SaveDataOld.currentSave.startTime.Subtract(System.DateTime.UtcNow);
			//		SaveDataOld.currentSave.lastSave = System.DateTime.UtcNow;// new System.DateTime(,System.DateTimeKind.Utc);
			//		savedGames[SaveDataOld.currentSave.getFileIndex()] = SaveDataOld.currentSave;
			//		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			//		System.IO.FileStream file = System.IO.File.Create(FILE_NAME);//Application.persistentDataPath + "/playerData.pkud"
			//		bf.Serialize(file, SaveLoad.savedGames);
			//		file.Close();
			//	}
			//}
		}

		public static bool Load()
		{
			//Debug.Log(Application.persistentDataPath);
			if (System.IO.File.Exists(FILE_NAME))
			{
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				System.IO.FileStream file = System.IO.File.Open(FILE_NAME, System.IO.FileMode.Open);
				//SaveLoad.savedGames = (SaveDataOld[])bf.Deserialize(file);

				//playerTrainer = new Trainer().LoadTrainer(trainerSaveData: trainerData);

				file.Close();
				return true;
			}
			return false;
		}

		//public static int getSavedGamesCount()
		//{
		//	int count = 0;
		//	for (int i = 0; i < savedGames.Length; i++)
		//	{
		//		if (savedGames[i] != null)
		//		{
		//			count += 1;
		//		}
		//	}
		//	return count;
		//}

		//public static void resetSaveGame(int index)
		//{
		//	savedGames[index] = null;
		//
		//	if (index < 2)
		//	{
		//		for (int i = index; i < 2; i++)
		//		{
		//			SaveLoad.savedGames[i] = SaveLoad.savedGames[i + 1];
		//			SaveLoad.savedGames[i + 1] = null;
		//		}
		//	}
		//
		//	bool sGN1 = savedGames[0] == null;
		//	bool sGN2 = savedGames[1] == null;
		//	bool sGN3 = savedGames[2] == null;
		//
		//	//Debug.Log(sGN1.ToString() + ", " + sGN2.ToString() + ", " + sGN3.ToString());
		//
		//	System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		//	System.IO.FileStream file = System.IO.File.Create(FILE_NAME);//Application.persistentDataPath + "/playerData.pkud"
		//	bf.Serialize(file, SaveLoad.savedGames);
		//	file.Close();
		//}
	}*/
	#endregion

	#region Active Battle and Misc Battle related Data
	/// <summary>
	/// Active Pokemon Battle the Player is currently involved in.
	/// Matches being spectated would be pass thru a non-static method
	/// </summary>
	/// ToDo: On Set, trigger UnityEngine EventHandler,
	/// Switch scenes, load rules, and animate pokemons
	//public static Battle Battle { get; set; }
	public static PokemonUnity.Battle.Battle battle
	{
		get
		{
			return _battle;
		} 
		set
		{
#if !DEBUG
			UnityEngine.Debug.Log(value);
#endif
		}
	}
	private static PokemonUnity.Battle.Battle _battle { get; set; }
	#endregion

	#region Audio 
	public static int? nextBattleBGM { get; set; }
	public static int? nextBattleME { get; set; }
	public static int? nextBattleBack { get; set; }
	#endregion
}

/// <summary>
/// <see cref="GlobalVariables"/> is a <see cref="UnityEngine.GameObject"/>
/// that persist throughout the game and inbetween scene levels.
/// <see cref="Settings"/> is not an <see cref="object"/>
/// but a series of const variables that will be used as rules or 
/// placeholders for the game mechanics.
/// </summary>
public static class Settings //: Settings<Translations.Languages>
{
    #region Constant Values and Game Rules
    public static Translator.Languages UserLanguage = Translator.Languages.English;
    public static bool TextLTR { get; private set; }
    //public XmlFileLocalizationDictionaryProvider TranslationText;// = new XmlFileLocalizationDictionaryProvider(Server.MapPath("~/App_Data/"));
    //information.Initialize("PokemonUnity");

    /// <summary>
    /// 
    /// </summary>
    public const float framesPerSecond = 30f;

	public const sbyte pokemonGeneration = (sbyte)Generation.All;

	public static int pokemonGenerationCount { 
		get { 
			int MaxPoke = 0;
            int Gen1 = 151;
			int Gen2 = 251;
			int Gen3 = 386;
			int Gen4 = 493;
			int Gen5 = 649;
			int Gen6 = 721;
			int Gen7 = 807;

            #region Generation
            switch (pokemonGeneration)
            {
                case (1):
                    MaxPoke = Gen1;
                    break;
                case (2):
                    MaxPoke = Gen2;
                    break;
                case (3):
                    MaxPoke = Gen3;
                    break;
                case (4):
                    MaxPoke = Gen4;
                    break;
                case (5):
                    MaxPoke = Gen5;
                    break;
                case (6):
                    MaxPoke = Gen6;
                    break;
                case (-1):
                case (0):
                case (7):
				default:
                    MaxPoke = Gen7;
                    break;
            }
            #endregion
			return MaxPoke; } 
	}
	#endregion

	#region Variables
	/// <summary>
	/// Constantly revolving random, that won't repeat the same seed number twice, 
	/// until it cycles thru all possible seed values
	/// </summary>
	public static Random Rand { get { return new Random(Seed()); } }
	/// <summary>
	/// Constantly revolving random, that uses the same seed number that was previously used
	/// </summary>
	public static Random RandWithSetSeed { get { return new Random(Seed(true)); } }
	private static System.UInt16? seed;// = 0x0000; //{ get; set; }
	public static UInt16 Seed(bool useFixedSeed = false)
    {
		//lock (Rand)
		//{
			if (!seed.HasValue) {
				//seed = (UInt16)new Random().Next(0, UInt16.MaxValue);
				seed = (UInt16)new Random(DateTime.Now.Millisecond).Next(0, UInt16.MaxValue); 
				seed ^= (UInt16)System.DateTime.Now.Ticks;
				seed &= UInt16.MaxValue;
			}
			if (!useFixedSeed) { 
				seed = (UInt16)(seed * 0x41C64E6D + 0x6073);
			} 
			return seed.Value;
		//}
    }

	/// <summary>
	/// Converts the string of a Pokemon Type to a color's hex value
	/// </summary>
	/// <param name="PokemonType">string of pokemon type or name of a color</param>
	/// <returns>return a Unity.Color</returns>
	/// <example>StringToColor(Electric)</example>
	/// <example>StringToColor(Yellow)</example>
	/// <remarks>might need to make a new enum in PokemonData, type = x.Color...</remarks>
	/// ToDo: Convert to Unity's Color? 
	public static string TypeToColorHEX (Types PokemonType) {
		//private System.Collections.Generic.Dictionary<string, Color> StringToColorDic = new System.Collections.Generic.Dictionary<string, Color>() {//Dictionary<PokemonData.Type, Color>
		//http://www.epidemicjohto.com/t882-type-colors-hex-colors
		switch (PokemonType)
		{
			case Types.NORMAL:
				//Normal Type:		
				return "A8A77A";
			case Types.FIGHTING:
				//Fighting Type:	
				return "C22E28";
			case Types.FLYING:
				//Flying Type:		
				return "A98FF3";
			case Types.POISON:
				//Poison Type:		
				return "A33EA1";
			case Types.GROUND:
				//Ground Type:		
				return "E2BF65";
			case Types.ROCK:
				//Rock Type:		
				return "B6A136";
			case Types.BUG:
				//Bug Type:		
				return "A6B91A";
			case Types.GHOST:
				//Ghost Type:		
				return "735797";
			case Types.STEEL:
				//Steel Type:		
				return "B7B7CE";
			case Types.FIRE:
				//Fire Type:		
				return "EE8130";
			case Types.WATER:
				//Water Type:		
				return "6390F0";
			case Types.GRASS:
				//Grass Type:		
				return "7AC74C";
			case Types.ELECTRIC:
				//Electric Type:	
				return "F7D02C";
			case Types.PSYCHIC:
				//Psychic Type:	
				return "F95587";
			case Types.ICE:
				//Ice Type:		
				return "96D9D6";
			case Types.DRAGON:
				//Dragon Type:		
				return "6F35FC";
			case Types.DARK:
				//Dark Type:		
				return "705746";
			case Types.FAIRY:
				//Fairy Type:		
				return "D685AD";
			case Types.NONE:
			case Types.UNKNOWN:
			case Types.SHADOW:
			default:
				//Black?
				return string.Empty;
		}
	}
	/*// <summary>
	/// Converts the Pokemon Type to a Color for use in Unity. 
	/// </summary>
	/// <param name="PokemonType">pokemon type</param>
	/// <returns>return a Unity.Color</returns>
	/// <example>StringToColor(Electric)</example>
	public Color TypeToColor(Types PokemonType) {
		//http://www.serebiiforums.com/showthread.php?289595-Pokemon-type-color-associations
		switch (PokemonType)
		{
			case Types.NORMAL:
				//Normal Type:		
				//Normal -white
				return "A8A77A";
			case Types.FIGHTING:
				//Fighting Type:	
				//Fighting - dark red
				return "C22E28";
			case Types.FLYING:
				//Flying Type:		
				//Flying - light blue
				return "A98FF3";
			case Types.POISON:
				//Poison Type:		
				//Poison -purple
				return "A33EA1";
			case Types.GROUND:
				//Ground Type:		
				//Ground - brown
				return "E2BF65";
			case Types.ROCK:
				//Rock Type:		
				//Rock - gray
				return "B6A136";
			case Types.BUG:
				//Bug Type:		
				//Bug - yellow green
				return "A6B91A";
			case Types.GHOST:
				//Ghost Type:		
				//Ghost - light purple
				return "735797";
			case Types.STEEL:
				//Steel Type:		
				//Steel - dark gray
				return "B7B7CE";
			case Types.FIRE:
				//Fire Type:		
				//Fire - red
				return "EE8130";
			case Types.WATER:
				//Water Type:		
				//Water -blue
				return "6390F0";
			case Types.GRASS:
				//Grass Type:		
				//Grass - green
				return "7AC74C";
			case Types.ELECTRIC:
				//Electric Type:	
				//Electric -yellow
				return "F7D02C";
			case Types.PSYCHIC:
				//Psychic Type:	
				//Psychic - magenta
				return "F95587";
			case Types.ICE:
				//Ice Type:		
				//Ice - cyan
				return "96D9D6";
			case Types.DRAGON:
				//Dragon Type:		
				//Dragon - orange
				return "6F35FC";
			case Types.DARK:
				//Dark Type:		
				//Dark - black
				return "705746";
			case Types.FAIRY:
				//Fairy Type:		
				return "D685AD";
			case Types.NONE:
			case Types.UNKNOWN:
			case Types.SHADOW:
			default:
				//return string.Empty;
				break;
		}
	}*/
	/*/// <summary>
	/// Only an example. Do not use, will  not work.
	/// <para>Could be combined with database values 
	/// and used with ints instead of strings</para>
	/// <para>Convert the pokemon type into a color 
	/// that can be used with Unity's color lighting</para>
	/// </summary>
	/// <param name="color"></param>
	/// <returns></returns>
	public UnityEngine.Color ColorToUnity(Color color) {
		switch (color)
		{
			//case 1:
			//	return StringToColorDic["text"];
			default:
				return StringToColor(color.ToString());
				break;
		}
				
		//{"Black",Color.black },//dark
		//{"", new Color() },//dark blue -> dark, 
		{ "Blue",Color.blue },//water
		{ "Clear",Color.clear },
		{ "Cyan",Color.cyan },
		{ "Gray",Color.gray },//grAy-American
		//{"Grey",Color.grey },//grEy-European
		//{"Grey",Color.grey },//dark grey -> rock,
		{ "Green",Color.green },//grass
		//{"", new Color() },//dark green -> bug,
		{ "Magenta",Color.magenta },//magenta, purple -> poison
		{ "Red",Color.red },//orange, dark red -> fire
		{ "White",Color.white },//normals
		{ "Yellow",Color.yellow },//electric
		{ "Purple", new Color() },//ghost
		{ "Brown", new Color() },//fighting
		{ "Pink", new Color() }//,//fairy
		//{"", new Color() },//pink, lavender -> psychic, 
		//{"", new Color() },//ocre, brown -> ground
		//{"", new Color() },
		//{"", new Color() },
		//{"", new Color() }//fly, drag, steel, psychic, ice, shadow, unknown, bug, ground, poison?
		
	}*/
	#endregion

	#region Custom Game Mode
	//Nuzlocke Challenge => Pokemon Centers cost money, every pokemon must be named, when defeated/fainted pokemon is gone, only allowed to capture first pokemon encountered when entering new map
	/// <summary>
	/// Basically, you use the Dexnav to find pokemon in the area, they appear as shadows in the grass, and you need to sneak up on them
	/// these pokemon can have egg moves, or even their HiddenAbility
	/// </summary>
	/// Apparently you can use the Sneaking feature to helps with this. 
	/// ToDo: OnlyAllowEggMovesWhenUsingDexNav or DexNavAllowsEggMoves
	public static bool CatchPokemonsWithEggMoves { get; private set; }
    #endregion

    #region Enumerators
    /// <summary>
    /// Still need to sort out Language Enums
    /// </summary>
    public enum Languages //: Languages<Translations.Languages>//Translator.Languages//
    {
        /// <summary>
        /// US English
        /// </summary>
        English = 9
    }
    /*public class Languagess : ILanguage<Translations.Languages>
    {
        public enum Translations.Languages { get; set; } 
    }*/
    #endregion

	#region Screen Options
	/// <summary>
	/// The default screen width (at a zoom of 1.0; size is half this at zoom 0.5).
	/// </summary>
	public const int DEFAULTSCREENWIDTH = 512;
	/// <summary>
	/// The default screen height (at a zoom of 1.0).
	/// </summary>
	public const int DEFAULTSCREENHEIGHT = 384;
	/// <summary>
	/// The default screen zoom. (1.0 means each tile is 32x32 pixels, 0.5 means
	/// each tile is 16x16 pixels, 2.0 means each tile is 64x64 pixels.)
	/// </summary>
	public const float DEFAULTSCREENZOOM = 1.0f;
	/// <summary>
	/// Whether full-screen display lets the border graphic go outside the edges of
	/// the screen (true), or forces the border graphic to always be fully shown
	/// </summary>
	public const bool FULLSCREENBORDERCROP = false;
	/// <summary>
	/// The width of each of the left and right sides of the screen border. This is
	/// added on to the screen width above, only if the border is turned on.
	/// </summary>
	public const int BORDERWIDTH = 80;
	/// <summary>
	/// The height of each of the top and bottom sides of the screen border. This is
	/// added on to the screen height above, only if the border is turned on.
	/// </summary>
	public const int BORDERHEIGHT = 80;
	/// <summary>
	/// Map view mode (0=original, 1=custom, 2=perspective).
	/// </summary>
	public const int MAPVIEWMODE = 1;
	#endregion

	#region Pokemon Settings
	/// <summary>
	/// The maximum level Pokémon can reach.
	/// </summary>
	public const byte MAXIMUMLEVEL = 100;
	/// <summary>
	/// The level of newly hatched Pokémon.
	/// </summary>
	public const byte EGGINITIALLEVEL = 1;
	/// <summary>
	/// The odds of a newly generated Pokémon being shiny (out of 65536).
	/// </summary>
	public const int SHINYPOKEMONCHANCE = 8;
	/// <summary>
	/// The odds of a wild Pokémon/bred egg having Pokérus (out of 65536).
	/// </summary>
	public const int POKERUSCHANCE = 3;
	#endregion

	#region OverWorld Rules
	/// <summary>
	/// Whether poisoned Pokémon will lose HP while walking around in the field.
	/// </summary>
	public const bool POISONINFIELD = true;
	/// <summary>
	/// Whether poisoned Pokémon will faint while walking around in the field
	///    (true), or survive the poisoning with 1HP (false).
	/// </summary>
	public const bool POISONFAINTINFIELD = false;
	/// <summary>
	/// Whether fishing automatically hooks the Pokémon (if false, there is a
	/// reaction test first).
	/// </summary>
	public const bool FISHINGAUTOHOOK = false;
	/// <summary>
	/// Whether the player can surface from anywhere while diving (true), or only in
	/// spots where they could dive down from above (false).
	/// </summary>
	public const bool DIVINGSURFACEANYWHERE = false;
	/// <summary>
	/// Whether planted berries grow according to Gen 4 mechanics (true) or Gen 3
	/// mechanics (false).
	/// </summary>
	public const bool NEWBERRYPLANTS = true;
	/// <summary>
	/// Whether TMs can be used infinitely as in Gen 5 (true), or are one-use-only
	/// as in older Gens (false).
	/// </summary>
	public const bool INFINITETMS = true;
	#endregion

	#region Battle Settings
	/// <summary>
	/// Whether a move's physical/special category depends on the move itself as in
	///    newer Gens (true), or on its type as in older Gens (false).
	/// </summary>
	public const bool USEMOVECATEGORY = true;
	/// <summary>
	/// Whether the battle mechanics mimic Gen 6 (true) or Gen 5 (false).
	/// </summary>
	public const bool USENEWBATTLEMECHANICS = false;
	/// <summary>
	/// Whether the Exp gained from beating a Pokémon should be scaled depending on
	///    the gainer's level as in Gen 5 (true), or not as in other Gens (false).
	/// </summary>
	public const bool USESCALEDEXPFORMULA = true;
	/// <summary>
	/// Whether the Exp gained from beating a Pokémon should be divided equally
	///    between each participant (false), or whether each participant should gain
	///    that much Exp. This also applies to Exp gained via the Exp Share (held
	///    item version) being distributed to all Exp Share holders. This is true in
	///    Gen 6 and false otherwise.
	/// </summary>
	public const bool NOSPLITEXP = false;
	/// <summary>
	/// Whether the critical capture mechanic applies (true) or not (false). Note
	///    that it is based on a total of 600+ species (i.e. that many species need
	///    to be caught to provide the greatest critical capture chance of 2.5x),
	///    and there may be fewer species in your game.
	/// </summary>
	public const bool USECRITICALCAPTURE = false;
	/// <summary>
	/// Whether Pokémon gain Exp for capturing a Pokémon (true) or not (false).
	/// </summary>
	public const bool GAINEXPFORCAPTURE = false;
	/// <summary>
	/// An array of items which act as Mega Rings for the player (NPCs don't need a
	///    Mega Ring item, just a Mega Stone).
	/// </summary>
	public static readonly Items[] MEGARINGS = new Items[] { Items.MEGA_RING, Items.MEGA_BRACELET, Items.MEGA_CUFF, Items.MEGA_CHARM };
	#endregion

	#region Badges
	/// <summary>
	/// The minimum number of badges required to boost each stat of a player's
	/// Pokémon by 1.1x, while using moves in battle only.
	/// </summary>
	public const int BADGESBOOSTATTACK = 1;
	/// <summary>
	/// The minimum number of badges required to boost each stat of a player's
	/// Pokémon by 1.1x, while using moves in battle only.
	/// </summary>
	public const int BADGESBOOSTDEFENSE = 5;
	/// <summary>
	/// The minimum number of badges required to boost each stat of a player's
	/// Pokémon by 1.1x, while using moves in battle only.
	/// </summary>
	public const int BADGESBOOSTSPEED = 3;
	/// <summary>
	/// The minimum number of badges required to boost each stat of a player's
	/// Pokémon by 1.1x, while using moves in battle only.
	/// </summary>
	public const int BADGESBOOSTSPATK = 7;
	/// <summary>
	/// The minimum number of badges required to boost each stat of a player's
	/// Pokémon by 1.1x, while using moves in battle only.
	/// </summary>
	public const int BADGESBOOSTSPDEF = 7;
	/// <summary>
	/// Whether the badge restriction on using certain hidden moves is either owning
	/// at least a certain number of badges (true), or owning a particular badge (false).
	/// </summary>
	public const bool HIDDENMOVESCOUNTBADGES = true;
	/// <summary>
	/// Depending on HIDDENMOVESCOUNTBADGES, either the number of badges required to
	/// use each hidden move, or the specific badge number required to use each
	/// move. Remember that badge 0 is the first badge, badge 1 is the second
	/// badge, etc.<para></para>
	/// e.g. To require the second badge, put false and 1.
	/// To require at least 2 badges, put true and 2.
	/// </summary>
	/// ToDo: Consider a mechanic that allows each region to have their own TM/HM #s Region[Generation,Badges] <see cref="PokemonUnity.Item.Items"/>
	/// <summary>
	/// </summary>
	public const int BADGEFORCUT = 1;
	/// <summary>
	/// </summary>
	public const int BADGEFORFLASH = 2;
	/// <summary>
	/// </summary>
	public const int BADGEFORROCKSMASH = 3;
	/// <summary>
	/// </summary>
	public const int BADGEFORSURF = 4;
	/// <summary>
	/// </summary>
	public const int BADGEFORFLY = 5;
	/// <summary>
	/// </summary>
	public const int BADGEFORSTRENGTH = 6;
	/// <summary>
	/// </summary>
	public const int BADGEFORDIVE = 7;
	/// <summary>
	/// </summary>
	public const int BADGEFORWATERFALL = 8;
	#endregion
		
	#region Player Storage
	/// <summary>
	/// The maximum number of slots per pocket (-1 means infinite number). Ignore
	///    the first number (0).
	/// </summary>
	/// The names of each pocket of the Bag. Leave the first entry blank.
	/// "Items"
	/// "Medicine"
	/// "Poké Balls"
	/// "TMs & HMs"
	/// "Berries"
	/// "Mail"
	/// "Battle Items"
	/// "Key Items"
	public static readonly int[] MAXPOCKETSIZE = new int[] { 0, -1, -1, -1, -1, -1, -1, -1, -1 };
	/// <summary>
	/// The maximum number of items each slot in the Bag can hold.
	/// </summary>
	public const int BAGMAXPERSLOT = 999;
	/// <summary>
	/// Whether each pocket in turn auto-sorts itself by item ID number. Ignore the
	///    first entry (the 0).
	/// </summary>
	public static readonly bool[] POCKETAUTOSORT = new bool[] { false/*null*/, false, false, false, true, true, false, false, false };
	/// <summary>
	/// The number of boxes in Pokémon storage.
	/// </summary>
	public const int STORAGEBOXES = 24;
	#endregion
		
	#region Pokedex
	/// <summary>
	/// Whether the Pokédex list shown is the one for the player's current region
	///    (true), or whether a menu pops up for the player to manually choose which
	///    Dex list to view when appropriate (false).
	/// </summary>
	public const bool DEXDEPENDSONLOCATION = false;
	/// <summary>
	/// The names of each Dex list in the game, in order and with National Dex at
	///    the end. This is also the order that PokemonGlobal.pokedexUnlocked is
	///    in, which records which Dexes have been unlocked (first is unlocked by
	///    default).<para></para>
	///    You can define which region a particular Dex list is linked to. This
	///    means the area map shown while viewing that Dex list will ALWAYS be that
	///    of the defined region, rather than whichever region the player is
	///    currently in. To define this, put the Dex name and the region number in
	///    an array, like the Kanto and Johto Dexes are. The National Dex isn't in
	///    an array with a region number, therefore its area map is whichever region
	///    the player is currently in.
	/// </summary>
	public class Pokedex {
		//[_INTL("Kanto Pokédex"),0],
		//[_INTL("Johto Pokédex"),1],
		//_INTL("National Pokédex")

		/// <summary>
		/// Translated from DB
		/// </summary>
		string Name;
		/// <summary>
		/// Generation
		/// </summary>
		/// Equal to or greater than 0 assigns to region id
		int Region = -1;
		int PokedexId;
		/// <summary>
		/// An array of numbers, where each number is that of a Dex list (National Dex
		///    is -1). All Dex lists included here have the species numbers in them
		///    reduced by 1, thus making the first listed species have a species number
		///    of 0 (e.g. Victini in Unova's Dex).
		/// </summary>
		/// ToDo: Pokedex should be assigned pokemonId start and end numbers
		/// Example: Johto is pokemons 1-151, while national is all... 
		int DEXINDEXOFFSETS;//    = []
	}
	/// <summary>
	/// Whether all forms of a given species will be immediately available to view
	///    in the Pokédex so long as that species has been seen at all (true), or
	///    whether each form needs to be seen specifically before that form appears
	///    in the Pokédex (false).
	/// </summary>
	public const bool ALWAYSSHOWALLFORMS = false;
	#endregion

	#region Currency Limit
	/// <summary>
	/// The amount of money the player starts the game with.
	/// </summary>
	public const int INITIALMONEY = 3000;
	/// <summary>
	/// The maximum amount of money the player can have.
	/// </summary>
	/// Do you really NEED 6 digits for money?
	public const int MAXMONEY = 999999;
	/// <summary>
	/// The maximum number of Game Corner coins the player can have.
	/// </summary>
	public const int MAXCOINS = 99999;
	/// <summary>
	/// The maximum length, in characters, that the player's name can be.
	/// </summary>
	public const int PLAYERNAMELIMIT = 10;
	#endregion
		
	#region Roaming Pokemons
	/// <summary>
	/// A list of maps used by roaming Pokémon. Each map has an array of other maps
	///    it can lead to.
	/// </summary>
	public class RoamingAreas
	{
		public int MapId;
		public int[] ConnectedMaps;
		private Dictionary<int, int[]> _areas;
		//ToDo: Make Arraychart below global, but still have instantiable variables
		private static Dictionary<int, int[]> Areas;
		//public static int[] this[int map] { get { return Areas[map]; } }
		//RoamingAreas = {
		//5  => [21,28,31,39,41,44,47,66,69],
		//21 => [5,28,31,39,41,44,47,66,69],
		//28 => [5,21,31,39,41,44,47,66,69],
		//31 => [5,21,28,39,41,44,47,66,69],
		//39 => [5,21,28,31,41,44,47,66,69],
		//41 => [5,21,28,31,39,44,47,66,69],
		//44 => [5,21,28,31,39,41,47,66,69],
		//47 => [5,21,28,31,39,41,44,66,69],
		//66 => [5,21,28,31,39,41,44,47,69],
		//69 => [5,21,28,31,39,41,44,47,66]
		//}
	}
	/// <summary>
	/// A set of arrays each containing the details of a roaming Pokémon. The
	///    information within is as follows:<para></para>
	///    - Species.<para></para>
	///    - Level.<para></para>
	///    - Global Switch; the Pokémon roams while this is ON.<para></para>
	///    - Encounter type (0=any, 1=grass/walking in cave, 2=surfing, 3=fishing,<para></para>
	///         4=surfing/fishing). See bottom of PField_RoamingPokemon for lists.<para></para>
	///    - Name of BGM to play for that encounter (optional).<para></para>
	///    - Roaming areas specifically for this Pokémon (optional).
	/// </summary>
	public class RoamingSpecies
	{
		public Pokemons Species;
		public int Level;
		public int EncounterType;
		public int BGM;
		public RoamingAreas MapId;
		//RoamingSpecies = [
		//[:LATIAS, 30, 53, 0, "Battle roaming"],
		//[:LATIOS, 30, 53, 0, "Battle roaming"],
		//[:KYOGRE, 40, 54, 2, nil, {
		//	2  => [21,31],
		//	21 => [2,31,69],
		//	31 => [2,21,69],
		//	69 => [21,31]
		//	}],
		//[:ENTEI, 40, 55, 1, nil]
		//]
		/// <summary>
		/// A set of arrays each containing the details of a roaming Pokémon.
		/// </summary>
		/// <param name="pkmn">Species.</param>
		/// <param name="level">Level.</param>
		/// <param name="encounter">Encounter type (0=any, 1=grass/walking in cave, 2=surfing, 
		/// 3=fishing, 4=surfing/fishing).</param>
		/// <param name="music">Name of BGM to play for that encounter (optional).</param>
		/// <param name="map">Roaming areas specifically for this Pokémon (optional).</param>
		public RoamingSpecies(Pokemons pkmn, int level, int encounter, int? music = null, RoamingAreas map = null)
		{
			Species = pkmn;
			Level = level;
			EncounterType = encounter;
			if(music.HasValue)BGM = music.Value;
			MapId = map;
		}
	}
	#endregion

	#region PokeRadar
	/// <summary>
	/// A set of arrays each containing details of a wild encounter that can only
	///    occur via using the Poké Radar. The information within is as follows:<para></para>
	///    - Map ID on which this encounter can occur.<para></para>
	///    - Probability that this encounter will occur (as a percentage).<para></para>
	///    - Species.<para></para>
	///    - Minimum possible level.<para></para>
	///    - Maximum possible level (optional).
	/// </summary>
	public class PokeRadars
	{
		public int MapId;
		public int EncounterChance;
		public Pokemons Species;
		/// <summary>
		/// LevelMin is 0 in Array.
		/// LevelMax is 1 in Array.
		/// LevelMax is Optional.
		/// </summary>
		public int[] LevelMinMax;
		/*POKERADAREXCLUSIVES=[
		[5,  20, :STARLY,     12, 15],
		[21, 10, :STANTLER,   14],
		[28, 20, :BUTTERFREE, 15, 18],
		[28, 20, :BEEDRILL,   15, 18]
		]*/
		/// <summary>
		/// A set of arrays each containing details of a wild encounter that can only
		/// occur via using the Poké Radar.
		/// </summary>
		/// <param name="map">Map ID on which this encounter can occur.</param>
		/// <param name="pkmn">Species.</param>
		/// <param name="chance">Probability that this encounter will occur (as a percentage).</param>
		/// <param name="min">Minimum possible level.</param>
		/// <param name="max">Maximum possible level (optional).</param>
		public PokeRadars(int map, Pokemons pkmn, int chance, byte min, byte max)
		{
			MapId = map;
			Species = pkmn;
			EncounterChance = chance;
			LevelMinMax = new int[] { min, max };
		}
		public PokeRadars(int map, Pokemons pkmn, int chance, byte min) : this(map, pkmn, chance, min, min)
		{

		}
	}
	#endregion

	#region Safari
	/// <summary>
	/// The number of steps allowed before a Safari Zone game is over (0=infinite).
	/// </summary>
	public const int SAFARISTEPS = 600;
	/// <summary>
	/// The number of seconds a Bug Catching Contest lasts for (0=infinite).
	/// </summary>
	public const int BUGCONTESTTIME = 1200;
	#endregion

	#region Battling and Encounter
	/// <summary>
	/// The Global Switch that is set to ON when the player whites out.
	/// </summary>
	public const bool STARTING_OVER_SWITCH = true;
	/// <summary>
	/// The Global Switch that is set to ON when the player has seen Pokérus in the
	///    Poké Center, and doesn't need to be told about it again.
	/// </summary>
	public const bool SEEN_POKERUS_SWITCH = false;
	/// <summary>
	/// The Global Switch which, while ON, makes all wild Pokémon created be
	///    shiny.
	/// </summary>
	public const bool SHINY_WILD_POKEMON_SWITCH = false;
	/// <summary>
	/// The Global Switch which, while ON, makes all Pokémon created considered to
	///    be met via a fateful encounter.
	/// </summary>
	public const bool FATEFUL_ENCOUNTER_SWITCH = false;
	/// <summary>
	/// The Global Switch which determines whether the player will lose money if
	///    they lose a battle (they can still gain money from trainers for winning).
	/// </summary>
	public const bool NO_MONEY_LOSS = true;
	/// <summary>
	/// The Global Switch which, while ON, prevents all Pokémon in battle from Mega
	///    Evolving even if they otherwise could.
	/// </summary>
	public const bool NO_MEGA_EVOLUTION = true;
	#endregion

	#region Audio
	public class Audio
	{

	}
	#endregion
}

public static class TransformExtension
{
    public static UnityEngine.Transform ClearChildren (this UnityEngine.Transform transform)
    {
        foreach (UnityEngine.Transform child in transform)
        {
            UnityEngine.GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
}

/// <summary>
/// This class will be inherited by all other classes, 
/// and offer a direct link to Unity's Engine
/// for ease of use utility and integration.
/// </summary>
public class UnityUtilityIntegration
#if !DEBUG //|| UNITY_EDITOR 
	//Not sure if this is something i want inheriting monobehavior...
	: UnityEngine.MonoBehaviour
#endif
{
	#region Debug Functions and Features
	public static bool debugMode { get; set; }
	public static void DebugLog(string text, bool? error = null)
	{
		if(!error.HasValue)
			Debug = text;
		else
		{
			//ToDo: If during production and game logs an ERROR, or maybe a warning too, store to text file, and upload to dev team?
			if(error.Value)
				DebugError = text;
			else
				DebugWarning = text;
		}
	}
	private static string Debug {
		set
		{
#if !DEBUG
			UnityEngine.Debug.Log(value);
#endif
		}
	}
	private static string DebugWarning {
		set
		{
#if !DEBUG
			UnityEngine.Debug.LogWarning(value);
#endif
		}
	}
	private static string DebugError {
		set
		{
#if !DEBUG
			UnityEngine.Debug.LogError(value);
#endif
		}
	}
	#endregion

	#region Unity Canvas UI
	#region Resources
	public static UnityEngine.Sprite[] LoadAllWindowSkinSprites()
	{
		return UnityEngine.Resources.LoadAll<UnityEngine.Sprite>(@"\Sprites\GUI\Frame\WindowSkin");
	}

	public static UnityEngine.Sprite[] LoadAllDialogSkinSprites()
	{
		return UnityEngine.Resources.LoadAll<UnityEngine.Sprite>(@"\Sprites\GUI\Frame\DialogSkin");
	}
	#endregion
	//Game UI
	//public UnityEngine.Texture2D DialogWindowSkin;
	//private UnityEngine.UI.Image DialogWindowSkin;
	/// <summary>
	/// Frame Style for all System Prompts and Text Displays
	/// </summary>
	public static UnityEngine.Sprite WindowSkinSprite { get { return LoadAllWindowSkinSprites()[GameVariables.WindowSkin]; } }
	/// <summary>
	/// Frame Style for all player and non-playable characters Speech bubbles
	/// </summary>
	public static UnityEngine.Sprite DialogSkinSprite { get { return LoadAllDialogSkinSprites()[GameVariables.DialogSkin]; } }
	/// <summary>
	/// In-game UI dialog window to prompt message to user
	/// </summary>
	/// ToDo: Allow game logic to pass npc scripts thru this
	/// ToDo: Option for dialog prompts, i.e. "Yes/No, Continue.."
	/// <param name="text"></param>
	/// <param name="error">Maybe something about interupting coroutine</param>
	public static void Dialog(string text, bool? error = null, params string[] promptOptions)
	{
		//ToDo: Pass values directly to DialogEventHandler
		//ToDo: Make a struct for each non-class (enum, etc) type and add a ToString(bool) override that output unity richtext color tags
		//Consider adding a Queue to dialog text... so messages arent replaced but appended
		if (!error.HasValue)
			Debug = text;
		else
		{
			if (error.Value)
				DebugError = text;
			else
				DebugWarning = text;
		}
	}
	protected static void Display(string text)
	{

	}
	protected static void DisplayBrief(string text)
	{

	}
	protected static void DisplayPause(string text)
	{

	}
	protected static void DisplayConfirm(string text)
	{

	}
	protected static string L(Text text, string textid, params string[] vs)
	{
		return LanguageExtension.Translate(text, textid, vs).Value;
	}
	#endregion
}

namespace PokemonUnity
{
	/*// <summary>
	/// Extension methods for <see cref="MenuItemDefinition"/>.
	/// </summary>
	public static class MenuItemDefinitionExtensions
	{
		/// <summary>
		/// Moves a menu item to top in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		public static void MoveMenuItemToTop(this IList<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(0, menuItem);
		}

		/// <summary>
		/// Moves a menu item to bottom in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		public static void MoveMenuItemToBottom(this IList<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.Count, menuItem);
		}

		/// <summary>
		/// Moves a menu item in the list after another menu item in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		/// <param name="targetMenuItemName">Target menu item (to move before it)</param>
		public static void MoveMenuItemBefore(this IList<MenuItemDefinition> menuItems, string menuItemName, string targetMenuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			var targetMenuItem = GetMenuItem(menuItems, targetMenuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.IndexOf(targetMenuItem), menuItem);
		}

		/// <summary>
		/// Moves a menu item in the list before another menu item in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		/// <param name="targetMenuItemName">Target menu item (to move after it)</param>
		public static void MoveMenuItemAfter(this IList<MenuItemDefinition> menuItems, string menuItemName, string targetMenuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			var targetMenuItem = GetMenuItem(menuItems, targetMenuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.IndexOf(targetMenuItem) + 1, menuItem);
		}

		private static MenuItemDefinition GetMenuItem(IEnumerable<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = menuItems.FirstOrDefault(i => i.Name == menuItemName);
			if (menuItem == null)
			{
				throw new Exception("Can not find menu item: " + menuItemName);
			}

			return menuItem;
		}
	}*/
}