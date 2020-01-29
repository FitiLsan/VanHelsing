#pragma strict
var InFrontOfTheDoor = false;
var MyPlayer:GameObject;
var N : int = 1;
var Sound : AudioClip;
private var drawGUI = false;
var style : GUIStyle;





function Start () {
MyPlayer = GameObject.FindWithTag("Player");
}

function Update () 
{
        if (Input.GetKeyDown(KeyCode.E) && InFrontOfTheDoor == true){
                if (GameObject.Find ("Spawn_Point_0" + N.ToString ()))
                        MyPlayer.transform.position = GameObject.Find ("Spawn_Point_0" + N.ToString ()).transform.position;
                        
                else
                        MyPlayer.transform.position = new Vector3 (0, 0, 0);
                                GetComponent.<AudioSource>().clip = Sound;
        GetComponent.<AudioSource>().Play();

                
                
        }
}


function OnTriggerEnter (theCollider : Collider)
{
        if (theCollider.tag == "Player")
        {
                InFrontOfTheDoor = true;
                drawGUI = true;
        }
}

function OnTriggerExit (theCollider : Collider)
{
        if (theCollider.tag == "Player")
        {
                InFrontOfTheDoor = false;
                drawGUI = false;
        }
}

function OnGUI ()
{
        if (drawGUI == true)
                GUI.Label (Rect (Screen.width / 2.3, Screen.height / 2.2, 1024, 768), "[E] Climb", style);
                
                
}

