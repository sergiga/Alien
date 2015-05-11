using UnityEngine;
using System.Collections;

public class BombGenerator : MonoBehaviour {

	public float bombRate;
	public int minHoles = 3;
	public GameObject bombPrefab;
	public Transform[] spawnPoints;

	int[] picked_2;

	void OnEnable () {

		bombRate = 0.8f;
		picked_2 = new int[7]{0,0,0,0,0,0,0};
		StartCoroutine (OnEnableCoroutine ());
	}

	IEnumerator OnEnableCoroutine() {

		while (!GameManager.instance.isDeath) {
			CreateBomb();
			yield  return new WaitForSeconds(bombRate);
		}
	}
	void CreateBomb () {
		int spawnPointIndex;

		spawnPointIndex = Random.Range (0, spawnPoints.Length);
		while(picked_2[spawnPointIndex] != 0) {
			spawnPointIndex = Random.Range (0, spawnPoints.Length);
			picked_2[spawnPointIndex]--;
			if(picked_2[spawnPointIndex] < 0){
				picked_2[spawnPointIndex] = 0;
			}
		}
		picked_2[spawnPointIndex] += 5;
		if(picked_2[spawnPointIndex] > 5){
			picked_2[spawnPointIndex] = 5;
		}
		Instantiate (bombPrefab, spawnPoints [spawnPointIndex].position, Quaternion.identity);
	}
}
