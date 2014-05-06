#pragma strict
#pragma strict

var MenuObject: MenuWindow;
// Use MenuManager type instead MenuWindow of if  you want  to operate  with whole menu system on scene
var buttonCode: KeyCode = KeyCode.Backspace;

private var questionEvent : QuestionEvent; 

function Awake()
{
	//Get the CSharp object. This object needs to be compiled in the Standard Assets folder to be assured
	//it is already compiled before this file is compiled. If it is not compiled there will be an error
	//So don't forget to place the 'QuestionEvent.cpp' file inside the 'Standard Asseats' folder
	questionEvent = this.GetComponent(QuestionEvent);
}	
	
function Update () {
	if (questionEvent)
	{  
		if (questionEvent.questionIndex)
		{	
	   		if (MenuObject)
	   		{
			    MenuObject.enabled = !MenuObject.enabled;
			    if (Time.timeScale == 0) Time.timeScale = 1; else Time.timeScale = 0;
			}
			else
			{
				var Script: MenuWindow = gameObject.GetComponent(MenuWindow);
				if (Script)  
					Script.enabled = !Script.enabled;
				else
					Debug.Log ("Sorry but there no MenuWindow script attached to current object and no assigned to ", MenuObject);
				
				if (Time.timeScale == 0) Time.timeScale = 1; else Time.timeScale = 0;
			}
			
			questionEvent.questionIndex=0;
			
			  
		}
	}
}

