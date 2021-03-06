﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject[] characterPrefab;
	public Vector3 restartPoint;

	public float startingBombRate = 0.8f;
	public float maxDifficulty = 0.4f;
	public float startingBombGravity = -4f;
	public float maxBombGravity = -15f;

	[HideInInspector] public float bombRate;
	[HideInInspector] public float bombGravity;
	[HideInInspector] public bool changeCharacter = false;
	[HideInInspector] public int changeDirection = 1;
	[HideInInspector] public bool gameOver = true;
	[HideInInspector] public bool respawn = true;
	[HideInInspector] public bool move = false;
	[HideInInspector] public GameObject finalScore;

	private GameObject player;
	private GameObject scoreText;

	private BombGenerator bombGenerator;

	private int difficultyCount = 0;
	private int characterIndex = 0;

	private int score = 0;
	private int bestScore;
	private int totalScore = 0;
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
		finalScore = GameObject.Find ("Final Score");
		finalScore.SetActive (false);
		gameOver = false;
		gamesPlayed = PlayerPrefs.GetInt ("GamesPlayed");
		totalScore = PlayerPrefs.GetInt ("TotalScore");
		bestScore = PlayerPrefs.GetInt ("BestScore");
		CheckUnlockedCharacters ();
	}

	void Update() {

		if (!move) return;

		UpdateDifficulty ();
	}

	public void StartGame() {

		bombRate = startingBombRate;
		bombGravity = startingBombGravity;
		gameOver = false;
		scoreText.SetActive (true);
		finalScore.SetActive (false);
		StartCoroutine (bombGenerator.StartGenerator ());
		move = true;
	}

	public void GameOver() {

		Text finalScoreText = finalScore.GetComponent<Text> ();
		gameOver = true;
		move = false;
		scoreText.GetComponent<Text> ().text = "0";
		scoreText.SetActive (false);
		gamesPlayed++;
		totalScore += score;
		finalScoreText.text = "Score: " + score + "\n";
		if (score > bestScore) {
			PlayerPrefs.SetInt("BestScore", score);
			bestScore = score;
			finalScoreText.text += "New best: " + score;
		}
		else {
			finalScoreText.text += "Best: " + bestScore;
		}
		finalScore.SetActive (true);
		PlayerPrefs.SetInt ("GamesPlayed", gamesPlayed);	
		PlayerPrefs.SetInt ("TotalScore", totalScore);
		AchievementManager.instance.CheckForAchievements (score, gamesPlayed, totalScore);

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
			//UpdateDifficulty();
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

	private void UpdateDifficulty () {

		if (bombRate > maxDifficulty) {
			bombRate -= 0.05f * Time.deltaTime/10;
		}
		if (bombGravity > maxBombGravity) {
			bombGravity -= Time.deltaTime/10;
		}
	}

	/* private void UpdateDifficulty() {
		
		if (difficultyCount == 10 && bombRate > 0.5f) {
			difficultyCount = 0;
			bombRate -= 0.05f;
		}
		if (score > 100 && difficultyCount == 100 && bombRate > 0.4f) {
			bombRate -= 0.005f;
		}
	} */

	private void CheckUnlockedCharacters () {

		unlockedCharacters.Clear ();
		unlockedCharacters.Add (characterPrefab [0]);
		foreach (Achievement achievement in AchievementManager.instance.rewardAchievements) {
			unlockedCharacters.Add(characterPrefab[achievement.Reward]);
		}
	}
}
