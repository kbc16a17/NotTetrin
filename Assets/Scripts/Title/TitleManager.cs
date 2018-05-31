using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private Animator titleView;

    private void Start() {
        if (PlayerPrefs.HasKey(@"name")) {
            nameField.text = PlayerPrefs.GetString(@"name");
        }
    }

    public void DeleteLocalData() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(@"Title");
    }
}
