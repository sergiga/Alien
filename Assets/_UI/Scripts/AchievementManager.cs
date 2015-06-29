using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {

	public static AchievementManager instance;

	public GameObject achievementPrefab;
	public GameObject visualAchievement;
	public GameObject achievementMenu;

	public Sprite[] images;
	public Sprite[] rewards;
	public Sprite unlockedSprite;

	public Text pointText;

	public float fadeTime = 1f;

	public Dictionary <string,Achievement> achievementDict = new Dictionary<string, Achievement>();

	public List<Achievement> rewardAchievements = new List<Achievement>();

	private bool showingAchievementMenu;

	void Start() {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		PlayerPrefs.DeleteAll ();

		// Testing achievements
		CreateAchievement ("General", "Easy", "Dodge 1 bomb in a single game.", 0, 10, 0, false);
		CreateAchievement ("General", "Normal", "Dodge 5 bombs in a single game.", 0, 10, 0, false);
		CreateAchievement ("General", "Hard", "Dodge 10 bombs in a single game.", 0, 10, 0, false);
		CreateAchievement ("General", "Master of bombs", "Earn all the dodge bomb achievements.", 0, 10, 1, true, new string[] {"Easy","Normal","Hard"});
		CreateAchievement ("General", "PlayOne", "Play one game", 0, 10, 2, true);
		CreateAchievement ("General", "PlayBlue", "Play one game", 0, 10, 3, true);
		CreateAchievement ("General", "UnlockAll", "Unlock all the characters", 0, 10, 4, true, new string[] {"Master of bombs","PlayOne","PlayBlue"});

		/*
		// True Achievements

		// Dogde bombs in a single game
		CreateAchievement ("General", "Easy", "Dodge 10 bomb in a single game.", 0, 10, 0, false);
		CreateAchievement ("General", "Normal", "Dodge 50 bombs in a single game.", 0, 10, 0, false);
		CreateAchievement ("General", "Hard", "Dodge 100 bombs in a single game.", 0, 10, 0, false);
		CreateAchievement ("General", "Insane", "Dodge 150 bombs in a single game.", 0, 10, 1, true);
		// Dodge Bombs
		CreateAchievement ("General", "Easy One", "Dodge 100 bombs.", 0, 10, 0, false);
		CreateAchievement ("General", "Normal One", "Dodge 200 bombs.", 0, 10, 0, false);
		CreateAchievement ("General", "Hard One", "Dodge 500 bombs.", 0, 10, 2, true);
		// Games played
		CreateAchievement ("General", "Easy Two", "Play 10 games.", 0, 10, 0, false);
		CreateAchievement ("General", "Normal Two", "Play 50 games.", 0, 10, 0, false);
		CreateAchievement ("General", "Hard Two", "Play 100 games.", 0, 10, 0, false);
		CreateAchievement ("General", "Insane Two", "Play 200 games.", 0, 10, 3, true);
		// Unlock all above
		CreateAchievement ("General", "Unlock All", "Unlock all the characters", 0, 10, 4, true, new string[] {"Insane","HardOne","InsaneTwo"});

		*/
		achievementMenu.SetActive (false);
	}

	public void CheckForAchievements(int score, int gamesPlayed) {

		// Testing achievements
		if (score >= 1) {
			EarnAchievement("Easy");
		}
		if (score >= 5) {
			EarnAchievement("Normal");
		}
		if (score >= 10) {
			EarnAchievement("Hard");
		}
		if (gamesPlayed >= 1) {
			EarnAchievement("PlayOne");
			EarnAchievement("PlayBlue");
		}
	}

	public void EarnAchievement(string title) {
	
		if (achievementDict [title].EarnAchievement ()) {
			if (achievementDict[title].UnlockReward) {
				GameObject achievement = Instantiate (visualAchievement) as GameObject;
				SetAchievementInfo(achievement, "EarnAch Canvas", title);
				StartCoroutine(FadeAchievement(achievement));
			}
			pointText.text = "points: " + PlayerPrefs.GetInt("Points");
		}
	}

	public void CreateAchievement(string parent, string title, string description, int imageIndex, int points, int reward, bool unlockReward, string[] dependencies = null) {

		GameObject achievement = Instantiate (achievementPrefab) as GameObject;
		Achievement newAchievement = new Achievement (achievement, title, description, imageIndex, points, reward, unlockReward);
		achievementDict.Add (title, newAchievement);
		SetAchievementInfo (achievement, parent, title);

		if (dependencies != null) {
			foreach(string achievementTitle in dependencies) {
				Achievement dependency = achievementDict[achievementTitle];
				dependency.Child = title;
				newAchievement.AddDependency(dependency);
			}
		}
	}

	public void SetAchievementInfo(GameObject achievement, string parent, string title) {

		achievement.transform.SetParent(GameObject.Find(parent).transform);
		achievement.transform.localScale = new Vector3 (1, 1, 1);
		achievement.transform.GetChild (0).GetComponent<Image> ().sprite = images [achievementDict[title].ImageIndex];
		achievement.transform.GetChild (1).GetComponent<Text> ().text = title;
		achievement.transform.GetChild (2).GetComponent<Text> ().text = achievementDict[title].Description;
		if (achievementDict[title].UnlockReward) {
			achievement.transform.GetChild (3).gameObject.SetActive(true);
			achievement.transform.GetChild (3).GetComponent<Image> ().sprite = rewards [achievementDict[title].Reward];
			achievement.transform.GetChild (4).gameObject.SetActive(false);
		} 
		else {
			achievement.transform.GetChild (3).gameObject.SetActive(false);
			achievement.transform.GetChild (4).gameObject.SetActive(true);
		}
	}

	private IEnumerator FadeAchievement (GameObject achievement) {

		CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup> ();
		float rate = 1.0f / fadeTime;
		float progress = 0.0f;

		yield return new WaitForSeconds (1.5f);
		while (progress < 1.0) {
			canvasGroup.alpha = Mathf.Lerp (1, 0, progress);
			progress += rate * Time.deltaTime;
			yield return null;
		}

		Destroy (achievement);
	}

	public void AchievementButtonPressed () {

		if (!showingAchievementMenu) {
			StartCoroutine (ShowAchievementMenu (false));
		}
	}

	public IEnumerator ShowAchievementMenu (bool playPressed) {
		
		CanvasGroup canvasGroup = GameObject.Find ("Achievements Canvas").GetComponent<CanvasGroup> ();
		float rate = 1.0f / 0.5f;
		float progress = 0.0f;
		int start;
		int end;

		showingAchievementMenu = true;
		if (!playPressed) {
			if (canvasGroup.alpha == 0) {
				achievementMenu.SetActive (!achievementMenu.activeSelf);
				start = 0;
				end = 1;
			} 
			else {
				start = 1;
				end = 0;
			}
		} 
		else if (canvasGroup.alpha == 1) {
			start = 1;
			end = 0;
		}
		else {
			start = 0;
			end = 0;
		}

		while (progress < 1.1) {
			canvasGroup.alpha = Mathf.Lerp (start, end, progress);
			progress += rate * Time.deltaTime;
			yield return null;
		}
		showingAchievementMenu = false;
		if (start == 1 && canvasGroup.alpha == 0) {
			achievementMenu.SetActive (!achievementMenu.activeSelf);
		}
	}
}
