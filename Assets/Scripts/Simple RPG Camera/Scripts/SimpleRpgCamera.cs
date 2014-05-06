using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleRpgCamera : MonoBehaviour
{
	#region Public Variables

	#region Collision Settings

	public LayerMask collisionLayers = new LayerMask();				// Determines what objects the camera collides with
	[HideInInspector]
	public float collisionBuffer = 0.2f;							// A small value to prevent camera clipping
	public LayerMask collisionAlphaLayers = new LayerMask();		// Determines what objects the camera will fade out in front of it
	[HideInInspector]
	public float collisionAlpha = 0.15f;							// The visibility of the faded objects in front of the camera
	[HideInInspector]
	public float collisionFadeSpeed = 10;							// The speed at which the objects in front of the camera fade in / out

	#endregion

	#region Target Settings

	[HideInInspector]
	public Transform target;										// The camera's target
	[HideInInspector]
	public string targetTag = string.Empty;							// The tag of the camera's target, used if no target is set
	[HideInInspector]
	public Vector3 targetOffset = new Vector3();					// An offset relative to the target's position
	[HideInInspector]
	public bool useTargetAxis = false;								// When true, camera follows and rotates relative to target's rotation

	#endregion

	#region Movement Settings

	[HideInInspector]
	public bool allowEdgeMovement = false;
	[HideInInspector]
	public bool allowEdgeKeys = false;
	[HideInInspector]
	public bool lockToTarget = false;
	[HideInInspector]
	public bool limitBounds = false;
	[HideInInspector]
	public Vector3 boundOrigin = new Vector3();
	[HideInInspector]
	public Vector3 boundSize = new Vector3();
	[HideInInspector]
	public float edgePadding = 20;
	[HideInInspector]
	public float scrollSpeed = 10;
	[HideInInspector]
	public KeyCode keyFollowTarget = KeyCode.Space;
	[HideInInspector]
	public KeyCode keyMoveUp = KeyCode.W;
	[HideInInspector]
	public KeyCode keyMoveDown = KeyCode.S;
	[HideInInspector]
	public KeyCode keyMoveLeft = KeyCode.A;
	[HideInInspector]
	public KeyCode keyMoveRight = KeyCode.D;
	[HideInInspector]
	public bool showEdges = false;
	[HideInInspector]
	public Texture2D edgeTexture;

	#endregion

	#region Rotation Settings

	[HideInInspector]
	public bool allowRotation = true;								// Whether or not the camera can be rotated by user input
	[HideInInspector]
	public bool invertRotationX = false;							// Reverse the rotation direction for X
	[HideInInspector]
	public bool invertRotationY = false;							// Reverse the rotation direction for Y
	[HideInInspector]
	public bool allowRotationLeft = true;							// Whether or not the camera can be rotated with the left mouse button
	[HideInInspector]
	public bool allowRotationMiddle = true;							// Whether or not the camera can be rotated with the middle mouse button
	[HideInInspector]
	public bool allowRotationRight = true;							// Whether or not the camera can be rotated with the right mouse button
	[HideInInspector]
	public Vector2 originRotation = new Vector2();					// The initial rotation of the camera
	[HideInInspector]
	public bool returnToOrigin = false;								// When true, camera will return to originRotation
	[HideInInspector]
	public bool stayBehindTarget = false;							// When true, camera will stay behind the target while there is no rotation input
	[HideInInspector]
	public bool setOriginLeft = false;								// When true, originRotation becomes current rotation while pressing left mouse button
	[HideInInspector]
	public bool setOriginMiddle = false;							// When true, originRotation becomes current rotation while pressing middle mouse button
	[HideInInspector]
	public bool setOriginRight = false;								// When true, originRotation becomes current rotation while pressing right mouse button
	[HideInInspector]
	public float minAngle = 0;									// The minimum Y rotation angle
	[HideInInspector]
	public float maxAngle = 85;										// The maximum Y rotation angle
	[HideInInspector]
	public float rotationSmoothing = 5;								// Determines how quickly the camera will reach its wanted rotation (higher = faster, slower = smoother)
	[HideInInspector]
	public Vector2 rotationSensitivity = new Vector2(5, 5);			// Mouse sensitivity for rotation
	[HideInInspector]
	public bool lockCursor = true;									// Whether or not to lock the cursor while rotating the camera with the mouse
	[HideInInspector]
	public bool lockLeft = true;									// Whether or not to lock the cursor while rotating the camera with the left mouse button
	[HideInInspector]
	public bool lockMiddle = true;									// Whether or not to lock the cursor while rotating the camera with the middle mouse button
	[HideInInspector]
	public bool lockRight = true;									// Whether or not to lock the cursor while rotating the camera with the right mouse button
	[HideInInspector]
	public bool allowRotationKeys = true;							// Whether or not the user can rotate the camera using the rotation keys
	[HideInInspector]
	public KeyCode keyRotateUp = KeyCode.Keypad5;					// The key for rotating the camera up
	[HideInInspector]
	public KeyCode keyRotateDown = KeyCode.Keypad2;					// The key for rotating the camera down
	[HideInInspector]
	public KeyCode keyRotateLeft = KeyCode.Keypad1;					// The key for rotating the camera left
	[HideInInspector]
	public KeyCode keyRotateRight = KeyCode.Keypad3;				// The key for rotating the camera right
	[HideInInspector]
	public Vector2 rotationKeySensitivity = new Vector2(3, 3);		// Key sensitivity for rotation
	[HideInInspector]
	public bool rotateObjects = false;								// Whether or not objects will face forward relative to the camera while rotating
	[HideInInspector]
	public List<Transform> objectsToRotate = new List<Transform>();	// The objects to rotate
	[HideInInspector]
	public bool rotateObjectsLeft = false;							// Rotates objects with the left mouse button
	[HideInInspector]
	public bool rotateObjectsMiddle = false;						// Rotates objects with the middle mouse button
	[HideInInspector]
	public bool rotateObjectsRight = false;							// Rotates objects with the right mouse button

	#endregion

	#region Zoom Settings

	[HideInInspector]
	public bool allowZoom = true;									// Whether or not the user can zoom in / out
	[HideInInspector]
	public float distance = 7;										// The distance between the camera and the target
	[HideInInspector]
	public float minDistance = 1;									// The minimum distance between the camera and the target
	[HideInInspector]
	public float maxDistance = 10;									// The maximum distance between the camera and the target
	[HideInInspector]
	public float zoomSpeed = 1;										// The distance the camera will travel while zooming
	[HideInInspector]
	public float zoomSmoothing = 5;									// Determines how quickly the camera will reach its wanted zoom level (higher = faster, slower = smoother)
	[HideInInspector]
	public bool invertZoom = false;									// Reverse the zoom direction
	[HideInInspector]
	public bool fadeObjects = false;								// Whether or not to fade objects near the camera
	[HideInInspector]
	public float fadeDistance = 1;									// When the camera reaches this distance, objects will be transparent
	[HideInInspector]
	public List<Renderer> objectsToFade = new List<Renderer>();		// The list of objects to be faded
	[HideInInspector]
	public bool allowZoomKeys = true;								// Whether or not the user can zoom in / out using zoom keys
	[HideInInspector]
	public KeyCode keyZoomIn = KeyCode.Home;						// The zoom in key
	[HideInInspector]
	public KeyCode keyZoomOut = KeyCode.End;						// The zoom out key
	[HideInInspector]
	public float keyZoomDelay = 0.5f;								// Delay before zoom is constant while holding a key

	#endregion

	#endregion

	#region Private Variables

	private float _oldDistance = 0;

	private Quaternion _oldRotation = new Quaternion();
	private Vector2 _angle = new Vector2();

	private float _zoomInTimer = 0;
	private float _zoomOutTimer = 0;

	private List<Material> _fadedMats = new List<Material>();
	private List<Material> _activeFadedMats = new List<Material>();

	private bool _controllable = true;

	private Transform _t;
	private Transform _focalPoint;

	#endregion

	#region Getters / Setters

	public bool Controllable
	{
		get { return _controllable; }
		set { _controllable = value; }
	}

	#endregion

	#region Unity Functions

	void Start()
	{
		_t = transform;

		_oldDistance = distance;
		_angle = originRotation;

		CreateFocalPoint();
	}

	void Update()
	{
		#region Rotation Input
		if (Time.timeScale!=0)
		{

			if(_controllable &&
				allowRotation && ((allowRotationLeft && Input.GetMouseButton(0)) ||
								  (allowRotationMiddle && Input.GetMouseButton(2)) ||
								  (allowRotationRight && Input.GetMouseButton(1))))
			{
				// Get the mouse axis for camera rotation
				_angle.x += Input.GetAxis("Mouse X") * rotationSensitivity.x * (invertRotationX ? -1 : 1);
				// Limit the Y rotation angle
				_angle.y = Mathf.Clamp(_angle.y - Input.GetAxis("Mouse Y") * rotationSensitivity.y * (invertRotationY ? -1 : 1), minAngle, maxAngle);

				ClampAngle(ref _angle);

				if((setOriginLeft && Input.GetMouseButton(0)) ||
				   (setOriginMiddle && Input.GetMouseButton(2)) ||
				   (setOriginRight && Input.GetMouseButton(1)))
				{
					// Sets the origin rotation to the current rotation
					originRotation = _angle;
				}

				// Lock the cursor if enabled
				Screen.lockCursor = (lockCursor && ((lockLeft && Input.GetMouseButton(0)) ||
													(lockMiddle && Input.GetMouseButton(2)) ||
													(lockRight && Input.GetMouseButton(1))));

				// Force the target's y rotation to face forward (if enabled) when right clicking
				if(rotateObjects && ((rotateObjectsLeft && Input.GetMouseButton(0)) ||
									 (rotateObjectsMiddle && Input.GetMouseButton(2)) ||
									 (rotateObjectsRight && Input.GetMouseButton(1))))
				{
					foreach(Transform o in objectsToRotate)
					{
						o.rotation = Quaternion.Euler(0, _angle.x, 0);
					}
				}
			}
			else if(_controllable && 
					allowRotationKeys && (Input.GetKey(keyRotateUp) ||
										  Input.GetKey(keyRotateDown) ||
										  Input.GetKey(keyRotateLeft) ||
										  Input.GetKey(keyRotateRight)))
			{
				// Shorthand for pressing either left or right rotation keys, but not both
				int directionX = !(Input.GetKey(keyRotateLeft) && Input.GetKey(keyRotateRight)) ?
									(Input.GetKey(keyRotateLeft) ? 1 :
										(Input.GetKey(keyRotateRight) ? -1 : 0)) : 0;

				_angle.x += directionX * rotationKeySensitivity.x * (invertRotationX ? -1 : 1);

				// Shorthand for pressing either up or down rotation keys, but not both
				int directionY = !(Input.GetKey(keyRotateUp) && Input.GetKey(keyRotateDown)) ?
									(Input.GetKey(keyRotateUp) ? -1 :
										(Input.GetKey(keyRotateDown) ? 1 : 0)) : 0;

				// Limit the Y rotation angle

				//_angle.y = Mathf.Clamp(_angle.y - directionY * rotationKeySensitivity.y * (invertRotationY ? -1 : 1), minAngle, maxAngle);
				_angle.y = Mathf.Clamp(_angle.y - directionY * rotationKeySensitivity.y * (invertRotationY ? -1 : 1), 5, 85);

				ClampAngle(ref _angle);
			}
			else
			{
				if(returnToOrigin)
				{
					// Forces the camera back to the origin rotation
					_angle = originRotation;
				}

				if(stayBehindTarget)
				{
					// Forces the camera to be behind the target
					_angle.x = target.rotation.eulerAngles.y;
				}

				// Unlock the cursor
				Screen.lockCursor = false;
			}

			#endregion

			#region Zoom Input

			float scrollDirection = 0;

			if(_controllable)
			{
				if(allowZoom)
				{
					// Zoom mouse control
					scrollDirection = Input.GetAxis("Mouse ScrollWheel");
				}

				if(allowZoomKeys)
				{
					// Zoom key control

					// If zoom in key pressed, add to the zoom in delay timer
					if(Input.GetKey(keyZoomIn))
					{
						if(_zoomInTimer < keyZoomDelay)
						{
							_zoomInTimer += Time.deltaTime;
						}
					}
					else
					{
						_zoomInTimer = 0;
					}

					// If zoom out key pressed, add to the zoom out delay timer
					if(Input.GetKey(keyZoomOut))
					{
						if(_zoomOutTimer < keyZoomDelay)
						{
							_zoomOutTimer += Time.deltaTime;
						}
					}
					else
					{
						_zoomOutTimer = 0;
					}

					// If both zoom keys are pressed, don't allow constant zooming
					if(Input.GetKey(keyZoomIn) && Input.GetKey(keyZoomOut))
					{
						_zoomInTimer = 0;
						_zoomOutTimer = 0;
					}

					if(Input.GetKeyDown(keyZoomIn) || _zoomInTimer >= keyZoomDelay)
					{
						scrollDirection = 1;
					}

					if(Input.GetKeyDown(keyZoomOut) || _zoomOutTimer >= keyZoomDelay)
					{
						scrollDirection = -1;
					}
				}
			}

			// Adjust the distance with the mouse scrollwheel while clamping it to min and max values and invert it if enabled
			distance = Mathf.Clamp(distance + (scrollDirection != 0 ? (scrollDirection < 0 ? (invertZoom ? -zoomSpeed : zoomSpeed) : (invertZoom ? zoomSpeed : -zoomSpeed)) : 0), minDistance, maxDistance);

			#endregion

			#region Movement Input

			if(_focalPoint)
			{
				if(_controllable && (allowEdgeMovement || allowEdgeKeys))
				{
					Vector3 mousePosition = Input.mousePosition;
					Vector3 scrollVelocity = new Vector3();

					float topEdge = Screen.height - edgePadding;
					float bottomEdge = edgePadding;
					float leftEdge = edgePadding;
					float rightEdge = Screen.width - edgePadding;

					// Set the movement direction based off of mouse position or key input
					if((mousePosition.y >= topEdge && allowEdgeMovement) || (Input.GetKey(keyMoveUp) && allowEdgeKeys))
					{
						scrollVelocity.z = -scrollSpeed;
					}
					else if((mousePosition.y <= bottomEdge && allowEdgeMovement) || (Input.GetKey(keyMoveDown) && allowEdgeKeys))
					{
						scrollVelocity.z = scrollSpeed;
					}

					if((mousePosition.x <= leftEdge && allowEdgeMovement) || (Input.GetKey(keyMoveLeft) && allowEdgeKeys))
					{
						scrollVelocity.x = scrollSpeed;
					}
					else if((mousePosition.x >= rightEdge && allowEdgeMovement) || (Input.GetKey(keyMoveRight) && allowEdgeKeys))
					{
						scrollVelocity.x = -scrollSpeed;
					}

					// Get the camera's forward direction so we can move relative to it
					Vector3 cameraDirection = _t.forward;
					cameraDirection.y = 0;
					Quaternion referentialShift = Quaternion.FromToRotation(-Vector3.forward, cameraDirection);
					Vector3 moveDirection = referentialShift * scrollVelocity;

					// Move the focal point
					_focalPoint.position += moveDirection * Time.deltaTime;

					if(limitBounds)
					{
						// Make sure we don't go out of bounds
						Vector3 position = _focalPoint.position;

						position.x = Mathf.Clamp(_focalPoint.position.x, boundOrigin.x - boundSize.x / 2f, boundOrigin.x + boundSize.x / 2f);
						position.y = Mathf.Clamp(_focalPoint.position.y, boundOrigin.y - boundSize.y / 2f, boundOrigin.y + boundSize.y / 2f);
						position.z = Mathf.Clamp(_focalPoint.position.z, boundOrigin.z - boundSize.z / 2f, boundOrigin.z + boundSize.z / 2f);

						_focalPoint.position = position;
					}
				}

				// If we aren't using edge movement or want to lock to the target, set the focal point to the target
				if(target)
				{
					if(Input.GetKey(keyFollowTarget) || lockToTarget || (!allowEdgeMovement && !allowEdgeKeys))
					{
						_focalPoint.position = target.position + target.rotation * targetOffset;
					}
				}
			}

			#endregion

			#region FocalPoint Creation, TargetTag and Fade Control

			if(!_focalPoint)
			{
				CreateFocalPoint();
			}

			if(!target)
			{
				// Find our target via tag if we don't have a target
				if(targetTag != string.Empty)
				{
					GameObject targetGameObject = GameObject.FindWithTag(targetTag);

					if(targetGameObject)
					{
						target = targetGameObject.transform;
					}
				}
			}

			if(fadeObjects)
			{
				// Fade objects according to Fade Distance (if enabled)
				foreach(Renderer r in objectsToFade)
				{
					if(r)
					{
						foreach(Material m in r.materials)
						{
							Color c = m.color;
							c.a = Mathf.Clamp(_oldDistance - fadeDistance, 0, 1);

							if(!fadeObjects)
							{
								c.a = 1;
							}

							m.color = c;
						}
					}
				}
			}

			// Fade back in the faded out objects that were in front of the camera
			foreach(Material mat in _fadedMats)
			{
				if(_activeFadedMats.Contains(mat))
				{
					continue;
				}

				if(mat.color.a == 1)
				{
					_fadedMats.Remove(mat);
					break;
				}
				else
				{
					Color c = mat.color;
					c.a = 1;
					mat.color = Color.Lerp(mat.color, c, Time.deltaTime * collisionFadeSpeed);
				}
			}

		}
		#endregion
	}

	void LateUpdate()
	{
		if(target && _focalPoint)
		{
			#region Camera Control

			// Smoothly rotate the camera based on input angle
			Quaternion angleRotation = Quaternion.Euler(_angle.y, _angle.x, 0);
			Quaternion cameraRotation = (useTargetAxis ? target.rotation * angleRotation : angleRotation);

			Quaternion currentRotation = Quaternion.Lerp(_oldRotation, cameraRotation, Time.deltaTime * rotationSmoothing);

			_oldRotation = currentRotation;

			// Smoothly adjust the distance from the target
			float currentDistance = Mathf.Lerp(_oldDistance, distance, Time.deltaTime * zoomSmoothing);

			// See where the camera WANTS to be so we can detect collisions
			Vector3 wantedPosition = _focalPoint.position - currentRotation * Vector3.forward * (distance + collisionBuffer);

			// Test if there are objects between the camera and the target using collision layers
			RaycastHit hit;

			if(Physics.Linecast(_focalPoint.position, wantedPosition, out hit, collisionLayers))
			{
				// If there's an object between the camera and target,
				// adjust the distance so the camera is in front of the object
				float collisionDistance = Vector3.Distance(_focalPoint.position, hit.point) - collisionBuffer;
				
				if(currentDistance > collisionDistance)
				{
					currentDistance = collisionDistance;
				}
			}

			_oldDistance = currentDistance;

			// Adjust the camera position using the focal point and calculated rotation and distance from above
			_t.position = _focalPoint.position - currentRotation * Vector3.forward * currentDistance; ;

			// Look at the focal point using the target's local up direction converted into world space
			_t.LookAt(_focalPoint.position, (useTargetAxis ? target.TransformDirection(Vector3.up) : Vector3.up));

			#endregion

			#region Fade Control

			// Fade out any objects in front of the camera in the alpha layer mask
			Ray ray = new Ray(_focalPoint.position, _t.position - _focalPoint.position);
			RaycastHit[] hits = Physics.RaycastAll(ray, distance, collisionAlphaLayers);

			_activeFadedMats.Clear();

			foreach(RaycastHit alphaHit in hits)
			{
				if(alphaHit.transform.gameObject.renderer)
				{
					Material[] mats = alphaHit.transform.gameObject.renderer.materials;

					foreach(Material mat in mats)
					{
						Color c = mat.color;
						c.a = collisionAlpha;

						mat.color = Color.Lerp(mat.color, c, Time.deltaTime * collisionFadeSpeed);

						_activeFadedMats.Add(mat);

						if(!_fadedMats.Contains(mat))
						{
							_fadedMats.Add(mat);
						}
					}
				}
			}

			#endregion
		}
	}

	// Draw the edge textures and bound limit, mostly for debugging
	void OnGUI()
	{
		if(allowEdgeMovement && showEdges && edgeTexture)
		{
			Rect topEdge = new Rect(0, 0, Screen.width, edgePadding);
			Rect bottomEdge = new Rect(0, Screen.height - edgePadding, Screen.width, Screen.height);
			Rect leftEdge = new Rect(0, 0, edgePadding, Screen.height);
			Rect rightEdge = new Rect(Screen.width - edgePadding, 0, Screen.width, Screen.height);

			GUI.DrawTexture(topEdge, edgeTexture);
			GUI.DrawTexture(bottomEdge, edgeTexture);
			GUI.DrawTexture(leftEdge, edgeTexture);
			GUI.DrawTexture(rightEdge, edgeTexture);
		}
	}

	void OnDrawGizmos()
	{
		if(limitBounds)
		{
			Gizmos.DrawWireCube(boundOrigin, boundSize);
		}
	}

	#endregion

	#region Helper Functions

	private void CreateFocalPoint()
	{
		GameObject go = new GameObject();
		go.name = "_SRPGCfocalPoint";
		_focalPoint = go.transform;

		if(target)
		{
			_focalPoint.position = target.position + target.rotation * targetOffset;
		}
	}

	// These functions just keep our angle values between -180 and 180
	private void ClampAngle(ref Vector3 angle)
	{
		if(angle.x < -180) angle.x += 360;
		else if(angle.x > 180) angle.x -= 360;

		if(angle.y < -180) angle.y += 360;
		else if(angle.y > 180) angle.y -= 360;

		if(angle.z < -180) angle.z += 360;
		else if(angle.z > 180) angle.z -= 360;
	}

	private void ClampAngle(ref Vector2 angle)
	{
		if(angle.x < -180) angle.x += 360;
		else if(angle.x > 180) angle.x -= 360;

		if(angle.y < -180) angle.y += 360;
		else if(angle.y > 180) angle.y -= 360;
	}

	#endregion
}