using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {

	public GameObject achievementPrefab;
	public GameObject visualAchievement;
	public GameObject achievementMenu;

	public Sprite[] images;
	public Sprite[] rewards;

	public Dictionary <string,Achievement> achievementDict = new Dictionary<string, Achievement>();

	void Start() {

		CreateAchievement ("General", "Noob", "Dodge 10 bombs in a single game.", 0, 0, true);
		/*foreach (GameObject achievementList in GameObject.FindGameObjectsWithTag("AchievementList")) {

			achievementList.SetActive(false);
		}*/
		achievementMenu.SetActive (false);
	}

	public void CheckForAchievements(int score) {
	
		if (score > 10) {
			EarnAchievement("Noob");
		}
	}

	public void EarnAchievement(string title) {
	
		if (achievementDict [title].EarnAchievement ()) {
			GameObject achievement = Instantiate (visualAchievement) as GameObject;
			SetAchievementInfo(achievement, "EarnAch Canvas", title);
			StartCoroutine(AchievementFade(achievement));
		}
	}

	public IEnumerator AchievementFade(GameObject achievement) {
	
		yield return new WaitForSeconds (3);
		Destroy (achievement);
	}

	public void CreateAchievement(string parent, string title, string description, int reward, int imageIndex, bool unlockReward) {

		GameObject achievement = Instantiate (achievementPrefab) as GameObject;
		Achievement newAchievement = new Achievement (achievement, name, description, imageIndex, reward, unlockReward);
		achievementDict.Add (title, newAchievement);
		SetAchievementInfo (achievement, parent, title);
	}

	public void SetAchievementInfo(GameObject achievement, string parent, string title) {

		achievement.transform.SetParent(GameObject.Find(parent).transform);
		achievement.transform.localScale = new Vector3 (1, 1, 1);
		achievement.transform.GetChild (0).GetComponent<Image> ().sprite = images [achievementDict[title].ImageIndex];
		achievement.transform.GetChild (1).GetComponent<Text> ().text = title;
		achievement.transform.GetChild (2).GetComponent<Text> ().text = achievementDict[title].Description;
		if (achievementDict[title].UnlockReward) {
			achievement.transform.GetChild (3).GetComponent<Image> ().sprite = rewards [achievementDict[title].ImageIndex];
		} 
		else {
			achievement.transform.GetChild (3).GetComponent<Text> ().text = achievementDict[title].Reward.ToString();
		}
	}

	public void ShowAchievementsMenu() {
		achievementMenu.SetActive (!achievementMenu.activeSelf);
	}
}
