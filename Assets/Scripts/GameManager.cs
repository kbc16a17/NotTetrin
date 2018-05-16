using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject MinoSpawner;
    [SerializeField]
    private Score score;

    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;

	// Use this for initialization
	void Start () {
        NextRound();
	}
	
    private void NextMino() {
        var obj = MinoSpawner.GetComponent<MinoSpawner>().Spawn();
        var controller = obj.GetComponent<MinoController>();
        controller.Hit += OnMinoHit;
        controller.HitOnCeiling += OnMinoHitOnCeiling;
    }

    private void NextRound() {
        OnRoundStart.Invoke();

        score.Reset();
        NextMino();
    }

    private void OnMinoHit(object sender, EventArgs args) {
        var controller = sender as MinoController;
        controller.Hit -= OnMinoHit;
        score.Increase(100);
        NextMino();
    }

    private void OnMinoHitOnCeiling(object sender, EventArgs args) {
        OnRoundEnd.Invoke();
        Invoke("NextRound", 3.0f);
    }
}
