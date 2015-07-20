using UnityEngine;
using System.Collections;

public class CloudGenerator : MonoBehaviour {

	public float cloudRate = 20f;
	public GameObject cloudPrefab;
	public Transform[] spawnPoints;
	
	private int spawnPointIndex;
	private float cloudRateProgress;

	void Start() {
		cloudRateProgress = 0;
	}

	void Update() {

		if (cloudRateProgress > 0) {
			cloudRateProgress -= Time.deltaTime;
		}

		else {
			cloudRateProgress = cloudRate;
			SpawnCloud();
		}
	}

	void SpawnCloud () {

		spawnPointIndex = Random.Range (0, spawnPoints.Length);
		Instantiate (cloudPrefab, spawnPoints [spawnPointIndex].position, Quaternion.identity);
	}
}
