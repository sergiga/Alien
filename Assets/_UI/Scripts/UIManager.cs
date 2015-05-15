using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	
	public Transform tapButton;
	public Transform tapGameOver;
	public GameObject player;
	
	Animator menuAnim;
	GameObject mainMenuContainer;
	GameObject gameOverContainer;
	GameObject tapToPlayContainer;
	
	void Start() {
		mainMenuContainer = GameObject.Find ("MainMenuContainer");
		gameOverContainer = GameObject.Find ("GameOverMenuContainer");
		tapToPlayContainer = GameObject.Find ("TapToPlayContainer");
		menuAnim = GetComponent<Animator> ();
		
		gameOverContainer.SetActive (false);
		tapToPlayContainer.SetActive(false);
	}
	
	public void TapToPlayButton() {
		menuAnim.SetTrigger ("Start");
	}
	
	public void CharacterSelectRight() {
		GameManager.instance.ChangeCharacter(1);
	}
	
	public void CharacterSelectLeft() {
		GameManager.instance.ChangeCharacter(-1);
	}
	
	public void StartGame() {
		GameManager.instance.move = true;
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
		//GameManager.instance.InitGame ();
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
