using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Achievement {

	private string name;
	private string description;
	private string child;
	private string type;

	private bool unlocked;
	private bool unlockReward;

	private int reward;
	private int points;

	private int goal;
	private int progression;

	private GameObject achievementRef;

	private List<Achievement> dependencies = new List<Achievement>();

	public Achievement(GameObject _achievement, string _name, string _description, int _points, int _reward, bool _unlockReward, string _type, int _goal) {

		achievementRef = _achievement;
		name = _name;
		description = _description;
		unlocked = false;
		unlockReward = _unlockReward;
		points = _points;
		reward = _reward;
		type = _type;
		goal = _goal;

		LoadAchievement ();
	}

	public void AddDependency(Achievement dependency) {
	
		dependencies.Add (dependency);
	}

	public bool EarnAchievement() {

		if (!unlocked && !dependencies.Exists(x => x.unlocked == false)) {
			achievementRef.GetComponent<Image>().sprite = AchievementManager.instance.unlockedSprite;
			achievementRef.transform.GetChild(2).GetComponent<Image>().color = new Color(255, 255, 255, 1);
			if(unlockReward) {
				AchievementManager.instance.rewardAchievements.Add(this);
			}
			SaveAchievement(true);
			if (child != null) {
				AchievementManager.instance.EarnAchievement(child);
			}
			return true;
		} 
		else { 
			return false;
		}
	}

	public void UpdateProgression(int currentProg) {

		progression = (currentProg > goal) ? goal : currentProg;
		achievementRef.transform.GetChild(0).GetComponent<Text> ().text = progression + "/" + goal;
		PlayerPrefs.SetInt (name + goal.ToString(), progression);
		PlayerPrefs.Save ();
	}

	public void SaveAchievement(bool value) {

		int tmpPoints = PlayerPrefs.GetInt ("Points");

		unlocked = value;
		PlayerPrefs.SetInt ("Points", tmpPoints + points);
		PlayerPrefs.SetInt (name, value ? 1 : 0);
		PlayerPrefs.Save ();
	}

	public void LoadAchievement() {

		unlocked = PlayerPrefs.GetInt (name) == 1 ? true : false;
		progression = PlayerPrefs.GetInt (name + goal.ToString(), progression);
		achievementRef.transform.GetChild(0).GetComponent<Text> ().text = progression + "/" + goal;
		if (unlocked) {
			AchievementManager.instance.pointText.text = "Points: " + PlayerPrefs.GetInt("Points");
			achievementRef.GetComponent<Image>().sprite = AchievementManager.instance.unlockedSprite;
			if (unlockReward) {
				AchievementManager.instance.rewardAchievements.Add(this);
				achievementRef.transform.GetChild(2).gameObject.SetActive(true);
				achievementRef.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 1);
			}
		}
	}

	public string Name {
		get {return name;}
		set {name = value;}
	}

	public string Description {
		get { return description; }
		set { description = value; }
	}

	public string Type {
		get { return type; }
		set { type = value; }
	}

	public bool Unlocked {
		get { return unlocked; }
		set { unlocked = value; }
	}

	public bool UnlockReward {
		get { return unlockReward; }
		set { unlockReward = value; }
	}

	public int Reward {
		get { return reward; }
		set { reward = value; }
	}

	public int Goal {
		get { return goal; }
		set { goal = value; }
	}

	public int Progression {
		get { return progression; }
		set { progression = value; }
	}

	public GameObject AchievementRef {
		get { return achievementRef; }
	}

	public string Child {
		get { return child; }
		set { child = value; }
	}

	public int Points {
		get { return points; }
		set { points = value; }
	}
}