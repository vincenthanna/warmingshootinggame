using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    float lifeTime = 6.0f;



    // Use this for initialization
    void Start ()
    {


    }
    
    // Update is called once per frame
    void Update ()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0) {
            Destroy (gameObject);
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            return;
        }
        Destroy (gameObject);
    }
}
