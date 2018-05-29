using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class MinoResolver : MonoBehaviour {
    [SerializeField]
    private GameObject[] prefabs;

    public int Length => prefabs.Length;

    public GameObject Get(int index) => prefabs[index];
}