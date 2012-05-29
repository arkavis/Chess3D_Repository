/* 
*  This file is part of the Unity networking tutorial by M2H (http://www.M2H.nl)
*  The original author of this code is Mike Hergaarden, even though some small parts 
*  are copied from the Unity tutorials/manuals.
*  Feel free to use this code for your own projects, drop us a line if you made something exciting! 
*/
#pragma strict
#pragma implicit
#pragma downcast

var serverPort : int = 45672;
var gameName : String = "Lobby";

private var launchingGame : boolean = false;
private var showMenu : boolean = false;

private var playerList : Array = new Array();
class PlayerInfo {
	var username : String;
	var player : NetworkPlayer;
}

private var serverMaxPlayers : int =2;
private var serverTitle : String = "Loading..";
private var serverPasswordProtected : boolean = false;

private var playerName : String = "";

private var mainMenuScript : MainMenu;


function Awake(){
	showMenu=false;
}


function Start(){
	mainMenuScript =  MainMenu.SP;
}


function EnableLobby(){
	playerName = PlayerPrefs.GetString("playerName");
	
	
	 
	lastRegTime=Time.time-3600;
	
	launchingGame=false;
	showMenu=true;
	
	var chat : LobbyChat = GetComponent(LobbyChat);		
	chat.ShowChatWindow();
}


function OnGUI () {
	if(!showMenu){
		return;
	}

	
	//Back to main menu
	if(GUI.Button(Rect(40,10,150,20), "Back to main menu")){
		leaveLobby();
	}
	
	if(launchingGame){		
		launchingGameGUI();
		
	} else if(!Network.isServer && !Network.isClient){
		//First set player count, server name and password option			
		hostSettings();
		
	} else {
		//Show the lobby		
		showLobby();
	}
}


function leaveLobby(){
	//Disconnect fdrom host, or shotduwn host
	if (Network.isServer || Network.isClient){
		if(Network.isServer){
			MasterServer.UnregisterHost();
		}
		Network.Disconnect();
		yield WaitForSeconds(0.3);
	}	
	
	var chat : LobbyChat = GetComponent(LobbyChat);
	chat.CloseChatWindow();
		
	mainMenuScript.OpenMenu("multiplayer");
	
	showMenu=false;
}


private var hostSetting_title : String = "No server title";
private var hostSetting_players : int = 2;
private var hostSetting_password : String = "";


function hostSettings(){
	
	GUI.BeginGroup (Rect (Screen.width/2-175, Screen.height/2-75-50, 350, 150));
	GUI.Box (Rect (0,0,350,150), "Server options");
	
	GUI.Label (Rect (10,20,150,20), "Server title");
	hostSetting_title = GUI.TextField (Rect (175,20,160,20), hostSetting_title);
	
	GUI.Label (Rect (10,40,150,20), "Server Port");
	serverPort = parseInt(GUI.TextField (Rect (175,40,160,20), serverPort+""));
	
	GUI.Label (Rect (10,60,150,50), "Password\n");
	hostSetting_password = (GUI.TextField (Rect (175,60,160,20), hostSetting_password));
	
	
	if(GUI.Button (Rect (100,115,150,20), "Go to lobby")){
		StartHost(hostSetting_password, 2, hostSetting_title, parseInt(serverPort));
	}
	GUI.EndGroup();
}


function StartHost(password : String, players : int, serverName : String, port : int){
	serverPort = port;
	if(players<1){
		players=1;
	}
	if(players>=32){
		players=32;
	}
	player = 2;
	if(password && password!=""){
		serverPasswordProtected  = true;
		Network.incomingPassword = password;
	}else{
		serverPasswordProtected  = false;
		Network.incomingPassword = "";
	}
	
	serverTitle = serverName;

	Network.InitializeSecurity();
	Network.InitializeServer((players-1), serverPort);	
}


function showLobby(){
	var players = "";
	var currentPlayerCount : int =0;
	for (var playerInstance : PlayerInfo in playerList) {
		players=playerInstance.username+"\n"+players;
		currentPlayerCount++;	
	}
	
	GUI.BeginGroup (Rect (Screen.width/2-200, Screen.height/2-200, 400, 180));
	GUI.Box (Rect (0,0,400,200), "Game lobby");
	

	var pProtected="no";
	if(serverPasswordProtected){
		pProtected="yes";
	}
	GUI.Label (Rect (10,20,150,20), "Password protected");
	GUI.Label (Rect (150,20,100,100), pProtected);
	
	GUI.Label (Rect (10,40,150,20), "Server title");
	GUI.Label (Rect (150,40,100,100), serverTitle);
	
	GUI.Label (Rect (10,60,150,20), "Server Port");
	GUI.Label (Rect (150,60,100,100), serverPort+"");
	
	GUI.Label (Rect (10,80,150,20), "Players");
	GUI.Label (Rect (150,80,100,100), currentPlayerCount+"/"+serverMaxPlayers);
	
	GUI.Label (Rect (10,100,150,20), "Current players");
	GUI.Label (Rect (150,100,100,100), players);
	
	
	if(Network.isServer){
		if(GUI.Button (Rect (25,140,150,20), "Start the game")){
			HostLaunchGame();
		}
	}else{
		GUI.Label (Rect (25,140,200,40), "Waiting for the server to start the game..");
	}
	
	GUI.EndGroup();
}


function OnConnectedToServer(){
	//Called on client
	//Send everyone this clients data
	playerList  = new Array();
	playerName = PlayerPrefs.GetString("playerName");
	networkView.RPC("addPlayer",RPCMode.AllBuffered, Network.player, playerName);	
}


function OnServerInitialized(){
	//Called on host
	//Add hosts own data to the playerlist	
	playerList  = new Array();
	networkView.RPC("addPlayer",RPCMode.AllBuffered, Network.player, playerName);
	
	var pProtected : boolean = false;
	if(Network.incomingPassword && Network.incomingPassword!=""){
		pProtected=true;
	}
	var maxPlayers : int = Network.maxConnections+1;
	
	networkView.RPC("setServerSettings",RPCMode.AllBuffered, pProtected, maxPlayers, hostSetting_title);
	
}


var lastRegTime : float = -60;
function Update(){
	if(Network.isServer && lastRegTime<Time.time-60){
		lastRegTime=Time.time;
		MasterServer.RegisterHost(gameName,hostSetting_title, "No description");
	}
}


@RPC
function setServerSettings(password : boolean, maxPlayers : int, newSrverTitle : String){
	serverMaxPlayers = maxPlayers;
	serverTitle  = newSrverTitle;
	serverPasswordProtected  = password;
}


function OnPlayerDisconnected(player: NetworkPlayer) {
	//Called on host
	//Remove player information from playerlist
	networkView.RPC("playerLeft", RPCMode.All, player);

	var chat : LobbyChat = GetComponent(LobbyChat);
	chat.addGameChatMessage("A player left the lobby");
}


@RPC
function addPlayer(player : NetworkPlayer, username : String){
	Debug.Log("got addplayer"+username);
	
	var playerInstance : PlayerInfo = new PlayerInfo();
	playerInstance.player = player;
	playerInstance.username = username;		
	playerList.Add(playerInstance);
}


@RPC
function playerLeft(player : NetworkPlayer){
	
	var deletePlayer : PlayerInfo;
	
	for (var playerInstance : PlayerInfo in playerList) {
		if (player == playerInstance.player) {			
			deletePlayer = playerInstance;
		}
	}
	playerList.Remove(deletePlayer);
	Network.RemoveRPCs(player);
	Network.DestroyPlayerObjects(player);
}

function HostLaunchGame(){
	if(!Network.isServer){
		return;
	}
	
	// Don't allow any more players
	Network.maxConnections = -1;
	MasterServer.UnregisterHost();	
	
	networkView.RPC("launchGame",RPCMode.All);
}


@RPC
function launchGame(){
	Network.isMessageQueueRunning=false;
	launchingGame=true;
}


function launchingGameGUI(){
	//Show loading progress, ADD LOADINGSCREEN?
	GUI.Box(Rect(Screen.width/4+180,Screen.height/2-30,280,50), "");
	if(Application.CanStreamedLevelBeLoaded ((Application.loadedLevel+1))){
		GUI.Label(Rect(Screen.width/4+200,Screen.height/2-25,285,150), "Loaded, starting the game!");
		Application.LoadLevel( (Application.loadedLevel+1) );
	}else{
		GUI.Label(Rect(Screen.width/4+200,Screen.height/2-25,285,150), "Starting..Loading the game: "+Mathf.Floor(Application.GetStreamProgressForLevel((Application.loadedLevel+1))*100)+" %");
	}	
}

@RPC
function setOther(){
	PlayerPrefs.SetInt("PlayerNo", 1);
}
