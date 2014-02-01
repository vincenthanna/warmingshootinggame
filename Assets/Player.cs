using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    GameObject stage;
    public GameObject bulletPrefab;
    public AudioClip[] gunsound;
    float fireTimer = 0.0f, fireTimerTime = 0.08f;

    // rotation variables
    float rotatePower = 0.0f;
    float rotatePowerMax = 64.0f;
    float rotatePowerIncValue = 2.0f;
    float rotateDirectionLast = 0.0f;


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

        //////////////////////////////////////////////////////
        /// process move forward / backward / right / left
        Vector3 blPos = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 128));
        Vector3 rtPos = Camera.main.ScreenToWorldPoint (new Vector3 (sw, sh, 128));
        
        blPos.z -= stage.transform.position.z; // move to stage world
        rtPos.z -= stage.transform.position.z; // move to stage world
        
        float horizontalMove = Input.GetAxis ("Horizontal");
        float verticalMove = Input.GetAxis ("Vertical");
        Vector3 pos = transform.localPosition;

        bool absoluteMove = false;
        if (absoluteMove) {
            pos.x += horizontalMove;
            pos.z += verticalMove;
        } else {
            float sensitivityV = 0.8f;
            float sensitivityH = 0.8f;
            if (verticalMove > 0) {
                pos += transform.forward * sensitivityV;
            }
            else if (verticalMove < 0) {
                pos -= transform.forward * sensitivityV;
            }

            if (horizontalMove > 0) {
                pos += transform.right * sensitivityH;
            }
            else if (horizontalMove < 0) {
                pos -= transform.right * sensitivityH;
            }
        }

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


        //Debug.Log (string.Format ("player position={0} localPosition={1}", transform.position, transform.localPosition));

        transform.localPosition = pos;


        //////////////////////////////////////////////////////
        /// process rotation

        //float rotatePower = 0.0f;
        //float rotateDirectionLast = 0.0f;

        float rotateRate = 0.0f;
        float rotateDirection = 0.0f;
        if (Input.GetKey ("z")) {
            rotateRate -= 1.0f;
            rotateDirection = -1.0f;
        }
        if (Input.GetKey ("x")) {
            rotateRate += 1.0f;
            rotateDirection = 1.0f;
        }

        if (rotateDirection != rotateDirectionLast) {
            rotatePower = 1.0f;
            Debug.Log("rotatePower reset!");
        }
        else {
            rotatePower += rotatePowerIncValue;
        }

        rotatePower = Mathf.Min (rotatePower, rotatePowerMax);

        rotateDirectionLast = rotateDirection;

        if (rotateRate != 0.0f) {

            //transform.rotation *= Quaternion.AngleAxis (rotateRate * rotatePower, new Vector3 (0, 1, 0));

            float speed = 4.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                 transform.rotation * Quaternion.AngleAxis (rotateRate * rotatePower, new Vector3 (0, 1, 0)),
                                                 Time.deltaTime*speed);

            //Debug.Log (string.Format("rotatePower={0}", rotatePower));
        }

        //////////////////////////////////////////////////////
        /// process fire


        fireTimer -= Time.deltaTime;
        if (fireTimer < 0.0f) {
            Vector3 bulletStartPos = transform.position;
            bulletStartPos += ((transform.forward.normalized) * 4.0f);
            GameObject bullet = (GameObject)Instantiate (bulletPrefab, bulletStartPos, transform.localRotation);
            bullet.transform.parent = stage.transform;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.rotation *= Quaternion.AngleAxis (90, Vector3.right);


            Vector3 speedVector = transform.forward;
            bullet.rigidbody.velocity = speedVector * 256.0f;

            GameObject mainCamera = (GameObject)GameObject.FindWithTag ("MainCamera");
            //AudioSource.PlayClipAtPoint(gunsound[0], mainCamera.transform.position);
            //AudioSource.PlayClipAtPoint(gunsound[0], transform.position);

            fireTimer = fireTimerTime;
        }
    }
}
