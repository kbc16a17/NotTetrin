using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NCMB;
// TODO
using NotTetrin.Ingame;
using NotTetrin.Utility;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    public MinoSpawner MinoSpawner;
    public NextMinos NextMinos;
    public HoldMino HoldMino;
    public Score Score;
    public HighScore HighScore;
    public Ranking Ranking;

    [SerializeField]
    private AudioClip[] bgmClips;

    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;

    private int currentIndex;
    private bool useHold = false;

    private List<GameObject> minos = new List<GameObject>();
    public GameObject CurrentMino => minos.Count != 0 ? minos[minos.Count - 1] : null;

    private AudioSource bgm;
    private AudioSource startSound;
    private AudioSource gameoverSound;

    private void Awake() {
        var sources = GetComponents<AudioSource>();
        bgm = sources[0];
        startSound = sources[1];
        gameoverSound = sources[2];
    }

    private void Start() {
        MinoSpawner.OnSpawn += (sender, args) => NextMinos.UpdateMinos();
        loadRanking();
        roundStart();
    }

    private void Update() {
        if (!useHold && Input.GetButtonDown(@"Hold")) {
            holdMino();
        }
        if (Input.GetButtonDown(@"Escape")) {
            SceneManager.LoadScene(@"Title");
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

        var clipIndex = Random.Range(0, bgmClips.Length - 1);
        bgm.clip = bgmClips[clipIndex];
        Debug.Log($"Playing theme{clipIndex + 1}");
        bgm.Play();

        startSound.Play();
        nextMino();
    }

    private void roundEnd() {
        OnRoundEnd.Invoke();
        bgm.Stop();
        gameoverSound.Play();

        var updated = HighScore.UpdateValue();
        if (updated) {
            saveRanking();
        }
        Invoke("loadRanking", 3.0f);
        Invoke("roundStart", 10.0f);
    }

    private void loadRanking() {
        Ranking.Fetch();
    }

    private void saveRanking() {
        var name = PlayerPrefs.GetString(@"name");
        var score = HighScore.Value;
        var ranker = new Ranker(name, score);
        Ranking.Save(ranker);
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
        Score.Increase(200);
        nextMino();
    }

    private void onMinoHitOnCeiling(object sender, EventArgs args) {
        roundEnd();
    }
}
