using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public MinoSpawner MinoSpawner;
    public NextMinos NextMinos;
    public HoldMino HoldMino;
    public Score Score;

    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;

    private int currentIndex;
    private bool useHold = false;

    private List<GameObject> minos = new List<GameObject>();
    public GameObject CurrentMino => minos.Count != 0 ? minos[minos.Count - 1] : null;

    private AudioSource bgm;
    private AudioSource startSound;
    private AudioSource gameoverSound;

    public void Awake() {
        var sources = GetComponents<AudioSource>();
        bgm = sources[0];
        startSound = sources[1];
        gameoverSound = sources[2];
    }

    public void Start () {
        MinoSpawner.OnSpawn += (sender, args) => NextMinos.UpdateMinos();
        roundStart();
	}

    public void Update() {
        if (!useHold && Input.GetKeyDown(KeyCode.LeftShift)) {
            holdMino();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            roundStart();
        }
    }

    private void holdMino() {
        int? value = HoldMino.Value;
        HoldMino.Hold(MinoSpawner.LastIndex);

        Destroy(CurrentMino);
        minos.RemoveAt(minos.Count - 1);

        if (value == null) {
            var obj = MinoSpawner.Spawn();
            changeMino(obj);
        } else {
            var obj = MinoSpawner.Spawn(value.Value);
            changeMino(obj);
        }

        useHold = true;
    }

    private void reset() {
        CancelInvoke("roundStart");
        gameoverSound.Stop();

        Score.Reset();

        MinoSpawner.Clear();

        HoldMino.Clear();
        useHold = false;

        minos.ForEach(Destroy);
        minos.Clear();
    }

    private void roundStart() {
        reset();
        OnRoundStart.Invoke();
        bgm.Play();
        startSound.Play();
        nextMino();
    }

    private void roundEnd() {
        OnRoundEnd.Invoke();
        bgm.Stop();
        gameoverSound.Play();
        Invoke("roundStart", 10.0f);
    }

    private void changeMino(GameObject mino) {
        if (CurrentMino != null) {
            var controller1 = CurrentMino.GetComponent<MinoController>();
            controller1.Hit -= onMinoHit;
            controller1.HitOnCeiling -= onMinoHitOnCeiling;
        }

        var controller2 = mino.GetComponent<MinoController>();
        controller2.Hit += onMinoHit;
        controller2.HitOnCeiling += onMinoHitOnCeiling;
        minos.Add(mino);
    }

    private void nextMino() {
        var obj = MinoSpawner.Spawn();
        changeMino(obj);

        useHold = false;
    }

    private void onMinoHit(object sender, EventArgs args) {
        Score.Increase(100);
        nextMino();
    }

    private void onMinoHitOnCeiling(object sender, EventArgs args) {
        roundEnd();
    }
}
