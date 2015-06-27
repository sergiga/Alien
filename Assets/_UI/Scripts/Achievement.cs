using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Achievement {

	private string name;
	private string description;
	private string child;

	private bool unlocked;
	private bool unlockReward;

	private int reward;
	private int imageIndex;
	private int points;

	private GameObject achievementRef;

	private List<Achievement> dependencies = new List<Achievement>();

	public Achievement(GameObject _achievement, string _name, string _description, int _imageIndex, int _points, int _reward, bool _unlockReward) {

		achievementRef = _achievement;
		name = _name;
		description = _description;
		imageIndex = _imageIndex;
		unlocked = false;
		unlockReward = _unlockReward;
		points = _points;
		reward = _reward;

		LoadAchievement ();
	}

	public void AddDependency(Achievement dependency) {
	
		dependencies.Add (dependency);
	}

	public bool EarnAchievement() {

		if (!unlocked && !dependencies.Exists(x => x.unlocked == false)) {
			achievementRef.GetComponent<Image>().sprite = AchievementManager.instance.unlockedSprite;
			achievementRef.transform.GetChild(3).GetComponent<Image>().color = new Color(255, 255, 255, 1);
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

	public void SaveAchievement(bool value) {

		int tmpPoints = PlayerPrefs.GetInt ("Points");

		unlocked = value;
		PlayerPrefs.SetInt ("Points", tmpPoints + points);
		PlayerPrefs.SetInt (name, value ? 1 : 0);
		PlayerPrefs.Save ();
	}

	public void LoadAchievement() {

		unlocked = PlayerPrefs.GetInt (name) == 1 ? true : false;

		if (unlocked) {
			AchievementManager.instance.pointText.text = "Points: " + PlayerPrefs.GetInt("Points");
			achievementRef.GetComponent<Image>().sprite = AchievementManager.instance.unlockedSprite;
			if (unlockReward) {
				unlockReward = false;
				AchievementManager.instance.rewardAchievements.Add(this);
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

	public int ImageIndex {
		get { return imageIndex; }
		set { imageIndex = value; }
	}

	public GameObject AchievementRef {
		get { return achievementRef; }
	}

	public string Child {
		get { return child; }
		set { child = value; }
	}
}