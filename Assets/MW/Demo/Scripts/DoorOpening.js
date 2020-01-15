#pragma strict

private var guiShow : boolean = false;

var isOpen : boolean = false;

var door : GameObject;

var rayLength = 10;

function Update()
{
	var hit : RaycastHit;
	var fwd = transform.TransformDirection(Vector3.forward);
	
	if(Physics.Raycast(transform.position, fwd, hit, rayLength))
	{
		if(hit.collider.gameObject.tag == "Door")
		{
			guiShow = true;
			if(Input.GetKeyDown("e") && isOpen == false)
			{
				door.GetComponent.<Animation>().Play("DoorOpen");
				isOpen = true;
				guiShow = false;
			}
			
			else if(Input.GetKeyDown("e") && isOpen == true)
			{
				door.GetComponent.<Animation>().Play("DoorClose");
				isOpen = false;
				guiShow = false;
			}
		}
	}
	
	else
	{
		guiShow = false;
	}
}

function OnGUI()
{
	if(guiShow == true && isOpen == false)
	{
		GUI.Box(Rect(Screen.width / 2, Screen.height / 2, 100, 25), "Use Door");
	}
}