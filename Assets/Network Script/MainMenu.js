/* 
*  This file is part of the Unity networking tutorial by M2H (http://www.M2H.nl)
*  The original author of this code is Mike Hergaarden, even though some small parts 
*  are copied from the Unity tutorials/manuals.
*  Feel free to use this code for your own projects, drop us a line if you made something exciting! 
*/
#pragma strict
#pragma implicit
#pragma downcast

static var SP : MainMenu;

private var joinMenuScript : JoinMenu;
private var gameLobbyScript : GameLobby;
private var multiplayerScript : MultiplayerMenu;

private var requirePlayerName : boolean = false;
private var playerNameInput : String = "";

function Awake(){
	SP=this;
	
	playerNameInput = PlayerPrefs.GetString("playerName", "");
	requirePlayerName=true;

	
	joinMenuScript = GetComponent(JoinMenu);
	gameLobbyScript = GetComponent(GameLobby);
	multiplayerScript = GetComponent(MultiplayerMenu);	

	OpenMenu("multiplayer");
}


function OnGUI(){
	if(requirePlayerName){
		myWindowRect = GUILayout.Window (9, Rect(Screen.width/2-150,Screen.height/2-100,300,100), NameMenu, "Please enter a name:");	
	}
}


function OpenMenu(newMenu : String){
	if(requirePlayerName){
		return;
	}
	
	if(newMenu=="multiplayer-quickplay"){					
		joinMenuScript.EnableMenu(true);//quickplay=true	
		
	}else if(newMenu=="multiplayer-host"){ 
		gameLobbyScript.EnableLobby();		
		
	}else if(newMenu=="multiplayer-join"){ 
		joinMenuScript.EnableMenu(false);//quickplay:false
		
	}else if(newMenu=="multiplayer"){ 
		multiplayerScript.EnableMenu();
		
	}else{			
		Debug.LogError("Wrong menu:"+newMenu);	
		
	}
}


function NameMenu(id : int){
	
	GUILayout.BeginVertical();
	GUILayout.Space(10);
			
	
	GUILayout.BeginHorizontal();
	GUILayout.Space(10);
		GUILayout.Label("Please enter your name");
	GUILayout.Space(10);
	GUILayout.EndHorizontal();
	
	
	GUILayout.BeginHorizontal();
	GUILayout.Space(10);
	playerNameInput = GUILayout.TextField(playerNameInput);
	GUILayout.Space(10);
	GUILayout.EndHorizontal();	
	
	
	
	
	 GUILayout.BeginHorizontal();
	GUILayout.Space(10);
		if(playerNameInput.length>=1){
			if(GUILayout.Button("Save")){
				requirePlayerName=false;
				PlayerPrefs.SetString("playerName", playerNameInput);
				OpenMenu("multiplayer");
			}
		}else{
			GUILayout.Label("Enter a name to continue...");
		}
	GUILayout.Space(10);
	GUILayout.EndHorizontal();
	
	
	GUILayout.EndVertical();
	
}

