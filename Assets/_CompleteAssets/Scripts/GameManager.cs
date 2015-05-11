using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public Text finalScoreText;
	public Vector3 initialPosition;
	public GameObject playerPrefab;
	public bool isDeath;

	PlayerMovement playerMovement;
	BombGenerator bombGenerator;
	int score = 0;
	int difficultyChange = 0;
	float difficulty = 0.8f;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		finalScoreText = GameObject.Find ("FinalScoreText").GetComponent<Text> ();
		bombGenerator = GameObject.Find("Generator").GetComponent<BombGenerator> ();
		playerMovement = GameObject.Find ("Player").GetComponent<PlayerMovement> ();
	}

	void Update() {
		if (difficultyChange == 10 && difficulty > 0.5f) {
			difficultyChange = 0;
			difficulty -= 0.05f;
			bombGenerator.bombRate = difficulty;
		}
		if (score > 100 && difficultyChange == 100 && difficulty > 0.4f) {
			difficulty -= 0.005f;
		}
	}

	public void InitGame() {
		GameObject currentPlayer = Instantiate (playerPrefab, initialPosition, Quaternion.identity) as GameObject;
		currentPlayer.name = "Player";
		score = 0;
		isDeath = false;
		playerMovement = currentPlayer.GetComponent<PlayerMovement> ();
	}

	public void GameOver() {
		isDeath = true;
		difficultyChange = 0;
		difficulty = 0.8f;
		playerMovement.playerDeath ();
		bombGenerator.enabled = false;
		finalScoreText.text = score.ToString ();
	}

	public void DodgeBomb() {
		if (!isDeath) {
			score++;
			difficultyChange++;
		}
	}
}
