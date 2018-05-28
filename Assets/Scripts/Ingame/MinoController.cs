using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class MinoController : MonoBehaviour {
    private ParticleSystem dropEffect;

    private static float LimitAngularVelocity = 180.0f;

    private bool isControlable = true;
    private bool onCeiling = false;
    private float fallSpeed = 1.5f;
    private float fallAccelaration = 0.0f;

    public AnimationCurve softdropInCurve;
    private int pressedFrame = 0;
    private static int peekFrame = 60;
    private static float peekAccelaration = 5.0f;

    public event EventHandler Hit;
    public event EventHandler HitOnCeiling;

    private AudioSource moveSound;
    private AudioSource turnSound;
    private AudioSource hitSound;

    public void Awake() {
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

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
                moveSound.Play();
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                velocity.x -= 0.11f;
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                velocity.x += 0.11f;
            }

            if (Input.GetKey(KeyCode.DownArrow)) {
                pressedFrame = Mathf.Clamp(pressedFrame + 1, 0, peekFrame);
                fallAccelaration = peekAccelaration * softdropInCurve.Evaluate((float)pressedFrame / peekFrame);
            } else {
                pressedFrame = 0;
                fallAccelaration *= 0.86f;
            }

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X)) {
                turnSound.Play();
            }
            if (Input.GetKey(KeyCode.Z)) {
                torque += 2.2f;
            }
            if (Input.GetKey(KeyCode.X)) {
                torque -= 2.2f;
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
