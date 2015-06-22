using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class PlayerController : MonoBehaviour {
	
	const float skinWidth= .015f;
	
	public LayerMask collisionMask;
	public int verticalRayCount = 4;
	
	float verticalRaySpacing;
	
	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;
	
	void Start() {
		
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}
	
	public void Move (Vector3 velocity) {
		
		UpdateRaycastOrigins ();
		collisions.Reset();
		if (velocity.y != 0) {
			VerticalCollision (ref velocity);
		}
		transform.Translate (velocity);
	}

	public IEnumerator Death () {

		SpriteRenderer[] bodyParts = GetComponentsInChildren<SpriteRenderer>();
		Vector3 explosionDirection = new Vector3(0f, 40f, 0f);
		Vector3 fallingDirection = new Vector3(0f, -6f, 0f);
		float rotationSpeed = 2f;
		float midPosition = 6.5f;
		float endPosition = 2f;
		float rotation = 0f;
		float scale;
		
		while (transform.position.y < midPosition) {
			transform.position += explosionDirection * Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		} 

		for (int i = 0; i < bodyParts.Length; i++) {
			bodyParts[i].sortingLayerName = "PlayerDeath";
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

	void VerticalCollision(ref Vector3 velocity) {
		
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;
		
		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft; 
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
			
			if (hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;
				
				collisions.below = directionY == -1;
			}
		} 
	}
	
	void UpdateRaycastOrigins () {
		
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}
	
	void CalculateRaySpacing () {
		
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);
		
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	
	struct RaycastOrigins {
		
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
	
	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public void Reset() {
			
			above = below = false;
			left = right = false;
		}
	}
}