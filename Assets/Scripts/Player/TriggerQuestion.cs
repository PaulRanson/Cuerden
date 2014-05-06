using UnityEngine;
using System.Collections;

public class TriggerQuestion : MonoBehaviour {

	private QuestionEvent questionEvent;

	void Awake()  
	{  
		questionEvent = GameObject.FindGameObjectWithTag("QuestionMenu").GetComponent<QuestionEvent> ();
	}


	// Use this for initialization
	void Start () 
	{

	}


	void OnTriggerEnter(Collider other)
	{
		questionEvent.questionIndex = 0;
		Debug.Log ("Object Entered the trigger");
	}





}
