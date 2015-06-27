using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject[] characterPrefab;
	public Vector3 restartPoint;

	[HideInInspector] public float bombRate = 0f;
	[HideInInspector] public bool changeCharacter = false;
	[HideInInspector] public int changeDirection = 1;
	[HideInInspector] public bool gameOver = true;
	[HideInInspector] public bool respawn = true;
	[HideInInspector] public bool move = false;

	private GameObject player;
	private GameObject scoreText;

	private BombGenerator bombGenerator;

	private int difficultyCount = 0;
	private int characterIndex = 0;

	private int score = 0;
	private int gamesPlayed = 0;

	public List<GameObject> unlockedCharacters = new List<GameObject>();

	void Start() {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		restartPoint = new Vector3 (0f, -1.688f, 0f);
		bombGenerator = GameObject.Find ("Generator").GetComponent<BombGenerator> ();
		scoreText = GameObject.Find ("Score Text");
		scoreText.SetActive (false);
		gameOver = false;
		gamesPlayed = PlayerPrefs.GetInt ("GamesPlayed");
		CheckUnlockedCharacters ();
	}

	public void StartGame() {

		bombRate = 0.8f;
		gameOver = false;
		scoreText.SetActive (true);
		StartCoroutine (bombGenerator.StartGenerator ());
		move = true;
	}

	public void GameOver() {

		gameOver = true;
		move = false;
		scoreText.GetComponent<Text> ().text = "0";
		scoreText.SetActive (false);
		gamesPlayed++;
		PlayerPrefs.SetInt ("GamesPlayed", gamesPlayed);	
		AchievementManager.instance.CheckForAchievements (score, gamesPlayed);
		CheckUnlockedCharacters ();
		score = 0;
	}
	public void RespawnPlayer () {

		player = Instantiate (unlockedCharacters [characterIndex], restartPoint, Quaternion.identity) as GameObject;
		player.name = "Player";
	}

	public void DodgeBomb() {
		
		if (!gameOver) {
			score++;
			scoreText.GetComponent<Text> ().text = score.ToString();
			difficultyCount++;
			UpdateDifficulty();
		}
	}
	
	public void ChangeCharacter(int direction) {
		
		characterIndex += direction;
		changeCharacter = true;
		if (characterIndex < 0) {
			characterIndex = unlockedCharacters.Count - 1;
		} 
		else if (characterIndex == unlockedCharacters.Count) {
			characterIndex = 0;
		}
		GameObject currentPlayer = GameObject.Find("Player");
		Destroy(currentPlayer);
		GameObject newPlayer = Instantiate(unlockedCharacters[characterIndex], restartPoint, Quaternion.identity) as GameObject;
		newPlayer.name = "Player";
		changeCharacter = false;
	}

	private void UpdateDifficulty() {
		
		if (difficultyCount == 10 && bombRate > 0.5f) {
			difficultyCount = 0;
			bombRate -= 0.05f;
		}
		if (score > 100 && difficultyCount == 100 && bombRate > 0.4f) {
			bombRate -= 0.005f;
		}
	}

	private void CheckUnlockedCharacters () {

		unlockedCharacters.Clear ();
		unlockedCharacters.Add (characterPrefab [0]);
		foreach (Achievement achievement in AchievementManager.instance.rewardAchievements) {
			unlockedCharacters.Add(characterPrefab[achievement.Reward]);
		}
	}
}
