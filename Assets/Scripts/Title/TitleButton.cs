using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour, IPointerEnterHandler {
    [SerializeField]
    private AudioSource selectSound;
    [SerializeField]
    private AudioSource decideSound;

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
        decideSound.Play();
        SceneManager.LoadScene(DestinationSceneName);
    }
}
