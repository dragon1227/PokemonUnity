﻿using System;

namespace PokemonUnity.Saving.SerializableClasses
{
    using PokemonUnity.Pokemon;
    using PokemonUnity.Attack;
    using PokemonUnity.Item;
    using PokemonUnity;

    /// <summary>
    /// Serializable version of Pokemon Unity's Pokemon class
    /// </summary>
    [System.Serializable]
    public class SeriPokemon
    {
        #region Variables
        public string NickName { get; private set; }
        public int Form { get; private set; }
        public int Species { get; private set; }

        public int Ability { get; private set; }
        public SeriNature Nature { get; private set; }

        public virtual bool IsShiny { get; private set; }
        public virtual bool? Gender { get; private set; }

        public bool? PokerusStage { get; private set; }
        public int[] Pokerus { get; private set; }
        public int PokerusStrain { get; private set; }

        public bool IsHyperMode { get; private set; }
        public bool IsShadow { get; private set; }
        public int? ShadowLevel { get; private set; }

        public int CurrentHP { get; private set; }
        public int Item { get; private set; }

        public byte[] IV { get; private set; }
        public byte[] EV { get; private set; }

        public int ObtainedLevel { get; private set; }
        public int CurrentLevel { get; private set; }
        public int CurrentExp { get; private set; }

        public int Happines { get; private set; }

        public int Status { get; private set; }
        public int StatusCount { get; private set; }

        public int EggSteps { get; private set; }

        public int BallUsed { get; private set; }
        public int Mail { get; private set; }

        public SeriMove[] Moves { get; private set; }

        public int[] Ribbons { get; private set; }
        public bool[] Markings { get; private set; }

        /// <summary>
        /// Trading/Obtaining
        /// </summary>
        public int PersonalId { get; private set; }
        public string PublicId { get; private set; }

        public int ObtainedMethod { get; private set; }
        public DateTimeOffset TimeReceived { get; private set; }
        public DateTimeOffset TimeEggHatched { get; private set; }
        #endregion

        #region Methods
        public SeriMove[] GetMoves()
        {
            return Moves;
        }
        #endregion

        public static implicit operator Pokemon(SeriPokemon pokemon)
        {
            Ribbon[] ribbons = new Ribbon[pokemon.Ribbons.Length];
            for (int i = 0; i < ribbons.Length; i++)
            {
                ribbons[i] = (Ribbon)pokemon.Ribbons[i];
            }

            Move[] moves = new Attack.Move[pokemon.Moves.Length];
            for (int i = 0; i < moves.Length; i++)
            {
                moves[i] = pokemon.Moves[i];
            }
            
            Pokemon normalPokemon = 
                new Pokemon
                (
                    pokemon.NickName, pokemon.Form,
                    (Pokemons)pokemon.Species, (Abilities)pokemon.Ability,
                    pokemon.Nature, pokemon.IsShiny, pokemon.Gender,
                    pokemon.Pokerus, pokemon.PokerusStrain, pokemon.ShadowLevel,
                    pokemon.CurrentHP, (Items)pokemon.Item, pokemon.IV, pokemon.EV,
                    pokemon.ObtainedLevel, pokemon.CurrentLevel, pokemon.CurrentExp,
                    pokemon.Happines, (Status)pokemon.Status, pokemon.StatusCount,
                    pokemon.EggSteps, (Items)pokemon.BallUsed, /*Fix Mail*/ new Item.Mail(Items.AIR_MAIL), 
                    moves, ribbons, pokemon.Markings, pokemon.PersonalId,
                    (Pokemon.ObtainedMethod)pokemon.ObtainedMethod,
                    pokemon.TimeReceived, pokemon.TimeEggHatched
                );
            return normalPokemon;
        }

        public static implicit operator SeriPokemon(Pokemon pokemon)
        {
            SeriPokemon seriPokemon = new SeriPokemon();

            seriPokemon.Species = (int)pokemon.Species;
            seriPokemon.Form = pokemon.Form;
            //Creates an error System OutOfBounds inside Pokemon
            seriPokemon.NickName = pokemon.Name;

            seriPokemon.Ability = (int)pokemon.Ability;
            seriPokemon.Nature = pokemon.getNature();

            seriPokemon.IsShiny = pokemon.IsShiny;
            seriPokemon.Gender = pokemon.Gender;

            seriPokemon.PokerusStage = pokemon.PokerusStage;
            seriPokemon.Pokerus = pokemon.Pokerus;
            seriPokemon.PokerusStrain = pokemon.PokerusStrain;

            seriPokemon.IsHyperMode = pokemon.isHyperMode;
            seriPokemon.IsShadow = pokemon.isShadow;
            seriPokemon.ShadowLevel = pokemon.ShadowLevel;

            seriPokemon.CurrentHP = pokemon.HP;
            seriPokemon.Item = (int)pokemon.Item;

            seriPokemon.IV = pokemon.IV;
            seriPokemon.EV = pokemon.EV;

            seriPokemon.ObtainedLevel = pokemon.ObtainLevel;
            seriPokemon.CurrentLevel = pokemon.Level;
            seriPokemon.CurrentExp = pokemon.Exp.Current;

            seriPokemon.Happines = pokemon.Happiness;

            seriPokemon.Status = (int)pokemon.Status;
            seriPokemon.StatusCount = pokemon.StatusCount;

            seriPokemon.EggSteps = pokemon.EggSteps;

            seriPokemon.BallUsed = (int)pokemon.ballUsed;
            //seriPokemon.Mail = pokemon.GetMail();

            for (int i = 0; i < 4; i++)
            {
                seriPokemon.Moves[i] = pokemon.moves[i];
            }

            seriPokemon.Ribbons = new int[pokemon.Ribbons.Count];
            for (int i = 0; i < seriPokemon.Ribbons.Length; i++)
            {
                seriPokemon.Ribbons[i] = (int)pokemon.Ribbons[i];
            }
            seriPokemon.Markings = pokemon.Markings;

            seriPokemon.PersonalId = pokemon.PersonalId;
            seriPokemon.PublicId = pokemon.PublicId;

            seriPokemon.ObtainedMethod = (int)pokemon.ObtainedMode;
            seriPokemon.TimeReceived = pokemon.TimeReceived;
            seriPokemon.TimeEggHatched = pokemon.TimeEggHatched;

            return seriPokemon;
        }
    }
}
