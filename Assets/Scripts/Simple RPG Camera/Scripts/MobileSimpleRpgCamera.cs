using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobileSimpleRpgCamera : MonoBehaviour
{
	public enum ControlType
	{
		Swipe,
		TwoTouchRotate,
		Drag
	}
	
	[HideInInspector] public Transform target;
	[HideInInspector] public string targetTag = string.Empty;
	
	public LayerMask collisionLayers = new LayerMask();
	public LayerMask collisionAlphaLayers = new LayerMask();
	[HideInInspector] public float collisionAlpha = 0.15f;
	[HideInInspector] public float collisionFadeSpeed = 10;
	
	[HideInInspector] public bool allowRotation = true;
	[HideInInspector] public Vector2 rotationAmount = new Vector2(45, 45);
	[HideInInspector] public bool invertX = false;
	[HideInInspector] public bool invertY = false;
	[HideInInspector] public bool stayBehindTarget = false;
	
	[HideInInspector] public bool allowPanning = false;
	[HideInInspector] public float panningSensitivity = 0.5f;
	[HideInInspector] public int panningTouchCount = 3;
	[HideInInspector] public bool panningAllowX = true;
	[HideInInspector] public bool panningAllowY = true;
	[HideInInspector] public Vector2 panAmount = new Vector2(1, 1);
	[HideInInspector] public float panSpeed = 4;
	[HideInInspector] public bool panInvertX = false;
	[HideInInspector] public bool panInvertY = false;
	
	[HideInInspector] public Vector2 targetOffset = new Vector2();
	
	[HideInInspector] public ControlType rotationType = ControlType.Swipe;
	[HideInInspector] public ControlType panType = ControlType.Drag;
	[HideInInspector] public float swipeActiveTime = 0.2f;
	[HideInInspector] public float swipeMinDistance = 150;
	
	[HideInInspector] public bool fadeObjects = false;
	[HideInInspector] public float fadeDistance = 1.5f;
	[HideInInspector] public List<Renderer> objectsToFade;
	
	[HideInInspector] public Vector2 originRotation = new Vector2();
	[HideInInspector] public float returnSmoothing = 3;
	
	[HideInInspector] public float distance = 5;
	[HideInInspector] public float minDistance = 0;
	[HideInInspector] public float maxDistance = 10;
	
	[HideInInspector] public float zoomSpeed = 0.01f;
	[HideInInspector] public float zoomSmoothing = 16;
	[HideInInspector] public bool zoomInvert = false;
	
	[HideInInspector] public float minAngle = -90;
	[HideInInspector] public float maxAngle = 90;
	
	private List<Material> _faded_mats = new List<Material>();
	private List<Material> _active_faded_mats = new List<Material>();
	
	private float _previous_distance;
	private float _wanted_distance;
	private Quaternion _rotation;
	private Vector2 _input_rotation;
	
	private float _previous_angle;
	
	private bool _controllable = true;
	
	private bool _zooming = false;
	
	private bool _swipe = false;
	private Vector2 _swipe_start = new Vector2();
	private float _swipe_start_time = 0;
	private float _swipe_time = 0;
	private Vector2 _swipe_dist = new Vector2();
	private Vector2 _swipe_direction = new Vector2();
	private Vector3 _wanted_swipe_position = new Vector3();
	
	private Transform _invisibleTarget;
	
	private Transform _t;
	
	public Vector2 InputRotation
	{
		get { return _input_rotation; }
	}
	
	public bool Controllable
	{
		get { return _controllable; }
		set { _controllable = value; }
	}
	
	public bool Swipe
	{
		get { return _swipe; }
	}
	
	public Vector2 SwipeDist
	{
		get { return _swipe_dist; }
	}
	
	public float SwipeTime
	{
		get { return _swipe_time; }
	}
	
	public Vector2 SwipeDirection
	{
		get { return _swipe_direction; }
	}
	
	public bool Zooming
	{
		get { return _zooming; }
	}
	
	void Start()
	{
		_t = transform;
		_wanted_distance = distance;
		_input_rotation = originRotation;
	}
	
	
	void Update()
	{
		if(target)
		{
			if(!_invisibleTarget)
			{
				CreateInvisibleTarget();
			}
			
			// Fade the target according to Fade Distance (if enabled)
			foreach(Renderer r in objectsToFade)
			{
				if(r)
				{
					foreach(Material m in r.materials)
					{
						Color c = m.color;
						c.a = Mathf.Clamp(distance - fadeDistance, 0, 1);
						
						if(!fadeObjects)
						{
							c.a = 1;
						}
						
						m.color = c;
					}
				}
			}
		}
		else
		{
			if(targetTag.Length > 0)
			{
				GameObject t = GameObject.FindWithTag(targetTag);
				
				if(t)
				{
					target = t.transform;
				}
			}
		}
		
		// Fade back in the faded out objects that were in front of the camera
		foreach(Material mat in _faded_mats)
		{
			if(_active_faded_mats.Contains(mat))
			{
				continue;
			}
			
			if(mat.color.a == 1)
			{
				_faded_mats.Remove(mat);
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
	
	// Camera movement should be done in LateUpdate()
	void LateUpdate()
	{
		if(target)
		{
			Transform currentTarget;
			
			if(!allowPanning)
			{
				currentTarget = target;
				_invisibleTarget.position = target.position;
			}
			else
			{
				currentTarget = _invisibleTarget;
			}
			
			if(_controllable)
			{
				// Zoom control
				if(Input.touchCount == 2)
				{
					Touch t1 = Input.GetTouch(0);
					Touch t2 = Input.GetTouch(1);
					_zooming = true;
					
					if(t1.phase == TouchPhase.Began ||
					   t2.phase == TouchPhase.Began)
					{
						_previous_distance = Vector2.Distance(t1.position, t2.position);
					}
					else if(t1.phase == TouchPhase.Moved ||
					        t2.phase == TouchPhase.Moved)
					{
						float d = Vector2.Distance(t1.position, t2.position);
						
						if(zoomInvert)
						{
							_wanted_distance += (_previous_distance - d) * zoomSpeed;
						}
						else
						{
							_wanted_distance -= (_previous_distance - d) * zoomSpeed;
						}
						
						_previous_distance = d;
					}
				}
				else if(Input.touchCount == 0 && _zooming)
				{
					_zooming = false;
				}
			}
			
			
			// Prevent wanted distance from going below or above min and max distance
			_wanted_distance = Mathf.Clamp(_wanted_distance, minDistance, maxDistance);
			
			// If user drags, change position based on drag direction and sensitivity
			// Stop at 90 degrees above / below object
			if(allowRotation && _controllable && Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);
				
				if(rotationType == ControlType.Swipe)
				{
					switch(touch.phase)
					{
					case TouchPhase.Began:
						_swipe = true;
						_swipe_start = touch.position;
						_swipe_start_time = Time.time;
						break;
						
					case TouchPhase.Moved:
					case TouchPhase.Stationary:
						if(Time.time - _swipe_start_time > swipeActiveTime)
						{
							_swipe = false;
						}
						break;
						
					case TouchPhase.Ended:
						if(_swipe)
						{
							_swipe_time = Time.time - _swipe_start_time;
							_swipe_dist.x = Mathf.Abs(touch.position.x - _swipe_start.x);
							_swipe_dist.y = Mathf.Abs(touch.position.y - _swipe_start.y);
							
							if((_swipe_time <= swipeActiveTime))
							{
								if(_swipe_dist.x > swipeMinDistance)
								{
									_swipe_direction.x = Mathf.Sign(touch.position.x - _swipe_start.x);
								}
								else
								{
									_swipe_direction.x = 0;
								}
								
								if(_swipe_dist.y > swipeMinDistance)
								{
									_swipe_direction.y = Mathf.Sign(touch.position.y - _swipe_start.y);
								}
								else
								{
									_swipe_direction.y = 0;
								}
								
								if(invertX)
								{
									originRotation.x += rotationAmount.x * _swipe_direction.x;
								}
								else
								{
									originRotation.x -= rotationAmount.x * _swipe_direction.x;
								}
								
								ClampRotation();
								
								if(invertY)
								{
									originRotation.y += rotationAmount.y * _swipe_direction.y;
								}
								else
								{
									originRotation.y -= rotationAmount.y * _swipe_direction.y;
								}
								
								originRotation.y = Mathf.Clamp(originRotation.y, minAngle, maxAngle);
							}
							
							_swipe = false;
						}
						break;
					}
				}
				else if(rotationType == ControlType.Drag)
				{
					if(touch.phase == TouchPhase.Moved && _swipe == false)
					{
						if(invertX)
						{
							originRotation.x -= touch.deltaPosition.x;
						}
						else
						{
							originRotation.x += touch.deltaPosition.x;
						}
						
						if(invertY)
						{
							originRotation.y += touch.deltaPosition.y;
						}
						else
						{
							originRotation.y -= touch.deltaPosition.y;
						}
					}
					
					originRotation.y = Mathf.Clamp(originRotation.y, minAngle, maxAngle);
				}
			}
			else if(allowRotation && _controllable && rotationType==ControlType.TwoTouchRotate && Input.touchCount==2)
			{
				// Two finger rotate control
				//if(Input.touchCount == 2)
				{
					Touch t1 = Input.GetTouch(0);
					Touch t2 = Input.GetTouch(1);
					
					if(t1.phase == TouchPhase.Began ||
					   t2.phase == TouchPhase.Began)
					{
						_previous_angle = GetAngle(t1.position, t2.position);
						
					}
					else if(t1.phase == TouchPhase.Moved ||
					        t2.phase == TouchPhase.Moved)
					{
						
						float a = GetAngle(t1.position, t2.position);
						float delta = (_previous_angle-a);
						
						
						if (delta>180F) 
						{
							delta=360F-delta;
						}
						else if (delta<-180f)
						{
							delta=delta+360f;
						}
						
						//Debug.Log (a);
						
						originRotation.x += delta;
						ClampRotation();
						
						_previous_angle = a;
						
					}
					
				}
				
			}
			// Keeps the camera behind the target when not controlling it (if enabled)
			if(stayBehindTarget && currentTarget)
			{
				originRotation.x = currentTarget.eulerAngles.y;
				ClampRotation();
			}
			
			// Move camera to the default position
			returnSmoothing = Mathf.Clamp(returnSmoothing, 1, 9999);
			_input_rotation = Vector3.Lerp(_input_rotation, originRotation, returnSmoothing * Time.deltaTime);
			
			_rotation = Quaternion.Euler(_input_rotation.y, _input_rotation.x, 0);
			
			// Lerp from current distance to wanted distance
			distance = Mathf.Clamp(Mathf.Lerp(distance, _wanted_distance, Time.deltaTime * zoomSmoothing), minDistance, maxDistance);
			
			// Test if there are objects between the camera and the target using collision layers
			
			if(currentTarget)
			{
				Vector3 camera_position = _rotation * new Vector3(0, 0, -_wanted_distance - 0.2f) + currentTarget.position + currentTarget.rotation * new Vector3(targetOffset.x, targetOffset.y, 0);
				Vector3 target_position = currentTarget.rotation * new Vector3(targetOffset.x, targetOffset.y, 0) + currentTarget.position;
				
				RaycastHit line_hit;
				
				if(Physics.Linecast(target_position, camera_position, out line_hit, collisionLayers))
				{
					distance = Vector3.Distance(target_position, line_hit.point) - 0.2f;
				}
				
				// Fade out any objects in front of the camera in the alpha layer mask
				Ray ray = new Ray(target_position, camera_position - target_position);
				RaycastHit[] hits = Physics.RaycastAll(ray, distance, collisionAlphaLayers);
				
				_active_faded_mats.Clear();
				
				foreach(RaycastHit hit in hits)
				{
					if(hit.transform.gameObject.renderer)
					{
						Material[] mats = hit.transform.gameObject.renderer.materials;
						
						foreach(Material mat in mats)
						{
							Color c = mat.color;
							c.a = collisionAlpha;
							
							mat.color = Color.Lerp(mat.color, c, Time.deltaTime * collisionFadeSpeed);
							
							_active_faded_mats.Add(mat);
							
							if(!_faded_mats.Contains(mat))
							{
								_faded_mats.Add(mat);
							}
						}
					}
				}
			}
			
			// Set the position and rotation of the camera
			if(allowPanning && _controllable)
			{
				if(panType == ControlType.Swipe)
				{
					if(Input.touchCount > 0)
					{
						Touch touch = Input.GetTouch(0);
						
						switch(touch.phase)
						{
						case TouchPhase.Began:
							_swipe = true;
							_swipe_start = touch.position;
							_swipe_start_time = Time.time;
							_wanted_swipe_position = currentTarget.position;
							break;
							
						case TouchPhase.Moved:
						case TouchPhase.Stationary:
							if(Time.time - _swipe_start_time > swipeActiveTime)
							{
								_swipe = false;
							}
							break;
							
						case TouchPhase.Ended:
							if(_swipe)
							{
								_swipe_time = Time.time - _swipe_start_time;
								_swipe_dist.x = Mathf.Abs(touch.position.x - _swipe_start.x);
								_swipe_dist.y = Mathf.Abs(touch.position.y - _swipe_start.y);
								
								if((_swipe_time <= swipeActiveTime))
								{
									if(_swipe_dist.x > swipeMinDistance)
									{
										_swipe_direction.x = Mathf.Sign(touch.position.x - _swipe_start.x);
									}
									else
									{
										_swipe_direction.x = 0;
									}
									
									if(_swipe_dist.y > swipeMinDistance)
									{
										_swipe_direction.y = Mathf.Sign(touch.position.y - _swipe_start.y);
									}
									else
									{
										_swipe_direction.y = 0;
									}
									
									Vector3 movement = new Vector3();
									
									if(panningAllowX)
									{
										if(panInvertX)
										{
											movement -= _t.right * _swipe_direction.x * panAmount.x;
										}
										else
										{
											movement += _t.right * _swipe_direction.x * panAmount.x;
										}
									}
									
									if(panningAllowY)
									{
										if(panInvertY)
										{
											movement -= _t.up * _swipe_direction.y * panAmount.y;
										}
										else
										{
											movement += _t.up * _swipe_direction.y * panAmount.y;
										}
									}
									
									_wanted_swipe_position += movement;
								}
								
								_swipe = false;
							}
							break;
						}
					}
					
					if(currentTarget)
					{
						currentTarget.position = Vector3.Lerp(currentTarget.position, _wanted_swipe_position, Time.deltaTime * panSpeed);
					}
				}
				else if(panType == ControlType.Drag)
				{
					if(Input.touchCount >= panningTouchCount)
					{
						Vector2 delta = Input.GetTouch(0).deltaPosition;
						
						if(panInvertX)
						{
							delta.x = -delta.x;
						}
						
						if(panInvertY)
						{
							delta.y = -delta.y;
						}
						
						if(!panningAllowX)
						{
							delta.x = 0;
						}
						
						if(!panningAllowY)
						{
							delta.y = 0;
						}
						
						Vector3 scrollVelocity = delta * panningSensitivity;
						
						Vector3 cameraDirection = _t.forward;
						cameraDirection.y = 0;
						Quaternion referentialShift = Quaternion.FromToRotation(-Vector3.forward, cameraDirection);
						Vector3 moveDirection = referentialShift * scrollVelocity;
						
						if(currentTarget)
						{
							currentTarget.position += moveDirection * Time.deltaTime;
						}
					}
				}
			}
			
			if(currentTarget)
			{
				_t.position = _rotation * new Vector3(0, 0, -distance) + currentTarget.position + currentTarget.rotation * new Vector3(targetOffset.x, targetOffset.y, 0);
			}
			
			_t.rotation = _rotation;
		}
	}
	
	private void ClampRotation()
	{
		if(originRotation.x < -180)
		{
			originRotation.x += 360;
		}
		else if(originRotation.x > 180)
		{
			originRotation.x -= 360;
		}
		
		if(_input_rotation.x - originRotation.x < -180)
		{
			_input_rotation.x += 360;
		}
		else if(_input_rotation.x - originRotation.x > 180)
		{
			_input_rotation.x -= 360;
		}
	}
	
	private float GetAngle(Vector2 fromVector2,Vector2 toVector2)
	{
		Vector2 v2 = fromVector2 - toVector2;
		float angle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg;;
		angle += 180f;
		_debugtext = "Angle " + angle + " T1X "+ fromVector2.x + " T1Y "+ fromVector2.y + " T2X "+ toVector2.x + " T2Y "+ toVector2.y;
		
		return angle;
	}
	
	private void CreateInvisibleTarget()
	{
		GameObject go = new GameObject();
		go.name = "_MSRPGCinvisibleTarget";
		_invisibleTarget = go.transform;
		
		if(target)
		{
			_invisibleTarget.position = target.position;
			_wanted_swipe_position = target.position;
		}
	}
	
	private string _debugtext;
	
	void OnGUI () 
	{
		GUI.Label (new Rect (0, 0, 100, 150), _debugtext);
	}
}