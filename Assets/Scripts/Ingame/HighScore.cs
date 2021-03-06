﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace NotTetrin.Ingame {
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

        public event EventHandler ValueChanged;
        public int Value {
            get {
                if (PlayerPrefs.HasKey(@"high_score")) {
                    return PlayerPrefs.GetInt(@"high_score");
                }
                return 0;
            }
            private set {
                PlayerPrefs.SetInt(@"high_score", value);
                updateText();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Awake() {
            updateText();
        }

        private void Update() {
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

        public bool UpdateValue() {
            if (score.Value > Value) {
                Value = score.Value;
                animationStopwatch.Start();
                return true;
            }

            return false;
        }

        private void updateText() {
            text.text = string.Format("{0:0000000}", Value);
        }
    }
}