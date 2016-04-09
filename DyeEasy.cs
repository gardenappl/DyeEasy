
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
		
		public DyeEasy()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
		
		public override void AddRecipes()
		{
			for(int i = 0; i < Main.recipe.Length; i++)
			{
				var recipe = Main.recipe[i];
				
				if(recipe.createItem.dye != 0 && recipe.createItem.stack == 1) //If result is 1 dye
				{
					int dyeIngredients = 0;
					var foundDyeTypes = new List<int>();
					bool foundBottledWater = false;
					
					foreach(var item in recipe.requiredItem)
					{
						if(item != null)
						{
							if(item.dye != 0)
							{
								dyeIngredients += item.stack;
								foundDyeTypes.Add(item.type);
							}
							else if(item.type == ItemID.BottledWater)
								foundBottledWater = true;
						}
					}
					
					//Lunar dyes (ignore)
					if(foundBottledWater)
						continue;
					//Basic dyes
					if(dyeIngredients == 0)
						recipe.createItem.stack = 3;
					//Compound dyes
					else if(foundDyeTypes.Count > 1)
						recipe.createItem.stack = dyeIngredients;
					
					changedRecipes.Add(i);
				}
			}
		}
		
		public override void Unload() //Revering changes
		{
			foreach(int recipeIndex in changedRecipes)
				if(recipeIndex < Main.recipe.Length) //You never know
					Main.recipe[recipeIndex].createItem.stack = 1;
		}
	}
}
