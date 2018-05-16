using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    [SerializeField]
    private Text scoreText;

    private int score;

    public event EventHandler ValueChanged;
    public int Value {
        get {
            return score;
        }
        private set {
            score = value;
            updateText();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Start () {
        Value = 0;
	}

    public void Reset() {
        Value = 0;
    }

    public void Increase(int amount) {
        Value += amount;
    }

    private void updateText() {
        scoreText.text = string.Format("Score: {0:0000000}", score);
    }
}
