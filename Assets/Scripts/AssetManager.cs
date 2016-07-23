using UnityEngine;
using System.Collections;

public class AssetManager : MonoBehaviour {

	private static AssetManager _instance;
	
    public ItemDatabase itemDatabase;
    public RecipeDatabase recipeDatabase;
    public Equipment equipment;
    public CraftingControl crafting;
    public MouseController mouseController;
    public Transaction transaction;
    public MenuController menuController;
    public Inventory playerInv;
    public AudioController audioController;  
    public Stack stack;
    public Tooltip tooltip;

	protected AssetManager () { }

	void Awake() {
		if (_instance == null) {
			_instance = this;
		}
		else {
			Debug.Log("Error: More than 1 AssetManager??");
		}
	}

	public static AssetManager GetInstance() {
		if (_instance == null) {
			_instance = FindObjectOfType<AssetManager>();
			if (_instance == null) {
				Debug.Log("Error: Found no instance of singleton AssetManager in scene.");
				return null;
			}
		}
		return _instance;
	}
}
