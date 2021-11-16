using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Bullet : MonoBehaviour {
	[SerializeField] Rigidbody myRigidbody;
	[SerializeField] float mySpeed;

	private Vector3 myDirection;

	// kill bullet by time
	[SerializeField] float myLiveTime;
	private float myKillTime;

	private GameObject myOrigin;

	public void Init (Vector3 g_direction, GameObject g_origin) {
		myDirection = g_direction;
		myOrigin = g_origin;

		// init kill time
		myKillTime = Time.time + myLiveTime;
	}

	private void Update () {
		myRigidbody.velocity = myDirection * mySpeed;

		// check if the bullet should be killed
		if (myKillTime < Time.time) {
			Destroy (this.gameObject);
		}
	}

	private void OnTriggerEnter (Collider g_other) {
		Debug.Log (myOrigin.name + ";" + g_other.gameObject.name);
		if (g_other.gameObject == myOrigin ||
			g_other.GetComponent<CS_Bullet> () == true) {
			return;
		}

		//// if enemy bullet hit enemy, do not do anything
		//if (myOrigin.GetComponent<CS_Enemy> () == true &&
		//	g_other.GetComponent<CS_Enemy> () == true) {
		//	return;
		//}

		//// hit player
		//if (g_other.GetComponent<CS_Player> () == true) {
		//	g_other.GetComponent<CS_Player> ().TakeDamage (1);
		//}

		//// kill enemy
		//if (g_other.GetComponent<CS_Enemy> () == true) {
		//	g_other.gameObject.SetActive (false);
		//	CS_GameManager.Instance.CheckGameOver ();
		//}

		//// kill box
		//if (g_other.GetComponent<CS_Box> () == true) {
		//	g_other.gameObject.SetActive (false);
		//}

		//Destroy (this.gameObject);
	}
}
