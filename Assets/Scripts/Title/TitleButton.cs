using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour, IPointerEnterHandler {
    [SerializeField]
    private AudioSource selectSound;
    [SerializeField]
    private AudioSource decideSound;
    [SerializeField]
    private GameObject errorMessage;

    public string DestinationSceneName;

    public void Update() {
        if (Input.GetButtonDown(@"Submit")) {
            OnClick();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        selectSound.Play();
    }

    public void OnClick() {
        // プレイヤー名が入っていない
        if (string.IsNullOrWhiteSpace(Player.Name)) {
            errorMessage.SetActive(true);
            return;
        }

        decideSound.Play();
        SceneManager.LoadScene(DestinationSceneName);
    }
}
