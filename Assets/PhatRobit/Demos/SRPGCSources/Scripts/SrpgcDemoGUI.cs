using UnityEngine;
using System.Collections;

public class SrpgcDemoGUI : MonoBehaviour
{
	public SimpleRpgCamera rpgCamera;
	public MobileSimpleRpgCamera mobileRpgCamera;
	public string demoSelectScene = string.Empty;
	public GUISkin skin;

	private Rect _window_rect;
	private string _version = "1.5.1";

	void Start()
	{
		_window_rect = new Rect(10, 10, 200, 32);
	}

	void OnGUI()
	{
		_window_rect = GUILayout.Window(0, _window_rect, DemoWindow, "Simple RPG Camera Demo");

		bool enableMouse = !_window_rect.Contains(Event.current.mousePosition);

		if(rpgCamera)
		{
			rpgCamera.Controllable = enableMouse;
		}
	}

	private void DemoWindow(int id)
	{
		if(skin && GUI.skin != skin)
		{
			GUI.skin = skin;
		}

		GUILayout.Label("v" + _version);

		if(demoSelectScene != string.Empty)
		{
			if(GUILayout.Button("Return To Demo Selection"))
			{
				Application.LoadLevel(demoSelectScene);
			}
		}

		if(rpgCamera)
		{
			rpgCamera.useTargetAxis = GUILayout.Toggle(rpgCamera.useTargetAxis, "Use Target Axis");
			rpgCamera.lockToTarget = GUILayout.Toggle(rpgCamera.lockToTarget, "Lock To Target");

			rpgCamera.allowEdgeMovement = GUILayout.Toggle(rpgCamera.allowEdgeMovement, "Allow Edge Movement");
			rpgCamera.allowEdgeKeys = GUILayout.Toggle(rpgCamera.allowEdgeKeys, "Allow Key Movement");

			rpgCamera.allowRotation = GUILayout.Toggle(rpgCamera.allowRotation, "Allow Rotation");

			if(rpgCamera.allowRotation)
			{
				rpgCamera.allowRotationLeft = GUILayout.Toggle(rpgCamera.allowRotationLeft, "Allow Left Button");
				rpgCamera.allowRotationMiddle = GUILayout.Toggle(rpgCamera.allowRotationMiddle, "Allow Middle Button");
				rpgCamera.allowRotationRight = GUILayout.Toggle(rpgCamera.allowRotationRight, "Allow Right Button");

				if(rpgCamera.allowRotationLeft)
				{
					rpgCamera.lockLeft = GUILayout.Toggle(rpgCamera.lockLeft, "Lock Left Button");
				}

				if(rpgCamera.allowRotationMiddle)
				{
					rpgCamera.lockMiddle = GUILayout.Toggle(rpgCamera.lockMiddle, "Lock Middle Button");
				}

				if(rpgCamera.allowRotationRight)
				{
					rpgCamera.lockRight = GUILayout.Toggle(rpgCamera.lockRight, "Lock Right Button");
				}

				rpgCamera.stayBehindTarget = GUILayout.Toggle(rpgCamera.stayBehindTarget, "Stay Behind Target");
				rpgCamera.returnToOrigin = GUILayout.Toggle(rpgCamera.returnToOrigin, "Return To Origin");
				rpgCamera.invertRotationX = GUILayout.Toggle(rpgCamera.invertRotationX, "Invert X");
				rpgCamera.invertRotationY = GUILayout.Toggle(rpgCamera.invertRotationY, "Invert Y");
				rpgCamera.rotateObjects = GUILayout.Toggle(rpgCamera.rotateObjects, "Rotate Objects");
			}

			rpgCamera.fadeObjects = GUILayout.Toggle(rpgCamera.fadeObjects, "Fade Objects [" + rpgCamera.fadeDistance + "]");

			if(rpgCamera.fadeObjects)
			{
				rpgCamera.fadeDistance = GUILayout.HorizontalSlider(rpgCamera.fadeDistance, rpgCamera.minDistance, rpgCamera.maxDistance);
			}

			GUILayout.Label("Target Offset\n[X:" + rpgCamera.targetOffset.x.ToString("F2") + ", Y:" + rpgCamera.targetOffset.y.ToString("F2") + ", Z:" + rpgCamera.targetOffset.z.ToString("F2") + "]");
			rpgCamera.targetOffset.x = GUILayout.HorizontalSlider(rpgCamera.targetOffset.x, -2, 2);
			rpgCamera.targetOffset.y = GUILayout.HorizontalSlider(rpgCamera.targetOffset.y, 0, 2);
			rpgCamera.targetOffset.z = GUILayout.HorizontalSlider(rpgCamera.targetOffset.z, -2, 2);
		}

		if(mobileRpgCamera)
		{
			if(mobileRpgCamera.rotationType == MobileSimpleRpgCamera.ControlType.Swipe ||
			mobileRpgCamera.panType == MobileSimpleRpgCamera.ControlType.Swipe)
			{
				GUILayout.Label("Swipe Detection");

				GUILayout.BeginHorizontal();

				GUILayout.BeginVertical();
				GUILayout.Label("Swipe:");
				GUILayout.Label("Distance:");
				GUILayout.Label("Time:");
				GUILayout.Label("Direction:");
				GUILayout.EndVertical();

				GUILayout.BeginVertical();
				GUILayout.Label(mobileRpgCamera.Swipe.ToString());
				GUILayout.Label(mobileRpgCamera.SwipeDist.ToString());
				GUILayout.Label(mobileRpgCamera.SwipeTime.ToString());
				GUILayout.Label(mobileRpgCamera.SwipeDirection.ToString());
				GUILayout.EndVertical();

				GUILayout.EndHorizontal();
			}
		}

		if(GUILayout.Button("Quit"))
		{
			Application.Quit();
		}
	}
}