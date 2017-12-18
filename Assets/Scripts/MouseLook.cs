using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseLook : MonoBehaviour {

	Vector2 mouseLook;
	Vector2 smoothV;
	public float sensitivity = 1.0f;
	public float smoothing =  2.0f;

	public Text mouseSmoothnessText;
	public Text mouseSpeedText;

	Vector3 initialPosition;
	Vector3 currentPosition;
	Vector3 previousPosition = Vector3.zero;

	GameObject character;

	private float mouseSmoothness = 0f;
	private float mouseSpeed = 0f;

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
		smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f/ smoothing);
		mouseLook += smoothV;
		mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

		currentPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
		if (previousPosition == Vector3.zero) {
			previousPosition = initialPosition;
		}
		mouseSpeed = Mathf.Abs(currentPosition.x - previousPosition.x) + Mathf.Abs(currentPosition.y - previousPosition.y);
		previousPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);

		updateTextValues();

		transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
	}

	void updateTextValues () {
		mouseSmoothnessText.text = "Smoothness: " + mouseSmoothness;
		mouseSpeedText.text = "Speed: " + mouseSpeed;
	}

}
