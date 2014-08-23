using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	Transform player_pos;

	// Use this for initialization
	void Start () {
		player_pos = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player_pos.position.x, transform.position.y, transform.position.z);
	}
}
