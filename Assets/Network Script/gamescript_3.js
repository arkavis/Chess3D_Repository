/* 
*  This file is part of the Unity networking tutorial by M2H (http://www.M2H.nl)
*  The original author of this code is Mike Hergaarden, even though some small parts 
*  are copied from the Unity tutorials/manuals.
*  Feel free to use this code for your own projects, drop us a line if you made something exciting! 
*/
#pragma strict
#pragma implicit
#pragma downcast

public var gameName = "Example1";

function Awake(){
	//RE-enable the network messages now we've loaded the right level
	Network.isMessageQueueRunning = true;
}


function OnGUI ()
{

	if (Network.peerType == NetworkPeerType.Disconnected){
	//We are currently disconnected: Not a client or host
		GUILayout.Label("Connection status: We've (been) disconnected");
		if(GUILayout.Button("Back to main menu")){
			Application.LoadLevel((Application.loadedLevel-1));
		}
		
	}else{
		//We've got a connection(s)!
		

		if (Network.peerType == NetworkPeerType.Connecting){
		
			GUILayout.Label("Connection status: Connecting");
			
		} else if (Network.peerType == NetworkPeerType.Client){
			
			GUILayout.Label("Connection status: Client!");
			GUILayout.Label("Ping to server: "+Network.GetAveragePing(  Network.connections[0] ) );		
			
		} else if (Network.peerType == NetworkPeerType.Server){
			
			GUILayout.Label("Connection status: Server!");
			GUILayout.Label("Connections: "+Network.connections.length);
			if(Network.connections.length>=1){
				GUILayout.Label("Ping to first player: "+Network.GetAveragePing(  Network.connections[0] ) );
			}			
		}

		if (GUILayout.Button ("Disconnect"))
		{
			Network.Disconnect(200);
		}
	}
	

}

//CLient function
function OnDisconnectedFromServer(info : NetworkDisconnection) {
	Debug.Log("This CLIENT has disconnected from a server");
}


//Server functions called by Unity
function OnPlayerConnected(player: NetworkPlayer) {
	Debug.Log("Player connected from: " + player.ipAddress +":" + player.port);
}

function OnPlayerDisconnected(player: NetworkPlayer) {
	Debug.Log("Player disconnected from: " + player.ipAddress+":" + player.port);

}



