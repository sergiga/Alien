﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject[] characterPrefab;
	public Vector3 leftSpawnPoint;
	public Vector3 rightSpawnPoint;
	public Vector3 restartPoint;

	[HideInInspector] public float bombRate = 0f;
	[HideInInspector] public bool changeCharacter = false;
	[HideInInspector] public int changeDirection = 1;
	[HideInInspector] public bool gameOver = true;
	[HideInInspector] public bool respawn = true;
	[HideInInspector] public bool move = false;

	private GameObject player;
	private BombGenerator bombGenerator;
	private AchievementManager achManager;
	private int difficultyCount = 0;
	private int characterIndex = 0;
	private int score = 0;

	void Start() {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		restartPoint = new Vector3 (0f, -1.688f, 0f);
		bombGenerator = GameObject.Find ("Generator").GetComponent<BombGenerator> ();
		achManager = GameObject.Find ("AchievementManager").GetComponent<AchievementManager> ();
	}

	public void StartGame() {

		bombRate = 0.8f;
		gameOver = false;
		StartCoroutine (bombGenerator.StartGenerator ());
		move = true;
	}

	public void GameOver() {

		gameOver = true;
		move = false;
		achManager.CheckForAchievements (score);
	}
	public void RespawnPlayer () {

		player = Instantiate (characterPrefab [characterIndex], restartPoint, Quaternion.identity) as GameObject;
		player.name = "Player";
	}

	public void DodgeBomb() {
		
		if (!gameOver) {
			score++;
			difficultyCount++;
			UpdateDifficulty();
		}
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
		changeCharacter = true;
		if (characterIndex < 0) {
			characterIndex = characterPrefab.Length - 1;
		} 
		else if (characterIndex == characterPrefab.Length) {
			characterIndex = 0;
		}
		GameObject currentPlayer = GameObject.Find("Player");
		Destroy(currentPlayer);
		GameObject newPlayer = Instantiate(characterPrefab[characterIndex], restartPoint, Quaternion.identity) as GameObject;
		newPlayer.name = "Player";
		changeCharacter = false;
	}
}
