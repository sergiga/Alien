using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject[] characterPrefab;
	public Vector3 spawnPoint;
	public Vector3 restartPoint;

	[HideInInspector] public float bombRate = 0f;
	[HideInInspector] public bool changeCharacter = false;
	[HideInInspector] public bool gameOver = true;
	[HideInInspector] public bool move = false;

	private GameObject player;
	private BombGenerator bombGenerator;
	private int difficultyCount = 0;
	private int characterIndex = 0;
	private int score = 0;

	void Start() {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		spawnPoint = Camera.main.ViewportToWorldPoint (spawnPoint);
		spawnPoint = new Vector3 (spawnPoint.x, -1.671f, -1.25f);
		restartPoint = new Vector3 (0f, -1.671f, -1.25f);
		bombGenerator = GameObject.Find ("Generator").GetComponent<BombGenerator> ();
	}

	public void DodgeBomb() {

		if (!gameOver) {
			score++;
			difficultyCount++;
			UpdateDifficulty();
		}
	}
	
	public void StartGame() {

		bombRate = 0.8f;
		gameOver = false;
		StartCoroutine (bombGenerator.StartGenerator ());
		move = true;
	}

	public void GameOver() {

		GameObject player;

		gameOver = true;
		move = false;
		player = Instantiate (characterPrefab [characterIndex], spawnPoint, Quaternion.identity) as GameObject;
		player.name = "Player";	
		changeCharacter = true;
	}

	void UpdateDifficulty() {

		if (difficultyCount == 10 && bombRate > 0.5f) {
			difficultyCount = 0;
			bombRate -= 0.05f;
		}
		if (score > 100 && difficultyCount == 100 && bombRate > 0.4f) {
			bombRate -= 0.005f;
		}
	}

	public void ChangeCharacter(int direction) {
		
		characterIndex += direction;
		if (characterIndex < 0) {
			characterIndex = characterPrefab.Length - 1;
		} 
		else if (characterIndex == characterPrefab.Length) {
			characterIndex = 0;
		}
		GameObject currentPlayer = GameObject.Find("Player");
		GameObject newPlayer = Instantiate(characterPrefab[characterIndex], spawnPoint, Quaternion.identity) as GameObject;
		changeCharacter = true;
		currentPlayer.name = "Last Player";
		newPlayer.name = "Player";
	}
}
