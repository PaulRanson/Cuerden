using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour 
{
		// Here we store the hash tags for various strings used in our animators.
		public int locomotionState;
		public int shoutState;
		public int deadBool;
		public int speedFloat;
		public int sneakingBool;
		public int shoutingBool;
		public int playerInSightBool;
		public int shotFloat;
		public int aimWeightFloat;
		public int angularSpeedFloat;
		public int openBool;
		
		
		void Awake ()
		{
			locomotionState = Animator.StringToHash("Base Layer.Locomotion");
			shoutState = Animator.StringToHash("Shouting.Shout");
			deadBool = Animator.StringToHash("Dead");
			speedFloat = Animator.StringToHash("Speed");
			sneakingBool = Animator.StringToHash("Sneaking");
			shoutingBool = Animator.StringToHash("Shouting");
			playerInSightBool = Animator.StringToHash("PlayerInSight");
			shotFloat = Animator.StringToHash("Shot");
			aimWeightFloat = Animator.StringToHash("AimWeight");
			angularSpeedFloat = Animator.StringToHash("AngularSpeed");
			openBool = Animator.StringToHash("Open");
		}

}
