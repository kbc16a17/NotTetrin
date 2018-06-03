using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame {
    public class HoldMino : MonoBehaviour {
        [SerializeField]
        private MinoResolver resolver;
        [SerializeField]
        private Transform frame;

        private Animator animator;
        private AudioSource sound;

        private GameObject obj;

        public int? Value { get; private set; } = null;

        private void Awake() {
            animator = GetComponent<Animator>();
            sound = GetComponent<AudioSource>();
        }

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

            animator.Play(@"HoldAnimation", 0, 0.0f);
            sound.Play();
        }
    }
}