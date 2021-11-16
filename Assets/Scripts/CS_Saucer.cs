using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Saucer : MonoBehaviour
{
	[SerializeField] Rigidbody myRigidbody;
	[SerializeField] float mySpeed;

	private Vector3 myDirection;

	// kill by time
	[SerializeField] float myLiveTime;
	private float myKillTime;

    
	//²âÊÔÓÃ ´ý»á¶ùÉ¾³ý
	private void Start()
    {
		myKillTime = Time.time + myLiveTime; 
	}
    public void Init(Vector3 g_direction)
	{
		myDirection = g_direction;
		// init kill time
		myKillTime = Time.time + myLiveTime;
		myRigidbody.velocity = myDirection * mySpeed;
	}
	private void Update()
	{
		
		// check if it should be killed
		if (myKillTime < Time.time)
		{
			//Debug.Log(1);
			Destroy(this.gameObject);
		}
	}

	public void MyDestroy()
	{
		Destroy(this.gameObject);
	}
}
