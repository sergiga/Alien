using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
	
	public Button StartButton;
	public Button AchievementButton;
	public Button RankingButton;
	public Button ExitButton;
	public Button PlayButton;
	public Button MenuButton;
	public Button NextCharacter;
	public Button PreviousCharacter;
	public Button RateButton;
	
	private bool gameOverMenu;
	private bool menuButtonEnabled = false;

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
			AchievementCanvasFade();
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
		GameManager.instance.finalScore.SetActive (false);
	}

	public void PlayButtonPressed () {

		if (menuButtonEnabled) {
			Menu ("Disable");
		}
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
		AchievementButton.animator.SetTrigger (state);
		RankingButton.animator.SetTrigger (state);
		ExitButton.animator.SetTrigger (state);
		RateButton.animator.SetTrigger (state);
	}

	public void TapToPlayMenu (string state) {

		PlayButton.animator.SetTrigger (state);
	}

	public void Menu (string state) {
		
		MenuButton.animator.SetTrigger (state);
		menuButtonEnabled = state.Contains ("Enable") ? true : false;
	}

	public void ChangeCharacter (string state) {

		NextCharacter.animator.SetTrigger (state);
		PreviousCharacter.animator.SetTrigger (state);
	}

	public void AchievementCanvasFade() {

		StartCoroutine(AchievementManager.instance.ShowAchievementMenu(true));
	}

	public void RateGame () {
	
		Application.OpenURL("https://www.google.com");
	}

	public void ExitGame() {

		Application.Quit();
	}
}
