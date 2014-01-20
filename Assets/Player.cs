using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    GameObject stage;
    public GameObject bulletPrefab;
    public AudioClip[] gunsound;
    float fireTimer = 0.0f, fireTimerTime = 0.08f;

    // Use this for initialization
    void Start ()
    {
        stage = GameObject.FindWithTag ("Stage");
    }
    
    // Update is called once per frame
    void Update ()
    {
        int sw = Screen.width;
        int sh = Screen.height;
        Vector3 blPos = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 128));
        Vector3 rtPos = Camera.main.ScreenToWorldPoint (new Vector3 (sw, sh, 128));
        
        blPos.z -= stage.transform.position.z; // move to stage world
        rtPos.z -= stage.transform.position.z; // move to stage world
        
        float horizontalMove = Input.GetAxis ("Horizontal");
        float verticalMove = Input.GetAxis ("Vertical");
        Vector3 pos = transform.localPosition;

        pos.x += horizontalMove;
        pos.z += verticalMove;

        if (pos.x < blPos.x) {
            pos.x = blPos.x;
        }
        
        if (pos.x > rtPos.x) {
            pos.x = rtPos.x;
        }


        if (pos.z < blPos.z) {
            pos.z = blPos.z;
        }
        
        if (pos.z > rtPos.z) {
            pos.z = rtPos.z;
        }

        //Debug.Log (string.Format("bottomLeft={0} topRight={1} playerPos={2}", blPos,rtPos, pos));


        Debug.Log (string.Format ("player position={0} localPosition={1}",
                                  transform.position, transform.localPosition));

        transform.localPosition = pos;

        fireTimer -= Time.deltaTime;
        if (fireTimer < 0.0f) {
            Vector3 bulletStartPos = transform.position;
            bulletStartPos.z += 2;
            GameObject bullet = (GameObject)Instantiate (bulletPrefab, bulletStartPos, transform.localRotation);
            bullet.transform.parent = stage.transform;
            bullet.transform.rotation = Quaternion.AngleAxis (90, Vector3.right);

            Vector3 speedVector = transform.forward;
            bullet.rigidbody.velocity = speedVector * 256.0f;

            GameObject mainCamera = (GameObject)GameObject.FindWithTag ("MainCamera");
            //AudioSource.PlayClipAtPoint(gunsound[0], mainCamera.transform.position);
            //AudioSource.PlayClipAtPoint(gunsound[0], transform.position);

            fireTimer = fireTimerTime;
        }
    }
}
