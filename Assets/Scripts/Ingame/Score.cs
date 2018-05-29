﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Stopwatch = System.Diagnostics.Stopwatch;

public class Score : MonoBehaviour {
    [SerializeField]
    private Text text;
    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    private float animationDuration;

    private static Vector2 START_SCALE = new Vector2(1.0f, 1.25f);
    private static Vector2 END_SCALE = new Vector2(1.0f, 1.0f);
    [SerializeField]
    private Gradient gradient;

    private int score;
    private Stopwatch animationStopwatch = new Stopwatch();
    private bool IsAnimating => animationStopwatch.IsRunning;

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

    public void Update() {
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

    public void Increase(int amount) {
        Value += amount;

        if (amount >= 100) {
            animationStopwatch.Start();
        }
    }

    private void updateText() {
        text.text = string.Format("{0:0000000}", score);
    }
}