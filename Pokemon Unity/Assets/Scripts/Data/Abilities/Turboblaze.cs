﻿using System.Collections;

namespace Data.Abilities
{
    public class Turboblaze : AbilityData
    {
        public Turboblaze()
        {
            name = "Turboblaze";
            fr_name = "Turbo-Brasier";
        }

        public override IEnumerator EffectOnSent(BattleHandler battleHandler, int pokemonIndex, int other)
        {
            DialogBoxHandlerNew dialog = battleHandler.getDialog();
            Pokemon user = battleHandler.getPokemon(pokemonIndex);
            string dialog_string = Language.getLang() switch
            {
                Language.Country.FRANCAIS => " dégage une aura\nde flammes incandescentes!",
                _ => " is radiating\na blazing aura!"
            };
            
            yield return base.EffectOnSent(battleHandler, pokemonIndex);

            yield return battleHandler.StartCoroutine(battleHandler.DisplayAbility(pokemonIndex, GetLangName()));

            dialog.DrawBlackFrame();
            yield return battleHandler.StartCoroutine(battleHandler.drawTextAndWait(battleHandler.preStringName(pokemonIndex) + dialog_string, 2f, 1f));
            dialog.UndrawDialogBox();

            //TODO Pressure effect
            
            yield return battleHandler.StartCoroutine(battleHandler.HideAbility(pokemonIndex));
        }
    }
}