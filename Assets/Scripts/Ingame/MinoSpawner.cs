using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotTetrin.Utility;

public class MinoSpawner : MonoBehaviour {
    [SerializeField]
    private Transform ceiling;
    [SerializeField]
    private MinoResolver resolver;
    private int[] indices;

    [HideInInspector]
    public List<int> NextIndices { get; private set; } = new List<int>();
    [HideInInspector]
    public int LastIndex { get; private set; }

    private Vector3 spawnPosition => new Vector3(ceiling.position.x, ceiling.position.y, 0.0f);

    public event EventHandler OnSpawn;

    public void Awake() {
        indices = new int[resolver.Length];
        for (int i = 0; i < indices.Length; i++) {
            indices[i] = i;
        }
    }

    public void Clear() {
        NextIndices.Clear();
    }

    public GameObject Spawn() {
        if (NextIndices.Count < resolver.Length) {
            enqueue();
        }

        LastIndex = NextIndices[0];
        var obj = Instantiate(resolver.Get(LastIndex), spawnPosition, Quaternion.identity);
        NextIndices.RemoveAt(0);

        OnSpawn?.Invoke(this, EventArgs.Empty);

        return obj;
    }

    public GameObject Spawn(int index) {
        var obj = Instantiate(resolver.Get(index), spawnPosition, Quaternion.identity);
        return obj;
    }

    private void enqueue() {
        NextIndices.AddRange(indices.Shuffle());
    }
}
