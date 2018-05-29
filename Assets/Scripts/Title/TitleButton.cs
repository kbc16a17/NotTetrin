using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour {
    public string DestinationSceneName;

    public void OnClick() {
        SceneManager.LoadScene(DestinationSceneName);
    }
}
