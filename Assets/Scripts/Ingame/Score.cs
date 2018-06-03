﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.Ingame {
    public class Score : MonoBehaviour {
        [SerializeField]
        private Text text;
        [SerializeField]
        private GameObject addScoreObject;

        private Animator animator;

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

        public void Awake() {
            animator = GetComponent<Animator>();
            updateText();
        }

        public void Reset() {
            Value = 0;
        }

        public void Increase(int amount) {
            Value += amount;

            if (amount >= 100) {
                addScoreObject.GetComponent<Animation>().Play();
                addScoreObject.GetComponentsInChildren<Text>()[1].text = $"{amount}";
                animator.Play(@"ScoreIncrement", 0, 0.0f);
            }
        }

        private void updateText() {
            text.text = string.Format("{0:0000000}", score);
        }
    }
}