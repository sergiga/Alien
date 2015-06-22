using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public Button StartButton;
	public Button RankButton;
	public Button ExitButton;
	public Button PlayButton;
	public Button MenuButton;
	public Button NextCharacter;
	public Button PreviousCharacter;

	private bool gameOverMenu;

	void Update () {

		if (GameManager.instance.gameOver && !gameOverMenu) {

			gameOverMenu = true;
			TapToPlayMenu ("Enable");
			Menu ("Enable");
		}
	}

	public void StartButtonPressed () {

		if (!GameManager.instance.changeCharacter) {
			MainMenu ("Disable");
			ChangeCharacter ("Disable");
			TapToPlayMenu ("Enable");
		}
	}

	public void RankingButtonPressed () {


	}

	public void MenuButtonPressed () {

		Menu ("Disable");
		TapToPlayMenu ("Disable");
		ChangeCharacter ("Enable");
		MainMenu ("Enable");
	}

	public void PlayButtonPressed () {

		if(MenuButton.transform.position.x > 0)
			Menu ("Disable");
		TapToPlayMenu ("Disable");
		GameManager.instance.StartGame ();
		gameOverMenu = false;
	}

	public void NextCharacterPressed() {

		if(!GameManager.instance.changeCharacter)
			GameManager.instance.ChangeCharacter (1);
	}

	public void PreviousCharacterPressed() {

		if(!GameManager.instance.changeCharacter)
			GameManager.instance.ChangeCharacter (-1);
	}

	public void MainMenu (string state) {

		StartButton.animator.SetTrigger (state);
		RankButton.animator.SetTrigger (state);
		ExitButton.animator.SetTrigger (state);
	}

	public void TapToPlayMenu (string state) {

		PlayButton.animator.SetTrigger (state);
	}

	public void Menu (string state) {
		
		MenuButton.animator.SetTrigger (state);
	}

	public void ChangeCharacter (string state) {

		NextCharacter.animator.SetTrigger (state);
		PreviousCharacter.animator.SetTrigger (state);
	}

	public void ExitGame() {

		Application.Quit();
	}
}
