﻿//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class Trainer : MonoBehaviour
{
    public enum Class
    {
        Trainer,
        AceTrainer,
        Youngster,
        Rival,
        NeoRocketGrunt,
        BugCatcher,
        Champion,
        Hiker,
        Lass,
        Fisherman,
        GymLeader
    };

    private static string[] classString = new string[]
    {
        "Trainer",
        "Ace Trainer",
        "Youngster",
        "Rival",
        "Team Neo-Rocket Grunt",
        "Bug Catcher",
        "Champion",
        "Hiker",
        "Lass",
        "Fisherman",
        "Gym Leader"
    };

    private static int[] classPrizeMoney = new int[]
    {
        100,
        60,
        16,
        50,
        80,
        16,
        200,
        32,
        16,
        32,
        120
    };

    [Header("Trainer Settings")]
    
    public string uniqueSprites;

    public Class trainerClass;
    public string trainerName;

    public int customPrizeMoney = 0;

    public bool isFemale = false;

    public PokemonInitialiser[] trainerParty = new PokemonInitialiser[1];
    private Pokemon[] party;

    [Header("Music")]
    
    public AudioClip battleBGM;
    public int samplesLoopStart;

    public AudioClip victoryBGM;
    public int victorySamplesLoopStart;

    public AudioClip lowHpBGM;
    public int lowHpBGMSamplesLoopStart;

    [Header("Environment")]
    public MapSettings.Environment environment;

    
    [Space]
    
    [Header("Dialogs")]
    
    [Space]
    
    [Header("English Dialogs")]
    [FormerlySerializedAs("tightSpotDialog")]
    public string[] en_tightSpotDialog;

    [FormerlySerializedAs("playerVictoryDialog")] 
    public string[] en_playerVictoryDialog;
    [FormerlySerializedAs("playerLossDialog")] 
    public string[] en_playerLossDialog;
    
    [Space]
    
    [Header("French Dialogs")]
    public string[] fr_tightSpotDialog;

    public string[] fr_playerVictoryDialog;
    public string[] fr_playerLossDialog;

    public Trainer(Pokemon[] party)
    {
        this.trainerClass = Class.Trainer;
        this.trainerName = "";

        this.party = party;
    }

    void Awake()
    {
        party = new Pokemon[trainerParty.Length];
    }

    void Start()
    {
        for (int i = 0; i < trainerParty.Length; i++)
        {
            party[i] = new Pokemon(trainerParty[i].ID, trainerParty[i].gender, trainerParty[i].level, "Poké Ball",
                trainerParty[i].heldItem, trainerName, trainerParty[i].ability);
            
            int moveIndex = 0;
            for (int k = 0; k < trainerParty[i].moveset.Length && k < 4; ++k)
            {
                if (trainerParty[i].moveset[k].Length > 0)
                {
                    party[i].replaceMove(moveIndex, trainerParty[i].moveset[k]);
                    moveIndex++;
                }
                else
                {
                    break;
                }
            }
            
           
        }
    }


    public Pokemon[] GetParty()
    {
        return party;
    }

    public string GetName()
    {
        return (!string.IsNullOrEmpty(trainerName))
            ? classString[(int) trainerClass] + " " + trainerName
            : classString[(int) trainerClass];
    }

    public Sprite[] GetSprites()
    {
        Sprite[] sprites = new Sprite[0];
        if (uniqueSprites.Length > 0)
        {
            sprites = Resources.LoadAll<Sprite>("TrainerBattlers/" + uniqueSprites);
        }
        else
        {
            //Try to load female sprite if female
            if (isFemale)
            {
                sprites = Resources.LoadAll<Sprite>("TrainerBattlers/" + classString[(int) trainerClass] + "_f");
            }
            //Try to load regular sprite if male or female load failed
            if (!isFemale || (isFemale && sprites.Length < 1))
            {
                sprites = Resources.LoadAll<Sprite>("TrainerBattlers/" + classString[(int) trainerClass]);
            }
        }
        //if all load calls failed, load null as an array
        if (sprites.Length == 0)
        {
            sprites = new Sprite[] {Resources.Load<Sprite>("null")};
        }
        
        for (int i = 0; i < sprites.Length; ++i)
        {
            sprites[i] = Sprite.Create(sprites[i].texture, sprites[i].rect, new Vector2(0.5f, 0), 16f);
        }
        
        return sprites;
    }

    public int GetPrizeMoney()
    {
        int prizeMoney = (customPrizeMoney > 0) ? customPrizeMoney : classPrizeMoney[(int) trainerClass];
        int averageLevel = 0;
        for (int i = 0; i < party.Length; i++)
        {
            averageLevel += party[i].getLevel();
        }
        averageLevel = Mathf.CeilToInt((float) averageLevel / (float) party.Length);
        return averageLevel * prizeMoney;
    }

    public void HealParty()
    {
        foreach (Pokemon pokemon in party)
        {
            pokemon.healFull();
        }
    }
}


[System.Serializable]
public class PokemonInitialiser
{
    public int ID;
    public int level;
    public Pokemon.Gender gender;
    public string heldItem;
    public int ability;
    public string[] moveset;
}