using UnityEngine;
using System.Collections;

public class BombGenerator : MonoBehaviour {

	public int minHoles = 3;
	public GameObject bombPrefab;
	public Transform[] spawnPoints;

	int[] picked_2;

	void Start () {

		picked_2 = new int[7]{0,0,0,0,0,0,0};
	}

	public IEnumerator StartGenerator() {

		while (!GameManager.instance.gameOver) {

			CreateBomb();
			yield  return new WaitForSeconds(GameManager.instance.bombRate);
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
