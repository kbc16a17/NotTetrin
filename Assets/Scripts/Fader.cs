using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
    [Header(@"Variables")]
    public Gradient Gradient;
    public float Duration = 1.0f;
    public bool PlayOnAwake = true;
    public bool Reversed = false;

    [Header(@"Fading Target")]
    public SpriteRenderer SpriteRenderer;
    public CanvasGroup CanvasGroup;
    public Image Image;
    public Text Text;

    [Header(@"Transitions")]
    public Fader[] Transitions;

    private bool isAnimating = false;
    private float elapsed;

    private void Awake() {
        var color = Gradient.Evaluate(Reversed ? Duration : 0);

        if (SpriteRenderer) { SpriteRenderer.color = color; }
        if (CanvasGroup) { CanvasGroup.alpha = color.a; }
        if (Image) { Image.color = color; }
        if (Text) { Text.color = color; }

        if (PlayOnAwake) {
            Play();
        }
    }

    public void Play() {
        isAnimating = true;

        if (Reversed) {
            elapsed = Duration;
        }
    }

    public void Stop() {
        isAnimating = false;

        foreach (var transition in Transitions) {
            transition.Play();
        }
    }

    private void Update () {
		if (isAnimating) {
            var time = Mathf.Clamp(elapsed, 0, Duration);
            var color = Gradient.Evaluate(time / Duration);

            if (SpriteRenderer) { SpriteRenderer.color = color; }
            if (CanvasGroup) { CanvasGroup.alpha = color.a; }
            if (Image) { Image.color = color; }
            if (Text) { Text.color = color; }

            if (Reversed && time == 0 || !Reversed && time == Duration) {
                Stop();
                return;
            }

            var deltaTime = Time.deltaTime;
            if (Reversed) { deltaTime *= -1; }
            elapsed += deltaTime;
        }
	}
}
