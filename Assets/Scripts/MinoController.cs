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

    // Use this for initialization
    void Start() {
        dropEffect = GetComponentInChildren<ParticleSystem>();
        dropEffect.Play();
    }

    // Update is called once per frame
    void Update() {
        if (isControlable) {
            var rigidbody = GetComponent<Rigidbody2D>();
            var velocity = new Vector2(rigidbody.velocity.x, -fallSpeed);
            var torque = 0.0f;

            var horizontal = Input.GetAxis("Horizontal");
            if (horizontal < 0) {
                velocity.x -= 0.11f;
            }
            if (horizontal > 0) {
                velocity.x += 0.11f;
            }

            var vertical = Input.GetAxis("Vertical");
            if (vertical < 0) {
                pressedFrame = Mathf.Clamp(pressedFrame + 1, 0, peekFrame);
                fallAccelaration = peekAccelaration * softdropInCurve.Evaluate((float)pressedFrame / peekFrame);
            } else {
                pressedFrame = 0;
                fallAccelaration *= 0.86f;
            }

            if (Input.GetButton("Rotate Left")) {
                torque += 2.2f;
            }
            if (Input.GetButton("Rotate Right")) {
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
