#pragma strict

var Grindstone : GameObject;
var Sound : AudioClip;


function OnTriggerEnter (theCollider : Collider)
{
	if (theCollider.tag == "Player")
	{
		Grindstone.GetComponent.<Animation>().Play("Grindstone_turn");
		GetComponent.<AudioSource>().clip = Sound;
        GetComponent.<AudioSource>().Play();
	}
}


function OnTriggerExit (theCollider : Collider)
{
	if (theCollider.tag == "Player")
	{
		Grindstone.GetComponent.<Animation>().Stop("Grindstone_turn");
		GetComponent.<AudioSource>().clip = Sound;
        GetComponent.<AudioSource>().Stop();
	}
}

