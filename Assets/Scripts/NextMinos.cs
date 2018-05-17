using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMinos : MonoBehaviour {
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
            obj.transform.localScale *= 0.75f;
            Destroy(obj.GetComponent<MinoController>());
            Destroy(obj.GetComponent<Rigidbody2D>());
            Destroy(obj.transform.GetChild(0).gameObject);
            objects.Add(obj);
        }
    }
}
