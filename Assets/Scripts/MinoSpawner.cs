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

    public void Awake() {
        indices = new int[resolver.Length];
        for (int i = 0; i < indices.Length; i++) {
            indices[i] = i;
        }
        enqueue();
    }

    public GameObject Spawn() {
        var position = new Vector3(ceiling.position.x, ceiling.position.y, 0.0f);

        var index = NextIndices[0];
        var obj = Instantiate(resolver.Get(index), position, Quaternion.identity);
        NextIndices.RemoveAt(0);

        if (NextIndices.Count < resolver.Length) {
            enqueue();
        }

        return obj;
    }

    private void enqueue() {
        NextIndices.AddRange(indices.Shuffle());
    }
}
