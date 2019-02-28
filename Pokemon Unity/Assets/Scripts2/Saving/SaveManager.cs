﻿using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace PokemonUnity.Saving
{
	using PokemonUnity.Pokemon;
	using PokemonUnity.Item;
	using PokemonUnity.Saving.Location;

	public static class SaveManager
	{
		public const string BuildVersion = "0.1.0";

		/// <summary>
		/// If UseAppdata = true, Pokemon Unity will save the save files into %AppData%/Roaming/Pokemon Unity/Saves
		/// <para></para> 
		/// If UseAppdata = false, Pokemon Unity will save the save files into Assets/Saves
		/// </summary>
		/// <remarks>
		/// Instead of cluttering workspace by saving non-CompileTime content in directory,
		/// it should be left in bin, until the project is ready to be shipped out (i.e. production/release ready)
		/// At least then, it remains disposable; and doesnt get in the way of coding/git...
		/// Also, this bool is unnecessary, when the #pragma below does the job a lot better
		/// </remarks>
		private const bool UseAppdate = false;
#if DEBUG
		private static string gameConfig = @"\ConfigFile.pku"; //TestProject\bin\Debug
		private static string playerSave = @"\SaveFile.pku"; //TestProject\bin\Debug
		private static string saveLocation = "\\Save\\"; //TestProject\bin\Debug
		//private static string saveLocation = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Test.data"; 
		//private static string saveLocation = System.Environment.CurrentDirectory + @"\SaveDirectory\SaveFile.pku"; //@"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; 
		//private static string saveLocation = @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
		private static string gameConfig = UnityEngine.Application.persistentDataPath + "/ConfigFile.pku";
		private static string playerSave = UnityEngine.Application.persistentDataPath + "/SaveFile.pku";
		//private static string saveLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\Pokemon Unity\Saves\";
		//private static string saveLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + "/Saves/";
		private static string saveLocation = UnityEngine.Application.persistentDataPath + "/Saves/";
		//private static string saveLocation = UnityEngine.Application.dataPath + "/Saves/"; //Use for production
#endif
		
		#region 0.0.1 Original Save Mechanic
		//private static UnityEngine.GameObject Player;
		private static List<SaveEvent> EventSaves = new List<SaveEvent>();

		//public static void RegisterPlayer(UnityEngine.GameObject player)
		//{
		//    Player = player;
		//    //UnityEngine.Debug.Log("Registered Player.");
		//	GameVariables.DebugLog("Registered Player.");
		//}

		/// <summary>
		/// Registers an event into the EventSaves List<>
		/// </summary>
		/// <param name="customEvent">The event that needs to be registered</param>
		public static void RegisterEvent(SaveEvent customEvent)
		{
			EventSaves.Add(customEvent);
			EventSaves = EventSaves.OrderBy(x => x.EventTime).ToList();
			//UnityEngine.Debug.Log(customEvent.ToString());
			GameVariables.DebugLog(customEvent.ToString());
		}

		/// <summary>
		/// Get's the relevant CustomSaveEvents for the current Scene.
		/// </summary>
		/// <param name="sceneIndex">The scene index that the Player is currently on.</param>
		/// <returns></returns>
		public static List<SaveEvent> GetRelaventSaveData(int sceneIndex)
		{
			return EventSaves.Where(x => x.SceneIndex == sceneIndex).ToList();
		}

		/*private static SaveData CreateSaveFile(string saveName, int activeScene)
		{
			Pokemon[] Party = GameVariables.playerTrainer.Trainer.Party;
			Pokemon[,] PC = GameVariables.PC_Poke;
			List<Items> PlayerBag = GameVariables.Bag_Items;

			return new SaveData
				(
				saveName,
				activeScene,
				GameVariables.playerTrainer.Trainer.Name,
				GameVariables.playerTrainer.TrainerID, GameVariables.playerTrainer.SecretID,
				GameVariables.playerTrainer.isMale, //GameVariables.playerTrainer.GymBadges,
				GameVariables.playerTrainer.playerPokedex,
				GameVariables.playerTrainer.playerTime,
				Player.transform.position, Player.GetComponent<Player>().playerDirection,
				Player.transform.Find("Follower").transform.position, Player.GetComponent<FollowerMovement>().direction,
				Party, PC, PlayerBag,
				EventSaves
				);
		}*/

		///// <summary>
		///// Saves the game using the standard data.
		///// </summary>
		//public static void Save(string saveName, int activeScene)
		//{
		//	if (!UseAppdate)
		//	{
		//		saveLocation = UnityEngine.Application.dataPath + "/Saves/";
		//	}
		//
		//	//SaveData DataToSave = CreateSaveFile(saveName, activeScene);
		//	//SerializeAndCreateSaveFile(DataToSave);
		//}

		/// <summary>
		/// Saves the game using a given SaveData class.
		/// </summary>
		/// <param name="saveData">The SaveData containing the data that needs to be saved.</param>
		public static void Save(SaveData saveData)
		{
			SerializeAndCreateSaveFile(saveData);
		}

		public static void Overwrite(SaveData saveData, int saveID)
		{
			SerializeAndCreateSaveFile(saveData, saveID);
		}

		private static void SerializeAndCreateSaveFile(SaveData saveData, int saveID)
		{
			BinaryFormatter bf = new BinaryFormatter();
			try
			{
				try
				{
					File.Delete(saveLocation + "Save" + saveID + ".pku");
				}
				catch (FileNotFoundException) {	}

				FileStream file = File.Open(saveLocation + "Save" + saveID + ".pku", FileMode.OpenOrCreate, FileAccess.Write);
				bf.Serialize(file, saveData);
				file.Close();
			}
			catch (DirectoryNotFoundException)
			{
				Directory.CreateDirectory(saveLocation.Substring(0, saveLocation.Length - 1));
				using (FileStream file = File.Open(saveLocation + "Save" + (Directory.GetFiles(saveLocation, "*.pku", SearchOption.TopDirectoryOnly).Length).ToString() + ".pku", FileMode.OpenOrCreate, FileAccess.Write))
				{
					bf.Serialize(file, saveData);
				}
			}
		}

		private static void SerializeAndCreateSaveFile(SaveData saveData)
		{
			BinaryFormatter bf = new BinaryFormatter();
			try
			{
				int saveAmount = Directory.GetFiles(saveLocation, "*pku", SearchOption.TopDirectoryOnly).Length;

				if (saveAmount < 0)
					saveAmount = 0;

				FileStream file = File.Open(saveLocation + "Save" + saveAmount.ToString() + ".pku", FileMode.OpenOrCreate, FileAccess.Write);
				bf.Serialize(file, saveData);
				file.Close();
			}
			catch (Exception)
			{
				Directory.CreateDirectory(saveLocation.Substring(0, saveLocation.Length - 1));

				FileStream file = File.Open(saveLocation + "Save" + (Directory.GetFiles(saveLocation, "*pku", SearchOption.TopDirectoryOnly).Length).ToString() + ".pku", FileMode.OpenOrCreate, FileAccess.Write);
				bf.Serialize(file, saveData);
				file.Close();
			}
		}

		/// <summary>
		/// Loads the Save file that corresponeds with the saveInex.
		/// </summary>
		/// <param name="saveIndex">The index of the save (starting from 0, FE: "Save0.pku")</param>
		public static void Load(int saveIndex)
		{
			if (!UseAppdate)
			{
				//saveLocation = UnityEngine.Application.dataPath + "/Saves/";
				if (!Directory.Exists(saveLocation.Substring(0, saveLocation.Length - 1)))
				{
					Directory.CreateDirectory(saveLocation.Substring(0, saveLocation.Length - 1));
				}
			}

			BinaryFormatter bf = new BinaryFormatter();
			try
			{
				FileStream file = File.Open(saveLocation + "Save" + saveIndex.ToString() + ".pku", FileMode.Open, FileAccess.Read);
				SaveData DataToLoad = (SaveData)bf.Deserialize(file);

				if (null != DataToLoad)
				{
					//!Pass the data through to GameVariables!

					/*
					//EventSaves contains all the Events that the Player has encountered
					EventSaves = DataToLoad.EventList;

					if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != DataToLoad.ActiveScene)
					{
						UnityEngine.SceneManagement.SceneManager.LoadScene(DataToLoad.ActiveScene);
					}
					else
					{
						//Loads the Trainer's Party into the GameVariables
						for (int i = 0; i < DataToLoad.PlayerParty.Length; i++)
						{
							//The Trainer.Party is `private set;`, so we just loop through it and set it individually
							GameVariables.playerTrainer.Trainer.Party[i] = DataToLoad.PlayerParty[i];
						}
						GameVariables.PC_Poke = DataToLoad.PC;

						//Loads the Bag (containing the Items that the player owns) into the GameVariables
						GameVariables.Bag_Items = DataToLoad.PlayerBag;


						//Loading Player
						UnityEngine.GameObject Player = UnityEngine.GameObject.FindGameObjectWithTag("Player");
						Player.transform.position = DataToLoad.PlayerPosition;
						Player.GetComponent<PlayerMovement>().direction = DataToLoad.PlayerDirection;
						//Loading Follower
						UnityEngine.GameObject Follower = Player.transform.Find("Follower").gameObject;
						Follower.transform.position = DataToLoad.FollowerPosition;
						Follower.GetComponent<FollowerMovement>().direction = DataToLoad.FollowerDirection;

						EventSaves = EventSaves.OrderBy(x => x.EventTime).ToList();
					}*/
				}
				file.Dispose();
			}
			catch (FileNotFoundException)
			{
				//UnityEngine.Debug.Log("Couldn't find \"Save" + saveIndex + ".pku\".");
				GameVariables.DebugLog("Couldn't find \"Save" + saveIndex + ".pku\".");

			}
		}

		public static SaveData GetSave(int saveID)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (FileStream fileStream = new FileStream(saveLocation + "Save" + saveID + ".pku", FileMode.Open, FileAccess.Read))
			{
				try
				{
					SaveData newSave = (SaveData)bf.Deserialize(fileStream);
					return newSave;
				}
				catch (FileNotFoundException)
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Gets the amount of save files that's specified in Amount.
		/// </summary>
		/// <param name="Amount">The amount of save files that needs to be loaded (0 for all the save files).</param>
		/// <returns>A List containing the relevant amount of save files.</returns>
		public static List<SaveData> GetSaves(int Amount)
		{
			List<SaveData> saveFiles = new List<SaveData>();
			foreach (string file in Directory.GetFiles(saveLocation))
			{
				BinaryFormatter bf = new BinaryFormatter();
				try
				{
					if (Path.GetExtension(file) == "pku")
					{
						using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
						{
							try
							{
								SaveData saveData = (SaveData)bf.Deserialize(fileStream);
								if (saveData.BuildVersion == BuildVersion)
								{
									saveFiles.Add(saveData);
								}
								else
								{
									//Try to convert the created file into the current version
								}
							}
							catch (Exception e)
							{
								//UnityEngine.Debug.Log(e.ToString());
								GameVariables.DebugLog(e.ToString());
							}
						}
					}
				}
				catch (IOException e)
				{
					throw e;
				}
			}

			saveFiles = saveFiles.OrderBy(x => x.TimeCreated).ToList();
			if (Amount == 0 || saveFiles.Count < Amount)
			{
				return saveFiles;
			}
			else
			{
				return saveFiles.Take(Amount).ToList();
			}
		}

		//public static string GetBuildVersion()
		//{
		//	return BuildVersion;
		//}
		#endregion

		#region 0.1.0 Save Mechanic Rewrite
		public static void CreateSaveFileAndSerialize(SaveData[] saveData)
		{
			BinaryFormatter bf = new BinaryFormatter();
			if (System.IO.File.Exists(playerSave))
			{
				using(FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
				{
					SaveData[] sd = GetSaves();
#if DEBUG
					using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII))
					{
						sw.Write(saveData);
					}
#else
					bf.Serialize(fs, saveData);
#endif
				}
			}
		}

		public static SaveData[] GetSaves()
		{
			BinaryFormatter bf = new BinaryFormatter();
			if (System.IO.File.Exists(playerSave))
			{
				using (FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
#if DEBUG
					using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.ASCII))
					{
						return (SaveData[])(object)sr.ReadToEnd();
					};
#else
					return (SaveData[])bf.Deserialize(fs);
#endif
					}
			}
			else return null;
		}

		public static void LoadGameState()
		{
			BinaryFormatter bf = new BinaryFormatter();
			if (System.IO.File.Exists(gameConfig))
			{
				using (FileStream fs = System.IO.File.Open(gameConfig, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
					dynamic data;
#if DEBUG
					using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.ASCII))
					{
						data = (dynamic)sr.ReadToEnd();
					};
#else
					data = bf.Deserialize(fs);
#endif
					GameVariables.UserLanguage	= (Settings.Languages)data.Language;
					//GameVariables.WindowSkin	= data.WindowBorder;
					//GameVariables.DialogSkin	= data.DialogBorder;
					GameVariables.textSpeed		= data.TextSpeed;
					//GameVariables.mvol			= data.mVol;
					//GameVariables.svol			= data.sVol;
					GameVariables.fullscreen	= data.Fullscreen;
				}
			}
			else
			{
				using (FileStream fs = System.IO.File.Open(gameConfig, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
				{
#if DEBUG
					using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII)) 
					{
						sw.Write(new
						{
							Language			= (int)GameVariables.UserLanguage,//(int)language;
							//WindowBorder		= GameVariables.WindowSkin,
							//DialogBorder		= GameVariables.DialogSkin,
							TextSpeed			= GameVariables.textSpeed,
							//mVol				= GameVariables.mvol,
							//sVol				= GameVariables.svol,
							Fullscreen			= GameVariables.fullscreen,
						});
					}
#else
					bf.Serialize(fs, new
					{
						Language			= (int)GameVariables.UserLanguage,//(int)language;
						//WindowBorder		= GameVariables.WindowSkin,
						//DialogBorder		= GameVariables.DialogSkin,
						TextSpeed			= GameVariables.textSpeed,
						//mVol				= GameVariables.mvol,
						//sVol				= GameVariables.svol,
						Fullscreen			= GameVariables.fullscreen,
					});
#endif
				}
			}
		}
#endregion
	}
}