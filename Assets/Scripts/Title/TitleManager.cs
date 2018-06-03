using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace NotTetrin.Title {
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

        private void Update() {
            if (Input.GetButtonDown(@"Escape")) {
                var state = titleView.GetCurrentAnimatorStateInfo(0);
                if (state.fullPathHash == Animator.StringToHash(@"Base Layer.PopupDeleteDialog")) {
                    titleView.Play(@"PopdownDeleteDialog");
                    return;
                }
                Application.Quit();
            }
        }

        public void DeleteLocalData() {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(@"Title");
        }
    }
}