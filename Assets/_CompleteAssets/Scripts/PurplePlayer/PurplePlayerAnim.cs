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
			int blinkChoice = Random.Range(0,7);
			switch(blinkChoice) {
				case 0:
					animator.SetTrigger("blink1");	
					break;
				case 1:
					animator.SetTrigger("blink2");	
					break;
				case 2:
					animator.SetTrigger("blink3");	
					break;
				case 3:
					animator.SetTrigger("blink1");
					animator.SetTrigger("blink2");
					break;
				case 4:
					animator.SetTrigger("blink1");	
					animator.SetTrigger("blink3");	
					break;
				case 5:
					animator.SetTrigger("blink2");
					animator.SetTrigger("blink1");
					break;
				case 6:
					animator.SetTrigger("blink1");
					animator.SetTrigger("blink2");
					animator.SetTrigger("blink3");
					break;
			}
			blink = Random.Range(1.0f, 1.5f);
		}
	}
}
