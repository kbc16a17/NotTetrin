using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private Score score;

    private int highScore;

    public event EventHandler ValueChanged;
    public int Value {
        get {
            return highScore;
        }
        private set {
            highScore = value;
            updateText();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

	void Start () {
        score.ValueChanged += onScoreValueChanged;
        Value = 0;
	}
    
    private void onScoreValueChanged(object sender, EventArgs args) {
        var score = sender as Score;
        if (score.Value > Value) {
            Value = score.Value;
        }
    }

    private void updateText() {
        highScoreText.text = string.Format("HighScore: {0:0000000}", highScore);
    }
}
