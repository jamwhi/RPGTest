using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipeSelector : MonoBehaviour {
  
    public int recipeAmount;
    public string tool;
    public Transform recipeContent;
    public List<RecipeSlot> recipes = new List<RecipeSlot>();
    public RecipeSlot recipePrefab;
    public RecipeSlot selectedSlot = null;
    public RecipeDatabase database;

    void Start() {
        // Add slots
        int i = 0;
        foreach (Recipe fromDatabase in database.database) {
            if (fromDatabase.tool == this.tool) {
                RecipeSlot newRecipe = Instantiate(recipePrefab) as RecipeSlot;
                newRecipe.transform.SetParent(recipeContent);
                newRecipe.name = "Recipe " + i.ToString();
                newRecipe.owner = this;
                newRecipe.recpNum = i;
                newRecipe.recipe = database.FetchRecipeByID(fromDatabase.id);
                newRecipe.image.sprite = newRecipe.recipe.sprite;
                recipes.Add(newRecipe);
                i++;
            }
        }
        recipeAmount = i;
		recipes[0].Select();
	}

    public void MoveUp() {
        if (selectedSlot.recpNum == 0) {
            return;
        } 
        else {
            int nextSlot = selectedSlot.recpNum - 1;
            selectedSlot.Deselect();
            recipes[nextSlot].Select();
            Vector2 endPos = new Vector2(0, 67f * (nextSlot - 2));
            StopAllCoroutines();
            StartCoroutine(SmoothMovement(endPos));
        }
    }

    public void MoveDown() {
        if (selectedSlot.recpNum == (recipeAmount - 1)) {
            return;
        } 
        else {
            int nextSlot = selectedSlot.recpNum + 1;
            selectedSlot.Deselect();
            recipes[nextSlot].Select();
            Vector2 endPos = new Vector2(0, 67f * (nextSlot - 2));
            StopAllCoroutines();
            StartCoroutine(SmoothMovement(endPos));
        }

    }
    
    public void MoveLeft() {
        if(selectedSlot.recpNum == 0) {

        } else {
            int nextSlot = selectedSlot.recpNum - 1;
            selectedSlot.Deselect();
            recipes[nextSlot].Select();
            Vector2 endPos;
            if (nextSlot > 1) {
                 endPos = new Vector2(-(67f * (nextSlot - 1)), 0);
            } 
            else {
                endPos = Vector2.zero;
            }
            StopAllCoroutines();
            StartCoroutine(SmoothMovement(endPos));
        }
    }

    public void MoveRight() {
        if (selectedSlot.recpNum == (recipeAmount - 1) ) {
            

        } else {
            int nextSlot = selectedSlot.recpNum + 1;
            selectedSlot.Deselect();
            recipes[nextSlot].Select();
            Vector2 endPos;
            if (nextSlot < recipeAmount - 1) {
                endPos = new Vector2(-(67f * (nextSlot - 1)), 0);
            }
            else {
                endPos = new Vector2(-67f * (recipeAmount - 3),0);
            }
            StopAllCoroutines();
            StartCoroutine(SmoothMovement(endPos));
        }
    }

    private IEnumerator SmoothMovement(Vector3 end) {

        float sqrRemainingDistance = (recipeContent.transform.localPosition - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {            
            Vector3 newPosition = Vector3.MoveTowards(recipeContent.transform.localPosition, end, 700 * Time.deltaTime);
            recipeContent.transform.localPosition = newPosition;
            sqrRemainingDistance = (recipeContent.transform.localPosition - end).sqrMagnitude;
            if (recipeContent.localPosition.y < 0) {            
                recipeContent.localPosition = Vector2.zero;
                StopAllCoroutines();
            }
            else if(recipeContent.localPosition.y > 390) {
                recipeContent.localPosition = new Vector2(0, 390f);
                StopAllCoroutines();
            }
            yield return null;
        }
    }


}
