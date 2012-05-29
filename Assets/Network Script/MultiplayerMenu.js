/* 
*  This file is part of the Unity networking tutorial by M2H (http://www.M2H.nl)
*  The original author of this code is Mike Hergaarden, even though some small parts 
*  are copied from the Unity tutorials/manuals.
*  Feel free to use this code for your own projects, drop us a line if you made something exciting! 
*/
#pragma strict
#pragma implicit
#pragma downcast

private var showMenu : boolean = false;
private var myWindowRect : Rect;
private var mainMenuScript : MainMenu;


function Awake(){
	myWindowRect  = Rect (Screen.width/2-150,Screen.height/2-100,300,200);	
}


function Start(){
	mainMenuScript =  MainMenu.SP;
}


function EnableMenu(){
	showMenu=true;
}

function OnGUI ()
{		
	if(!showMenu){
		return;
	}
	myWindowRect = GUILayout.Window (0, myWindowRect, windowGUI, "Multiplayer");			
}


function windowGUI(id : int){

	GUILayout.BeginVertical();
	GUILayout.Space(10);
	GUILayout.EndVertical();
	
	GUILayout.BeginHorizontal();
	GUILayout.Space(10);	
	GUILayout.Label("");
	GUILayout.Space(10);
	GUILayout.EndHorizontal();	
	
	
	if(GUI.Button(Rect(50,60,200,20), "Host a game")){
		showMenu=false;
		mainMenuScript.OpenMenu("multiplayer-host");
	}
	
	if(GUI.Button(Rect(50,90,200,20), "Select a game to join")){
		showMenu=false;
		mainMenuScript.OpenMenu("multiplayer-join");
	}
	
	if(GUI.Button(Rect(50,120,200,20), "Join a game via quickplay")){
		showMenu=false;
		mainMenuScript.OpenMenu("multiplayer-quickplay");
	}
	
}
		
	

