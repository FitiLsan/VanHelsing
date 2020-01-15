#pragma strict
var InFrontOfTheDoor = false;
var MyPlayer:GameObject;
var N : int = 1;
private var drawGUI = false;
private var drawGUIloading : boolean = false;
var loadscreen : Texture;
var myLevel : String;
var style : GUIStyle;




function Start () {
MyPlayer = GameObject.FindWithTag("Player");
}

function Update () 
{
        if (Input.GetKeyDown(KeyCode.E) && InFrontOfTheDoor == true)
        
        
        {
                if (GameObject.Find ("Spawn_Point_0" + N.ToString ()))
                        MyPlayer.transform.position = GameObject.Find ("Spawn_Point_0" + N.ToString ()).transform.position;
                        
                        
                else
                        MyPlayer.transform.position = new Vector3 (0, 0, 0);
						drawGUIloading = true;

                Application.LoadLevel(myLevel);


                
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
                GUI.Label (Rect (Screen.width / 2.3, Screen.height / 2.2, 1024, 768), "[E] Enter", style);
                
                
                if (drawGUIloading == true)
        	{
                GUI.DrawTexture(Rect(0f, 0f, Screen.width, Screen.height), loadscreen);
			}
}

