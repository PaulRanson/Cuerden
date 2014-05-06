using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(MobileSimpleRpgCamera))]
public class MobileSimpleRpgCameraEditor : Editor
{
	private bool _foldSettingCollision = false;
	private bool _foldSettingTarget = false;
	private bool _foldSettingMovement = false;
	private bool _foldSettingRotation = false;
	private bool _foldSettingZoom = false;

	private bool _foldInvert = false;
	private bool _foldObjectsToFade = false;
	private bool _foldDistance = false;
	private bool _foldAngle = false;
	private bool _foldPanningVector = false;
	private bool _foldPanningInvert = false;

	private int _objectsToFadeSize = 0;

	private bool _init = false;

	private GUIContent _content;

	private MobileSimpleRpgCamera _self;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		_self = (MobileSimpleRpgCamera)target;

		if(!_init)
		{
			_init = true;
			_objectsToFadeSize = _self.objectsToFade.Count;
		}

		bool allowSceneObjects = !EditorUtility.IsPersistent(_self);

		// ---- Collision Settings ----
		if(_self.collisionAlphaLayers.value != 0)
		{
			_foldSettingCollision = EditorGUILayout.Foldout(_foldSettingCollision, "Collision Settings");

			if(_foldSettingCollision)
			{
				EditorGUI.indentLevel++;

				_content = new GUIContent("Collision Alpha", "The alpha value for faded objects in front of the target");
				_self.collisionAlpha = EditorGUILayout.Slider(_content, _self.collisionAlpha, 0, 1);
				_content = new GUIContent("Collision Fade Speed", "Modifier for the time it takes to fade objects in / out");
				_self.collisionFadeSpeed = EditorGUILayout.FloatField(_content, _self.collisionFadeSpeed);

				EditorGUI.indentLevel--;
			}
		}
		// ---- ----

		// ---- Target Settings ----
		_foldSettingTarget = EditorGUILayout.Foldout(_foldSettingTarget, "Target Settings");

		if(_foldSettingTarget)
		{
			EditorGUI.indentLevel++;

			_content = new GUIContent("Target Tag", "Search for a target with the specified tag");
			_self.targetTag = EditorGUILayout.TextField(_content, _self.targetTag);

			if(_self.targetTag.Length == 0)
			{
				_self.target = (Transform)EditorGUILayout.ObjectField("Target", _self.target, typeof(Transform), allowSceneObjects);
			}
			else
			{
				_self.target = null;
			}

			_self.targetOffset = EditorGUILayout.Vector2Field("Target Offset", _self.targetOffset);

			EditorGUI.indentLevel--;
		}
		// ---- ----

		// ---- Movement Settings ----
		_foldSettingMovement = EditorGUILayout.Foldout(_foldSettingMovement, "Movement Settings");

		if(_foldSettingMovement)
		{
			EditorGUI.indentLevel++;

			_self.allowPanning = EditorGUILayout.Toggle("Allow Panning", _self.allowPanning);

			if(_self.allowPanning)
			{
				EditorGUI.indentLevel++;

				_self.panType = (MobileSimpleRpgCamera.ControlType)EditorGUILayout.EnumPopup("Pan Type", _self.panType);

				if(_self.panType == MobileSimpleRpgCamera.ControlType.Drag)
				{
					_self.panningSensitivity = EditorGUILayout.Slider("Sensitivity", _self.panningSensitivity, 0.01f, 10);
					_self.panningTouchCount = EditorGUILayout.IntSlider("Touch Count", _self.panningTouchCount, 1, 3);
				}
				else if(_self.panType == MobileSimpleRpgCamera.ControlType.Swipe)
				{
					_self.swipeActiveTime = EditorGUILayout.FloatField("Swipe Active Time", _self.swipeActiveTime);
					_self.swipeMinDistance = EditorGUILayout.FloatField("Swipe Min Distance", _self.swipeMinDistance);
					_self.panAmount = EditorGUILayout.Vector2Field("Pan Amount", _self.panAmount);
					_self.panSpeed = EditorGUILayout.FloatField("Pan Speed", _self.panSpeed);
				}

				_foldPanningVector = EditorGUILayout.Foldout(_foldPanningVector, "Allowed Directions");

				if(_foldPanningVector)
				{
					EditorGUI.indentLevel++;

					_self.panningAllowX = EditorGUILayout.Toggle("X", _self.panningAllowX);
					_self.panningAllowY = EditorGUILayout.Toggle("Y", _self.panningAllowY);

					EditorGUI.indentLevel--;
				}

				_foldPanningInvert = EditorGUILayout.Foldout(_foldPanningInvert, "Invert Direction");

				if(_foldPanningInvert)
				{
					EditorGUI.indentLevel++;

					_self.panInvertX = EditorGUILayout.Toggle("X", _self.panInvertX);
					_self.panInvertY = EditorGUILayout.Toggle("Y", _self.panInvertY);

					EditorGUI.indentLevel--;
				}

				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}
		// ---- ----

		// ---- Rotation Settings ----
		_foldSettingRotation = EditorGUILayout.Foldout(_foldSettingRotation, "Rotation Settings");

		if(_foldSettingRotation)
		{
			EditorGUI.indentLevel++;

			_self.originRotation = EditorGUILayout.Vector2Field("Origin Rotation", _self.originRotation);

			_self.returnSmoothing = EditorGUILayout.Slider("Rotation Smoothing", _self.returnSmoothing, 1, 25);
			_self.stayBehindTarget = EditorGUILayout.Toggle("Stay Behind Target", _self.stayBehindTarget);

			_self.allowRotation = EditorGUILayout.Toggle("Allow Rotation", _self.allowRotation);

			if(_self.allowRotation)
			{
				EditorGUI.indentLevel++;

				_self.rotationType = (MobileSimpleRpgCamera.ControlType)EditorGUILayout.EnumPopup("Rotation Type", _self.rotationType);

				if(_self.rotationType == MobileSimpleRpgCamera.ControlType.Swipe)
				{
					_self.swipeActiveTime = EditorGUILayout.FloatField("Swipe Active Time", _self.swipeActiveTime);
					_self.swipeMinDistance = EditorGUILayout.FloatField("Swipe Min Distance", _self.swipeMinDistance);
					_self.rotationAmount = EditorGUILayout.Vector2Field("Rotation Amount", _self.rotationAmount);
				}

				_foldAngle = EditorGUILayout.Foldout(_foldAngle, "Angle");

				if(_foldAngle)
				{
					EditorGUI.indentLevel++;
					_self.minAngle = EditorGUILayout.Slider("Min", _self.minAngle, -_self.maxAngle, _self.maxAngle);
					_self.maxAngle = EditorGUILayout.Slider("Max", _self.maxAngle, _self.minAngle, 359);
					EditorGUI.indentLevel--;
				}

				_foldInvert = EditorGUILayout.Foldout(_foldInvert, "Invert Rotation");

				if(_foldInvert)
				{
					EditorGUI.indentLevel++;
					_self.invertX = EditorGUILayout.Toggle("X", _self.invertX);
					_self.invertY = EditorGUILayout.Toggle("Y", _self.invertY);
					EditorGUI.indentLevel--;
				}

				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}
		// ---- ----

		// ---- Zoom Settings ----
		_foldSettingZoom = EditorGUILayout.Foldout(_foldSettingZoom, "Zoom Settings");

		if(_foldSettingZoom)
		{
			EditorGUI.indentLevel++;

			_content = new GUIContent("Distance", "The distance between the camera and target");
			_foldDistance = EditorGUILayout.Foldout(_foldDistance, _content);

			if(_foldDistance)
			{
				EditorGUI.indentLevel++;

				_self.minDistance = EditorGUILayout.Slider("Min", _self.minDistance, 0.01f, _self.maxDistance);
				_self.distance = EditorGUILayout.Slider("Current", _self.distance, _self.minDistance, _self.maxDistance);
				_self.maxDistance = EditorGUILayout.Slider("Max", _self.maxDistance, _self.minDistance, 100);

				EditorGUI.indentLevel--;
			}

			_self.zoomSpeed = EditorGUILayout.FloatField("Zoom Speed", _self.zoomSpeed);
			_self.zoomSmoothing = EditorGUILayout.FloatField("Zoom Smoothing", _self.zoomSmoothing);
			_self.zoomInvert = EditorGUILayout.Toggle("Invert Direction", _self.zoomInvert);

			_self.fadeObjects = EditorGUILayout.Toggle("Fade Objects", _self.fadeObjects);

			if(_self.fadeObjects)
			{
				EditorGUI.indentLevel++;

				_self.fadeDistance = EditorGUILayout.FloatField("Fade Distance", _self.fadeDistance);

				_foldObjectsToFade = EditorGUILayout.Foldout(_foldObjectsToFade, "Objects To Fade");

				if(_foldObjectsToFade)
				{
					EditorGUI.indentLevel++;

					_objectsToFadeSize = EditorGUILayout.IntField("Size", _objectsToFadeSize);

					if(_objectsToFadeSize < 0)
					{
						_objectsToFadeSize = 0;
					}

					Renderer[] objectsToFade = new Renderer[_objectsToFadeSize];

					for(int i = 0; i < _objectsToFadeSize; i++)
					{
						if(_self.objectsToFade.Count == i)
						{
							break;
						}

						objectsToFade[i] = _self.objectsToFade[i];
					}

					for(int i = 0; i < _objectsToFadeSize; i++)
					{
						objectsToFade[i] = (Renderer)EditorGUILayout.ObjectField("Element " + i, objectsToFade[i], typeof(Renderer), allowSceneObjects);
					}

					_self.objectsToFade = new List<Renderer>();

					foreach(Renderer r in objectsToFade)
					{
						if(r)
						{
							_self.objectsToFade.Add(r);
						}
					}

					EditorGUI.indentLevel--;
				}

				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}
		// ---- ----
	}
}