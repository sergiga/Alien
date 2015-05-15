using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public Button StartButton;
	public Button RankButton;
	public Button ExitButton;
	public Button PlayButton;
	public Button MenuButton;

	private bool gameOverMenu;

	void Update () {

		if (GameManager.instance.gameOver && !gameOverMenu) {

			gameOverMenu = true;
			TapToPlayMenu ("Enable");
			Menu ("Enable");
		}
	}

	public void StartButtonPressed () {

		MainMenu ("Disable");
		TapToPlayMenu ("Enable");
	}

	public void RankingButtonPressed () {


	}

	public void MenuButtonPressed () {
		
		Menu ("Disable");
		TapToPlayMenu ("Disable");
		MainMenu ("Enable");
	}

	public void PlayButtonPressed () {

		TapToPlayMenu ("Disable");
		GameManager.instance.StartGame ();
		gameOverMenu = false;
	}

	public void MenuPressed () {
	

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

	public void ExitGame() {

		Application.Quit();
	}
}
