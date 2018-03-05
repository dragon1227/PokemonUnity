﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 
/// </summary>
/// ToDo: Consider nesting PokemonData class?
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
	private bool? mail;
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
	private bStatus status;
	public enum bStatus
	{
		None,
		Sleep,
		Poison,
		Paralysis,
		Burn,
		Frozen
	}
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
	private eMoves.Move[] moves = new eMoves.Move[4]; //{ eMoves.Move.NONE, eMoves.Move.NONE, eMoves.Move.NONE, eMoves.Move.NONE };
	/// <summary>
	/// The moves known when this Pokemon was obtained
	/// </summary>
	private eMoves.Move[] firstMoves = new eMoves.Move[4];
	/// <summary>
	/// Ball used
	/// </summary>
	private eItems.Item ballUsed = (eItems.Item)0; //ToDo: None?
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
	/// Array of ribbons
	/// </summary>
	/// <remarks>
	/// Make 2d Array (Array[,]) separated by regions/Gens
	/// </remarks>
	private bool[] ribbons; //= new bool[numberOfRegions,RibbonsPerRegion];
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
	public Pokemon(PokemonData.Pokemon pokemon) //ToDo: Redo PokemonDatabase/PokemonData -- DONE
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
	/// Returns the ID of this Pokemon's nature
	/// Sets this Pokemon's nature to a particular nature (and calculates the stat modifications).
	/// </summary>
	public NatureDatabase.Nature Nature { get { return this.natureFlag; } set { this.natureFlag = value; /*nature.calcStats();*/ } }//ToDo:

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
	public bool hasType(PokemonData.Type type)
	{
		return this._base.Types[0] == type || this._base.Types[1] == type;
	}

	/// <summary>
	/// Returns this Pokemon's first type
	/// </summary>
	public PokemonData.Type Type1 { get { return this._base.Types[0]; } }

	/// <summary>
	/// Returns this Pokemon's second type
	/// </summary>
	public PokemonData.Type Type2 { get { return this._base.Types[1]; } }
	#endregion

	#region Moves
	/*// <summary>
	/// Returns the number of moves known by the Pokémon.
	/// </summary>
	int numMoves()
	{
		int ret = 0;
		for (int i = 0; i < 4; i++) {//foreach(var move in this.moves){ 
			if ((int)this.moves[i] != 0) ret += 1;//move.id
		}
		return ret;
	}*/

	/*// <summary>
	/// Returns true if the Pokémon knows the given move.
	/// </summary>
	bool hasMove(eMoves.Move move) {
		//if (move <= 0) return false;//move == null ||
		for (int i = 0; i < 4; i++)
		{
			if (this.moves[i] == move) return true;
		}
		return false;
	}*/

	//bool knowsMove(eMoves.Move move) { return this.hasMove (move); }

	/*// <summary>
    /// Returns the list of moves this Pokémon can learn by levelling up.
    /// </summary>
    /// ToDo: Custom<int Level, eMove move> Class
    eMoves.Move[] getMoveList() {
        //movelist =[]
        //_base.MovesetMoves
        //for k in 0..length - 1
        //movelist.push([level, move])}
        return movelist
     }*/

	/*// <summary>
    /// Sets this Pokémon's movelist to the default movelist it originally had.
    /// </summary>
    void resetMoves()
    {
        //eMoves.Move moves = this.getMoveList();
        eMoves.Move[] movelist;
        foreach(var i in moves) {//for (int i = 0; i < moveList; i++){
            if (i[0] <= this.level)
            {
                movelist[movelist.length] = i[1];
            }
        }
        //movelist|=[] // Remove duplicates
        int listend = movelist.length - 4;
        listend = listend < 0 ? 0 : listend;
        int j = 0;
        for (int i = 0; i < listend + 4; i++) { //i in listend...listend+4
            moveid = (i >= movelist.length) ? 0 : movelist[i];
            @moves[j] = PBMove.new(moveid);
            j += 1;
        }
    }*/

	/*// <summary>
	/// Silently learns the given move. Will erase the first known move if it has to.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	void LearnMove(eMoves.Move move) {
		if ((int)move <= 0) return;
		for (int i = 0; i < 4; i++) {
			if (moves[i].id == move) {
				int j = i + 1;
				while (j < 4) {
					if (moves[j].id == 0) break;
					tmp = @moves[j];
					@moves[j] = @moves[j - 1];
					@moves[j - 1] = tmp;
					j += 1;
				}
				return;
			}
		}
		for (int i = 0; i < 4; i++) {
			if (@moves[i].id == 0) {
				@moves[i] = new Move(move);
				return;
			}
		}
		@moves[0] = @moves[1];
		@moves[1] = @moves[2];
		@moves[2] = @moves[3];
		@moves[3] = new Move(move);
	}*/

	/*// <summary>
	/// Deletes the given move from the Pokémon.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	void pbDeleteMove(move) {
		return if !move || move <= 0
		newmoves =[]
		for (int i = 0; i < 4; i++) { 
			if (moves[i].id != move) newmoves.push(@moves[i]);
		}

		newmoves.push(PBMove.new(0));
		for (int i = 0; i< 4; i++) {
			@moves[i] = newmoves[i];
		}
	 }*/

	/*// <summary>
	/// Deletes the move at the given index from the Pokémon.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	void DeleteMoveAtIndex(index) {
		newmoves =[];

		for (int i = 0; i < 4; i++) {
			if (i != index) newmoves.push(@moves[i]);
		}

		newmoves.push(PBMove.new(0));

		for (int i = 0; i < 4; i++) {
			@moves[i] = newmoves[i];
		}
	}*/

	/*// <summary>
	/// Deletes all moves from the Pokémon.
	/// </summary>
	void DeleteAllMoves() { 
		//for (int i = 0; i< 4; i++) { 
		//	moves[i]= new Move(0);
		//}
		moves = new eMoves.Move[4];
	}*/

	/*// <summary>
	/// Copies currently known moves into a separate array, for Move Relearner.
	/// </summary>
	void RecordFirstMoves() {
		//for (int i = 0; i < 4; i++) {
		//	if (moves[i].id > 0) firstmoves.push(moves[i].id);
		//}
		//firstmoves = moves;
	}*/

	/*void AddFirstMove(eMoves.Move move) {
		if (move > 0 && !firstMoves.include(move)) firstMoves.push(move);
		return;
	}*/

	/*void RemoveFirstMove(eMoves.Move move) {
		//if (move > 0) firstMoves.delete(move); 
		return;
	}*/

	/*void ClearFirstMoves() {
		firstMoves = new eMoves.Move[4];
	}*/

	/*bool isCompatibleWithMove(move) {
		return SpeciesCompatible(this.species, move);
	}*/

	/// <summary>
	/// Reduce the global clutter, and add to more readable 
	/// and maintainable code by encapsulation to logically 
	/// group classes that are only used in one place.
	/// </summary>
	internal class Moves
	{

	}
    #endregion

    #region Contest attributes, ribbons
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
    #endregion
}