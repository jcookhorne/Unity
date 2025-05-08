using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;


    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {


        FollowPlayer();
    }


    void FollowPlayer()
    {
        if (player){
            float x = player.transform.position.x;
            float y = player.transform.position.y; 
            float z = player.transform.position.z;
            gameObject.transform.position = new Vector3(x, y, z);

        }
    }


}
