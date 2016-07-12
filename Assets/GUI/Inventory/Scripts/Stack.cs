using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stack : MonoBehaviour {

    //private InputField amountText;

    public Inventory inventory;
    public GameObject stack;
    public InputField stackAmount;
    public Slider stackSlider;
    public ItemData itemToSeperate;
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
    }

    // Cancel button callback
    public void StackCancel() {
        stack.SetActive(false);
        isActive = false;
    }

    // Confirm button callback
    public void StackConfirm() {

        if (stackSlider.value == maxStack) {
            itemToSeperate.AttachToMouse();
        }
        else {
            itemToSeperate.amount -= (int) stackSlider.value;
            itemToSeperate.transform.GetChild(0).GetComponent<Text>().text = itemToSeperate.amount.ToString();
            inventory.CreateItemFromStack(itemToSeperate.item.ID, (int)stackSlider.value);
        }

        stack.SetActive(false);
        isActive = false;
    }

    // Update the input field text
    public void ChangeStackText(float fromSlider) {

        stackAmount.text = fromSlider.ToString("##");
        return;
    }

    // Update the slider value
    public void ChangeSliderValue(string fromInput) {

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
            inputVal = int.Parse(fromInput);
        }
        catch {
            inputVal = 1;
        }

        stackSlider.value = inputVal;
        stackAmount.text = inputVal.ToString();

    }
}
