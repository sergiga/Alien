using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	const float skinWidth= .015f;

	public AudioClip explosion;
	public LayerMask collisionMask;
	public LayerMask playerCollisionMask;
	public float gravity;
	public int verticalRayCount = 4;

	float verticalRaySpacing;
	
	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	[HideInInspector] public CollisionInfo collisions;

	Animator explode;
	bool dangerous;
	bool grounded;

	Vector3 velocity;

	void Start() {

		dangerous = false;
		grounded = false;
		explode = GetComponent<Animator> ();
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	void Update() {

		if (collisions.below) {
			PlayerAboveExplosion ();
			if (!grounded) {
				velocity.y = 0;
				grounded = true;
				dangerous = true;
				explode.SetTrigger ("explode");
			}
		}
		velocity.y += gravity * Time.deltaTime;
		Move (velocity * Time.deltaTime);
	}

	void Move (Vector3 velocity) {
		
		UpdateRaycastOrigins ();
		collisions.Reset();

		if (velocity.y != 0) {
			VerticalCollision (ref velocity);
		}
		transform.Translate (velocity);
	}

	void PlayerAboveExplosion() {
		float directionY = 1;
		float rayLength = collider.bounds.size.y;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = raycastOrigins.bottomLeft; 
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, playerCollisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
			
			if (hit  && dangerous && !GameManager.instance.gameOver) {
				dangerous = false;
				SoundManager.instance.RandomizeSfx(explosion);
				GameManager.instance.GameOver();
			}
		} 
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

	public void ExplosionEnds() {
		GameObject.Destroy(gameObject);
	}
	
	public void NotDangerous() {
		dangerous = false;
		GameManager.instance.DodgeBomb();
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
