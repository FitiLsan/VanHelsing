#pragma strict

private var guiShow : boolean = false;

var riddle : Texture;

function OnTriggerStay (Col : Collider)
{
	if(Col.tag == "Player")
	{
		guiShow = true;
	}
}

function OnTriggerExit (Col : Collider)
{
	if(Col.tag == "Player")
	{
		guiShow = false;
	}
}

function OnGUI()
{
	if(guiShow == true)
	{
		GUI.DrawTexture(Rect(0f, 0f, Screen.width, Screen.height), riddle);
	}
}