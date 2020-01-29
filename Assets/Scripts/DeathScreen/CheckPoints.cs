using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] Transform _chekpoint1;//точки респаунов 
	[SerializeField] Transform _chekpoint2;

	public bool CheckPoint1;
	public bool CheckPoint2;
	public bool CanRespawn;

	void Update()
	{
		if (CanRespawn)//CanRespawn равно тру если жизни = 0 в DeathScreen
		{
			if (CheckPoint1)
			{
				player.transform.position = _chekpoint1.position;
			}
			else if (CheckPoint2)
			{
				 
				player.transform.position = _chekpoint2.position;
			}
		}
		if(player.transform.position == _chekpoint1.position|| player.transform.position == _chekpoint2.position)
		{
			CanRespawn = false;//если не отключить CanRespawn то персонаж не будет двигаться 
		}
	}


	private void OnTriggerEnter(Collider collision) 
	{
		if (collision.gameObject.tag == "CheckPoint1")
		{
			CheckPoint1 = true;
			CheckPoint2 = false;
		}
		if (collision.gameObject.tag == "CheckPoint2")
		{
            CheckPoint2 = true;
			CheckPoint1 = false;
		}
	}
}

