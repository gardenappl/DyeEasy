
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DyeEasy
{
	public class DyeEasy : Mod
	{
		List<int> changedRecipes = new List<int>();
		
		public override void SetModInfo(out string name, ref ModProperties properties)
		{
			name = "DyeEasy";
			properties.Autoload = true;
			properties.AutoloadGores = true;
			properties.AutoloadSounds = true;
		}
		
		public override void AddRecipes()
		{
			for(int i = 0; i < Main.recipe.Length; i++)
			{
				Recipe recipe = Main.recipe[i];
				
				if(recipe.createItem.dye != 0 && recipe.createItem.stack == 1) //If result is 1 dye
				{
					int dyeIngredients = 0;
					bool bottledWater = false;
					bool otherIngredients = false;
					List<int> foundDyeTypes = new List<int>();
					
					foreach (Item item in recipe.requiredItem)
					{
						if(item != null && item.type != 0)
						{
							if (item.dye != 0)
							{
								dyeIngredients += item.stack;
								foundDyeTypes.Add(item.type);
							}
							else if(item.type == ItemID.BottledWater)
								bottledWater = true;
							else
								otherIngredients = true;
						}
					}
							
					if(otherIngredients && dyeIngredients == 0 && !bottledWater) //Basic dyes
					{ 
						changedRecipes.Add(i);
						recipe.createItem.stack = 3;
					}
					if(dyeIngredients > 0 && !bottledWater && !otherIngredients) //Compound dyes
					{
						if(foundDyeTypes.Count == 1) //If only one type of dye is used in the recipe
						   continue; //Then don't change (ignore Intense Flame/Rainbow dyes etc.)
						changedRecipes.Add(i);
						recipe.createItem.stack = dyeIngredients;
					}
				}
			}
		}
		
		public override void Unload() //Revering changes
		{
			foreach(int recipeIndex in changedRecipes)
				Main.recipe[recipeIndex].createItem.stack = 1;
		}
	}
}
