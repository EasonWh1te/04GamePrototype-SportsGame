using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CS_Player : MonoBehaviour {

	[SerializeField] Vector2 myRotateSpeed;
	[SerializeField] Transform myCameraTransform;

	[SerializeField] float myCameraEulerXLimit = 80;
	private float myCameraEulerX;

	[SerializeField] Rigidbody myRigidbody;
	[SerializeField] float myMoveSpeed;

	private bool isJumping = false;
	[SerializeField] float myJumpSpeed;
	[SerializeField] float myLandDeltaY;

	// bullet
	[SerializeField] GameObject myBullet;
	[SerializeField] Transform myGunTipTransform;

	// pickup
	[SerializeField] float myPickupRange;
	[SerializeField] Transform myPickupParent;
	private Transform myPickupTransform;
	// 是否能够射击
	public int shootChance = 0;
	//开始结束控制
	public static bool gameStart = false;
	//计分
	public static int score = 0;
	//UI
	[SerializeField] Text myScore;
	[SerializeField] Text myStartTips;
	[SerializeField] Text myRound;
	[SerializeField] GameObject myGenerator;
	// Music
	public AudioSource myMusic;
	public AudioClip soundShoot;
	public AudioClip soundNoBullet;
	private void Start () {
		// lock cursor
		Cursor.lockState = CursorLockMode.Locked;

		// init camera euler
		myCameraEulerX = 0;
		myCameraTransform.localRotation = Quaternion.Euler (myCameraEulerX, 0, 0);

		// init jump
		isJumping = false;

		// 重置分数
		CS_Player.score = 0;

		// inti UI
		myScore.enabled = false;
		myRound.enabled = false;
	}

	private void Update () {
		if (Input.GetKeyDown(KeyCode.E) && CS_Generate.isInput == false && gameStart == false)//开始游戏
		{
			Debug.Log("Start!");
			gameStart = true;
			myScore.enabled = true;
			myRound.enabled = true;
			myStartTips.enabled = false;
		}
		// 操控控制
		Update_Rotation ();
		//Update_Movement ();
		//Update_Shoot ();
		Update_Shoot1();
		// UI更新
		myScore.text = "Score:"+score.ToString();
		myRound.text = "Round:" + myGenerator.GetComponent<CS_Generate>().round.ToString()+"/3";
	}

	private void Update_Movement () {
		Vector3 t_moveDirection = Vector3.zero;

		if (Input.GetKey (KeyCode.A)) {
			t_moveDirection.x -= 1;
		}

		if (Input.GetKey (KeyCode.D)) {
			t_moveDirection.x += 1;
		}

		if (Input.GetKey (KeyCode.W)) {
			t_moveDirection.z += 1;
		}

		if (Input.GetKey (KeyCode.S)) {
			t_moveDirection.z -= 1;
		}

		// normalize
		t_moveDirection = t_moveDirection.normalized;

		// rotate the move direction
		t_moveDirection = this.transform.rotation * t_moveDirection;

		// jump
		// set y velocity to current y velocity
		/*Vector3 t_JumpVecter = Vector3.up * myRigidbody.velocity.y;
		if (Input.GetKeyDown (KeyCode.Space) && isJumping == false) {
			// set jump speed
			t_JumpVecter.y = myJumpSpeed;
			isJumping = true; 
		}*/

		// set velocity
		myRigidbody.velocity = t_moveDirection * myMoveSpeed;
		//myRigidbody.velocity = t_moveDirection * myMoveSpeed + t_JumpVecter;
	}

	private void Update_Rotation () {
		// 需要复习re
		// get rotation input
		float t_deltaX = Input.GetAxis ("Mouse X");
		float t_deltaY = Input.GetAxis ("Mouse Y") * -1;

		// update left right rotation
		this.transform.Rotate (Vector3.up, t_deltaX * Time.deltaTime * myRotateSpeed.x);

		// update up down rotation
		myCameraEulerX += t_deltaY * Time.deltaTime * myRotateSpeed.y;
		// limit up down rotation
		myCameraEulerX = Mathf.Clamp (myCameraEulerX, -myCameraEulerXLimit, myCameraEulerXLimit);
		myCameraTransform.localRotation = Quaternion.Euler (myCameraEulerX, 0, 0);
		
		// set angular velocity to 0
		myRigidbody.angularVelocity = Vector3.zero;
	}

	private void Update_Shoot () {
		if (Input.GetMouseButtonDown (0)) {
			// shoot
			GameObject t_bullet = Instantiate (myBullet, myGunTipTransform.position, Quaternion.identity);
			t_bullet.GetComponent<CS_Bullet> ().Init (
				myCameraTransform.forward, this.gameObject);
		}
	}

	private void Update_Shoot1 () {
		if (Input.GetMouseButtonDown(0) && shootChance != 0)
		{
			// pickup ray cast
			Ray t_ray = new Ray(myCameraTransform.position, myCameraTransform.forward);
			RaycastHit t_raycastHit;
			if (Physics.Raycast(t_ray, out t_raycastHit, myPickupRange) == true)
			{
				Transform t_transform = t_raycastHit.transform;
				if (t_transform.GetComponent<CS_Saucer>() == true)
				{
					// hit
					score++;
					t_transform.GetComponent<CS_Saucer>().MyDestroy();
				}
			}
			myMusic.clip = soundShoot;
			myMusic.Play();
			shootChance--;
		}
		else if (Input.GetMouseButtonDown(0) && shootChance == 0)
		{
			myMusic.clip = soundNoBullet;
			myMusic.Play();
		}
	}

	private void OnCollisionEnter (Collision g_collision) {
		for (int i = 0; i< g_collision.contacts.Length; i++) {
			if (g_collision.contacts[i].point.y < this.transform.position.y + myLandDeltaY) {
				// landed
				isJumping = false;
				break;
			}
		}
	}
}
