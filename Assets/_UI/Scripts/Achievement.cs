using UnityEngine;
using System.Collections;

public class Achievement {

	private string name;

	public string Name {
		get {return name;}
		set {name = value;}
	}

	private string description;

	public string Description {
		get { return description; }
		set { description = value; }
	}

	private bool unlocked;

	public bool Unlocked {
		get { return unlocked; }
		set { unlocked = value; }
	}

	private bool unlockReward;

	public bool UnlockReward {
		get { return unlockReward; }
		set { unlockReward = value; }
	}

	private int reward;

	public int Reward {
		get { return reward; }
		set { reward = value; }
	}

	private int imageIndex;

	public int ImageIndex {
		get { return imageIndex; }
		set { imageIndex = value; }
	}

	private GameObject achievementRef;

	public GameObject AchievementRef {
		get { return achievementRef; }
		set { achievementRef = value; }
	}

	public Achievement(GameObject _achievement, string _name, string _description, int _imageIndex, int _reward, bool _unlockReward) {

		achievementRef = _achievement;
		name = _name;
		description = _description;
		imageIndex = _imageIndex;
		unlocked = false;
		unlockReward = _unlockReward;
		reward = _reward;
	}

	public bool EarnAchievement() {

		if (!unlocked) {
			unlocked = true;
			return true;
		} 
		else { 
			return false;
		}
	}
}
