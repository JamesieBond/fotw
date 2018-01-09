using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseLook : MonoBehaviour {

	Vector2 mouseLook;
	Vector2 smoothV;
	public float sensitivity = 1.0f;
	public float smoothing =  2.0f;

	public Text mouseAccelerationText;
	public Text mouseVelocityText;

	Vector3 initialPosition;
	Vector3 currentPosition;
	Vector3 previousPosition = Vector3.zero;

	GameObject character;

	private float timeInterval = 0f;

	private float mouseSmoothness = 0f;
	private float mouseDistance = 0f;
	private float mouseVelocity = 0f;
	private float mouseAcceleration = 0f;

	private float initialSpeed = 0f;

	private float mouseXMovement = 0f;
	private float mouseYMovement = 0f;

	private int updateCount = 0;
	private float timeCount;


	private bool resetRequired = false;

	void OnGUI() {
		GUI.Box (new Rect (1000, 0, 100, 50), (1 / Time.deltaTime).ToString ());
	}

	// Use this for initialization
	void Start () {
		character = this.transform.parent.gameObject;
		print (Input.mousePosition);
		previousPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
		updateTextValues();
	}
	
	// Update is called once per frame
	void Update () {
		var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
		smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f/ smoothing);
//		smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f/ smoothing);
		mouseLook += smoothV;
		mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

		currentPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
		print ("WorldToScreenPoint = " + Camera.main.ScreenToWorldPoint(Input.mousePosition) + "WorldToViewportPoint = " + Camera.main.WorldToViewportPoint(Input.mousePosition));
//		currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (previousPosition == Vector3.zero) {
			previousPosition = initialPosition;
		}

		if (updateCount < 5) {
//			print ("Update Time: " + Time.deltaTime);
			timeCount += Time.deltaTime;
			updateCount++;
		}

		if (timeInterval < 0.2f) {
			timeInterval += Time.deltaTime;
			mouseXMovement += (Mathf.Abs (currentPosition.x) - Mathf.Abs (previousPosition.x)) / 100f;
			mouseYMovement += (Mathf.Abs (currentPosition.y) - Mathf.Abs (previousPosition.y)) / 100f;
		} else {
			mouseXMovement += (Mathf.Abs (currentPosition.x) - Mathf.Abs (previousPosition.x)) / 100f;
			mouseYMovement += (Mathf.Abs (currentPosition.y) - Mathf.Abs (previousPosition.y)) / 100f;
			timeInterval += Time.deltaTime;
			mouseDistance = mouseXMovement + mouseYMovement;
			mouseVelocity = mouseDistance / timeInterval;
			mouseAcceleration = mouseVelocity - initialSpeed / timeInterval;
			print ("Mouse Acceleration: " + mouseAcceleration);
			initialSpeed = mouseVelocity;
			timeInterval = 0f;
			mouseXMovement = 0;
			mouseYMovement = 0;
		}
		 
		previousPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
//		previousPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		updateTextValues ();

		if (updateCount > 5) {
			resetCalculatedValues ();
		}

		transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
	}

	void updateTextValues () {
		mouseAccelerationText.text = "Acceleration: " + mouseAcceleration;
		mouseVelocityText.text = "Speed: " + mouseVelocity;
	}

	void resetCalculatedValues () {
		print ("Ten frames update: " + timeCount);
		timeCount = Time.deltaTime;

		updateCount = 0;
	}
}
