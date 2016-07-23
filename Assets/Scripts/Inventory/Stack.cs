using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stack : MonoBehaviour {

    public Inventory inventory;
    public MouseController mouseController;
    public GameObject stack;
    public InputField stackAmount;
    public Slider stackSlider;
    public ItemData itemToSeperate;
    public AudioController audioController;
    public AudioClip menuSound;
    public bool isActive = false;
    public int maxStack = 20;

    void Start(){
        stack.SetActive(false);
    }

    // Activate the stack window
    public void Activate(ItemData item, Vector2 pos) {

        itemToSeperate = item;
        maxStack = itemToSeperate.amount;
        stackSlider.maxValue = maxStack;
        stackSlider.value = 1;

        stack.SetActive(true);
        isActive = true;
        stack.transform.position = pos;
        mouseController.CreateFrontBlocker();
    }

    public void Deactivate() {
        audioController.PlaySfx(menuSound);
        stack.SetActive(false);
        isActive = false;
        mouseController.DisableAllFrontLayer();
    }
    
    // Confirm button callback
    public void StackConfirm() {

        if (stackSlider.value == maxStack) {
			itemToSeperate.slot.item = null;
            mouseController.AttachItemToMouse(itemToSeperate);
        }
        else {
            itemToSeperate.amount = itemToSeperate.amount - (int) stackSlider.value;
            ItemData newItems = inventory.CreateItemFromStack(itemToSeperate.item.id, (int)stackSlider.value);
            mouseController.AttachItemToMouse(newItems); 
        }
        Deactivate();
    }
    

    // Update the input field text
    public void SliderChanged(float fromSlider) {

        stackAmount.text = ((int)fromSlider).ToString("##");
        return;
    }

    // Update the slider value
    public void TextInputChanged(string fromInput) {

        int inputVal;

        try {
            inputVal = int.Parse(fromInput);
        }
        catch {
            inputVal = 1;
        }

        if (inputVal < 1) {
            inputVal = 1;
            stackAmount.text = "1";
        }
        else if (inputVal > maxStack) {
            inputVal = maxStack;
            stackAmount.text = maxStack.ToString();
        }

        stackSlider.value = inputVal;
    }

    // Check the value if a person presses Enter to end the input interaction
    public void CheckEndInputValue(string fromInput) {

        int inputVal;

        try {
            inputVal = (int)stackSlider.value;
        }
        catch {
            inputVal = 1;
        }

        stackSlider.value = inputVal;
        stackAmount.text = inputVal.ToString();

    }
}
