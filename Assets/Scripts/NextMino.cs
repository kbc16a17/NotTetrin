using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMino : MonoBehaviour {
    public MinoSpawner spawner;
    public MinoResolver resolver;
    public Transform[] frames;

    private List<GameObject> objects = new List<GameObject>();

	public void UpdateMinos () {
        foreach (var obj in objects) {
            Destroy(obj);
        }
        objects.Clear();

        for (int i = 0; i < frames.Length; i++) {
            var index = spawner.NextIndices[i];
            var obj = Instantiate(resolver.Get(index), frames[i].position, Quaternion.identity);
            obj.GetComponent<MinoController>().enabled = false;
            obj.GetComponent<Rigidbody2D>().simulated = false;
            objects.Add(obj);
        }
    }
}
