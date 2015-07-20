using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {

	public static AchievementManager instance;

	public GameObject achievementPrefab;
	public GameObject visualAchievement;
	public GameObject achievementMenu;

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

		//PlayerPrefs.DeleteAll ();

		// Testing achievements
		/*
		CreateAchievement ("General", "Easy", "Dodge 1 bomb in a single game.", 10, 0, false);
		CreateAchievement ("General", "Normal", "Dodge 5 bombs in a single game.", 20, 0, false);
		CreateAchievement ("General", "Hard", "Dodge 10 bombs in a single game.", 30, 0, false);
		CreateAchievement ("General", "Master of bombs", "Earn all the dodge bomb achievements.", 40, 1, true, new string[] {"Easy","Normal","Hard"});
		CreateAchievement ("General", "PlayOne", "Play one game", 40, 2, true);
		CreateAchievement ("General", "PlayBlue", "Play one game", 40, 3, true);
		CreateAchievement ("General", "UnlockAll", "Unlock all the characters", 50, 4, true, new string[] {"Master of bombs","PlayOne","PlayBlue"});
		*/
		

		// True Achievements

		// Dogde bombs in a single game
		CreateAchievement ("General", "Easy", "Dodge 10 bomb in a single game.", 10, 0, false, "single", 0);
		CreateAchievement ("General", "Normal", "Dodge 50 bombs in a single game.", 20, 0, false, "single", 0);
		CreateAchievement ("General", "Hard", "Dodge 100 bombs in a single game.", 30, 0, false, "single", 0);
		CreateAchievement ("General", "Insane", "Dodge 150 bombs in a single game.", 50, 1, true, "single", 0);
		// Dodge Bombs
		CreateAchievement ("General", "Easy Total", "Dodge 100 bombs.", 10, 0, false , "bombs", 100);
		CreateAchievement ("General", "Normal Total", "Dodge 200 bombs.", 20, 0, false, "bombs", 200);
		CreateAchievement ("General", "Hard Total", "Dodge 500 bombs.", 30, 0, false,  "bombs", 500);
		CreateAchievement ("General", "Insane Total", "Dodge 1000 bombs.", 50, 2, true,  "bombs", 1000);
		// Games played
		CreateAchievement ("General", "Easy Deaths", "Play 10 games.", 10, 0, false, "games", 10);
		CreateAchievement ("General", "Normal Deaths", "Play 50 games.", 20, 0, false, "games", 50);
		CreateAchievement ("General", "Hard Deaths", "Play 100 games.", 30, 0, false, "games", 100);
		CreateAchievement ("General", "Insane Deaths", "Play 200 games.", 50, 3, true, "games", 200);
		// Unlock all above
		CreateAchievement ("General", "Unlock All", "Unlock all the characters", 100, 4, true, "total", 0, new string[] {"Insane","Insane Total","Insane Deaths"});


		achievementMenu.SetActive (false);
	}

	public void CheckForAchievements(int score, int gamesPlayed, int totalScore) {

		// Testing achievements
		/*
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
		}*/ 

		// True Achievements
		if (score >= 10) {
			EarnAchievement("Easy");
		}
		if (score >= 50) {
			EarnAchievement("Normal");
		}
		if (score >= 100) {
			EarnAchievement("Hard");
		}
		if (gamesPlayed >= 150) {
			EarnAchievement("Insane");
		}

		if (totalScore > 100) {
			EarnAchievement("Easy Total");
		}
		if (totalScore > 200) {
			EarnAchievement("Normal Total");
		}
		if (totalScore > 500) {
			EarnAchievement("Hard Total");
		}
		if (totalScore > 1000) {
			EarnAchievement("Insane Total");
		}

		if (gamesPlayed > 10) {
			EarnAchievement("Easy Deaths");
		}
		if (gamesPlayed > 50) {
			EarnAchievement("Normal Deaths");
		}
		if (gamesPlayed > 100) {
			EarnAchievement("Hard Deaths");
		}
		if (gamesPlayed > 200) {
			EarnAchievement("Insane Deaths");
		}

		UpdateAchievementProgression (gamesPlayed, totalScore);
	}

	private void UpdateAchievementProgression(int gamesPlayed, int totalScore) {

		foreach (Achievement entry in achievementDict.Values) {
			if (entry.Type.Contains("bombs")) {
				entry.UpdateProgression(totalScore);
			}
			else if (entry.Type.Contains("games")) {
				entry.UpdateProgression(gamesPlayed);
			}
		}
	}

	public void EarnAchievement(string title) {
	
		if (achievementDict [title].EarnAchievement ()) {
			if (achievementDict[title].UnlockReward) {
				GameObject achievement = Instantiate (visualAchievement) as GameObject;
				SetAchievementInfo(achievement, "EarnAch Canvas", title, achievementDict[title].Goal);
				StartCoroutine(FadeAchievement(achievement));
			}
			pointText.text = "points: " + PlayerPrefs.GetInt("Points");
		}
	}

	public void CreateAchievement(string parent, string title, string description, int points, int reward, bool unlockReward, string tipe, int goal, string[] dependencies = null) {

		GameObject achievement = Instantiate (achievementPrefab) as GameObject;
		Achievement newAchievement = new Achievement (achievement, title, description, points, reward, unlockReward, tipe, goal);
		achievementDict.Add (title, newAchievement);
		SetAchievementInfo (achievement, parent, title, goal);

		if (dependencies != null) {
			foreach(string achievementTitle in dependencies) {
				Achievement dependency = achievementDict[achievementTitle];
				dependency.Child = title;
				newAchievement.AddDependency(dependency);
			}
		}
	}

	public void SetAchievementInfo(GameObject achievement, string parent, string title, int goal) {

		achievement.transform.SetParent(GameObject.Find(parent).transform);
		achievement.transform.localScale = new Vector3 (1, 1, 1);
		if (achievementDict[title].Goal != 0) {
			string currentProg = achievementDict[title].Progression + "/" + achievementDict[title].Goal;
			achievement.transform.GetChild (0).gameObject.SetActive(true);
			//achievement.transform.GetChild (0).GetComponent<Text> ().text = currentProg;
		}
		achievement.transform.GetChild (1).GetComponent<Text> ().text = achievementDict[title].Description;
		achievement.transform.GetChild (3).gameObject.SetActive(true);
		achievement.transform.GetChild (4).GetComponent<Text> ().text = achievementDict[title].Points.ToString();
		if (achievementDict[title].UnlockReward) {
			achievement.transform.GetChild (2).gameObject.SetActive(true);
			achievement.transform.GetChild (2).GetComponent<Image> ().sprite = rewards [achievementDict[title].Reward];
		} 
		else {
			achievement.transform.GetChild (2).gameObject.SetActive(false);
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
