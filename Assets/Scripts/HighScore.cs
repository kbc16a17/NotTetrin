using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Stopwatch = System.Diagnostics.Stopwatch;

public class HighScore : MonoBehaviour {
    [SerializeField]
    private Text text;
    [SerializeField]
    private Score score;

    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    private float animationDuration;

    private static Vector2 START_SCALE = new Vector2(1.0f, 1.25f);
    private static Vector2 END_SCALE = new Vector2(1.0f, 1.0f);
    [SerializeField]
    private Gradient gradient;

    private Stopwatch animationStopwatch = new Stopwatch();
    private bool IsAnimating => animationStopwatch.IsRunning;

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
    
    void Update() {
        if (IsAnimating) {
            var seconds = (float)animationStopwatch.Elapsed.TotalSeconds;
            if (seconds < animationDuration) {
                var t = animationCurve.Evaluate(seconds / animationDuration);
                text.rectTransform.localScale = Vector2.Lerp(START_SCALE, END_SCALE, t);
                text.color = gradient.Evaluate(t);
            } else {
                text.rectTransform.localScale = END_SCALE;
                text.color = gradient.Evaluate(1.0f);
                animationStopwatch.Reset();
            }
        }
    }

    private void onScoreValueChanged(object sender, EventArgs args) {
        var score = sender as Score;
        if (score.Value > Value) {
            Value = score.Value;
            animationStopwatch.Start();
        }
    }

    private void updateText() {
        text.text = string.Format("{0:0000000}", highScore);
    }
}
