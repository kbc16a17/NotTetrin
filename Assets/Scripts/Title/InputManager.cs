using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
    [SerializeField]
    private InputField nameField;

    public void Start() {
        nameField.text = Player.Name;
    }
}
