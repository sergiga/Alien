using UnityEngine;
using System.Collections;

public class GreenPlayerAnim : MonoBehaviour {

	private Animator animator;
	private float blink = 1f;
	private float breath = 1f;

	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	void Update () {

		CheckBlink ();
		if (!GameManager.instance.move) return;
		CheckBreath ();
	}
	
	private void CheckBlink() {
		
		blink -= Time.deltaTime;
		if (blink < 0) {
			blink = Random.Range(4.0f, 5.0f);
			animator.SetTrigger("blink");
		}
	}
	
	private void CheckBreath() {
		
		breath -= Time.deltaTime;
		if (breath < 0) {
			breath = Random.Range(4.0f, 5.0f);
			animator.SetTrigger("breath");
		}
	}
}
