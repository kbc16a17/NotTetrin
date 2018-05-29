using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoController : MonoBehaviour {
    private ParticleSystem dropEffect;

    private static float LimitAngularVelocity = 180.0f;
    private float prevHorizontal;

    private bool isControlable = true;
    private bool onCeiling = false;
    private float fallSpeed = 1.5f;
    private float fallAccelaration = 0.0f;

    public AnimationCurve softdropInCurve;
    private int pressedFrame = 0;
    private static int PeekFrame = 60;
    private static float PeekAccelaration = 5.4f;

    public event EventHandler Hit;
    public event EventHandler HitOnCeiling;

    private AudioSource moveSound;
    private AudioSource turnSound;
    private AudioSource hitSound;

    private Score score;
    private static int ScoreIncrementDuration = 5;

    public void Awake() {
        score = GameObject.Find(@"Score").GetComponent<Score>();

        var sources = GetComponents<AudioSource>();
        moveSound = sources[0];
        turnSound = sources[1];
        hitSound = sources[2];

        dropEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void Start() {
        dropEffect.Play();
    }

    public void Update() {
        if (isControlable) {
            var rigidbody = GetComponent<Rigidbody2D>();
            var velocity = new Vector2(rigidbody.velocity.x, -fallSpeed);
            var torque = 0.0f;

            var horizontal = Input.GetAxis(@"Horizontal");
            if (prevHorizontal <= 0 && horizontal > 0 || prevHorizontal >= 0 && horizontal < 0) {
                moveSound.Play();
            }
            if (horizontal < 0) {
                velocity.x -= 0.1f;
            }
            if (horizontal > 0) {
                velocity.x += 0.1f;
            }
            prevHorizontal = horizontal;

            var vertical = Input.GetAxis(@"Vertical");
            if (vertical < 0) {
                pressedFrame++;
                if (pressedFrame % ScoreIncrementDuration == 0) {
                    score.Increase(1);
                }
                var frames = Mathf.Clamp(pressedFrame, 0, PeekFrame);
                fallAccelaration = PeekAccelaration * softdropInCurve.Evaluate((float)frames / PeekFrame);
            } else {
                pressedFrame = 0;
                fallAccelaration *= 0.86f;
            }

            if (Input.GetButtonDown(@"Rotate Left") || Input.GetButtonDown(@"Rotate Right")) {
                turnSound.Play();
            }
            if (Input.GetButton(@"Rotate Left")) {
                torque += 2.0f;
            }
            if (Input.GetButton(@"Rotate Right")) {
                torque -= 2.0f;
            }

            velocity.y -= fallAccelaration;

            rigidbody.AddTorque(torque);
            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = Mathf.Clamp(rigidbody.angularVelocity, -LimitAngularVelocity, LimitAngularVelocity);

            dropEffect.transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (isControlable && other.gameObject.tag != "Wall") {
            isControlable = false;

            hitSound.Play();
            dropEffect.Stop();
            Destroy(dropEffect.gameObject, 1.0f);

            if (onCeiling) {
                HitOnCeiling?.Invoke(this, EventArgs.Empty);
            } else {
                Hit?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        onCeiling = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        onCeiling = false;
    }
}
