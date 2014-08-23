using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("character");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x + 2, transform.position.y, transform.position.z);
	}
}
