using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    GameObject player;
    GameObject stage;
    bool destroyed = false;
    bool exploded = false;
    float destroyTimer = 1.0f;
    public GameObject[] explosionPrefabs;
    GameObject smokeEffect;
    public AudioClip[] explosionACs;
    public AudioClip[] gunshotACs;
    GameObject mainCamera;
    int hull = 1;
    enumEnemyType enemyType = 0;
    float tooFarTimer = 0.0f, tooFarTimerTime = 1.0f;

    // Use this for initialization
    void Start ()
    {
        int max = (int)(enumEnemyType.ENEMY_TYPE_MAX - 1);

        enemyType = (enumEnemyType)Random.Range (0, max);
        player = GameObject.FindWithTag ("Player");
        stage = GameObject.FindWithTag ("Stage");
        Vector3 forwardVector = transform.forward;

        Vector3 targetVector = (player.transform.position - transform.position).normalized;
        rigidbody.transform.forward = targetVector.normalized;

        mainCamera = (GameObject)GameObject.FindWithTag ("MainCamera");
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (destroyed == false) {
    
            switch (enemyType) {
            case enumEnemyType.ENEMY_TYPE_001:
                {
                    Vector3 forwardVector = transform.forward;
                    Vector3 targetVector = (player.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle (forwardVector, targetVector);

                    Vector3 tmp = (forwardVector.normalized) + (targetVector.normalized);

                    tmp = tmp.normalized;
                    //Debug.Log (string.Format ("({0} {1} {2}) deltaTime={3}", tmp.x, tmp.y, tmp.z, Time.deltaTime));

                    rigidbody.transform.forward = tmp;
                    rigidbody.velocity = forwardVector * 32.0f;
                }
                break;
            case enumEnemyType.ENEMY_TYPE_002:
            case enumEnemyType.ENEMY_TYPE_003:
            case enumEnemyType.ENEMY_TYPE_004:
            case enumEnemyType.ENEMY_TYPE_005:
                {
                    Vector3 forwardVector = new Vector3 (0, 0, -1);
                    rigidbody.transform.forward = forwardVector;
                    rigidbody.velocity = forwardVector * 64.0f;
                }
                break;
            }

        } else {    
            destroyTimer -= Time.deltaTime;
            if (destroyTimer < 0.2) {
                if (!exploded) {
                    GameObject expEffect = (GameObject)Instantiate (explosionPrefabs [1], transform.position, transform.rotation);
                    //smokeEffect.transform.parent = expEffect.transform;
                    exploded = true;
                    AudioSource.PlayClipAtPoint (explosionACs [0], mainCamera.transform.position);
                }
            }
            if (destroyTimer < 0.0f) {
                Destroy (gameObject);
            }
        }
        if (destroyed == false) {
            if (transform.position.y != stage.transform.position.y) {
                Vector3 pos = transform.position;
                pos.y = stage.transform.position.y;
                transform.position = pos;
            }
        }

        tooFarTimer -= Time.deltaTime;
        if (tooFarTimer < 0) {
            float distance = Vector3.Distance (transform.position, player.transform.position);
            if (distance > 300.0f) {
                Destroy (gameObject);
            }
            tooFarTimer = tooFarTimerTime;
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        if (destroyed) {
            return;
        }

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet") {

            hull -= 1;
            if (hull < 0) {
                rigidbody.AddForce (-transform.forward * 100.0f, ForceMode.Impulse);
                destroyed = true;
                smokeEffect = (GameObject)Instantiate (explosionPrefabs [0], transform.position, transform.rotation);
                smokeEffect.transform.parent = transform;
            }
        }

        if (collision.gameObject.tag == "Bullet") {
            int gunshotSoundIndex = Random.Range (0, 6);
            AudioSource.PlayClipAtPoint (gunshotACs [gunshotSoundIndex], mainCamera.transform.position);
        }
    }
}
