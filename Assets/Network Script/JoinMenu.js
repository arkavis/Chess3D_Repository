/* 
*  This file is part of the Unity networking tutorial by M2H (http://www.M2H.nl)
*  The original author of this code is Mike Hergaarden, even though some small parts 
*  are copied from the Unity tutorials/manuals.
*  Feel free to use this code for your own projects, drop us a line if you made something exciting! 
*/
#pragma strict
#pragma implicit
#pragma downcast

var skin : GUISkin;

var gameName = "Example3_Lobby";
var serverPort = 45671;

private var timeoutHostList = 0.0;
private var lastHostListRequest = -1000.0;
private var hostListRefreshTimeout = 60.0;

private var windowRect1;
private var windowRect2;

static var quickplayMode : boolean = false;
static var quickplayModeStarted  : float = 0.0;
private var tryingToConnectquickplay : boolean = false;
private var tryingToConnectquickplayNumber : int = 0;
private var readyToConnect : boolean = false;

private var remotePort : int[] = new int[3];
private var remoteIP : String[] = new String[3];
private var connectionInfo : String = "";

private var lastMSConnectionAttemptForcedNat : boolean= false;
private var NAToptionWasSwitchedForTesting : boolean = false;
private var officialNATstatus : boolean = true;
private var errorMessage : String = "";
private var lastquickplayConnectionTime : float;


var customButton : GUIStyle;

private var showMenu : boolean = false;

private var mainMenuScript : MainMenu;

/////////////////////////////

function Awake(){
	windowRect1 = Rect (Screen.width/2-305,Screen.height/2-140,380,280);
  	windowRect2 = Rect (Screen.width/2+110,Screen.height/2-140,220,100);
}

function Start(){
	mainMenuScript =  MainMenu.SP;
}


function EnableMenu(quickplay : boolean){
	showMenu=true;
	
	tryingToConnectquickplayModeNumber=0;
	tryingToConnectquickplay=false;
	
	quickplayMode=quickplay;
	
	if(quickplay){		
		quickplayModeStarted=Time.time;
	}
		
	MasterServer.RequestHostList (gameName);
	lastHostListRequest = Time.realtimeSinceStartup;
	
	remoteIP[0]="127.0.0.1";
	remotePort[0]=serverPort;
}


function OnFailedToConnectToMasterServer(info: NetworkConnectionError)
{
	//Dont request the MS list the next 60 sec
	lastHostListRequest = Time.realtimeSinceStartup + 60;
}

function OnFailedToConnect(info: NetworkConnectionError)
{
	Debug.Log("Failed To Connect info: "+info);
	
	var invalidPass : boolean = false;
	if(info==NetworkConnectionError.InvalidPassword){
		invalidPass=true;
	}
	FailedConnRetry(invalidPass);	
}

function FailedConnRetry(invalidPassword : boolean){
	if(tryingToConnectquickplay){
		//Try again without NAT if we used NAT
		if(lastMSConnectionAttemptForcedNat){
			Debug.Log("Failed 1A");
		
			lastMSConnectionAttemptForcedNat=false;

			remotePort[0]=serverPort;//Fall back to default server port
			Network.Connect(remoteIP, remotePort[0]);
			lastquickplayConnectionTime=Time.time;
		}else{
			Debug.Log("Failed 1B");
		

						
			tryingToConnectquickplayNumber++;
			tryingToConnectquickplay=false;
		}
	}else{
		
		//Direct connect or host list
		connectionInfo="Failed to connect!";
		
		if( lastMSConnectionAttemptForcedNat){
			Debug.Log("Failed 2A");
		
			Network.Connect(remoteIP, remotePort[0]);
			lastquickplayConnectionTime=Time.time;
		}else{
			Debug.Log("Failed 2B");
		
			errorMessage="Failed to connect to "+remoteIP[0]+":"+remotePort[0];
			if(invalidPassword){
				errorMessage+="\nYou used the wrong password (or you didn't need to enter one!).";
			}
		//reset default port
			remotePort[0]=serverPort;
			
	
			
		}
	}	
}


function OnConnectedToServer(){
	Debug.Log("Connected to lobby!");
	showMenu=false;
	var gameLobbyScript : GameLobby = GetComponent(GameLobby);
	gameLobbyScript.EnableLobby();
	
}

function OnGUI ()
{		
	if(!showMenu){
		return;
	}


	//Back to main menu
	if(GUI.Button(Rect(40,10,150,20), "Back to main menu")){
		showMenu=false;
		mainMenuScript.OpenMenu("multiplayer");
	}

	
	if(errorMessage && errorMessage!=""){	
		GUI.Box(Rect(Screen.width/2-100,Screen.height/2-30,200,60), "Error");
		GUI.Label(Rect(Screen.width/2-90,Screen.height/2-15,180,50), errorMessage);
		if(GUI.Button(Rect(Screen.width/2+40,Screen.height/2+5,50,20), "Close")){
			errorMessage="";
		}
	}
	
	if(quickplayMode){
		quickplayFunction();
	}else{
		if(!errorMessage || errorMessage==""){ //Hide windows on error
			windowRect1 = GUILayout.Window (0, windowRect1, listGUI, "Join a game via the list");	
			windowRect2 = GUILayout.Window (1, windowRect2, directConnectGUIWindow, "Directly join a game via an IP");	
			//windowRect3 = GUILayout.Window (2, windowRect3, hostGUI, "Host a game");
		}	
	}
		
	
}


function quickplayFunction(){
	
		GUI.Box(Rect(Screen.width/4+180,Screen.height/2-30,280,50), "");
		
		var i : int=0;
		var data : HostData[] = MasterServer.PollHostList();
		for (var element in data)
		{
			// Do not display NAT enabled games if we cannot do NAT punchthrough
			if ( !(natTester.filterNATHosts && element.useNat) && !element.passwordProtected )
			{
				aHost=1;
								
				if(element.connectedPlayers<element.playerLimit)
				{					
					if(tryingToConnectquickplay){
						var natText;
					
						GUI.Label(Rect(Screen.width/4+200,Screen.height/2-25,285,50), "Trying to connect to host "+(tryingToConnectquickplayNumber+1)+"/"+data.length+" "+natText);
						if( lastquickplayConnectionTime+0.75<=Time.time || lastquickplayConnectionTime+5.00<=Time.time){
							FailedConnRetry(false);							
						}
						return;	
					}		
					if(!tryingToConnectquickplay && tryingToConnectquickplayNumber<=i){
						Debug.Log("Trying to connect to game nr "+i+" & "+tryingToConnectquickplayNumber);
						tryingToConnectquickplay=true;
						tryingToConnectquickplayNumber=i;
						// Enable NAT functionality based on what the hosts if configured to do
						lastMSConnectionAttemptForcedNat=element.useNat;
						remoteIP=element.ip;
						remotePort[0]=element.port;
				
						var connectPort : int=element.port;
						
							print("Connecting directly to host");
						
						Debug.Log("connecting to "+element.gameName+" "+element.ip+":"+connectPort);
						Network.Connect(element.ip, connectPort);	
						lastquickplayConnectionTime=Time.time;
					}
					i++;		
				}
			}
			
		}
		
		//List is empty, d ont give up yet: Give MS 5 seconds to feed the list
		if(Time.time<quickplayModeStarted+5){
			//Debug.Log("PlayNow: delay up to 3 sec (no hosts yet)" );		
			GUI.Label(Rect(Screen.width/4+200,Screen.height/2-25,280,50), "Waiting for masterserver..."+Mathf.Ceil((quickplayModeStarted+5)-Time.time) );
			return;	
		}
		
		if(!tryingToConnectquickplay){
			Debug.Log("PlayNow: No games hosted, so hosting one ourselves");			
				showMenu=false;
				var gameLobbyScript : GameLobby = GetComponent(GameLobby);
				gameLobbyScript.EnableLobby();	
		}
		
	
}

var password : String = "";
function directConnectGUIWindow(id : int){

	GUILayout.BeginVertical();
	GUILayout.Space(5);
	GUILayout.EndVertical();
	GUILayout.Label(connectionInfo);
		
	
	GUILayout.BeginHorizontal();
	GUILayout.Space(10);
		remoteIP[0] = GUILayout.TextField(remoteIP[0], GUILayout.MinWidth(70));
		remotePort[0] = parseInt(GUILayout.TextField(remotePort[0]+""));
	GUILayout.Space(10);
	GUILayout.EndHorizontal();

	GUILayout.BeginHorizontal();
	GUILayout.Space(10);
	GUILayout.Label("Password");	
	password = GUILayout.TextField(password, GUILayout.MinWidth(50));
	GUILayout.Space(10);
	GUILayout.EndHorizontal();
	

	
	
	GUILayout.BeginHorizontal();
	GUILayout.Space(10);
	GUILayout.FlexibleSpace();
	if (GUILayout.Button ("Connect"))
	{
	
		connectionInfo="";
		Network.Connect(remoteIP, remotePort[0], password);
	}		
	GUILayout.FlexibleSpace();
	GUILayout.EndHorizontal();
	
}

var scrollPosition : Vector2;

function listGUI (id : int) {
	
		GUILayout.BeginVertical();
		GUILayout.Space(6);
		GUILayout.EndVertical();
	
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(200);

		// Refresh hosts
		if (GUILayout.Button ("Refresh available Servers") || Time.realtimeSinceStartup > lastHostListRequest + hostListRefreshTimeout)
		{
			if(Time.realtimeSinceStartup > lastHostListRequest + 5){//max once per 5 sec
				MasterServer.RequestHostList (gameName);
				lastHostListRequest = Time.realtimeSinceStartup;
			}
		}
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();

		
		
		scrollPosition = GUILayout.BeginScrollView (scrollPosition);

		var aHost =0;
		
		var data : HostData[] = MasterServer.PollHostList();
		for (var element in data)
		{
			
			GUILayout.BeginHorizontal();

			// Do not display NAT enabled games if we cannot do NAT punchthrough
			if ( !(natTester.filterNATHosts && element.useNat) )
			{
				aHost=1;
				var name = element.gameName + " ";
				
				GUILayout.Label(name);	
				GUILayout.FlexibleSpace();
				GUILayout.Label(element.connectedPlayers + "/" + element.playerLimit);
				
				if(element.useNat){
					GUILayout.Label(".");
				}
				GUILayout.FlexibleSpace();
				GUILayout.Label("[" + element.ip[0] + ":" + element.port + "]");	
				GUILayout.FlexibleSpace();
				
				if(element.passwordProtected){
					GUILayout.Label("PASSWORD");
					if (GUILayout.Button("Connect"))
					{
						if(password==""){
							errorMessage="You must enter a password if you want to join this game!";
							return;	
						}// Enable NAT functionality based on what the hosts if configured to do
						
						lastMSConnectionAttemptForcedNat = element.useNat;
						
							print("Connecting directly to host");
						
						remoteIP=element.ip;
						remotePort[0]=element.port;
						var connectPort2 : int=element.port;
						Network.Connect(element.ip, connectPort2, password);			
					}

				}else{
					if (GUILayout.Button("Connect"))
					{
						// Enable NAT functionality based on what the hosts if configured to do
						
						lastMSConnectionAttemptForcedNat = element.useNat;
						
							print("Connecting directly to host");
						
						remoteIP=element.ip;
						remotePort[0]=element.port;
						var connectPort : int=element.port;	
						Network.Connect(element.ip, connectPort);			
					}
				}
				GUILayout.Space(15);
			}
			GUILayout.EndHorizontal();	
		}
		if(aHost==0){
			GUILayout.Label("No games hosted at the moment..");
		}
		
		GUILayout.EndScrollView ();
}
