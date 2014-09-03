Select *
From (
     Select * From Recipe
     Where Recipe.Name like "%h%"
     AND Recipe.Description like "%k%"
) as init
Join
(Select ingf.* from(
     (Select RecipeID From RecipeItem Where RecipeItem.IngredientID = 18)
     UNION ALL
     (Select RecipeID From RecipeItem Where RecipeItem.IngredientID = 29)
) as ingf Group By ingf.RecipeID Having Count(*)>=2) as result ON result.RecipeID=init.ID