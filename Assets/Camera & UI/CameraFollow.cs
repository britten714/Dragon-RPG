using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    
	
	void Start ()
    {
		player = GameObject.FindGameObjectWithTag("Player");
	}
		
	void Update ()
    {
		
	}

    void LateUpdate()
    {
        transform.position = player.transform.position;
    }

}
