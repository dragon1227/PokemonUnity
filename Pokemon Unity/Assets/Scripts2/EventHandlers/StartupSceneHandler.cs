﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;
//using UnityEngine;

public class StartupSceneHandler : UnityEngine.MonoBehaviour, UnityEngine.EventSystems.ISubmitHandler, UnityEngine.EventSystems.IScrollHandler
{
	#region Variables
	public static GameVariables PersistantPlayerData { get; private set; }
    private static UnityEngine.GameObject MainMenu;// = UnityEngine.GameObject.Find("MainMenu");
    /// <summary>
    /// This is the panel display that shows save data for currently selected CONTINUE option
    /// </summary>
    private static UnityEngine.GameObject FileDataPanel;// = MainMenu.transform.GetChild(0).gameObject;
    private static UnityEngine.GameObject MenuOptions;// = MainMenu.transform.GetChild(1).gameObject;
    //private static UnityEngine.UI.Text DialogUITextDump = MainMenu.GetComponent<UnityEngine.UI.Text>();
    //private static UnityEngine.UI.Text DialogUIScrollText = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
	//map class with headers
	//pokemon array for list of encounters on current map loaded
	#endregion

	#region Unity MonoBehavior
    void Awake()
    {
		PersistantPlayerData = new GameVariables();
		//ToDo: On Start-up, Load & Process GameVariables, to begin and instantiate game
        MainMenu = UnityEngine.GameObject.Find("MainMenu");
        FileDataPanel = MainMenu.transform.GetChild(0).gameObject;
        MenuOptions = MainMenu.transform.GetChild(1).gameObject;
		//ToDo: Awake Audio Components
    }
    void OnEnable()
    {
        /* If no previous saved data was found: 
         * disable the right playerData window,
         * disable continue menu-option,
         * extend menu option width size,
         * transform menu positions to collapse 
         * to top and fill in empty gap
         */
        //Load Any/All GameSaves
        //"ContinuePanel"
        MenuOptions.transform.GetChild(0).gameObject.SetActive(GameVariables.SaveFileFound);
        FileDataPanel.SetActive(GameVariables.SaveFileFound);
        if (!GameVariables.SaveFileFound)
        {
            //"MainMenu"
            //Stretch menu to fit width across
            MenuOptions.GetComponent<UnityEngine.RectTransform>().anchorMax = new UnityEngine.Vector2(1, 1);
            //Move options up to fill in gap
            MenuOptions.transform.GetChild(1).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            MenuOptions.transform.GetChild(2).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            MenuOptions.transform.GetChild(3).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            //UnityEngine.Debug.Log(MenuOptions.transform.GetChild(1).gameObject.transform.position);
            //UnityEngine.Debug.Log(MenuOptions.transform.GetChild(1).gameObject.transform.localPosition);
            //ToDo: Git was giving build error on `ForceUpdateRectTransforms()`; says it doesnt exist...
            //Refresh the changes to display the new position
            //MenuOptions.transform.GetChild(1).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
            //MenuOptions.transform.GetChild(2).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
            //MenuOptions.transform.GetChild(3).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
        }
    }
    void Start()
    {
    }
    void Update()
    {
        /* Ping GameNetwork server every 15-45secs
         * If game server is offline or unable to
         * ping connection:
         * Netplay toggle-icon bool equals false
         * otherwise toggle bool to true
         */
        //StartCoroutine(PingServerEveryXsec);
        //Use coroutine that has a while loop instead of using here
        /*while (MainMenu.activeSelf)
        {
            //While scene is enabled, run coroutine to ping server
            break;
        }*/
        //int index = (int)(UnityEngine.Time.timeSinceLevelLoad * Settings.framesPerSecond);
        //index = index % sprites[].Length;
    }
	#endregion

	#region GameStart and Main Menu
	/* If Continue option is available:
     * file slot data should reflect in the 
     * playerData window on the right side;
     * disable slot options with no data
     */
	void ContinueSavedGame()
    {
        //If Continue Option is select
        if (MenuOptions.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Toggle>().isOn)
        {
            //Get Toggle Value from Toggle group for which toggleOption is selected
            //use gamesave toggle to load game from that slot
        }
    }

    public void ChangeDataPanel(int slot)
    {
        //Refresh the panel for continue screen to reflect gamesave data
        UnityEngine.Debug.Log(slot);
    }

    public void OnScroll(PointerEventData eventData)
    {
        //throw new NotImplementedException();

    }

    public void OnSubmit(BaseEventData eventData)
    {
        //throw new NotImplementedException();
        switch (eventData.selectedObject.name)
        {
            //If the object is slots, submit continue
            case "":
            //If the object is continue, transistion to next scene
            case "1":
            default:
                break;
        }
    }

	/* If settings option is accessed, 
     * Use GameVariables.ChangeScene to transition
     */
	#endregion
		
	#region Methods
	//Your map class should be the one talking to unity and triggering if battles should occur

	/// <summary>
	/// Start a single wild battle
	/// </summary>
	public bool WildBattle(Pokemon pkmn, bool cantescape = true, bool canlose = false)
	{
		if (GameVariables.playerTrainer.Trainer.Party.Length == 0 || (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.LeftControl) && GameVariables.debugMode))
		{
			if (GameVariables.playerTrainer.Trainer.Party.Length > 0)
				GameVariables.DebugLog("SKIPPING BATTLE...");
			GameVariables.nextBattleBGM = null;
			GameVariables.nextBattleME = null;
			GameVariables.nextBattleBack = null;
			return true;
		}
		Pokemon[] generateWildPkmn = new Pokemon[1];
		generateWildPkmn[0] = pkmn; //new Pokemon();
		//int decision = 0;
		Battle battle =
		//GameVariables.battle =
			new Battle(
				GameVariables.playerTrainer.Trainer,
				new Trainer(generateWildPkmn)
			)
			.InternalBattle(true)
			.CantEscape(!cantescape);
			//.StartBattle(canlose); //Switch to battle scene and trigger coroutine 
			//.AfterBattle(ref decision,canlose);
		//GameVariables.battle.StartBattle(canlose);  
		IEnumerator<Battle.BattleResults> e = BattleAnimationHandler.BattleCoroutineResults;
		//while battle scene is active
		//delay results of battle
		//on battle end return the results of the battle 
		//ToDo: and save data to profile?... maybe that would be done from battle class
		return e.Current != Battle.BattleResults.LOST;
	}

	/// <summary>
	/// Start a double wild battle
	/// </summary>
	public bool DoubleWildBattle(Pokemon pkmn1, Pokemon pkmn2, bool cantescape = true, bool canlose = false)
	{
		if (GameVariables.playerTrainer.Trainer.Party.Length == 0 || (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.LeftControl) && GameVariables.debugMode))
		{
			if (GameVariables.playerTrainer.Trainer.Party.Length > 0)
				GameVariables.DebugLog("SKIPPING BATTLE...");
			GameVariables.nextBattleBGM = null;
			GameVariables.nextBattleME = null;
			GameVariables.nextBattleBack = null;
			return true;
		}
		Pokemon[] generateWildPkmn = new Pokemon[] { pkmn1, pkmn2 };//new Pokemon(), new Pokemon()
		//int decision = 0;
		Battle battle =
		//GameVariables.battle =
			new Battle(
				GameVariables.playerTrainer.Trainer,
				new Trainer(generateWildPkmn) { IsDouble = true }
			)
			.InternalBattle(true)
			.CantEscape(!cantescape);
			//.StartBattle(canlose); //Switch to battle scene and trigger coroutine 
			//.AfterBattle(ref decision,canlose);
		//GameVariables.battle.StartBattle(canlose);  
		IEnumerator<Battle.BattleResults> e = BattleAnimationHandler.BattleCoroutineResults;
		//while battle scene is active
		//delay results of battle
		//on battle end return the results of the battle 
		//ToDo: and save data to profile?... maybe that would be done from battle class
		//return battle.decision;
		return (e.Current != Battle.BattleResults.LOST && e.Current != Battle.BattleResults.DRAW);
	}
	#endregion
}
