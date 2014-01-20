using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

    public GameObject[] enemyPrefabs;
    float genTimer = 0.3f; float genTimerTime = 0.3f;

    GameObject stage;

	// Use this for initialization
	void Start () {
        stage = GameObject.FindWithTag ("Stage");
	}
	
	// Update is called once per frame
	void Update () {
        genTimer -= Time.deltaTime;
        if (genTimer < 0.0f) {

            genTimer = genTimerTime;
            Vector3 pos = transform.position;
            pos.x = Random.Range (-80,80);

            GameObject inst = (GameObject)Instantiate (enemyPrefabs[0], pos, Quaternion.identity);
            
            // 이것이 핵심
            //inst.transform.parent = gameObject.transform;
            inst.transform.parent = stage.transform;
        }
	}
}
