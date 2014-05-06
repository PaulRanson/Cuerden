using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown(0) )
		{
			RaycastHit hit;

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100.0f))
			{
				Debug.Log(hit.collider.gameObject.name);
			}
		}
	}
}
