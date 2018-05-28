﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Pokemon;
using PokemonUnity.Move;

/// <summary>
/// 
/// </summary>
/// ToDo: Consider nesting PokemonData class?
[System.Serializable]
public class Pokemon //: ePokemons //PokemonData
{
	#region Variables
	/// <summary>
	/// Current Total HP
	/// </summary>
	public int TotalHP { get { return totalHP; } }
	private int totalHP = 1;
	/// <summary>
	/// Current HP
	/// </summary>
	private int hp = 1;
	/// <summary>
	/// Current Attack Stat
	/// </summary>
	private int ATK;
	/// <summary>
	/// Current Defense stat
	/// </summary>
	private int DEF;
	/// <summary>
	/// Current Special Attack Stat
	/// </summary>
	private int SPA;
	/// <summary>
	/// Current Special Defense Stat
	/// </summary>
	private int SPD;
	/// <summary>
	/// Current Speed Stat
	/// </summary>
	private int SPE;
	/// <summary>
	/// Array of 6 Individual Values for HP, Atk, Def, Speed, Sp Atk, and Sp Def
	/// </summary>
	private readonly int[] IV = new int[6];
	/// <summary>
	/// Effort Values
	/// </summary>
	private readonly int[] EV = new int[6]; //{ 0, 0, 0, 0, 0, 0 }; //same thing
	/// <summary>
	/// Species (National Pokedex number)
	/// </summary>
	/// ToDo: Fetch from PokemonData : _base.PokeId
	private int species;
	/// <summary>
	/// Personal/Pokemon ID
	/// </summary>
	/// ToDo: String value?
	private int PersonalId;
	/// <summary>
	/// 32-bit Trainer ID (the secret ID is in the upper 16-bits);
	/// Deprecated
	/// </summary>
	/// ToDo: Remove this, and fetch from Trainer Class?
	/// Can also store hexadecimal/binary values in int
	private int TrainerId;
	/// <summary>
	/// Pokerus strain and infection time
	/// </summary>
	/// <remarks>
	/// ToDo: Custom class?
	/// 3 Values; Not Infected, Cured, Infected.
	/// [0] = Pokerus Strain; [1] = Days until cured.
	/// if ([0] && [1] == 0) => Not Infected
	/// </remarks>
	private readonly int[] pokerus = new int[2]; //{ 0, 0 };
	/// <summary>
	/// Held item
	/// </summary>
	public  eItems.Item Item { get { return this.item; } set { this.item = value; } }
	private eItems.Item item;
	/// <summary>
	/// Consumed held item (used in battle only)
	/// </summary>
	private bool itemRecycle;
	/// <summary>
	/// Resulting held item (used in battle only)
	/// </summary>
	private bool itemInitial;
	/// <summary>
	/// Where Pokemon can use Belch (used in battle only)
	/// </summary>
	private bool belch;
	/// <summary>
	/// Mail?...
	/// </summary>
	private string mail;
	/// <summary>
	/// Perform a null check; if anything other than null, there is a message
	/// </summary>
	/// ToDo: Item category
	public string Mail
	{
		get {
			if (this.mail == null) return null; //If empty return null
			if (mail.Length == 0 || this.Item == 0)//|| this.item.Category != eItems.Item.Category.Mail )
			{
				mail = null; return null;
			}
			return mail;
		}
	}
	/// <summary>
	/// The pokemon fused into this one.
	/// </summary>
	private int fused;
	/// <summary>
	/// Nickname
	/// </summary>
	private string name;
	/// <summary>
	/// Current experience points
	/// </summary>
	private int exp;
	/// <summary>
	/// Current happiness
	/// </summary>
	/// <remarks>
	/// This is the samething as "friendship";
	/// </remarks>
	private int happiness;
	public enum HappinessMethods
	{
		WALKING,
		LEVELUP,
		GROOM,
		FAINT,
		VITAMIN,
		EVBERRY,
		POWDER,
		ENERGYROOT,
		REVIVALHERB
	}
	/// <summary>
	/// Status problem (PBStatuses)
	/// </summary>
	/// ToDo: Status Class
	private Status status;
	/// <summary>
	/// Sleep count/Toxic flag
	/// </summary>
	/// ToDo: Add to Status Class
	private int statusCount;
	/// <summary>
	/// Steps to hatch egg, 0 if Pokemon is not an egg
	/// </summary>
	private int eggSteps;
	/// <summary>
	/// Moves (PBMove)
	/// </summary>
	/// ToDo Move class, not enum
	private Move[] moves = new Move[4]; //{ Move.MoveData.Move.NONE, Move.MoveData.Move.NONE, Move.MoveData.Move.NONE, Move.MoveData.Move.NONE };
	/// <summary>
	/// The moves known when this Pokemon was obtained
	/// </summary>
	private List<Moves> firstMoves = new List<Moves>();
	/// <summary>
	/// Ball used
	/// </summary>
	private eItems.Item ballUsed;// = (eItems.Item)0; //ToDo: None?
	/// <summary>
	/// Markings
	/// </summary>
	private readonly bool[] markings = new bool[6]; //{ false, false, false, false, false, false };
	/// <summary>
	/// Manner Obtained:
	/// </summary>
	private ObtainedMethod ObtainedMode;
	public enum ObtainedMethod
	{
		MET = 0,
		EGG = 1,
		TRADED = 2,
		/// <summary>
		/// NPC-Event?
		/// </summary>
		FATEFUL_ENCOUNTER = 4
	}
	/// <summary>
	/// Map where obtained
	/// </summary>
	/// <remarks>
	/// Doubles as "HatchedMap"
	/// ToDo: Make this an enum
	/// </remarks>
	private int obtainMap;
	/// <summary>
	/// Replaces the obtain map's name if not null
	/// </summary>
	private string obtainString;
	/// <remarks>
	/// Wouldnt this change again when traded to another trainer?
	/// </remarks>
	private int obtainLevel; // = 0;
	private System.DateTimeOffset obtainWhen;
	private System.DateTimeOffset hatchedWhen;
	/// <summary>
	/// Original Trainer's Name
	/// </summary>
	/// <remarks>
	/// ToDo: PlayerTrainer Class here
	/// </remarks>
	private Trainer OT;
	/// <summary>
	/// Forces the first/second/hidden (0/1/2) ability
	/// </summary>
	private eAbility.Ability[] abilityFlag = new eAbility.Ability[2];// readonly
	/// <summary>
	/// </summary>
	/// <remarks>
	/// isMale; null = genderless?
	/// Should consider gender as byte? bool takes up same amount of space
	/// </remarks>
	private bool? genderFlag;
	/// <summary>
	/// Forces a particular nature
	/// </summary>
	/// ToDo: Redo NatureDatabase Class
	private NatureDatabase.Nature natureFlag;
	/// <summary>
	/// Forces the shininess
	/// </summary>
	private bool? shinyFlag;
	/// <summary>
	/// Deprecated; Array of ribbons
	/// </summary>
	/// <remarks>
	/// Make 2d Array (Array[,]) separated by regions/Gens
	/// </remarks>
	private bool[] ribbon; //= new bool[numberOfRegions,RibbonsPerRegion];
	private List<Ribbon> ribbons = new List<Ribbon>();
	/// <summary>
	/// Each region/ribbon sprite should have it's own Ribbon.EnumId
	/// </summary>
	/// <example>Pokemon acquired beauty ribbon in region1 AND 2?</example>
	/// I didnt know ribbons could be upgraded...
	/// Make each ribbon into sets, where next number up is upgrade? (or multiply?)
	/// Does it make a difference if pokemon won contest in different regions?
	public enum Ribbon
	{
		NONE = 0,
		HOENNCOOL = 1,
		HOENNCOOLSUPER = 2,
		HOENNCOOLHYPER = 3,
		HOENNCOOLMASTER = 4,
		HOENNBEAUTY = 5,
		HOENNBEAUTYSUPER = 6,
		HOENNBEAUTYHYPER = 7,
		HOENNBEAUTYMASTER = 8,
		HOENNCUTE = 9,
		HOENNCUTESUPER = 10,
		HOENNCUTEHYPER = 11,
		HOENNCUTEMASTER = 12,
		HOENNSMART = 13,
		HOENNSMARTSUPER = 14,
		HOENNSMARTHYPER = 15,
		HOENNSMARTMASTER = 16,
		HOENNTOUGH = 17,
		HOENNTOUGHSUPER = 18,
		HOENNTOUGHHYPER = 19,
		HOENNTOUGHMASTER = 20,
		SINNOHCOOL = 21,
		SINNOHCOOLSUPER = 22,
		SINNOHCOOLHYPER = 23,
		SINNOHCOOLMASTER = 24,
		SINNOHBEAUTY = 25,
		SINNOHBEAUTYSUPER = 26,
		SINNOHBEAUTYHYPER = 27,
		SINNOHBEAUTYMASTER = 28,
		SINNOHCUTE = 29,
		SINNOHCUTESUPER = 30,
		SINNOHCUTEHYPER = 31,
		SINNOHCUTEMASTER = 32,
		SINNOHSMART = 33,
		SINNOHSMARTSUPER = 34,
		SINNOHSMARTHYPER = 35,
		SINNOHSMARTMASTER = 36,
		SINNOHTOUGH = 37,
		SINNOHTOUGHSUPER = 38,
		SINNOHTOUGHHYPER = 39,
		SINNOHTOUGHMASTER = 40,
		WINNING = 41,
		VICTORY = 42,
		ABILITY = 43,
		GREATABILITY = 44,
		DOUBLEABILITY = 45,
		MULTIABILITY = 46,
		PAIRABILITY = 47,
		WORLDABILITY = 48,
		CHAMPION = 49,
		SINNOHCHAMP = 50,
		RECORD = 51,
		EVENT = 52,
		LEGEND = 53,
		GORGEOUS = 54,
		ROYAL = 55,
		GORGEOUSROYAL = 56,
		ALERT = 57,
		SHOCK = 58,
		DOWNCAST = 59,
		CARELESS = 60,
		RELAX = 61,
		SNOOZE = 62,
		SMILE = 63,
		FOOTPRINT = 64,
		ARTIST = 65,
		EFFORT = 66,
		BIRTHDAY = 67,
		SPECIAL = 68,
		CLASSIC = 69,
		PREMIER = 70,
		SOUVENIR = 71,
		WISHING = 72,
		NATIONAL = 73,
		COUNTRY = 74,
		BATTLECHAMPION = 75,
		REGIONALCHAMPION = 76,
		EARTH = 77,
		WORLD = 78,
		NATIONALCHAMPION = 79,
		WORLDCHAMPION = 80
	}
	public List<Ribbon> Ribbons { get { return this.ribbons; } }
	/// <summary>
	/// Contest stats
	/// </summary>
	private int cool, beauty, cute, smart, tough, sheen;
	private PokemonData _base;
	/// <summary>
	/// Max total EVs
	/// </summary>
	const int EVLIMIT = 510; //static readonly
	/// <summary>
	/// Max EVs that a single stat can have
	/// </summary>
	/// ToDo: use const instead?
	/// Can be referenced as [Attribute] if is a const value
	const int EVSTATLIMIT = 252; //static readonly
	/// <summary>
	/// Maximum length a Pokemon's nickname can be
	/// </summary>
	const int NAMELIMIT = 10; //static readonly
	#endregion

	public Pokemon() { }

	/// <summary>
	/// Uses PokemonData to initialize a Pokemon from base stats
	/// </summary>
	/// <param name="pokemon"></param>
	/// ToDo: Inherit PokemonData 
	public Pokemon(PokemonData.Pokemons pokemon) //ToDo: Redo PokemonDatabase/PokemonData -- DONE
	{
		//PersonalId = 
		_base = PokemonData.GetPokemon(pokemon);
		//Gender = isMale();
		Abilities[0] = _base.Abilities[1] == eAbility.Ability.NONE ? _base.Abilities[0] : _base.Abilities[new Random().Next(0, 2)];
		Nature = (NatureDatabase.Nature)(new Random().Next(0, 24));
		//IsShiny
	}

	#region Ownership, obtained information
	/// <summary>
	/// Returns whether or not the specified Trainer is the NOT this Pokemon's original trainer
	/// </summary>
	/// <param name="trainer"></param>
	/// <returns></returns>
	public bool isForeign(Trainer trainer) {
		return trainer != this.OT; //ToDo: Match HashId 
	}

	/// <summary>
	/// Returns the public portion of the original trainer's ID
	/// </summary>
	/// <returns></returns>
	/*public string PublicId()
    {
        //return TrainerId.ToString();
        return OT.ToString(); //ToDo: TrainerId fix here
    }*/
	public string PublicId
	{
		get { return OT.ToString(); }
	}

	/// <summary>
	/// Returns this Pokemon's level when this Pokemon was obtained
	/// </summary>
	/// <returns></returns>
	/*public int ObtainLevel()
    {
        //int level = 0; int.TryParse(this.obtainLevel, out level);
        return this.obtainLevel;
    }*/
	public int ObtainLevel
	{
		get { return this.obtainLevel; }
	}

	/*// <summary>
    /// Returns the time when this Pokemon was obtained
    /// </summary>
    /// <returns></returns>
    public System.DateTimeOffset TimeReceived()
    {
        if (obtainWhen == null) TimeReceived(DateTimeOffset.UtcNow); //ToDo: Global DateTime Variable
        return obtainWhen;
    }

    /// <summary>
    /// Set the time when this Pokemon was obtained
    /// </summary>
    /// <param name="UTCdate"></param>
    public void TimeReceived(DateTimeOffset UTCdate)
    {
        obtainWhen = UTCdate;
    }*/
	/// <summary>
	/// Sets or Returns the time when this Pokemon was obtained
	/// </summary>
	public DateTimeOffset TimeReceived
	{
		get
		{
			if (obtainWhen == null) this.obtainWhen = DateTimeOffset.UtcNow;
			return this.obtainWhen;
		}
		set { this.obtainWhen = value; }
	}

	/*// <summary>
    /// Returns the time when this Pokemon hatched
    /// </summary>
    /// <returns></returns>
    public DateTimeOffset TimeEggHatched()
    {
        if (this.ObtainedMode == ObtainedMethod.EGG)
        {
            if (hatchedWhen == null) TimeEggHatched(DateTimeOffset.UtcNow);
            return hatchedWhen;
        }
        else
            return DateTimeOffset.UtcNow; //ToDo: Something else?
    }

    /// <summary>
    /// Set the time when this Pokemon hatched
    /// </summary>
    /// <param name="UTCdate"></param>
    public void TimeEggHatched (DateTimeOffset UTCdate)
    {
        hatchedWhen = UTCdate;
    }*/
	/// <summary>
	/// Sets or Returns the time when this Pokemon hatched
	/// </summary>
	public DateTimeOffset TimeEggHatched
	{
		get
		{
			if (this.ObtainedMode == ObtainedMethod.EGG)
			{
				if (hatchedWhen == null) this.hatchedWhen = DateTimeOffset.UtcNow;
				return this.hatchedWhen;
			}
			else
				//return DateTimeOffset.UtcNow; //ToDo: Something else? Maybe error?
				throw new Exception("Trainer did not acquire Pokemon as an egg.");
		}
		set { this.hatchedWhen = value; }
	}
	#endregion

	#region Level
	public int Level
	{
		get
		{
			return 0;
			// ToDo: return Experience.GetLevelFromExperience(this.exp, this.GrowthRate)
		}
		set
		{
			if (value < 1 || value > 100) //Experience.MAXLEVEL
				throw new Exception(string.Format("The level number {0} is invalid", value));
			// ToDo: return Experience.GetStartExperience(value, this.GrowthRate)
		}
	}

	public bool isEgg
	{
		get
		{
			return eggSteps > 0;
		}
	}

	public PokemonData.LevelingRate GrowthRate
	{
		get
		{
			return _base.GrowthRate; //ToDo: Return as int?
		}
	}

	int baseExp
	{
		get
		{
			return _base.BaseExpYield; //ToDo: 
		}
	}
	#endregion

	#region Gender
	/// <summary>
	/// Returns this Pokemons gender. male; female; genderless.
	/// Sets this Pokemon's gender to a particular gender (if possible)
	/// </summary>
	public bool? Gender { get { return this.genderFlag; } }

	/*// <summary>
    /// Helper function that determines whether the input values would make a female.
    /// </summary>
    /// ToDo: isMale; isFemale; isGenderless... properties?
    public bool? isMale/isFemale/isGenderless//(float genderRate = this._base.MaleRatio)
    {
        get
        {
            if (genderRate == 100f) return true; 
            if (genderRate == 100f) return false; //Always female
            return null; //genderless
        }
    }*/

	/// <summary>
	/// Returns whether this Pokemon species is restricted to only ever being one gender (or genderless)
	/// </summary>
	public bool isSingleGendered { get { return true; } }
	#endregion

	#region Ability
	/// <summary>
	/// Returns the ID of the Pokemons Ability.
	/// </summary>
	/// ToDo: Sets this Pokemon's ability to a particular ability (if possible)
	/// ToDo: Ability 1 or 2, never both...
	/// ToDo: Error on non-compatible ability?
	public eAbility.Ability[] Abilities { get { return abilityFlag; } set { abilityFlag = value; } }//ToDo: Check against getAbilityList()?

	/// <summary>
	/// Returns whether this Pokemon has a partiular ability
	/// </summary>
	/// <param name="ability"></param>
	/// <returns></returns>
	public bool hasAbility(eAbility.Ability ability = eAbility.Ability.NONE)
	{
		if (ability == eAbility.Ability.NONE) return (int)Abilities[0] > 0 || (int)Abilities[1] > 0;// || (int)Abilities[2] > 0;
		else
		{
			return Abilities[0] == ability || Abilities[1] == ability;// || Abilities[2] == ability;
		}
		//return false;
	}

	public bool hasHiddenAbility()
	{
		return Abilities[1] != eAbility.Ability.NONE;
	}

	/// <summary>
	/// Returns a list of abilities this Pokemon can have.
	/// </summary>
	/// <returns></returns>
	/// Is there a list of abilities a pokemon can have outside of "default" values?
	public eAbility.Ability[] getAbilityList()
	{
		//List<eAbility.Ability> abilList;
		//foreach(){ list.add() }
		//eAbility.Ability[] abilities = abilList.ToArray();
		//return abilities;
		return this._base.Abilities; //ToDo: List of abilities?
	}
	#endregion

	#region Nature
	/// <summary>
	/// Returns the ID of this Pokemon's nature or
	/// Sets this Pokemon's nature to a particular nature (and calculates the stat modifications).
	/// </summary>
	public NatureDatabase.Nature Nature { get { return this.natureFlag; } set { this.natureFlag = value; calcStats(); } }

	/// <summary>
	/// Returns whether this Pokemon has a particular nature
	/// </summary>
	/// <param name="nature"></param>
	/// <returns></returns>
	public bool hasNature(NatureDatabase.Nature nature = 0) //None
	{
		if ((int)nature < 1) return (int)this.Nature >= 1;
		else
		{
			return this.Nature == nature;
		}
	}
	#endregion

	#region Shininess
	/// <summary>
	/// Uses math to determine if Pokemon is shiny.
	/// Returns whether this Pokemon is shiny (differently colored)
	/// </summary>
	/// <returns></returns>
	/// Use this when rolling for shiny...
	/// Honestly, without this math, i probably would've done something a lot more primative.
	/// Look forward to primative math on wild pokemon encounter chances...
	public bool isShiny()
	{
		if (shinyFlag.HasValue) return shinyFlag.Value;
		int a = this.PersonalId ^ this.TrainerId; //Wild Pokemon TrainerId?
		int b = a & 0xFFFF;
		int c = (a >> 16) & 0xFFFF;
		int d = b ^ c;
		return d < _base.ShinyChance;
	}

	/// <summary>
	/// Makes this Pokemon shiny or not shiny
	/// </summary>
	public bool IsShiny
	{
		//If not manually set, use math to figure out.
		//ToDo: Store results to save on redoing future execution? 
		get { return shinyFlag ?? isShiny()/*false*/; }
		set { shinyFlag = value; }
	}
	#endregion

	#region Pokerus
	/// <summary>
	/// Returns the full value of this Pokemon's Pokerus
	/// </summary>
	/// <returns>
	/// 3 Values; Not Infected, Cured, Infected.
	/// [0] = Pokerus Strain; [1] = Days until cured.
	/// if ([0] && [1] == 0) => Not Infected
	/// </returns>
	/*public int[] Pokerus()
    {
        return this.pokerus;
    }*/
	public int[] Pokerus { get { return this.pokerus; } }

	/// <summary>
	/// Returns the Pokerus infection stage for this Pokemon
	/// </summary>
	/// <returns></returns>
	/*public int PokerusStrain()
    {
        return this.pokerus[0] / 16;
    }*/
	public int PokerusStrain { get { return this.pokerus[0] / 16; } }

	/// <summary>
	/// Returns the Pokerus infection stage for this Pokemon
	/// </summary>
	/// <returns>
	/// if null, not infected; 
	/// true infected, and false cured?
	/// </returns>
	/*public bool? PokerusStage()
    {
        if (pokerus[0] == 0 && pokerus[1] == 0) return null;        // Not Infected
        if (pokerus[0] > 0 && pokerus[1] == 0) return false;        // Cured; (pokerus[0] % 16) == 0
        return true;                                                // Infected
    }*/
	public bool? PokerusStage
	{
		get
		{
			if (pokerus[0] == 0 && pokerus[1] == 0) return null;        // Not Infected
			if (pokerus[0] > 0 && pokerus[1] == 0) return false;        // Cured; (pokerus[0] % 16) == 0
			return true;                                                // Infected
		}
	}

	/// <summary>
	/// Gives this Pokemon Pokerus (either the specified strain or a random one)
	/// </summary>
	/// <param name="strain"></param>
	public void GivePokerus(int strain = 0)
	{
		if (this.PokerusStage.HasValue ? !this.PokerusStage.Value : false) return; // Cant re-infect a cured Pokemon
		if (strain <= 0 || strain >= 16) strain = new Random().Next(1, 16);
		pokerus[1] = 1 + (strain % 4);
		pokerus[0] |= strain; //strain << 4
	}

	/// <summary>
	/// Resets the infection time for this Pokemon's Pokerus (even if cured)
	/// </summary>
	public void ResetPokerusTime()
	{
		if (pokerus[0] == 0) return;
		pokerus[1] = 1 + (pokerus[0] % 4);
	}

	/// <summary>
	/// Reduces the time remaining for this Pokemon's Pokerus (if infected)
	/// </summary>
	public void LowerPokerusCount()
	{
		if (this.PokerusStage.HasValue ? !this.PokerusStage.Value : true) return;
		pokerus[1] -= 1;
	}
	#endregion

	#region Types
	/// <summary>
	/// Returns whether this Pokemon has the specified type.
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public bool hasType(PokemonData.Types type)
	{
		return this._base.Type[0] == type || this._base.Type[1] == type;
	}

	/// <summary>
	/// Returns this Pokemon's first type
	/// </summary>
	public PokemonData.Types Type1 { get { return this._base.Type[0]; } }

	/// <summary>
	/// Returns this Pokemon's second type
	/// </summary>
	public PokemonData.Types Type2 { get { return this._base.Type[1]; } }
	#endregion

	#region Moves
	/// <summary>
	/// Returns the number of moves known by the Pokémon.
	/// </summary>
	public int numMoves()
	{
		int ret = 0;
		for (int i = 0; i < 4; i++) {//foreach(var move in this.moves){ 
			if ((int)this.moves[i].MoveId != 0) ret += 1;//move.id
		}
		return ret;
	}

	/// <summary>
	/// Returns true if the Pokémon knows the given move.
	/// </summary>
	public bool hasMove(Moves move) {
		//if (move <= 0) return false;//move == null ||
		for (int i = 0; i < 4; i++)
		{
			if (this.moves[i].MoveId == move) return true;
		}
		return false;
	}

	public bool knowsMove(Moves move) { return this.hasMove (move); }

	/*// <summary>
    /// Returns the list of moves this Pokémon can learn by levelling up.
    /// </summary>
    /// ToDo: Custom<int Level, eMove move> Class
    public PokemonMoveset[] getMoveList() {
		//Move.MoveData.Move[] movelist = _base.MovesetMoves;
		//for (int k = 0; k < movelist.Length - 1; k++)
		//{
		//	//Array to List/Dictionary
		//	//separate into Move.value and Pokemon.Level 
		//	//needed to learn the skill
		//	//movelist([level, move])}
		//}
		//return movelist;
		return _base.MoveSet;
     }*/

	/// <summary>
    /// Sets this Pokémon's movelist to the default movelist it originally had.
    /// </summary>
    public void resetMoves()
    {
		//Move.MoveData.Move moves = this.getMoveList();
		//Move.MoveData.Move[] movelist = new Move.MoveData.Move[4];
		List<Moves> movelist = new List<Moves>(); 
        for (int i = 0; i < _base.MoveSet.Length; i++){//foreach(var i in _base.MoveSet)
            if (_base.MoveSet[i].Level <= this.Level)
            {
                movelist.Add(_base.MoveSet[i].MoveId);
            }
        }
        //movelist|=[] // Remove duplicates
        int listend = movelist.Count - 4;
        listend = listend < 0 ? 0 : listend;
        int j = 0;
        for (int i = 0; i < listend + 4; i++) { 
            Moves moveid = (i >= movelist.Count) ? 0 : movelist[i];
			this.moves[j] = new Move(moveid);
            //moves[j] = (i >= movelist.Count) ? 0 : new Move(movelist[i]);
            j += 1;
        }
    }

	/// <summary>
	/// Silently learns the given move. Will erase the first known move if it has to.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	public void LearnMove(Moves move) {
		if ((int)move <= 0) return;
		for (int i = 0; i < 4; i++) {
			if (moves[i].MoveId == move) {
				int j = i + 1;
				while (j < 4) {
					if (moves[j].MoveId == 0) break;
					Move tmp = moves[j];
					moves[j] = moves[j - 1];
					moves[j - 1] = tmp;
					j += 1;
				}
				return;
			}
		}
		for (int i = 0; i < 4; i++) {
			if (moves[i].MoveId == 0) {
				moves[i] = new Move(move);
				return;
			}
		}
		moves[0] = moves[1];
		moves[1] = moves[2];
		moves[2] = moves[3];
		moves[3] = new Move(move);
	}

	/// <summary>
	/// Deletes the given move from the Pokémon.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	public void DeleteMove(Moves move) {
		if (move <= 0) return;
		List<Move> newmoves = new List<Move>();
		for (int i = 0; i < 4; i++) { 
			if (moves[i].MoveId != move) newmoves.Add(moves[i]);
		}

		newmoves.Add(new Move(0));
		for (int i = 0; i< 4; i++) {
			moves[i] = newmoves[i];
		}
	 }

	/// <summary>
	/// Deletes the move at the given index from the Pokémon.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	public void DeleteMoveAtIndex(int index) {
		List<Move> newmoves = new List<Move>();

		for (int i = 0; i < 4; i++) {
			if (i != index) newmoves.Add(moves[i]);
		}
		
		newmoves.Add(new Move(0));

		for (int i = 0; i < 4; i++) {
			moves[i] = newmoves[i];
		}
	}

	/// <summary>
	/// Deletes all moves from the Pokémon.
	/// </summary>
	public void DeleteAllMoves() { 
		//moves = new Move.MoveData.Move[4];
		for (int i = 0; i< 4; i++) { 
			moves[i]= new Move(0);
		}
	}

	/// <summary>
	/// Copies currently known moves into a separate array, for Move Relearner.
	/// </summary>
	public void RecordFirstMoves() {
		for (int i = 0; i < 4; i++) {
			if (moves[i].MoveId > 0) firstMoves.Add(moves[i].MoveId);
		}
	}

	public void AddFirstMove(Moves move) {
		if (move > 0 && !firstMoves.Contains(move)) firstMoves.Add(move);
		return;
	}

	public void RemoveFirstMove(Moves move) {
		if (move > 0) firstMoves.Remove(move); 
		return;
	}

	public void ClearFirstMoves() {
		firstMoves.Clear(); //= new Move.MoveData.Move[4];
	}

	/*bool isCompatibleWithMove(Move.MoveData.Move move) {
		return SpeciesCompatible(this.species, move);
	}*/

	/*// <summary>
	/// Reduce the global clutter, and add to more readable 
	/// and maintainable code by encapsulation to logically 
	/// group classes that are only used in one place.
	/// </summary>
	internal class Moves
	{

	}*/
	#endregion

	#region Contest attributes, ribbons
	/// <summary>
	/// Returns whether this Pokémon has the specified ribbon.
	/// </summary>
	/// <param name="ribbon"></param>
	/// <returns></returns>
	public bool hasRibbon(Ribbon ribbon)
	{
		if (Ribbons.Count == 0) return false;
		return Ribbons.Contains(ribbon);
	}
	/// <summary>
	/// Gives this Pokémon the specified ribbon.
	/// </summary>
	/// <param name="ribbon"></param>
	public void giveRibbon(Ribbon ribbon)
	{
		if (ribbon <= 0) return;
		if (!Ribbons.Contains(ribbon)) this.ribbons.Add(ribbon);
	}
	/// <summary>
	/// Replaces one ribbon with the next one along, if possible.
	/// </summary>
	/// <param name="ribbon"></param>
	/// ToDo: Not finished here...
	public void upgradeRibbon(params Ribbon[] ribbon)//(Ribbon ribbon, Ribbon? upgradedRibbon = null)
	{
		//if(Ribbons.Count)
		//for(int i = 0; i < ribbon.Length)
	}
	/// <summary>
	/// Removes the specified ribbon from this Pokémon.
	/// </summary>
	/// <param name="ribbon"></param>
	public void takeRibbon(Ribbon ribbon)
	{
		if (ribbons.Count == 0) return;
		if (ribbon <= 0) return;
		for(int i = 0; i < ribbons.Count; i++)
		{
			if (Ribbons[i] == ribbon)
			{
				ribbons[i] = Ribbon.NONE;
				break;
			}
		}
		ribbons.Remove(Ribbon.NONE); //ToDo: List.RemoveAll(Ribbon == NONE)
	}
	/// <summary>
	/// Removes all ribbons from this Pokémon.
	/// </summary>
	public void clearAllRibbons()
	{
		ribbons.Clear();
	}
    #endregion

    #region Items
    #endregion

    #region Other
    /// <summary>
    /// Nickname; 
    /// Returns Pokemon species name if not nicknamed.
    /// </summary>
    public string Name { get { return name ?? _base.Name; } }

    public int Form { set { /*if(value <= _base.Forms)*/_base.Form = value; } }//ToDo: Fix Forms and uncomment

    /// <summary>
    /// Returns the species name of this Pokemon
    /// </summary>
    /*// <returns></returns>
    public string SpeciesName()
    {
        return _base.getName();
    }*/
    public string SpeciesName { get { return this._base.Species; } }

    /// <summary>
    /// Returns the markings this Pokemon has
    /// </summary>
    /// <returns>6 Markings</returns>
    /*public bool[] Markings()
    {
        return markings;
    }*/
    public bool[] Markings { get { return this.markings; } }

    /// <summary>
    /// Returns a string stathing the Unown form of this Pokemon
    /// </summary>
    /// <returns></returns>
    public char UnknownShape()
    {
        return "ABCDEFGHIJKLMNOPQRSTUVWXYZ?!".ToCharArray()[_base.ArrayId]; //ToDo: FormId; "if pokemon is an Unknown"
    }

    /// <summary>
    /// Returns the EV yield of this Pokemon
    /// </summary>
    /*// <returns></returns>
    public int[] evYield()
    {
        return EV;
    }*/
    public int[] evYield { get { return this.EV; } }//_base.getBaseStats();

    /// <summary>
    /// Sets this Pokemon's HP;
    /// Changes status on fainted
    /// </summary>
    /*// <param name="value"></param>
    public void HP(int value)
    {
        hp = value < 0 ? 0 : value;
        if (hp == 0) status = 0; // statusCount = 0; //ToDo: Fainted
    }*/
    public int HP
    {
        get { return this.hp; } //ToDo: If greater than totalHP throw error?
        set
        {
            this.hp = value < 0 ? 0 : value > this.TotalHP ? TotalHP : value;
            if (this.hp == 0) this.status = 0; // statusCount = 0; //ToDo: Fainted
        }
    }

    public bool isFainted()
    {
        return !this.isEgg //eggSteps == 0 //not an egg
            && this.HP <= 0;//hp <= 0;
    }

    /// <summary>
    /// Heals all HP of this Pokemon
    /// </summary>
    public void HealHP()
    {
        if (this.isEgg) return;     //ToDo: Throw exception error on returns
        this.HP = totalHP;          //ToDo: Return 'true' on success?
    }

    /// <summary>
    /// Heals the status problem of this Pokemon
    /// </summary>
    public void HealStatus()
    {
        if (this.isEgg) return;
        this.status = 0; statusCount = 0; //remove status ailments
    }

    /// <summary>
    /// Heals all PP of this Pokemon
    /// </summary>
    /// <param name="index"></param>
    public void HealPP(int index = -1)
    {
        if (this.isEgg) return;
        if (index >= 0) moves[index] = moves[index]; // ToDo: pp = totalpp
        else
        {
            for (int i = 0; i < 3; i++){
                moves[index] = moves[index]; // ToDo: pp = totalpp
            }
        }
    }

    /// <summary>
    /// Heals all HP, PP, and status problems of this Pokemon
    /// </summary>
    public void Heal()
    {
        if (this.isEgg) return;
        HealHP();
        HealStatus();
        HealPP();
    }

    /// <summary>
    /// Changes the happiness of this Pokemon depending on what happened to change it
    /// </summary>
    /// <param name="method"></param>
    public void ChangeHappiness(HappinessMethods method)
    {
        int gain = 0; bool luxury = false;
        switch (method)
        {
            case HappinessMethods.WALKING:
                gain = 1;
                gain += happiness < 200 ? 1 : 0;
                //gain += this.metMap.Id == currentMap.Id ? 1 : 0; //change to "obtainMap"?
                break;
            case HappinessMethods.LEVELUP:
                gain = 2;
                if (happiness < 200) gain = 3;
                if (happiness < 100) gain = 5;
                luxury = true;
                break;
            case HappinessMethods.GROOM:
                gain = 4;
                if (happiness < 200) gain = 10;
                luxury = true;
                break;
            case HappinessMethods.FAINT:
                gain = -1;
                break;
            case HappinessMethods.VITAMIN:
                gain = 2;
                if (happiness < 200) gain = 3;
                if (happiness < 100) gain = 5;
                break;
            case HappinessMethods.EVBERRY:
                gain = 2;
                if (happiness < 200) gain = 5;
                if (happiness < 100) gain = 10;
                break;
            case HappinessMethods.POWDER:
                gain = -10;
                if (happiness < 200) gain = -5;
                break;
            case HappinessMethods.ENERGYROOT:
                gain = -15;
                if (happiness < 200) gain = -10;
                break;
            case HappinessMethods.REVIVALHERB:
                gain = -20;
                if (happiness < 200) gain = -15;
                break;
            default:
                break;
        }
        gain += luxury && this.ballUsed == eItems.Item.LUXURY_BALL ? 1 : 0;
        if (this.item == eItems.Item.SOOTHE_BELL && gain > 0)
            gain = (int)Math.Floor(gain * 1.5f);
        happiness += gain;
        happiness = happiness > 255 ? 255 : happiness < 0 ? 0 : happiness; //Max 255, Min 0
    }
	#endregion

	#region Stat calculations, Pokemon creation
	/// <summary>
	/// Returns the maximum HP of this Pokémon.
	/// </summary>
	/// <param name="baseHP"></param>
	/// <param name="level"></param>
	/// <param name="iv"></param>
	/// <param name="ev"></param>
	/// <returns></returns>
	public int calcHP(int baseHP, int level, int iv, int ev)
	{
		if (baseHP == 1) return 1;
		return (int)Math.Floor((decimal)(baseHP * 2 + iv + (ev >> 2)) * level / 100) + level + 10;
	}
	/// <summary>
	/// Returns the specified stat of this Pokémon (not used for total HP).
	/// </summary>
	/// <param name="baseStat"></param>
	/// <param name="level"></param>
	/// <param name="iv"></param>
	/// <param name="ev"></param>
	/// <param name="pv"></param>
	/// <returns></returns>
	public int calcStat(int baseStat, int level, int iv, int ev, int pv)
	{
		return (int)Math.Floor((Math.Floor((decimal)(baseStat * 2 + iv + (ev >> 2)) * level / 100) + 5) * pv / 100);
	}
	public void calcStats()
	{
		int[] pvalues = new int[] { 100, 100, 100, 100, 100 };
		//Nature
	}
	#endregion

	#region Nested Classes
	public class PokemonData
	{
		#region Variables
		/// <summary>
		/// Id is the database value for specific pokemon+form
		/// Different Pokemon forms share the same Pokedex number. 
		/// Values are loaded from <see cref="Pokemons"/>, where each form is registered to an Id.
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		/// <remarks>If game event/gm wants to "give" player Deoxys-Power form and not Speed form</remarks>
		private Pokemons id;
		/// <summary>
		/// Different Gens assign different pokedex num
		/// </summary>
		/// <remarks>Think there is 3 pokedex</remarks>
		private int[] regionalPokedex;
		//private enum habitat; //ToDo: Grassland, Mountains...
		/// <summary>
		/// Name of the specific pokemon+form
		/// for given Id in database
		/// <para>Charizard is a Species. 
		/// But megaCharizard is a Form.
		/// </para>
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		private string name;
		/// <summary>
		/// Species is the pokemon breed/genus
		/// <para>Charizard is a Species. 
		/// But megaCharizard is a Form.
		/// </para>
		/// </summary>
		/// <example>
		/// Deoxys-Power, Speed, Defense
		/// are all part of the same species.
		/// </example>
		/// <remarks>Should be an int, not a string</remarks>
		private string species;
		/// <summary>
		/// </summary>
		private string pokedexEntry;
		/// ToDo: Maybe PokemonData contains count of # of forms?
		//private int forms; 
		private string[] forms = new string[0]; 
		/// <summary>
		/// Represents CURRENT form, if no form is active, current or does not exist
		/// then value is 0.
		/// </summary>
		/// ToDo: Make a PokemonForm class, that establishes the rule for 
		/// <see cref="Pokemons"/> and <see cref="Form"/>
		private int form;// = 0; 

		private Types type1;
		private Types type2;
		/// <summary>
		/// All three pokemon abilities 
		/// (Abiltiy1, Ability2, HiddenAbility).
		/// </summary>
		///	<remarks>
		/// Should be [int? a1,int? a2,int? a3]
		/// instead of above...
		/// </remarks> 
		private readonly eAbility.Ability[] abilities = new eAbility.Ability[3];

		/// <summary>
		/// The male ratio.
		/// </summary>
		/// instead of a float value...
		private GenderRatio maleRatio;
		/// <summary>max is 255</summary> 
		private int catchRate;
		private EggGroups eggGroup1;
		private EggGroups eggGroup2;
		private int hatchTime;

		//private float hitboxWidth; //used for 3d battles; just use collision detection from models
		private float height;
		private float weight;
		/// ToDo: Make this an enum
		private int shapeID; 

		private int baseExpYield;
		private LevelingRate levelingRate;

		private Color pokedexColor;
		/// <summary>
		/// Friendship levels is the same as pokemon Happiness.
		/// </summary>
		private int baseFriendship;

		private int baseStatsHP;
		private int baseStatsATK;
		private int baseStatsDEF;
		private int baseStatsSPA;
		private int baseStatsSPD;
		private int baseStatsSPE;

		private float luminance;
		private UnityEngine.Color lightColor;

		/// <summary>
		/// [item id,% chance]
		/// </summary>
		/// <example>int[,] heldItems = { {1,2,3},{4,5,6} }
		/// <para>heldItems[1,0] == 4 as int</para>
		/// </example>
		/// <remarks>
		/// Or maybe...[item id,% chance,generationId/regionId]
		/// Custom Class needed here... <see cref="eItems.Item"/> as itemId
		/// </remarks>
		private int[,] heldItem;

		private int[] movesetLevels;
		private Moves[] movesetMoves;
		private PokemonMoveset[] moveSet;

		//private string[] tmList; //Will be done thru ItemsClass

		private int[] evolutionID;
		/// ToDo: Evolution class type array here
		PokemonEvolution[] evolutions;
		private string[] evolutionRequirements;
		/// <summary>
		/// The item that needs to be held by a parent when breeding in order for the egg to be this species. 
		/// If neither parent is holding the required item, the egg will be the next evolved species instead.
		/// <para></para>
		/// The only species that should have this line are ones which cannot breed, 
		/// but evolve into a species which can. That is, the species should be a "baby" species.
		/// Not all baby species need this line.
		/// </summary>
		private eItems.Item Incense;
		#endregion

		/// ToDo: Should any of the property values be Set-able?
		#region Properties
		/// <summary>
		/// Id is the database value for specific pokemon+form
		/// Different Pokemon forms share the same Pokedex number. 
		/// Values are loaded from <see cref="Pokemons"/>, where each form is registered to an Id.
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		/// <remarks>If game event/gm wants to "give" player Deoxys-Power form and not Speed form</remarks>
		public Pokemons ID { get { return this.id; } }
		/// <summary>
		/// Different Gens assign different pokedex num
		/// example: Bulbasaur = [1,231]
		/// </summary>
		/// <remarks>Think there is 3 pokedex</remarks>
		public int[] RegionalPokedex { get { return this.regionalPokedex; } }
		/// <summary>
		/// Name of the specific pokemon+form
		/// for given Id in database
		/// <para>Charizard is a Species. 
		/// But megaCharizard is a Form.
		/// </para>
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		/// ToDo: Form = 0 should return null
		//public string Name { get { return PokemonData.GetPokedexTranslation(this.ID).Forms[this.Form] ?? this.name; } }
		public string Name { get
            {
                /*List<string> formvalues = new List<string>();
                foreach (var formValue in this.ID.ToString().Translate().FieldNames) {
                    //fieldnames.Add(field);
                    if (formValue.Key.Contains("form")){
                        //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                        //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                        formvalues.Add(formValue.Value);
                    }
                }
                return formvalues.ToArray()[this.Form] ?? this.name*/
                return this.forms[this.Form] ?? this.name; } }
		/// <summary>
		/// Species is the pokemon breed/genus
		/// <para>Charizard is a Species. 
		/// But megaCharizard is a Form.
		/// </para>
		/// </summary>
		/// <example>
		/// Deoxys-Power, Speed, Defense
		/// are all part of the same species.
		/// </example>
		/// <remarks>Should be an int, not a string</remarks>
		public string Species { get { return this.species; } }
		/// <summary>
		/// </summary>
		public string PokedexEntry { get { return this.pokedexEntry; } }
		/// <summary>
		/// Form is the same Pokemon Pokedex entry. 
		/// Changing forms should change name value
		/// </summary>
		/// but a different PokemonId
		/// If null, returns this.Pokemon.Id
		/// ToDo: Changing forms should change name value
		public int Form { get { return this.form; } set { this.form = value; } }
		/// ToDo: I should use the # of Forms from the .xml rather than from the database initializer/constructor
		public int Forms { get { return this.forms.Length; } }

		//public virtual Type Type1 { get { return this.type1; } }
		//public virtual Type Type2 { get { return this.type2; } }
		//Maybe use this instead?
		public virtual Types[] Type { get { return new PokemonData.Types[] { this.type1, this.type2 }; } }
		/// <summary>
		/// All three pokemon abilities 
		/// (Abiltiy1, Ability2, HiddenAbility).
		/// </summary>
		///	<remarks>
		/// Should be [int? a1,int? a2,int? a3]
		/// instead of above...
		/// </remarks> 
		public eAbility.Ability[] Abilities { get { return this.abilities; } }

		/// <summary>
		/// The male ratio.
		/// <value>-1f is interpreted as genderless</value>
		/// </summary>
		public GenderRatio MaleRatio { get { return this.maleRatio; } }
		public float ShinyChance { get; set; }
		/// <summary>
		/// The catch rate of the species. 
		/// Is a number between 0 and 255 inclusive. 
		/// The higher the number, the more likely a capture 
		/// (0 means it cannot be caught by anything except a Master Ball).
		/// </summary> 
		/// Also known as Pokemon's "Rareness"...
		public int CatchRate { get { return this.catchRate; } }
		public EggGroups[] EggGroup { get { return new EggGroups[] { this.eggGroup1, this.eggGroup2 }; } }
		//public EggGroup EggGroup2 { get { return this.eggGroup2; } }
		public int HatchTime { get { return this.hatchTime; } }

		//public float hitboxWidth { get { return this.hitboxWidth; } } //used for 3d battles just use collision detection from models
		public float Height { get { return this.height; } }
		public float Weight { get { return this.weight; } }
		/// ToDo: Make this an enum
		public int ShapeID { get { return this.shapeID; } } 

		public int BaseExpYield { get { return this.baseExpYield; } }
		public LevelingRate GrowthRate { get { return this.levelingRate; } }

		public Color PokedexColor { get { return this.pokedexColor; } }
		/// <summary>
		/// Friendship levels is the same as pokemon Happiness.
		/// </summary>
		public int BaseFriendship { get { return this.baseFriendship; } }

		public int BaseStatsHP { get { return this.baseStatsHP; } }
		public int BaseStatsATK { get { return this.baseStatsATK; } }
		public int BaseStatsDEF { get { return this.baseStatsDEF; } }
		public int BaseStatsSPA { get { return this.baseStatsSPA; } }
		public int BaseStatsSPD { get { return this.baseStatsSPD; } }
		public int BaseStatsSPE { get { return this.baseStatsSPE; } }

		public float Luminance { get { return this.luminance; } }
		public UnityEngine.Color LightColor { get { return this.lightColor; } }

		/// <summary>
		/// Returns the items this species can be found holding in the wild.
		/// [item id,% chance]
		/// </summary>
		/// <example>int[,] heldItems = { {1,2,3},{4,5,6} }
		/// <para>heldItems[1,0] == 4 as int</para>
		/// </example>
		/// <remarks>
		/// Or maybe...[item id,% chance,generationId/regionId]
		/// Custom Class needed here... <see cref="eItems.Item"/> as itemId
		/// </remarks>
		/// ToDo: Consider [itemcommon || 0,itemuncommon || 0,itemrare || 0]
		public int[,] HeldItem { get { return this.heldItem; } }

		public int[] MovesetLevels { get { return this.movesetLevels; } }
		public Moves[] MovesetMoves { get { return this.movesetMoves; } }
		public PokemonMoveset[] MoveSet { get { return this.moveSet; } }

		public int[] EvolutionID { get { return this.evolutionID; } }
		/// <summary>
		/// <example>
		/// E.G.	Poliwhirl(61)
		///		<code>new int[]{62,186},
		///		new string[]{"Stone,Water Stone","Trade\Item,King's Rock"}),</code>
		/// <para>
		/// E.G. to evolve to sylveon
		///		<code>new int[]{..., 700},
		///		new string[]{..., "Amie\Move,2\Fairy"}),</code>
		/// </para> 
		/// </example>
		/// <list type="bullet"> 
		/// <item>
		/// <term>Level,int level</term>
		///	<description>if pokemon's level is greater or equal to int level</description>
		///	<item>
		/// <term>Stone,string itemName</term>
		///	<description>if name of stone is equal to string itemName</description>
		///	</item>
		/// <item>
		/// <term>Trade</term>
		///	<description>if currently trading pokemon</description>
		///	</item>
		/// <item>
		/// <term>Friendship</term>
		///	<description>if pokemon's friendship is greater or equal to 220</description>
		///	</item>
		/// <item>
		/// <term>Item,string itemName</term>
		///	<description>if pokemon's heldItem is equal to string itemName</description>
		/// </item>
		///	<item>
		/// <term>Gender,Pokemon.Gender</term>
		/// <description>if pokemon's gender is equal to Pokemon.Gender</description>
		/// </item>
		///	<item>
		/// <term>Move,string moveName</term>
		///	<description>if pokemon has a move thats name or typing is equal to string moveName</description>
		/// </item>
		///	<item>
		///	<term>Map,string mapName</term>
		///	<description>if currentMap is equal to string mapName</description>
		/// </item>
		///	<item>
		///	<term>Time,string dayNight</term>
		///	<description>if time is between 9PM and 4AM time is "Night". else time is "Day".
		///	if time is equal to string dayNight (either Day, or Night)</description>
		/// </item>
		/// <listheader><term>Exceptions</term><description>
		///		Unique evolution methods:
		/// </description></listheader>
		///	<item>
		/// <term>Mantine</term>
		///	<description>if party contains a Remoraid</description>
		/// </item>
		///	<item>
		///	<term>Pangoro</term>
		///	<description>if party contains a dark pokemon</description>
		/// </item>
		///	<item>
		///	<term>Goodra</term>
		///	<description>if currentMap's weather is rain</description>
		/// </item>
		///	<item>
		///	<term>Hitmonlee</term>
		///	<description>if pokemon's ATK is greater than DEF</description>
		/// </item>
		///	<item>
		///	<term>Hitmonchan</term>
		///	<description>if pokemon's ATK is lower than DEF</description>
		/// </item>
		///	<item>
		///	<term>Hitmontop</term>
		/// <description>if pokemon's ATK is equal to DEF</description>
		/// </item>
		///	<item>
		///	<term>Silcoon</term>
		/// <description>if pokemon's shinyValue divided by 2's remainder is equal to 0</description>
		/// </item>
		///	<item>
		///	<term>Cascoon</term>
		///	<description>if pokemon's shinyValue divided by 2's remainder is equal to 1</description>
		/// </item>
		/// </list>
		/// </summary>
		public string[] EvolutionRequirements { get { return this.evolutionRequirements; } }
		#endregion

		#region Enumerators
		public enum Types
		{
			NONE = 0,
			NORMAL = 1,
			FIGHTING = 2,
			FLYING = 3,
			POISON = 4,
			GROUND = 5,
			ROCK = 6,
			BUG = 7,
			GHOST = 8,
			STEEL = 9,
			FIRE = 10,
			WATER = 11,
			GRASS = 12,
			ELECTRIC = 13,
			PSYCHIC = 14,
			ICE = 15,
			DRAGON = 16,
			DARK = 17,
			FAIRY = 18,
			UNKNOWN = 10001,
			SHADOW = 10002
		};
		public enum EggGroups
		{
			NONE = 0,
			MONSTER = 1,
			WATER1 = 2,
			BUG = 3,
			FLYING = 4,
			FIELD = 5, //"Ground"?
			FAIRY = 6,
			GRASS = 7, //"Plant"
			HUMANLIKE = 8, //"humanshape"
			WATER3 = 9,
			MINERAL = 10,
			AMORPHOUS = 11, //"indeterminate"
			WATER2 = 12,
			DITTO = 13,
			DRAGON = 14,
			UNDISCOVERED = 15 //"no-eggs"
		};
		/// <summary>
		/// The likelihood of a Pokémon of the species being a certain gender.
		/// </summary>
		public enum GenderRatio
		{
			AlwaysMale,
			FemaleOneEighth,
			Female25Percent,
			Female50Percent,
			Female75Percent,
			FemaleSevenEighths,
			AlwaysFemale,
			Genderless
		}
		public enum LevelingRate
		{
			ERRATIC = 6, //fast then very slow?
			FAST = 3,
			MEDIUMFAST = 2, //Medium?
			MEDIUMSLOW = 4,
			SLOW = 1,
			FLUCTUATING = 5 //slow then fast?
		};
		public enum Color
		{
			RED = 8,
			BLUE = 2,
			YELLOW = 10,
			GREEN = 5,
			BLACK = 1,
			BROWN = 3,
			PURPLE = 7,
			GRAY = 4,
			WHITE = 9,
			PINK = 6,
			NONE = 0
		};
		/// <summary>
		/// Pokemon ids are connected to XML file.
		/// </summary>
		/// <remarks>Can now code with strings or int and
		/// access the same value.</remarks>
		public enum Pokemons
		{
			NONE = 0,
            bulbasaur = 1,
            ivysaur = 2,
            venusaur = 3,
            charmander = 4,
            charmeleon = 5,
            charizard = 6,
            squirtle = 7,
            wartortle = 8,
            blastoise = 9,
            caterpie = 10,
            metapod = 11,
            butterfree = 12,
            weedle = 13,
            kakuna = 14,
            beedrill = 15,
            pidgey = 16,
            pidgeotto = 17,
            pidgeot = 18,
            rattata = 19,
            raticate = 20,
            spearow = 21,
            fearow = 22,
            ekans = 23,
            arbok = 24,
            pikachu = 25,
            raichu = 26,
            sandshrew = 27,
            sandslash = 28,
            /// <summary>
            /// NidoranFemale
            /// </summary>
            nidoran_f = 29,
            nidorina = 30,
            nidoqueen = 31,
            /// <summary>
            /// NidoranMale
            /// </summary>
            nidoran_m = 32,
            nidorino = 33,
            nidoking = 34,
            clefairy = 35,
            clefable = 36,
            vulpix = 37,
            ninetales = 38,
            jigglypuff = 39,
            wigglytuff = 40,
            zubat = 41,
            golbat = 42,
            oddish = 43,
            gloom = 44,
            vileplume = 45,
            paras = 46,
            parasect = 47,
            venonat = 48,
            venomoth = 49,
            diglett = 50,
            dugtrio = 51,
            meowth = 52,
            persian = 53,
            psyduck = 54,
            golduck = 55,
            mankey = 56,
            primeape = 57,
            growlithe = 58,
            arcanine = 59,
            poliwag = 60,
            poliwhirl = 61,
            poliwrath = 62,
            abra = 63,
            kadabra = 64,
            alakazam = 65,
            machop = 66,
            machoke = 67,
            machamp = 68,
            bellsprout = 69,
            weepinbell = 70,
            victreebel = 71,
            tentacool = 72,
            tentacruel = 73,
            geodude = 74,
            graveler = 75,
            golem = 76,
            ponyta = 77,
            rapidash = 78,
            slowpoke = 79,
            slowbro = 80,
            magnemite = 81,
            magneton = 82,
            farfetchd = 83,
            doduo = 84,
            dodrio = 85,
            seel = 86,
            dewgong = 87,
            grimer = 88,
            muk = 89,
            shellder = 90,
            cloyster = 91,
            gastly = 92,
            haunter = 93,
            gengar = 94,
            onix = 95,
            drowzee = 96,
            hypno = 97,
            krabby = 98,
            kingler = 99,
            voltorb = 100,
            electrode = 101,
            exeggcute = 102,
            exeggutor = 103,
            cubone = 104,
            marowak = 105,
            hitmonlee = 106,
            hitmonchan = 107,
            lickitung = 108,
            koffing = 109,
            weezing = 110,
            rhyhorn = 111,
            rhydon = 112,
            chansey = 113,
            tangela = 114,
            kangaskhan = 115,
            horsea = 116,
            seadra = 117,
            goldeen = 118,
            seaking = 119,
            staryu = 120,
            starmie = 121,
            mr_mime = 122,
            scyther = 123,
            jynx = 124,
            electabuzz = 125,
            magmar = 126,
            pinsir = 127,
            tauros = 128,
            magikarp = 129,
            gyarados = 130,
            lapras = 131,
            ditto = 132,
            eevee = 133,
            vaporeon = 134,
            jolteon = 135,
            flareon = 136,
            porygon = 137,
            omanyte = 138,
            omastar = 139,
            kabuto = 140,
            kabutops = 141,
            aerodactyl = 142,
            snorlax = 143,
            articuno = 144,
            zapdos = 145,
            moltres = 146,
            dratini = 147,
            dragonair = 148,
            dragonite = 149,
            mewtwo = 150,
            mew = 151,
            chikorita = 152,
            bayleef = 153,
            meganium = 154,
            cyndaquil = 155,
            quilava = 156,
            typhlosion = 157,
            totodile = 158,
            croconaw = 159,
            feraligatr = 160,
            sentret = 161,
            furret = 162,
            hoothoot = 163,
            noctowl = 164,
            ledyba = 165,
            ledian = 166,
            spinarak = 167,
            ariados = 168,
            crobat = 169,
            chinchou = 170,
            lanturn = 171,
            pichu = 172,
            cleffa = 173,
            igglybuff = 174,
            togepi = 175,
            togetic = 176,
            natu = 177,
            xatu = 178,
            mareep = 179,
            flaaffy = 180,
            ampharos = 181,
            bellossom = 182,
            marill = 183,
            azumarill = 184,
            sudowoodo = 185,
            politoed = 186,
            hoppip = 187,
            skiploom = 188,
            jumpluff = 189,
            aipom = 190,
            sunkern = 191,
            sunflora = 192,
            yanma = 193,
            wooper = 194,
            quagsire = 195,
            espeon = 196,
            umbreon = 197,
            murkrow = 198,
            slowking = 199,
            misdreavus = 200,
            unown = 201,
            wobbuffet = 202,
            girafarig = 203,
            pineco = 204,
            forretress = 205,
            dunsparce = 206,
            gligar = 207,
            steelix = 208,
            snubbull = 209,
            granbull = 210,
            qwilfish = 211,
            scizor = 212,
            shuckle = 213,
            heracross = 214,
            sneasel = 215,
            teddiursa = 216,
            ursaring = 217,
            slugma = 218,
            magcargo = 219,
            swinub = 220,
            piloswine = 221,
            corsola = 222,
            remoraid = 223,
            octillery = 224,
            delibird = 225,
            mantine = 226,
            skarmory = 227,
            houndour = 228,
            houndoom = 229,
            kingdra = 230,
            phanpy = 231,
            donphan = 232,
            porygon2 = 233,
            stantler = 234,
            smeargle = 235,
            tyrogue = 236,
            hitmontop = 237,
            smoochum = 238,
            elekid = 239,
            magby = 240,
            miltank = 241,
            blissey = 242,
            raikou = 243,
            entei = 244,
            suicune = 245,
            larvitar = 246,
            pupitar = 247,
            tyranitar = 248,
            lugia = 249,
            ho_oh = 250,
            celebi = 251,
            treecko = 252,
            grovyle = 253,
            sceptile = 254,
            torchic = 255,
            combusken = 256,
            blaziken = 257,
            mudkip = 258,
            marshtomp = 259,
            swampert = 260,
            poochyena = 261,
            mightyena = 262,
            zigzagoon = 263,
            linoone = 264,
            wurmple = 265,
            silcoon = 266,
            beautifly = 267,
            cascoon = 268,
            dustox = 269,
            lotad = 270,
            lombre = 271,
            ludicolo = 272,
            seedot = 273,
            nuzleaf = 274,
            shiftry = 275,
            taillow = 276,
            swellow = 277,
            wingull = 278,
            pelipper = 279,
            ralts = 280,
            kirlia = 281,
            gardevoir = 282,
            surskit = 283,
            masquerain = 284,
            shroomish = 285,
            breloom = 286,
            slakoth = 287,
            vigoroth = 288,
            slaking = 289,
            nincada = 290,
            ninjask = 291,
            shedinja = 292,
            whismur = 293,
            loudred = 294,
            exploud = 295,
            makuhita = 296,
            hariyama = 297,
            azurill = 298,
            nosepass = 299,
            skitty = 300,
            delcatty = 301,
            sableye = 302,
            mawile = 303,
            aron = 304,
            lairon = 305,
            aggron = 306,
            meditite = 307,
            medicham = 308,
            electrike = 309,
            manectric = 310,
            plusle = 311,
            minun = 312,
            volbeat = 313,
            illumise = 314,
            roselia = 315,
            gulpin = 316,
            swalot = 317,
            carvanha = 318,
            sharpedo = 319,
            wailmer = 320,
            wailord = 321,
            numel = 322,
            camerupt = 323,
            torkoal = 324,
            spoink = 325,
            grumpig = 326,
            spinda = 327,
            trapinch = 328,
            vibrava = 329,
            flygon = 330,
            cacnea = 331,
            cacturne = 332,
            swablu = 333,
            altaria = 334,
            zangoose = 335,
            seviper = 336,
            lunatone = 337,
            solrock = 338,
            barboach = 339,
            whiscash = 340,
            corphish = 341,
            crawdaunt = 342,
            baltoy = 343,
            claydol = 344,
            lileep = 345,
            cradily = 346,
            anorith = 347,
            armaldo = 348,
            feebas = 349,
            milotic = 350,
            castform = 351,
            kecleon = 352,
            shuppet = 353,
            banette = 354,
            duskull = 355,
            dusclops = 356,
            tropius = 357,
            chimecho = 358,
            absol = 359,
            wynaut = 360,
            snorunt = 361,
            glalie = 362,
            spheal = 363,
            sealeo = 364,
            walrein = 365,
            clamperl = 366,
            huntail = 367,
            gorebyss = 368,
            relicanth = 369,
            luvdisc = 370,
            bagon = 371,
            shelgon = 372,
            salamence = 373,
            beldum = 374,
            metang = 375,
            metagross = 376,
            regirock = 377,
            regice = 378,
            registeel = 379,
            latias = 380,
            latios = 381,
            kyogre = 382,
            groudon = 383,
            rayquaza = 384,
            jirachi = 385,
            deoxys = 386,
            turtwig = 387,
            grotle = 388,
            torterra = 389,
            chimchar = 390,
            monferno = 391,
            infernape = 392,
            piplup = 393,
            prinplup = 394,
            empoleon = 395,
            starly = 396,
            staravia = 397,
            staraptor = 398,
            bidoof = 399,
            bibarel = 400,
            kricketot = 401,
            kricketune = 402,
            shinx = 403,
            luxio = 404,
            luxray = 405,
            budew = 406,
            roserade = 407,
            cranidos = 408,
            rampardos = 409,
            shieldon = 410,
            bastiodon = 411,
            burmy = 412,
            wormadam = 413,
            mothim = 414,
            combee = 415,
            vespiquen = 416,
            pachirisu = 417,
            buizel = 418,
            floatzel = 419,
            cherubi = 420,
            cherrim = 421,
            shellos = 422,
            gastrodon = 423,
            ambipom = 424,
            drifloon = 425,
            drifblim = 426,
            buneary = 427,
            lopunny = 428,
            mismagius = 429,
            honchkrow = 430,
            glameow = 431,
            purugly = 432,
            chingling = 433,
            stunky = 434,
            skuntank = 435,
            bronzor = 436,
            bronzong = 437,
            bonsly = 438,
            mime_jr = 439,
            happiny = 440,
            chatot = 441,
            spiritomb = 442,
            gible = 443,
            gabite = 444,
            garchomp = 445,
            munchlax = 446,
            riolu = 447,
            lucario = 448,
            hippopotas = 449,
            hippowdon = 450,
            skorupi = 451,
            drapion = 452,
            croagunk = 453,
            toxicroak = 454,
            carnivine = 455,
            finneon = 456,
            lumineon = 457,
            mantyke = 458,
            snover = 459,
            abomasnow = 460,
            weavile = 461,
            magnezone = 462,
            lickilicky = 463,
            rhyperior = 464,
            tangrowth = 465,
            electivire = 466,
            magmortar = 467,
            togekiss = 468,
            yanmega = 469,
            leafeon = 470,
            glaceon = 471,
            gliscor = 472,
            mamoswine = 473,
            porygon_z = 474,
            gallade = 475,
            probopass = 476,
            dusknoir = 477,
            froslass = 478,
            rotom = 479,
            uxie = 480,
            mesprit = 481,
            azelf = 482,
            dialga = 483,
            palkia = 484,
            heatran = 485,
            regigigas = 486,
            giratina = 487,
            cresselia = 488,
            phione = 489,
            manaphy = 490,
            darkrai = 491,
            shaymin = 492,
            arceus = 493,
            victini = 494,
            snivy = 495,
            servine = 496,
            serperior = 497,
            tepig = 498,
            pignite = 499,
            emboar = 500,
            oshawott = 501,
            dewott = 502,
            samurott = 503,
            patrat = 504,
            watchog = 505,
            lillipup = 506,
            herdier = 507,
            stoutland = 508,
            purrloin = 509,
            liepard = 510,
            pansage = 511,
            simisage = 512,
            pansear = 513,
            simisear = 514,
            panpour = 515,
            simipour = 516,
            munna = 517,
            musharna = 518,
            pidove = 519,
            tranquill = 520,
            unfezant = 521,
            blitzle = 522,
            zebstrika = 523,
            roggenrola = 524,
            boldore = 525,
            gigalith = 526,
            woobat = 527,
            swoobat = 528,
            drilbur = 529,
            excadrill = 530,
            audino = 531,
            timburr = 532,
            gurdurr = 533,
            conkeldurr = 534,
            tympole = 535,
            palpitoad = 536,
            seismitoad = 537,
            throh = 538,
            sawk = 539,
            sewaddle = 540,
            swadloon = 541,
            leavanny = 542,
            venipede = 543,
            whirlipede = 544,
            scolipede = 545,
            cottonee = 546,
            whimsicott = 547,
            petilil = 548,
            lilligant = 549,
            basculin = 550,
            sandile = 551,
            krokorok = 552,
            krookodile = 553,
            darumaka = 554,
            darmanitan = 555,
            maractus = 556,
            dwebble = 557,
            crustle = 558,
            scraggy = 559,
            scrafty = 560,
            sigilyph = 561,
            yamask = 562,
            cofagrigus = 563,
            tirtouga = 564,
            carracosta = 565,
            archen = 566,
            archeops = 567,
            trubbish = 568,
            garbodor = 569,
            zorua = 570,
            zoroark = 571,
            minccino = 572,
            cinccino = 573,
            gothita = 574,
            gothorita = 575,
            gothitelle = 576,
            solosis = 577,
            duosion = 578,
            reuniclus = 579,
            ducklett = 580,
            swanna = 581,
            vanillite = 582,
            vanillish = 583,
            vanilluxe = 584,
            deerling = 585,
            sawsbuck = 586,
            emolga = 587,
            karrablast = 588,
            escavalier = 589,
            foongus = 590,
            amoonguss = 591,
            frillish = 592,
            jellicent = 593,
            alomomola = 594,
            joltik = 595,
            galvantula = 596,
            ferroseed = 597,
            ferrothorn = 598,
            klink = 599,
            klang = 600,
            klinklang = 601,
            tynamo = 602,
            eelektrik = 603,
            eelektross = 604,
            elgyem = 605,
            beheeyem = 606,
            litwick = 607,
            lampent = 608,
            chandelure = 609,
            axew = 610,
            fraxure = 611,
            haxorus = 612,
            cubchoo = 613,
            beartic = 614,
            cryogonal = 615,
            shelmet = 616,
            accelgor = 617,
            stunfisk = 618,
            mienfoo = 619,
            mienshao = 620,
            druddigon = 621,
            golett = 622,
            golurk = 623,
            pawniard = 624,
            bisharp = 625,
            bouffalant = 626,
            rufflet = 627,
            braviary = 628,
            vullaby = 629,
            mandibuzz = 630,
            heatmor = 631,
            durant = 632,
            deino = 633,
            zweilous = 634,
            hydreigon = 635,
            larvesta = 636,
            volcarona = 637,
            cobalion = 638,
            terrakion = 639,
            virizion = 640,
            tornadus = 641,
            thundurus = 642,
            reshiram = 643,
            zekrom = 644,
            landorus = 645,
            kyurem = 646,
            keldeo = 647,
            meloetta = 648,
            genesect = 649,
            chespin = 650,
            quilladin = 651,
            chesnaught = 652,
            fennekin = 653,
            braixen = 654,
            delphox = 655,
            froakie = 656,
            frogadier = 657,
            greninja = 658,
            bunnelby = 659,
            diggersby = 660,
            fletchling = 661,
            fletchinder = 662,
            talonflame = 663,
            scatterbug = 664,
            spewpa = 665,
            vivillon = 666,
            litleo = 667,
            pyroar = 668,
            flabebe = 669,
            floette = 670,
            florges = 671,
            skiddo = 672,
            gogoat = 673,
            pancham = 674,
            pangoro = 675,
            furfrou = 676,
            espurr = 677,
            meowstic = 678,
            honedge = 679,
            doublade = 680,
            aegislash = 681,
            spritzee = 682,
            aromatisse = 683,
            swirlix = 684,
            slurpuff = 685,
            inkay = 686,
            malamar = 687,
            binacle = 688,
            barbaracle = 689,
            skrelp = 690,
            dragalge = 691,
            clauncher = 692,
            clawitzer = 693,
            helioptile = 694,
            heliolisk = 695,
            tyrunt = 696,
            tyrantrum = 697,
            amaura = 698,
            aurorus = 699,
            sylveon = 700,
            hawlucha = 701,
            dedenne = 702,
            carbink = 703,
            goomy = 704,
            sliggoo = 705,
            goodra = 706,
            klefki = 707,
            phantump = 708,
            trevenant = 709,
            pumpkaboo = 710,
            gourgeist = 711,
            bergmite = 712,
            avalugg = 713,
            noibat = 714,
            noivern = 715,
            xerneas = 716,
            yveltal = 717,
            zygarde = 718,
            diancie = 719,
            hoopa = 720,
            volcanion = 721
        }
		#endregion

		#region Constructors
		public PokemonData() { }// this.name = PokemonData.GetPokedexTranslation(this.ID).Forms[this.Form] ?? this.Name; } //name equals form name unless there is none.
		public PokemonData(Pokemons Id) : this()
        {
            //PokedexTranslation translation = PokemonData.GetPokedexTranslation(Id);
            var translation = Id.ToString().Translate();
            this.id = Id;
            //this.name = translation.Name;
            //this.species = translation.Species;
            //this.pokedexEntry = translation.PokedexEntry;
            this.pokedexEntry = translation.Value;
            //this.forms = forms; //| new Pokemon[] { Id }; //ToDo: need new mechanic for how this should work
            List<string> formvalues = new List<string>();
            foreach (var fieldValue in translation.FieldNames)
            {
                //fieldnames.Add(field);
                if (fieldValue.Key.Contains("form"))
                {
                    //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                    //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                    formvalues.Add(fieldValue.Value);
                }
                if (fieldValue.Key.Contains("genus"))
                {
                    this.species = fieldValue.Value;
                }
                if (fieldValue.Key.Contains("name"))
                {
                    this.name = fieldValue.Value ?? translation.Identifier;
                }
            }
            this.forms = formvalues.ToArray();
        }

		public PokemonData(Pokemons Id, int[] regionalDex/*, string name*/, Types? type1, Types? type2, eAbility.Ability[] abilities, //eAbility.Ability? ability1, eAbility.Ability? ability2, eAbility.Ability? hiddenAbility,
							GenderRatio maleRatio, int catchRate, EggGroups eggGroup1, EggGroups eggGroup2, int hatchTime,
							float height, float weight, int baseExpYield, LevelingRate levelingRate,
							/*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
							Color pokedexColor, int baseFriendship,//* / string species, string pokedexEntry,*/
							int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
							float luminance, /*Color lightColor,*/ int[] movesetLevels, Moves[] movesetMoves, int[] tmList,
							int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,*/ int forms,
							int[,] heldItem = null) : this (Id)
        {//new PokemonData(1,1,"Bulbasaur",12,4,65,null,34,45,1,7,20,7f,69f,64,4,PokemonData.PokedexColor.GREEN,"Seed","\"Bulbasaur can be seen napping in bright sunlight. There is a seed on its back. By soaking up the sun’s rays, the seed grows progressively larger.\"",45,49,49,65,65,45,0f,new int[]{1,3,7,9,13,13,15,19,21,25,27,31,33,37},new int[]{33,45,73,22,77,79,36,75,230,74,38,388,235,402},new int[]{14,15,70,76,92,104,113,148,156,164,182,188,207,213,214,216,218,219,237,241,249,263,267,290,412,447,474,496,497,590},new int[]{2},new int[]{16},new int[]{1})
            this.regionalPokedex = regionalDex;

            this.type1 = type1 != null ? (PokemonData.Types)type1 : PokemonData.Types.NONE;
			this.type2 = type2 != null ? (PokemonData.Types)type2 : PokemonData.Types.NONE;
			this.abilities = abilities;
			//this.ability1Id = (eAbility.Ability)ability1;
			//this.ability2Id = (eAbility.Ability)ability2;
			//this.hiddenAbilityId = (eAbility.Ability)hiddenAbility;

			this.maleRatio = maleRatio; //ToDo
			this.catchRate = catchRate;
			this.eggGroup1 = eggGroup1;
			this.eggGroup2 = eggGroup2;
			this.hatchTime = hatchTime;

			this.height = height;
			this.weight = weight;
			this.baseExpYield = baseExpYield;
			this.levelingRate = (LevelingRate)levelingRate; //== null ? (PokemonData.LevelingRate)levelingRate : PokemonData.LevelingRate.NONE;

			this.baseStatsHP = baseStatsHP;
			this.baseStatsATK = baseStatsATK;
			this.baseStatsDEF = baseStatsDEF;
			this.baseStatsSPA = baseStatsSPA;
			this.baseStatsSPD = baseStatsSPD;
			this.baseStatsSPE = baseStatsSPE;
			this.baseFriendship = baseFriendship; //ToDo: forgot to implement when transfering database

			this.luminance = luminance;
			//this.lightColor = lightColor;
			this.pokedexColor = pokedexColor | PokemonData.Color.NONE;

			//ToDo: wild pokemon held items not yet implemented
			this.heldItem = heldItem; //[item id,% chance]

			this.movesetLevels = movesetLevels;
			this.movesetMoves = movesetMoves; //ToDo: Array Cast conversion
            //this.tmList = tmList; //ToDo: Need new item database array/enum for this; one that's regional/generation dependant

			this.evolutionID = evolutionID;
			//this.evolutionMethod = evolutionMethod; //ToDo:
			//this.evolutionRequirements = evolutionRequirements;
		}

        [Obsolete]
		public static PokemonData CreatePokemonData(Pokemons Id, int[] PokeId/*, string name*/, int? type1, int? type2, int? ability1, int? ability2, int? hiddenAbility,
							float maleRatio, int catchRate, int? eggGroup1, int? eggGroup2, int hatchTime,
							float height, float weight, int baseExpYield, int levelingRate,
							/*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
							Color pokedexColor, int baseFriendship,//*/ string species, string pokedexEntry,
							int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
							float luminance, /*Color lightColor,*/ int[] movesetLevels, int[] movesetMoves, int[] tmList,
							int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,*/ int forms,
							int[,] heldItem = null)
		{
			return CreatePokemonData(//new PokemonData(
				(Pokemons)Id,
				PokeId,
				(PokemonData.Types)type1 | PokemonData.Types.NONE,//!= null ? (PokemonData.Type)type1 : PokemonData.Type.NONE,
				(PokemonData.Types)type2 | PokemonData.Types.NONE,//!= null ? (PokemonData.Type)type2 : PokemonData.Type.NONE,
                //new eAbility.Ability[] { 
					(eAbility.Ability)ability1 | eAbility.Ability.NONE,//!= null ? (eAbility.Ability)ability1 : eAbility.Ability.NONE,
					(eAbility.Ability)ability2 | eAbility.Ability.NONE,//!= null ? (eAbility.Ability)ability2 : eAbility.Ability.NONE,
					(eAbility.Ability)hiddenAbility | eAbility.Ability.NONE,//!= null ? (eAbility.Ability)hiddenAbility : eAbility.Ability.NONE
                //}, 
				0,//ToDo: maleRatio, 
				catchRate,
				(EggGroups)eggGroup1 | PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup1 : PokemonData.EggGroup.NONE, 
				(EggGroups)eggGroup2 | PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup2 : PokemonData.EggGroup.NONE, 
				hatchTime,
				height,
				weight,
				baseExpYield,
				(LevelingRate)levelingRate,
				pokedexColor | PokemonData.Color.NONE,
				baseFriendship, baseStatsHP, baseStatsATK, baseStatsDEF, baseStatsSPA, baseStatsSPD, baseStatsSPE,
				luminance, movesetLevels, System.Array.ConvertAll(movesetMoves, move => (Moves)move), tmList,
				evolutionID, evolutionLevel, evolutionMethod, forms, heldItem);//
		}

		public static PokemonData CreatePokemonData(Pokemons Id, int[] PokeId/*, string name*/, Types type1, Types type2, eAbility.Ability ability1, eAbility.Ability ability2, eAbility.Ability hiddenAbility,
							GenderRatio maleRatio, int catchRate, EggGroups eggGroup1, EggGroups eggGroup2, int hatchTime,
							float height, float weight, int baseExpYield, LevelingRate levelingRate,
							/*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
							Color pokedexColor, int baseFriendship,//*/ string species, string pokedexEntry,
							int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
							float luminance, /*Color lightColor,*/ int[] movesetLevels, Moves[] movesetMoves, int[] tmList,
							int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,*/ int forms,
							int[,] heldItem = null)
		{
			return new PokemonData(
				Id,
				PokeId,
				type1, //| PokemonData.Type.NONE,//!= null ? (PokemonData.Type)type1 : PokemonData.Type.NONE,
				type2, //| PokemonData.Type.NONE,//!= null ? (PokemonData.Type)type2 : PokemonData.Type.NONE,
				new eAbility.Ability[] {
				ability1, //| eAbility.Ability.NONE,//!= null ? (eAbility.Ability)ability1 : eAbility.Ability.NONE,
                ability2, //| eAbility.Ability.NONE,//!= null ? (eAbility.Ability)ability2 : eAbility.Ability.NONE,
                hiddenAbility, //| eAbility.Ability.NONE,//!= null ? (eAbility.Ability)hiddenAbility : eAbility.Ability.NONE
				},
				maleRatio,
				catchRate,
				eggGroup1, //| PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup1 : PokemonData.EggGroup.NONE, 
				eggGroup2, //| PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup2 : PokemonData.EggGroup.NONE, 
				hatchTime,
				height,
				weight,
				baseExpYield,
				levelingRate,
				pokedexColor | PokemonData.Color.NONE,
				baseFriendship, baseStatsHP, baseStatsATK, baseStatsDEF, baseStatsSPA, baseStatsSPD, baseStatsSPE,
				luminance, movesetLevels, movesetMoves, /*System.Array.ConvertAll(movesetMoves, move => (Move.MoveData.Move)move),*/ tmList,
				evolutionID, evolutionLevel, evolutionMethod, forms, heldItem);//
		}

        /// Not const because translation values
        public static readonly PokemonData[] Database = new PokemonData[] {
            //null
            //  PokemonData.CreatePokemonData(ID, NAME, PokemonData.Type.TYPE1, PokemonData.Type.TYPE2, Ability1, Ability2, HiddenAbility,
            //				MaleRatio, CatchRate, PokemonData.EggGroup.EGGGROUP1, PokemonData.EggGroup.EGGGROUP2, HatchTime, Height, Weight,
            //				EXPYield, PokemonData.LevelingRate.LEVELINGRATE, evYieldHP,ATK,DEF,SPA,SPD,SPE, PokemonData.PokedexColor.COLOR, BaseFriendship,
            //				Species, PokedexEntry (choose your favourite) //needs to be loaded seperately...
            //				baseStatsHP,ATK,DEF,SPA,SPD,SPE, Luminance (0 if unknown), LightColor (Color.clear if unknown)
            //				new int[]{ level, level, level, etc...}
            //				new string[]{ "move", "move", "move", etc...} ), //needs to be loaded separately...
            //				new int[]{pokemonID}, 
            //				new string[]{"Method,Parameter"}),
            new PokemonData( Id: Pokemons.NONE, regionalDex: new int[1], type1: Types.NONE, type2: Types.NONE, abilities: new eAbility.Ability[] { eAbility.Ability.NONE, eAbility.Ability.NONE, eAbility.Ability.NONE },
                        maleRatio: GenderRatio.AlwaysMale /*0f*/, catchRate: 100, eggGroup1: EggGroups.NONE, eggGroup2: EggGroups.NONE, hatchTime: 1000,
                        height: 10f, weight: 150f, baseExpYield: 15, levelingRate: LevelingRate.ERRATIC,
                        /*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
                        pokedexColor: Color.NONE, baseFriendship: 50,
                        baseStatsHP: 10, baseStatsATK: 5, baseStatsDEF: 5, baseStatsSPA: 5, baseStatsSPD: 5, baseStatsSPE: 5,
                        luminance: 0f, movesetLevels: new int[] { 1,2,3 }, movesetMoves: new Moves[4], tmList: null,
                        evolutionID: null, evolutionLevel: null, evolutionMethod: null, forms: 4,
                        heldItem: null), //Test
        };
        /*static PokemonData()
		{
			Database = new PokemonData[] {
				//null
				//  PokemonData.CreatePokemonData(ID, NAME, PokemonData.Type.TYPE1, PokemonData.Type.TYPE2, Ability1, Ability2, HiddenAbility,
				//				MaleRatio, CatchRate, PokemonData.EggGroup.EGGGROUP1, PokemonData.EggGroup.EGGGROUP2, HatchTime, Height, Weight,
				//				EXPYield, PokemonData.LevelingRate.LEVELINGRATE, evYieldHP,ATK,DEF,SPA,SPD,SPE, PokemonData.PokedexColor.COLOR, BaseFriendship,
				//				Species, PokedexEntry (choose your favourite) //needs to be loaded seperately...
				//				baseStatsHP,ATK,DEF,SPA,SPD,SPE, Luminance (0 if unknown), LightColor (Color.clear if unknown)
				//				new int[]{ level, level, level, etc...}
				//				new string[]{ "move", "move", "move", etc...} ), //needs to be loaded separately...
				//				new int[]{pokemonID}, 
				//				new string[]{"Method,Parameter"}),
				PokemonData.CreatePokemonData(Pokemon.NONE, new int[1], Type.NONE, Type.NONE, eAbility.Ability.NONE, eAbility.Ability.NONE, eAbility.Ability.NONE,
							0f, 100, EggGroups.NONE, EggGroups.NONE, 1000,
							10f, 150f, 15, LevelingRate.ERRATIC,
							//*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,* /
							Color.NONE, 50,
							10, 5, 5, 5, 5, 5,
							0f, new int[] { 1,2,3 }, new Move.MoveData.Move[4], null,//int[] tmList,
							null, null, null, 4,//int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, //int forms, 
							null) //Test
			};
		}*/

        #region Obsolete Translation Method
#if DEBUG
        [Obsolete]
		private static Dictionary<int, PokedexTranslation> _pokeTranslations;// = LoadPokedexTranslations();
#else
        [Obsolete]
        private static Dictionary<int, PokedexTranslation> _pokeTranslations;// = LoadPokedexTranslations(SaveData.currentSave.playerLanguage | Settings.Language.English);
#endif
		/// <summary>
		/// 
		/// </summary>
        [Obsolete]
		private static Dictionary<int, PokedexTranslation> _pokeEnglishTranslations;// = LoadEnglishPokedexTranslations();
		/// <summary>
		/// 
		/// </summary>
		///ToDo: Should be a void that stores value to _pokeTranslations instead of returning...
        [Obsolete]
		public static void/*Dictionary<int, PokedexTranslation>*/ LoadPokedexTranslations(Settings.Languages language = Settings.Languages.English)//, int form = 0
		{
			var data = new Dictionary<int, PokedexTranslation>();

			string fileLanguage;
			switch (language)
			{
				case Settings.Languages.English:
					fileLanguage = "en-us";
					break;
				default: //Default in case new language is added to game but not programmed ahead of time here...
					fileLanguage = "en-us";
					break;
			}
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument(); // xmlDoc is the new xml document.
			//ToDo: Consider "/Resources/Database/PokemonTranslations/Pokemon_"
#if DEBUG
			string file = @"..\..\..\\Pokemon Unity\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
        string file = UnityEngine.Application.dataPath + "/Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
#endif
			System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open);
			xmlDoc.Load(fs);

			if (xmlDoc.HasChildNodes)
			{
				foreach (System.Xml.XmlNode node in xmlDoc.GetElementsByTagName("Pokemon"))
				{
					var translation = new PokedexTranslation();
					translation.Name = node.Attributes["name"].Value; //ToDo: Name = "name" Where formId == 0; else name = formId.name
					//ToDo: Maybe add a forms array, and a new method for single name calls
					translation.Forms = new string[node.Attributes.Count - 3]; int n = 1;//only count forms?
					//int n = 1;translation.Forms[0] = node.Attributes["name"].Value;//that or return an empty array T[0]
					for (int i = 4; i < node.Attributes.Count; i++)//foreach(System.Xml.XmlAttribute attr in node)
					{
						//Skipping first 4 values will save processing
						if (node.Attributes[i].LocalName.Contains("form")) //Name vs LocalName?
						{
							//translation.Forms[i-4] = node.Attributes[i].Value; //limits xml to only 4 set values 
							translation.Forms[n] = node.Attributes[i].Value; n++;
						}
					}
					translation.Species = node.Attributes["genus"].Value;
					translation.PokedexEntry = node.InnerText;
					data.Add(int.Parse(node.Attributes["id"].Value), translation); //Is this safe? Possible overwritting of values with bad entries
				}
			}

			//ToDo: Is filestream still open or does it need to be closed and disposed of?
			fs.Dispose(); fs.Close();
			_pokeTranslations = data;//return data;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		/// <remarks>ToDo: If not in foreign language, check and load in English; else...</remarks>
        [Obsolete]
		public static PokedexTranslation GetPokedexTranslation(Pokemons id, Settings.Languages language = Settings.Languages.English)// int form = 0,
		{
			if (_pokeTranslations == null) //should return english if player's default language is null
			{
				LoadPokedexTranslations(language);//, form
			}

			int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
			if (!_pokeTranslations.ContainsKey(arrayId) && language == Settings.Languages.English)
			{
				//Debug.LogError("Failed to load pokedex translation for pokemon with id: " + (int)id); //ToDo: Throw exception error
				throw new System.Exception(string.Format("Failed to load pokedex translation for pokemon with id: {0}_{1}", (int)id, id.ToString()));
				//return new PokedexTranslation();
			}
			//ToDo: Show english text for missing data on foreign languages 
			else if (!_pokeTranslations.ContainsKey(arrayId) && language != Settings.Languages.English)
			{
				return _pokeEnglishTranslations[arrayId];
			}

			return _pokeTranslations[arrayId];// int id
		}
		#endregion
        #endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static PokemonData GetPokemon(Pokemons ID)
		{
			//Debug.Log("Get Pokemons");
			/*PokemonData result = null;
			int i = 1;
			while(result == null){
				if(Database[i].ID == ID){
					//Debug.Log("Pokemon DB Success");
					return result = Database[i];
				}
				i += 1;
				if(i >= Database.Length){
					Debug.Log("Pokemon DB Fail");
					return null;}
			}
			return result;*/
			foreach (PokemonData pokemon in Database)
			{
				if (pokemon.ID == ID) return pokemon;
			}
			throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
			//return null;
		}

		static int getPokemonArrayId(Pokemons ID)
		{
			//Debug.Log("Get Pokemons");
			/*PokemonData result = null;
			int i = 1;
			while(result == null){
				if(Database[i].ID == ID){
					//Debug.Log("Pokemon DB Success");
					return result = Database[i];
				}
				i += 1;
				if(i >= Database.Length){
					Debug.Log("Pokemon DB Fail");
					return null;}
			}
			return result;*/
			/*foreach(PokemonData pokemon in Database)
			{
				if (pokemon.ID == ID) return pokemon;
			}*/
			for (int i = 0; i < Database.Length; i++)
			{
				if (Database[i].ID == ID)
				{
					return i;
				}
			}
			throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
		}

		/// <summary>
		/// Returns int value of Pokemon from PokemonData[] <see cref="Database"/>
		/// </summary>
		public int ArrayId
		{//(Pokemon ID)
			get
			{
				//Debug.Log("Get Pokemons");
				/*PokemonData result = null;
				int i = 1;
				while(result == null){
					if(Database[i].ID == ID){
						//Debug.Log("Pokemon DB Success");
						return result = Database[i];
					}
					i += 1;
					if(i >= Database.Length){
						Debug.Log("Pokemon DB Fail");
						return null;}
				}
				return result;*/
				/*foreach(PokemonData pokemon in Database)
				{
					if (pokemon.ID == ID) return pokemon;
				}*/
				for (int i = 0; i < Database.Length; i++)
				{
					if (Database[i].ID == ID)
					{
						return i;
					}
				}
				throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
			}
		}
		/*
		/// <summary>
		/// [ability1,ability2,hiddenAbility]
		/// </summary>
		/// <example>int[,] Abilities = getAbilities() = { {1,2,3} }
		/// <para>int hiddenAbility = Abilities[0,3]</para>
		/// </example>
		/// <remarks>Something i think would work better than a string</remarks>
		public string getAbility(int ability){
			switch (ability)
			{
				case 0:
					return ability1;
				case 1:
					return ability2;
				case 3:
					return hiddenAbility;
				default:
					return null;
			}
		}

		public string[] GenerateMoveset(int level){
			//Set moveset based off of the highest level moves possible.
			string[] moveset = new string[4];
			int i = movesetLevels.Length-1; //work backwards so that the highest level move possible is grabbed first
			while(moveset[3] == null){
				if(movesetLevels[i] <= level){
					moveset[3] = movesetMovesStrings[i];
				}
				i -= 1;
			}
			if(i >= 0){ //if i is at least 0 still, then you can grab the next move down.
				moveset[2] = movesetMovesStrings[i];
				i -= 1;
				if(i >= 0){ //if i is at least 0 still, then you can grab the next move down.
					moveset[1] = movesetMovesStrings[i];
					i -= 1;
					if(i >= 0){ //if i is at least 0 still, then you can grab the last move down.
						moveset[0] = movesetMovesStrings[i];
						i -= 1;
					}
				}
			}
			i = 0;
			int i2 = 0;			//if the first move is null, then the array will need to be packed down
			if (moveset[0] == null){ 		//(nulls moved to the end of the array)
				while(i < 3){ 
					while(moveset[i] == null){
						i += 1;
					}
					moveset[i2] = moveset[i];
					moveset[i] = null;
					i2 += 1;
				}
			}
			return moveset;
		}

		/// <summary>
		/// Converts the string of a Pokemon Type to a Color in Unity. 
		/// </summary>
		/// <param name="PokemonType">string of pokemon type or name of a color</param>
		/// <returns>return a Unity.Color</returns>
		/// <example>StringToColor(Electric)</example>
		/// <example>StringToColor(Yellow)</example>
		/// <remarks>might need to make a new enum in PokemonData, type = x.Color...</remarks>
		public Color StringToColor(string PokemonType) {
			//private System.Collections.Generic.Dictionary<string, Color> StringToColorDic = new System.Collections.Generic.Dictionary<string, Color>() {//Dictionary<PokemonData.Type, Color>
			//http://www.epidemicjohto.com/t882-type-colors-hex-colors
			//Normal Type: A8A77A
			//Fire Type:  EE8130
			//Water Type: 6390F0
			//Electric Type:  F7D02C
			//Grass Type: 7AC74C
			//Ice Type: 96D9D6
			//Fighting Type:  C22E28
			//Poison Type: A33EA1
			//Ground Type: E2BF65
			//Flying Type: A98FF3
			//Psychic Type: F95587
			//Bug Type: A6B91A
			//Rock Type: B6A136
			//Ghost Type: 735797
			//Dragon Type:  6F35FC
			//Dark Type: 705746
			//Steel Type:  B7B7CE
			//Fairy Type: D685AD

			//http://www.serebiiforums.com/showthread.php?289595-Pokemon-type-color-associations
			//Normal -white
			//Fire - red
			//Water -blue
			//Electric -yellow
			//Grass - green
			//Ice - cyan
			//Poison -purple
			//Psychic - magenta
			//Fighting - dark red
			//Ground - brown
			//Rock - gray
			//Bug - yellow green
			//Flying - light blue
			//Dragon - orange
			//Ghost - light purple
			//Steel - dark gray
			//Dark - black

			/*
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
			* /
			return new UnityEngine.Color();//StringToColorDic[PokemonType];
		}
		/// <summary>
		/// Converts the Pokemon Type to a Color in Unity. 
		/// </summary>
		/// <param name="PokemonType">pokemon type</param>
		/// <returns>return a Unity.Color</returns>
		/// <example>StringToColor(Electric)</example>
		public Color StringToColor(PokemonData.Type PokemonType) {
			return StringToColor(PokemonType.ToString()); //Will fix later
		}
		/// <summary>
		/// Only an example. Do not use, will  not work.
		/// <para>Could be combined with database values 
		/// and used with ints instead of strings</para>
		/// <para>Convert the pokemon type into a color 
		/// that can be used with Unity's color lighting</para>
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public Color StringToColor(int color) {
			switch (color)
			{
				//case 1:
				//	return StringToColorDic["text"];
				default:
					return StringToColor(color.ToString());
			}
		}*/
		#endregion

	}
	/// <summary>
	/// The moves that all Pokémon of the species learn as they level up. 
	/// </summary>
	public class PokemonMoveset
	{
		public enum LearnMethod
		{
			levelup = 1,
			egg = 2,
			tutor = 3,
			machine = 4,
			stadium_surfing_pikachu = 5,
			light_ball_egg = 6,
			colosseum_purification = 7,
			xd_shadow = 8,
			xd_purification = 9,
			form_change = 10
		}
		public LearnMethod TeachMethod;
		/// <summary>
		/// Level at which the move is learned 
		/// (0 means the move can only be learned when 
		/// a Pokémon evolves into this species).
        /// Default level is 0; 
        /// only really important if learn method is <see cref="LearnMethod.levelup"/>
		/// </summary>
		/// Level needed to learn Move
        /// leaves door open to new game designs, if players want to limit move methods to a level requirement
        /// use to set min or max level needed for designated learn method
		public int Level;
		/// <summary>
		/// Move learned upon leveling-up
		/// </summary>
		public Moves MoveId;
		//public PokemonMoveset() { }
		/*public PokemonMoveset(Move.MoveData.Move move, int level)
		{
			this.Level = level;
			this.MoveId = move;
		}
		public PokemonMoveset(Move.MoveData.Move move)
		{
			this.MoveId = move;
		}*/
		public PokemonMoveset(Moves moveId, LearnMethod method = LearnMethod.levelup, int level = 0) //: this()
		{
			this.Level = level;
			this.MoveId = moveId;
            this.TeachMethod = method;
		}
	}
	/// <summary>
	/// All the moves this pokemon species can learn, and the methods by which they learn them
	/// </summary>
	public class PokemonMoveTree
	{
        /// <summary>
        /// to use: LevelUp.OrderBy(x => x.Value).ThenBy(x => x.Key)
        /// </summary>
        public SortedList<Moves, int> LevelUp { get; private set; }
		public Moves[] Egg { get; private set; }
        public Moves[] Tutor { get; private set; }
		public Moves[] Machine { get; private set; }
        //Teach pikachu surf?... do we really need it to know surf?... Maybe if "specal form" (surfboard pickachu) is added to game
        //public Move.MoveData.Move[] stadium_surfing_pikachu { get; private set; }
        /// <summary>
        /// If <see cref="eItems.Item.LIGHT_BALL"/> is held by either parent of a <see cref="PokemonData.Pokemons.Pichu"/> when the Egg is produced,
        /// the Pichu that hatches will know the move <see cref="Move.MoveData.Move.Volt_Tackle"/>.
        /// </summary>
        /// Not sure about this one
        public Moves[] light_ball_egg { get; private set; }
        //public Move.MoveData.Move[] colosseum_purification { get; private set; }
        //public Move.MoveData.Move[] xd_shadow { get; private set; }
        //public Move.MoveData.Move[] xd_purification { get; private set; }
        /// <summary>
        /// </summary>
        /// Merge both Colosseum and XD into one list
        public Moves[] Shadow { get; private set; }
        /// <summary>
        /// When a pokemon is purified from a shadow state, the moves they can potentially unlock?...
        /// </summary>
        /// Merge both Colosseum and XD into one list
        public Moves[] Purification { get; private set; }
        public Moves[] FormChange { get; private set; }
        public PokemonMoveTree(
                SortedList<Moves, int> levelup = null,
                Moves[] egg = null,
                Moves[] tutor = null,
                Moves[] machine = null,
                //Move.MoveData.Move[] stadium_surfing_pikachu = null,
                //Move.MoveData.Move[] light_ball_egg = null,
                Moves[] shadow = null,
                Moves[] purification = null,
                Moves[] form_change = null
            )
        {
            this.LevelUp = levelup ?? new SortedList<Moves, int>();                       
			this.Egg = egg ?? new Moves[0];
            this.Tutor = tutor ?? new Moves[0];
            this.Machine = machine ?? new Moves[0];
            //this.stadium_surfing_pikachu = 5,
            //this.light_ball_egg = light_ball_egg ?? new Move.MoveData.Move[0];
            this.Shadow = shadow ?? new Moves[0];
            this.Purification = purification ?? new Moves[0];
            this.FormChange = form_change ?? new Moves[0];
        }
    }
	/// <summary>
	/// The evolution paths this species can take. 
	/// For each possible evolution of this species, 
	/// there are three parts
	/// </summary>
	public class PokemonEvolution//<T> where T : new()
	{
		/// <summary>
		/// no parameter,
		/// Positive integer,
		/// Item  = <see cref="eItems.Item"/>,
		/// Move = <see cref="Moves"/>,
		/// Species = <see cref="PokemonData.Pokemons"/>,
		/// Type = <see cref="PokemonData.Types"/>
		/// </summary>
		/// <example>
		/// <para>E.G.	Poliwhirl(61)
		///		<code>new int[]{62,186},
		///		new string[]{"Stone,Water Stone","Trade\Item,King's Rock"}),</code></para> 
		/// <para>
		/// E.G. to evolve to sylveon
		///		<code>new int[]{..., 700},
		///		new string[]{..., "Amie\Move,2\Fairy"}),</code>
		/// </para> 
		/// </example>
		/// ToDo: Custom class => new Evolve(){ EvolveMethod.Item, Items.Item } 
		/// ToDo: If all conditions are met in "new Evolve()" instance, then evolve.
		/// Ideas:
		/// Merge two or more existing methods together into a new one.
		/// Evolution that depends on the Pokémon's nature or form.
		/// Fusion evolution(e.g. for Magnemite/Slowpoke). Check that there is a Shellder in the party, and if so, delete it and evolve the levelled-up Slowpoke.
		/// Check how many EVs the Pokémon has, and allows evolution only if that amount is greater than or equal to the EV threshold value set by the parameter.
		/// Shiny-Random?
		public enum EvolutionMethod
		{
			/// <summary>
			///	if pokemon's level is greater or equal to int level
			/// <code>Level,int level</code>
			/// </summary>
			Level,
			/// <summary>
			///	Exactly the same as "Level", except the Pokémon must also be male.
			/// <code>Level,int level</code>
			/// </summary>
			/// <example>Burmy</example>
			LevelMale,
			/// <summary>
			///	Exactly the same as "Level", except the Pokémon must also be female.
			/// <code>Level,int level</code>
			/// </summary>
			/// <example>Burmy, Combee</example>
			LevelFemale,
			///	<summary>
			///	The Pokémon will evolve if a particular item is used on it 
			///	(named by the parameter - typically an evolution stone).
			/// <code>Stone,string itemName</code>
			///	</summary>
			/// <example>Clefairy, Cottonee, Eelektrik, Eevee, Exeggcute, Gloom, 
			/// Growlithe, Jigglypuff, Lampent, Lombre, Minccino, Misdreavus, 
			/// Munna, Murkrow, Nidorina, Nidorino, Nuzleaf, Panpour, Pansage, 
			/// Pansear, Petilil, Pikachu, Poliwhirl, Roselia, Shellder, Skitty, 
			/// Staryu, Sunkern, Togetic, Vulpix, Weepinbell</example>
			Item,
			/// <summary>
			///	Exactly the same as "<see cref="Item"/>", 
			///	except the Pokémon must also be male.
			/// </summary>
			/// <example>Kirlia</example>
			ItemMale,
			/// <summary>
			///	Exactly the same as "<see cref="Item"/>", 
			///	except the Pokémon must also be female.
			/// </summary>
			/// <example>Snorunt</example>
			ItemFemale,
			/// <summary>
			///	The Pokémon will evolve immediately after it is traded.
			///	</summary>
			/// <example>Boldore, Graveler, Gurdurr, Haunter, Kadabra, Machoke</example>
			Trade,
			/// <summary>
			///	Exactly the same as "Trade", 
			///	except the Pokémon must also be holding a particular item 
			///	(named by the parameter). 
			///	That item is removed afterwards.
			///	</summary>
			/// <example>Clamperl, Dusclops, Electabuzz, Feebas, Magmar, 
			/// Onix, Porygon, Porygon2, Poliwhirl, Rhydon, Scyther, Seadra, 
			/// Slowpoke</example>
			TradeItem,
			/// <summary>
			///	Exactly the same as "Trade", 
			///	except the Pokémon must have been traded for a Pokémon of a certain species 
			///	(named by the parameter).
			///	</summary>
			/// <example>Karrablast, Shelmet</example>
			TradeSpecies,
			/// <summary>
			///	if pokemon's happiness is greater or equal to 220.
			///	Note: Happiness checks should come last before all other methods.
			///	</summary>
			/// <example>Azurill, Buneary, Chansey, Cleffa, Golbat, 
			/// Igglybuff, Munchlax, Pichu, Swadloon, Togepi, Woobat</example>
			Happiness,
			/// <summary>
			///	Exactly the same as "Happiness", 
			///	but will only evolve during the daytime.
			///	</summary>
			/// <example>Budew, Eevee, Riolu</example>
			HappinessDay,
			/// <summary>
			///	Exactly the same as "Happiness", 
			///	but will only evolve during the night-time.
			///	</summary>
			/// <example>Chingling, Eevee</example>
			HappinessNight,
			/// <summary>
			///	This method is almost identical to "Happiness", 
			///	with the sole changes of replacing the "greater than" sign 
			///	to a "less than" sign, and changing the threshold value. 
			///	If you use this method, 
			///	you may also want to make it easier to get a Pokémon to hate you in-game 
			///	(currently the only ways to do this are fainting and using herbal medicine, 
			///	which can easily be countered by the many more happiness-boosting methods).
			///	</summary>
			Hatred,
			/// <summary>
			///	if time is between 9PM and 4AM time is "Night". else time is "Day".
			///	if time is equal to string dayNight (either Day, or Night).
			///	<code>Time,DatetimeOffset/bool dayNight</code>
			/// </summary>
			Time,
			/// <summary>
			///	if date is between Day-Month or maybe certain 1/4 (quarter) of the year.
			///	if date is equal to string season (either Summer, Winter, Spring, or Fall).
			///	<code>Time,DatetimeOffset/bool season</code>
			/// </summary>
            /// is holiday to "spot-on" as an occasion or requirement for leveling-up?
			Season,
			/// <summary>
			///	if pokemon's heldItem is equal to string itemName
			/// <example>Item,string itemName</example>
			/// </summary>
			/// Holding a certain item after leveling-up?
			HoldItem,
			/// <summary>
			///	The Pokémon will evolve if it levels up during the daytime 
			///	while holding a particular item (named by the parameter).
			/// <code>Item,string itemName</code>
			/// </summary>
			/// <example>Happiny</example>
			HoldItemDay,
			/// <summary>
			///	The Pokémon will evolve if it levels up during the night-time 
			///	while holding a particular item (named by the parameter).
			/// <code>Item,string itemName</code>
			/// </summary>
			/// <example>Gligar, Sneasel</example>
			HoldItemNight,
			/// <summary>
			/// The Pokémon will evolve when it levels up, 
			/// if its beauty stat is greater than or equal to the parameter.
			/// </summary>
			/// <example>Feebas</example>
			Beauty,
			/// <summary>
			///	The Pokémon will evolve if it levels up while knowing a particular move (named by the parameter).
			/// <example>Move,string moveName</example>
			/// </summary>
			/// <example>Aipom, Bonsly, Lickitung, Mime Jr., Piloswine, Tangela, Yanma</example>
			Move,
			/// <summary>
			///	The Pokémon will evolve if it levels up while the player has a Pokémon of a certain species in their party 
			///	(named by the parameter). 
			///	The named Pokémon is unaffected.
			/// <example>Pokemon,string pokemonName</example>
			/// </summary>
			/// <example>Mantyke, Mantine</example>
			/// if party contains a Remoraid
			Party,
			/// <summary>
			///	The Pokémon will evolve if it levels up while the player has a Pokémon of a certain type in their party 
			///	(named by the parameter). 
			/// <example>Type,string pokemonTypeName</example>
			/// </summary>
			/// <example>Pangoro</example>
			/// if party contains a dark pokemon
			Type,
			/// <summary>
			/// The Pokémon will evolve when it levels up, 
			/// if the player is currently on the map given by the parameter.
			///	<code>Map,string mapName</code>
			/// </summary>
			Location,
			///	<summary>
			///	The Pokémon will evolve if it levels up only during a certain kind of overworld weather.
			///	</summary>
			/// <example>Goodra</example>
			///	if currentMap's weather is rain
			Weather,
			///	<summary>
			///	Exactly the same as "Level", 
			///	except the Pokémon's Attack stat must also be greater than its Defense stat.
			///	</summary>
			/// <example>Hitmonlee</example>
			AttackGreater,
			///	<summary>
			///	Exactly the same as "Level", 
			///	except the Pokémon's Attack stat must also be lower than its Defense stat.
			///	</summary>
			/// <example>Hitmonchan</example>
			DefenseGreater,
			/// <summary>
			/// Exactly the same as "Level", 
			/// except the Pokémon's Attack stat must also be equal to its Defense stat.
			/// </summary>
			/// <example>Hitmontop</example>
			AtkDefEqual,
			///	<summary>if pokemon's shinyValue divided by 2's remainder is equal to 0 or 1</summary>
			///	What about level parameter? Maybe "ShinyYes/ShinyNo" or "Shiny0/Shiny1", in combination with Level-parameter? 
			Shiny,
			/// <summary>Unique evolution methods: if pokemon's shinyValue divided by 2's remainder is equal to 0</summary>
			/// Shiny value? I thought it was based on "Friendship"
			Silcoon,
			///	<summary>Unique evolution methods: if pokemon's shinyValue divided by 2's remainder is equal to 1</summary>
			Cascoon,
			///	<summary>
			///	Unique evolution methods: 
			///	Exactly the same as "Level". 
			///	There is no difference between the two methods at all. 
			///	Is used alongside the method "Shedinja".
			///	</summary>
			Ninjask,
			///	<summary>
			///	Unique evolution methods: 
			///	Must be used with the method "Ninjask". 
			///	Duplicates the Pokémon that just evolved 
			///	(if there is an empty space in the party), 
			///	and changes the duplicate's species to the given species.
			///	</summary>
            ///	how is this different from "party"?
			Shedinja,
            /// <summary>
            /// </summary>
            /// Just wanted to see a requirement for after "fainting" too many times, your pokemon just died, and became a ghost-type...  
            Deaths
		}
		/// <summary>
		/// The PokemonId of the evolved species.
		/// The PokemonId of the species this pokemon evolves into.
		/// </summary>
		public PokemonData.Pokemons Species;
		/// <summary>
		/// The evolution method.
		/// </summary>
		public EvolutionMethod EvolveMethod;
        //public object EvolutionMethodValue;
        //public PokemonEvolution<T> EvolutionMethodValue;
        //public class T { }
        //public PokemonEvolution(){}
        public PokemonEvolution(PokemonData.Pokemons EvolveTo, EvolutionMethod EvolveHow){
            this.Species = EvolveTo;
            this.EvolveMethod = EvolveHow;
        }
    }
	public class PokemonEvolution<T> : PokemonEvolution
	{
		/*// <summary>
		/// The PokemonId of the evolved species.
		/// </summary>
		public PokemonData.Pokemon EvolvesTo;
		/// <summary>
		/// The evolution method.
		/// </summary>
		public int EvolveMethod;
		//public object EvolveValue;
		//public T EvolveValue<T>() { return GetValue(); };*/
		/// <summary>
		/// The value-parameter to <see cref="EvolveMethod"/> as mentioned KEY.
		/// </summary>
		public T EvolveValue;

        //public PokemonEvolution<T> (T objects){}
        //void evolve(PokemonData.Pokemon EvolveTo, EvolutionMethod EvolveHow, T Value) { }

        public PokemonEvolution(PokemonData.Pokemons EvolveTo, EvolutionMethod EvolveHow, T Value) : base(EvolveTo: EvolveTo, EvolveHow: EvolveHow) {
            #region Switch
            //This should trigger after the class has been initialized, right?
            switch (EvolveHow)
            {
                case EvolutionMethod.Level:
                case EvolutionMethod.LevelFemale:
                case EvolutionMethod.LevelMale:
                case EvolutionMethod.Ninjask:
                case EvolutionMethod.Beauty:
                case EvolutionMethod.Happiness:
                case EvolutionMethod.HappinessDay:
                case EvolutionMethod.HappinessNight:
                case EvolutionMethod.Hatred:
                    //if(typeof(T) || this.GetType() == typeof(string))
                    if (Value.GetType() != typeof(int))
                        //throw new Exception("Type not acceptable for Method-Value pair.");
                        //Instead of throwing an exception, i'll correct the problem instead?
                        Convert.ChangeType(Value, typeof(int));
                    Value = default(T);
                    break;
                case EvolutionMethod.Item:
                case EvolutionMethod.ItemFemale:
                case EvolutionMethod.ItemMale:
                case EvolutionMethod.TradeItem:
                case EvolutionMethod.HoldItem:
                case EvolutionMethod.HoldItemDay:
                case EvolutionMethod.HoldItemNight:
                    if (this.EvolveValue.GetType() != typeof(eItems))
                        Convert.ChangeType(Value, typeof(eItems));
                    Value = default(T);
                    break;
                case EvolutionMethod.TradeSpecies:
                case EvolutionMethod.Party:
                case EvolutionMethod.Shedinja:
                    if (this.EvolveValue.GetType() != typeof(PokemonData.Pokemons))
                        Convert.ChangeType(Value, typeof(PokemonData.Pokemons));
                    Value = default(T);
                    break;
                case EvolutionMethod.Move:
                    if (this.EvolveValue.GetType() != typeof(Moves))
                        Convert.ChangeType(Value, typeof(Moves));
                    Value = default(T);
                    break;
                case EvolutionMethod.Type:
                    if (this.EvolveValue.GetType() != typeof(PokemonData.Types))
                        Convert.ChangeType(Value, typeof(PokemonData.Types));
                    Value = default(T);
                    break;
                case EvolutionMethod.Time:
                case EvolutionMethod.Season:
                case EvolutionMethod.Location:
                case EvolutionMethod.Weather:
                default:
                    //if there's no problem, just ignore it, and move on...
                    break;
            }
            #endregion
            this.EvolveValue = Value;
        }
        /*public PokemonEvolution(PokemonData.Pokemon EvolveTo, EvolutionMethod EvolveHow, T Value) : base(EvolveTo: EvolveTo, EvolveHow: EvolveHow) {
            #region Switch
            //This should trigger after the class has been initialized, right?
            switch (this.EvolveMethod)
            {
                case EvolutionMethod.Level:
                case EvolutionMethod.LevelFemale:
                case EvolutionMethod.LevelMale:
                case EvolutionMethod.Ninjask:
                case EvolutionMethod.Beauty:
                case EvolutionMethod.Happiness:
                case EvolutionMethod.HappinessDay:
                case EvolutionMethod.HappinessNight:
                case EvolutionMethod.Hatred:
                    //if(typeof(T) || this.GetType() == typeof(string))
                    if (this.EvolveValue.GetType() != typeof(int))
                        //throw new Exception("Type not acceptable for Method-Value pair.");
                        //Instead of throwing an exception, i'll correct the problem instead?
                        Convert.ChangeType(this.EvolveValue, typeof(int));
                        this.EvolveValue = default(T);
                    break;
                case EvolutionMethod.Item:
                case EvolutionMethod.ItemFemale:
                case EvolutionMethod.ItemMale:
                case EvolutionMethod.TradeItem:
                case EvolutionMethod.HoldItem:
                case EvolutionMethod.HoldItemDay:
                case EvolutionMethod.HoldItemNight:
                    if (this.EvolveValue.GetType() != typeof(eItems))
                        Convert.ChangeType(this.EvolveValue, typeof(eItems));
                        this.EvolveValue = default(T);
                    break;
                case EvolutionMethod.TradeSpecies:
                case EvolutionMethod.Party:
                case EvolutionMethod.Shedinja:
                    if (this.EvolveValue.GetType() != typeof(PokemonData.Pokemon))
                        Convert.ChangeType(this.EvolveValue, typeof(PokemonData.Pokemon));
                        this.EvolveValue = default(T);
                    break;
                case EvolutionMethod.Move:
                    if (this.EvolveValue.GetType() != typeof(Move.MoveData.Move))
                        Convert.ChangeType(this.EvolveValue, typeof(Move.MoveData.Move));
                        this.EvolveValue = default(T);
                    break;
                case EvolutionMethod.Type:
                    if (this.EvolveValue.GetType() != typeof(PokemonData.Type))
                        Convert.ChangeType(this.EvolveValue, typeof(PokemonData.Type));
                        this.EvolveValue = default(T);
                    break;
                case EvolutionMethod.Time:
                case EvolutionMethod.Season:
                case EvolutionMethod.Location:
                case EvolutionMethod.Weather:
                default:
                    //if there's no problem, just ignore it, and move on...
                    break;
            }
            #endregion
        }
        //private static Con<T2>(object ObjIn)
		/*public int IntValue;
		public string StringValue;
		private int GetValue(int p)
		{
			return this.IntValue;
		}
		private string GetValue(string p)
		{
			return this.StringValue;
		}*/
	}
	/// <summary>
	/// </summary>
	/// Experience can be it's own class away from Pokemon. But there's no need for it to be global.
	/// ToDo: Consider making Experience class a Pokemon extension class...
	private static class Experience
	{
		#region expTable
		private static int[] expTableErratic = new int[]{
			0,15,52,122,237,406,637,942,1326,1800,
			2369,3041,3822,4719,5737,6881,8155,9564,11111,12800,
			14632,16610,18737,21012,23437,26012,28737,31610,34632,37800,
			41111,44564,48155,51881,55737,59719,63822,68041,72369,76800,
			81326,85942,90637,95406,100237,105122,110052,115105,120001,125000,
			131324,137795,144410,151165,158056,165079,172229,179503,186894,194400,
			202013,209728,217540,225443,233431,241496,249633,257834,267406,276458,
			286328,296358,305767,316074,326531,336255,346965,357812,567807,378880,
			390077,400293,411686,423190,433572,445239,457001,467489,479378,491346,
			501878,513934,526049,536557,548720,560922,571333,583539,591882,600000};
		/// <summary>
		/// Medium (Medium Fast)
		/// </summary>
		private static int[] expTableFast = new int[]{
			0,6,21,51,100,172,274,409,583,800,
			1064,1382,1757,2195,2700,3276,3930,4665,5487,6400,
			7408,8518,9733,11059,12500,14060,15746,17561,19511,21600,
			23832,26214,28749,31443,34300,37324,40522,43897,47455,51200,
			55136,59270,63605,68147,72900,77868,83058,88473,94119,100000,
			106120,112486,119101,125971,133100,140492,148154,156089,164303,172800,
			181584,190662,200037,209715,219700,229996,240610,251545,262807,274400,
			286328,298598,311213,324179,337500,351180,365226,379641,394431,409600,
			425152,441094,457429,474163,491300,508844,526802,545177,563975,583200,
			602856,622950,643485,664467,685900,707788,730138,752953,776239,800000};

		private static int[] expTableMediumFast = new int[]{
			0,8,27,64,125,216,343,512,729,1000,
			1331,1728,2197,2744,3375,4096,4913,5832,6859,8000,
			9261,10648,12167,13824,15625,17576,19683,21952,24389,27000,
			29791,32768,35937,39304,42875,46656,50653,54872,59319,64000,
			68921,74088,79507,85184,91125,97336,103823,110592,117649,125000,
			132651,140608,148877,157464,166375,175616,185193,195112,205379,216000,
			226981,238328,250047,262144,274625,287496,300763,314432,328509,343000,
			357911,373248,389017,405224,421875,438976,456533,474552,493039,512000,
			531441,551368,571787,592704,614125,636056,658503,681472,704969,729000,
			753571,778688,804357,830584,857375,884736,912673,941192,970299,1000000};
		/// <summary>
		/// Parabolic (Medium Slow)
		/// </summary>
		private static int[] expTableMediumSlow = new int[]{
			0,9,57,96,135,179,236,314,419,560,
			742,973,1261,1612,2035,2535,3120,3798,4575,5460,
			6458,7577,8825,10208,11735,13411,15244,17242,19411,21760,
			24294,27021,29949,33084,36435,40007,43808,47846,52127,56660,
			61450,66505,71833,77440,83335,89523,96012,102810,109923,117360,
			125126,133229,141677,150476,159635,169159,179056,189334,199999,211060,
			222522,234393,246681,259392,272535,286115,300140,314618,329555,344960,
			360838,377197,394045,411388,429235,447591,466464,485862,505791,526260,
			547274,568841,590969,613664,636935,660787,685228,710266,735907,762160,
			789030,816525,844653,873420,902835,932903,963632,995030,1027103,1059860};

		private static int[] expTableSlow = new int[]{
			0,10,33,80,156,270,428,640,911,1250,
			1663,2160,2746,3430,4218,5120,6141,7290,8573,10000,
			11576,13310,15208,17280,19531,21970,24603,27440,30486,33750,
			37238,40960,44921,49130,53593,58320,63316,68590,74148,80000,
			86151,92610,99383,106480,113906,121670,129778,138240,147061,156250,
			165813,175760,186096,196830,207968,219520,231491,243890,256723,270000,
			283726,297910,312558,327680,343281,359370,375953,393040,410636,428750,
			447388,466560,486271,506530,527343,548720,570666,593190,616298,640000,
			664301,689210,714733,740880,767656,795070,823128,851840,881211,911250,
			941963,973360,1005446,1038230,1071718,1105920,1140841,1176490,1212873,1250000};

		private static int[] expTableFluctuating = new int[]{
			0,4,13,32,65,112,178,276,393,540,
			745,967,1230,1591,1957,2457,3046,3732,4526,5440,
			6482,7666,9003,10506,12187,14060,16140,18439,20974,23760,
			26811,30146,33780,37731,42017,46656,50653,55969,60505,66560,
			71677,78533,84277,91998,98415,107069,114205,123863,131766,142500,
			151222,163105,172697,185807,196322,210739,222231,238036,250562,267840,
			281456,300293,315059,335544,351520,373744,390991,415050,433631,459620,
			479600,507617,529063,559209,582187,614566,639146,673863,700115,737280,
			765275,804997,834809,877201,908905,954084,987754,1035837,1071552,1122660,
			1160499,1214753,1254796,1312322,1354652,1415577,1460276,1524731,1571884,1640000};
		#endregion

		private static int GetExperience(Pokemon.PokemonData.LevelingRate levelingRate, int currentLevel)
		{
			int exp = 0;
			//if (currentLevel > 100) currentLevel = 100; 
			//if (currentLevel > Settings.MAXIMUMLEVEL) currentLevel = Settings.MAXIMUMLEVEL; 
			if (levelingRate == PokemonData.LevelingRate.ERRATIC)
			{
				if (currentLevel > 100)
				{
					//exp = (int)System.Math.Floor((System.Math.Pow(currentLevel, 3)) * (160 - currentLevel) / 100);
					exp = (int)System.Math.Floor((System.Math.Pow(currentLevel, 3)) * (currentLevel * 6 / 10) / 100);
				}
				else exp = expTableErratic[currentLevel - 1]; //Because the array starts at 0, not 1.
			}
			else if (levelingRate == PokemonData.LevelingRate.FAST)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (4 / 5));
				}
				else exp = expTableFast[currentLevel - 1];
			}
			else if (levelingRate == PokemonData.LevelingRate.MEDIUMFAST)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3));
				}
				else exp = expTableMediumFast[currentLevel - 1];
			}
			else if (levelingRate == PokemonData.LevelingRate.MEDIUMSLOW)
			{
				if (currentLevel > 100)
				{
					//Dont remember why currentlevel minus 1 is in formula...
					//I think it has to deal with formula table glitch for lvl 1-2 pokemons...
					//exp = (int)System.Math.Floor(((6 / 5) * System.Math.Pow(currentLevel - 1, 3)) - (15 * System.Math.Pow(currentLevel - 1, 3)) + (100 * (currentLevel - 1)) - 140);
					exp = (int)System.Math.Floor((6 * System.Math.Pow(currentLevel - 1, 3) / 5) - 15 * System.Math.Pow(currentLevel - 1, 2) + 100 * (currentLevel - 1) - 140);
				}
				else exp = expTableMediumSlow[currentLevel - 1];
			}
			else if (levelingRate == PokemonData.LevelingRate.SLOW)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (5 / 4));
				}
				else exp = expTableSlow[currentLevel - 1];
			}
			else if (levelingRate == PokemonData.LevelingRate.FLUCTUATING)
			{
				if (currentLevel > 100)
				{
					int rate = 82;
					//Slow rate with increasing level
					rate -= (currentLevel - 100) / 2;
					if (rate < 40) rate = 40;

					//exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * ((System.Math.Floor(System.Math.Pow(currentLevel, 3) / 2) + 32) / 50));
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (((currentLevel * rate / 100) / 50)));
				}
				else exp = expTableFluctuating[currentLevel - 1];
			}

			return exp;


		}
		/// <summary>
		/// Gets the maximum Exp Points possible for the given growth rate.
		/// </summary>
		/// <param name="levelingRate"></param>
		/// <returns></returns>
		public static int GetMaxExperience(Pokemon.PokemonData.LevelingRate levelingRate)
		{
			return GetExperience(levelingRate, Settings.MAXIMUMLEVEL);
		}
		/// <summary>
		/// Gets the number of Exp Points needed to reach the given
		/// level with the given growth rate.
		/// </summary>
		/// <param name="levelingRate"></param>
		/// <param name="currentLevel"></param>
		/// <returns></returns>
		public static int GetStartExperience(Pokemon.PokemonData.LevelingRate levelingRate, int currentLevel)
		{
			if (currentLevel < 0) currentLevel = 1;
			if (currentLevel > Settings.MAXIMUMLEVEL) currentLevel = Settings.MAXIMUMLEVEL;
			return GetExperience(levelingRate, currentLevel);
		}
		/// <summary>
		/// Adds experience points ensuring that the new total doesn't
		/// exceed the maximum Exp. Points for the given growth rate.
		/// </summary>
		/// <param name="levelingRate">Growth rate.</param>
		/// <param name="currentExperience">Current Exp Points.</param>
		/// <param name="experienceGain">Exp. Points to add</param>
		/// <returns></returns>
		/// Subtract ceiling exp for left over experience points remaining after level up?...
		public static int AddExperience(Pokemon.PokemonData.LevelingRate levelingRate, int currentExperience, int experienceGain)
		{
			int exp = currentExperience + experienceGain;
			int maxexp = GetExperience(levelingRate, Settings.MAXIMUMLEVEL);
			if (exp > maxexp) exp = maxexp;
			return exp;
		}
		/// <summary>
		/// Calculates a level given the number of Exp Points and growth rate.
		/// </summary>
		/// <param name="levelingRate">Growth rate.</param>
		/// <param name="experiencePoints">Current Experience Points</param>
		/// <returns></returns>
		public static int GetLevelFromExperience(Pokemon.PokemonData.LevelingRate levelingRate, int experiencePoints)
		{
			int maxexp = GetExperience(levelingRate, Settings.MAXIMUMLEVEL);
			if (experiencePoints > maxexp) experiencePoints = maxexp;
			for (int i = 0; i < Settings.MAXIMUMLEVEL; i++)
			{
				if (GetExperience(levelingRate, Settings.MAXIMUMLEVEL) == experiencePoints) return i;
				if (GetExperience(levelingRate, Settings.MAXIMUMLEVEL) > experiencePoints) return i-1;
			}
			return Settings.MAXIMUMLEVEL;
		}
	}
	#endregion
}

namespace PokemonUnity
{
    /// <summary>
    /// Namespace to nest all Pokemon Enums
    /// </summary>
    namespace Pokemon
    {

    }

}