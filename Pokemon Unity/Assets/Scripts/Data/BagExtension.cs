﻿using PokemonUnity.Character;
using PokemonUnity.Inventory;

public static class BagExtension
{
    public static bool addItem(this Bag bag, Items item, int amount)
    {
        //returns false if will exceed the quantity limit (999)
        if (bag.GetItemAmount(item) + amount > 999)
        {
            return false;
        }
        bag.AddItem(item, amount);
        return true;
    }

    public static bool removeItem(this Bag bag,  Items item, int amount)
    {
        //returns false if trying to remove more items than exist
        if (bag.GetItemAmount(item) - amount < 0)
            return false;
        bag.RemoveItem(item, amount);
        return true;
    }

    public static string[] getItemTypeArray(this Bag bag, ItemPockets itemType, bool allSellables)
    {
        string[] result = new string[bag.Contents.Length];
        int resultPos = 0;
        int length = bag.Contents.Length;
        if (!allSellables)
        {
            for (int s = 0; s < bag[itemType].Count; s++)
            {
                result[resultPos] = bag[itemType].Keys[s].toString();
                resultPos += 1;
            }
        }
        else
        {
            for (int s = 0; s < bag[ItemPockets.MISC].Count; s++)
            {
                result[resultPos] = bag[ItemPockets.MISC].Keys[s].toString();
                resultPos += 1;
            }

            for (int s = 0; s < bag[ItemPockets.MEDICINE].Count; s++)
            {
                result[resultPos] = bag[ItemPockets.MEDICINE].Keys[s].toString();
                resultPos += 1;
            }

            for (int s = 0; s < bag[ItemPockets.BERRY].Count; s++)
            {
                result[resultPos] = bag[ItemPockets.BERRY].Keys[s].toString();
                resultPos += 1;
            }
        }
        string[] cleanedResult = new string[resultPos];
        for (int i = 0; i < cleanedResult.Length; i++)
        {
            cleanedResult[i] = result[i];
        }

        return cleanedResult;
    }
}