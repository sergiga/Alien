using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public Transform tapButton;
	public Transform tapGameOver;

	private Animator menuAnim;
	private GameObject mainMenuContainer;
	private GameObject gameOverContainer;
	private GameObject tapToPlayContainer;
	private GameObject player;
	private BombGenerator bombGenerator;
	bool gameOver = false;

	void Awake() {
		mainMenuContainer = GameObject.Find ("MainMenuContainer");
		gameOverContainer = GameObject.Find ("GameOverMenuContainer");
		tapToPlayContainer = GameObject.Find ("TapToPlayContainer");
		menuAnim = GetComponent<Animator> ();
		player = GameObject.Find ("Player");
		bombGenerator = GameObject.Find ("Generator").GetComponent<BombGenerator> ();

		gameOverContainer.SetActive (false);
		tapToPlayContainer.SetActive(false);
	}

	void Update() {
		if (!gameOver && player.transform.position.y > 6) {
			gameOver = true;
			menuAnim.SetTrigger("GameOver");
		}
	}

	public void TapToPlayButton() {
		menuAnim.SetTrigger ("Start");
	}

	public void StartGame() {
		player.GetComponent<PlayerMovement> ().enabled = true;
		bombGenerator.enabled = true;
		mainMenuContainer.SetActive (false);
		tapToPlayContainer.SetActive (false);
		gameOverContainer.SetActive (false);
		tapButton.GetComponent<Button> ().interactable = true;
	}

	public void InitialMenuConfig() {
		mainMenuContainer.SetActive (true);
		gameOverContainer.SetActive (false);
		tapToPlayContainer.SetActive (false);
	}

	public void StartGameButton() {
		menuAnim.SetTrigger ("GameScene");
	}

	public void DisableMenuButton() {
		tapGameOver.GetComponent<Button> ().interactable = false;
	}

	public void DisableTapButton() {
		tapButton.GetComponent<Button> ().interactable = false;
		tapGameOver.GetComponent<Button> ().interactable = false;
	}

	public void EnableTapMenu() {
		mainMenuContainer.SetActive (false);
		gameOverContainer.SetActive (false);
		tapToPlayContainer.SetActive (true);
	}

	public void InitialGameOverConfig() {
		gameOver = false;
		GameManager.instance.InitGame ();
		player = GameObject.Find ("Player");
		tapGameOver.GetComponent<Button> ().interactable = true;
		gameOverContainer.SetActive (true);
	}

	public void Restart() {
		menuAnim.SetTrigger ("Restart");
	}

	public void MenuButton() {
		menuAnim.SetTrigger ("Menu");
		mainMenuContainer.SetActive (true);
	}

	public void ExitGame() {
		Application.Quit();
	}
}
