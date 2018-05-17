using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldMino : MonoBehaviour {
    public MinoResolver resolver;
    public Transform frame;

    private GameObject obj;
    public int? Value { get; private set; } = null;

    public void Clear() {
        if (obj != null) {
            Destroy(obj);
        }
        Value = null;
    }

    public void Hold(int index) {
        Clear();

        obj = Instantiate(resolver.Get(index), frame.position, Quaternion.identity);
        Destroy(obj.GetComponent<MinoController>());
        Destroy(obj.GetComponent<Rigidbody2D>());
        Destroy(obj.transform.GetChild(0).gameObject);
        Value = index;
    }
}
