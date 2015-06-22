using UnityEngine;
using System.Collections;

public class PurplePlayerAnim : MonoBehaviour {

	private Animator animator;
	private float blink = 1f;

	void Start() {
		animator = GetComponent<Animator> ();
	}

	void Update () {

		blink -= Time.deltaTime;
		if (blink < 0) {
			string blinkAnim = "blink" +Random.Range(1,4);
			blink = Random.Range(1.0f, 1.5f);
			animator.SetTrigger(blinkAnim);
		}
	}
}
