using UnityEngine;
using System.Collections;

public class PlayerChange : PlayerMove {
	
	private Animator animator;
	private Vector3 direction;
	private Vector3 scale;
	private float speed = 6f;

	void OnEnable() {

		animator = GetComponent<Animator> ();
		direction = new Vector3 (0.8f, 0f, 0f);
		direction *= GameManager.instance.changeDirection;
		scale = new Vector3 (0.8f * GameManager.instance.changeDirection, 0.8f, 0.8f);
		if (direction.x > 0 && transform.position.x < -1f) {
			StartCoroutine (RightChange (0f));
			ChangeSide (scale);
		} else if (direction.x < 0 && transform.position.x > 1f) {
			StartCoroutine (LeftChange (0f));
			ChangeSide (scale);
		} else {
			if (direction.x < 0) {
				StartCoroutine (LeftChange (-5f));
				ChangeSide (scale);
			}
			else {
				StartCoroutine ( RightChange(5f));
				ChangeSide (scale);
			}
		}
	}

	private IEnumerator RightChange(float target) {

		animator.SetTrigger ("move");
		while (transform.position.x < target) {
			transform.position += direction * speed * Time.deltaTime;
			if (transform.position.x > rightBorder.x)
				Destroy(gameObject);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		animator.SetTrigger ("stop");
		GameManager.instance.changeCharacter = false;
		this.GetComponent<PlayerChange> ().enabled = false;
	}

	private IEnumerator LeftChange(float target) {

		animator.SetTrigger ("move");
		while (transform.position.x > target) {
			transform.position += direction * speed * Time.deltaTime;
			if (transform.position.x < rightBorder.x * (-1))
				Destroy(gameObject);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		animator.SetTrigger ("stop");
		ChangeSide (new Vector3(0.8f, 0.8f, 0.8f));
		GameManager.instance.changeCharacter = false;
		this.GetComponent<PlayerChange> ().enabled = false;
	}
}
