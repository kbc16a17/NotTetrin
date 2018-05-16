using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public NextMino NextMino;
    public MinoSpawner MinoSpawner;
    public Score Score;

    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;

	// Use this for initialization
	void Start () {
        nextRound();
	}
	
    private void nextMino() {
        var obj = MinoSpawner.Spawn();
        NextMino.UpdateMinos();
        var controller = obj.GetComponent<MinoController>();
        controller.Hit += onMinoHit;
        controller.HitOnCeiling += onMinoHitOnCeiling;
    }

    private void nextRound() {
        OnRoundStart.Invoke();

        Score.Reset();
        nextMino();
    }

    private void onMinoHit(object sender, EventArgs args) {
        var controller = sender as MinoController;
        controller.Hit -= onMinoHit;
        Score.Increase(100);
        nextMino();
    }

    private void onMinoHitOnCeiling(object sender, EventArgs args) {
        OnRoundEnd.Invoke();
        Invoke("nextRound", 3.0f);
    }
}
