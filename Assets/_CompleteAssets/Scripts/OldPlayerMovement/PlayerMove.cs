using UnityEngine;
using System.Collections;

public abstract class PlayerMove : MonoBehaviour {
	
	public Vector3 leftBorder;
	public Vector3 rightBorder;
	public Rigidbody2D rb2d;

	protected virtual void Start() {

		rb2d = GetComponent<Rigidbody2D> ();
		leftBorder = Camera.main.ViewportToWorldPoint (leftBorder);
		rightBorder = Camera.main.ViewportToWorldPoint (rightBorder);
		leftBorder = new Vector3 (leftBorder.x, -1.671f, -1.25f);
		rightBorder = new Vector3 (rightBorder.x, -1.671f, -1.25f);	
	}

	protected void Move (Vector3 _direction, float _speed) {

		transform.position += _direction * _speed;
	}

	protected IEnumerator Death () {

		Vector3 explosionDirection = new Vector3(0f, 40f, 0f);
		Vector3 fallingDirection = new Vector3(0f, -6f, 0f);
		float rotationSpeed = 2f;
		float midPosition = 6.5f;
		float endPosition = 2f;
		float rotation = 0f;
		float scale;

		rb2d.gravityScale = 0;
		while (transform.position.y < midPosition) {
			transform.position += explosionDirection * Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		} 
		GameManager.instance.RespawnPlayer ();
		transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		transform.position = new Vector3 (transform.position.x, 6.5f, 1f);
		while (transform.position.y > endPosition) {
			scale = Time.deltaTime;
			rotation += rotationSpeed;
			transform.position += fallingDirection * Time.deltaTime;
			transform.localScale -= new Vector3(scale, scale, scale);
			transform.Rotate (0f, 0f, rotation);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		Destroy (gameObject);
	}

	protected void CheckLimits() {

		Vector3 playerPosition = Camera.main.WorldToViewportPoint (transform.position);
		Vector3 border = leftBorder;

		if (playerPosition.x > 1.02f) {
			border = leftBorder;
			transform.position = border;
		} 
		else if (playerPosition.x < -0.02f) {
			leftBorder.x *= -1;
			transform.position = border;
		}
	}

	protected void ChangeSide (Vector3 _scale) {

		transform.localScale = _scale;
	}
}
