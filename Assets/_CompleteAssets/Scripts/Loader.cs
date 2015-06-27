using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	
	public GameObject gameManager;
	public GameObject achievementManager;

	void Awake () {
		if (GameManager.instance == null) {
			Instantiate (gameManager);
		}

		/* if (AchievementManager.instance == null) {
			Instantiate (achievementManager);
		}*/
	}
}