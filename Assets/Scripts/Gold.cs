using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gold : MonoBehaviour {

    public int goldAmount = 0;
    public Text goldDisplay;

    void Start() {
        goldDisplay.text = goldAmount.ToString();
    }
}
