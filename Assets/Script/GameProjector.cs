using UnityEngine;
using System.Collections;

public class GameProjector : MonoBehaviour {
	public string gameName = "Chess";
	
	public GameObject [] PlayerObject;
	
	public GameObject PlayerPrefab;
	public GameObject BoxPrefab;
	public GameObject MoveableHintPrefab;
	public int column;
	public int row;
	public float gab;
	
	private GameObject currentPlayerActive;
	private string currentPlayerActiveName;
	private bool isCurrentPlayerActive;

	private float minX,minZ,maxX,maxZ;
	
	private int currentTempPlayer;
	private int currentPlayer; //0,1
	public bool isCurrentMyTurn; // is player turn
	private int IAmPlayer; // which Player is 
	
	void Awake(){
		//RE-enable the network messages now we've loaded the right level
		Network.isMessageQueueRunning = true;
	}
	
	// Use this for initialization
	void Start () {

		currentPlayerActive = null;
		currentPlayerActiveName = "";
		isCurrentPlayerActive = false;
		
		createBox();
		
		if(Network.isServer){
			IAmPlayer = 0;	
		}
		else{
			Debug.Log("I am client");
			IAmPlayer = 1;	
		}
		
		Debug.Log("I am Player "+IAmPlayer);

		setCurrentPlayer(0);
		checkCurrentPlayerTurn(currentPlayer);
	}

	// Update is called once per frame
	void Update () {
		if(isCurrentMyTurn){
			if(isCurrentPlayerActive){
				mouseSelectedMovePoint();
			}
			else{
				mouseSelectedPlayer();
			}
		}
	}
	
	// creat box
	void createBox(){
		for(int i = 0; i < row; i++){
			for(int j = 0; j < column; j++){
				
				float positionX = (BoxPrefab.transform.localScale.x+gab)*i;
				float positionY = BoxPrefab.transform.localScale.y;
				float positionZ = (BoxPrefab.transform.localScale.z+gab)*j;
				
				if(i==0&&j==0){
					minX = positionX;
					minZ = positionZ;
				}
				if(i==row-1&&j==column-1){
					maxX = positionX;
					maxZ = positionZ;
				}
				
				//create BOX
				Vector3 position = new Vector3(positionX,positionY,positionZ);
				GameObject Box = Instantiate(BoxPrefab,position,BoxPrefab.transform.rotation) as GameObject;
				Box.transform.parent = GameObject.Find("BoxObjects").transform;
				Box.name = "BOX"+i+j;
				Box box = Box.gameObject.GetComponent("Box") as Box;
				box.setMaterial((i+j)%2);

				// create Object currentPlayer = 1
				if(i<2){
					Vector3 positionPlayer = new Vector3(positionX,BoxPrefab.transform.localScale.y+PlayerObject[0].transform.localScale.y+PlayerObject[0].transform.localScale.y/2,positionZ);

					if(i==0&&j==0||i==0&&j==7){ 
						GameObject Player = Instantiate(PlayerObject[4],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						Player.name = "Rook"+i+j; 
						player.setMaterial("White"); 
						player.setClassName("Rook");
						player.setCurrentPlayer(0); // player 0
					}
					else if(i==0&&j==1||i==0&&j==6){ 
						GameObject Player = Instantiate(PlayerObject[3],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(0); // player 0
						Player.name = "Knight"+i+j; 
						player.setMaterial("White"); 
						player.setClassName("Knight");
					}
					else if(i==0&&j==2||i==0&&j==5){ 
						GameObject Player = Instantiate(PlayerObject[2],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(0); // player 0
						Player.name = "Bishop"+i+j; 
						player.setMaterial("White"); 
						player.setClassName("Bishop");
					}
					else if(i==0&&j==3){ 
						GameObject Player = Instantiate(PlayerObject[1],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(0); // player 0
						Player.name = "Queen"+i+j; 
						player.setMaterial("White"); 
						player.setClassName("Queen");
					}
					else if(i==0&&j==4){ 
						GameObject Player = Instantiate(PlayerObject[0],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(0); // player 0
						Player.name = "King"+i+j; 
						player.setMaterial("White");  
						player.setClassName("King");
					}
					else if(i==1){ 
						GameObject Player = Instantiate(PlayerObject[5],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(0); // player 0
						Player.name = "Pawn"+i+j; 
						player.setMaterial("White"); 
						player.setClassName("Pawn");
						player.setPawnOnFirstRow(true);
					}			
				}
				
				// create Object currentPlayer = 2
				if(i>5){
					Vector3 positionPlayer = new Vector3(positionX,BoxPrefab.transform.localScale.y+PlayerObject[0].transform.localScale.y+PlayerObject[0].transform.localScale.y/2,positionZ);
					
					if(i==7&&j==7||i==7&&j==0){ 
						GameObject Player = Instantiate(PlayerObject[4],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(1); // player 1
						Player.name = "Rook"+i+j; 
						player.setMaterial("Black"); 
						player.setClassName("Rook");	
					}
					else if(i==7&&j==1||i==7&&j==6){ 
						GameObject Player = Instantiate(PlayerObject[3],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(1); // player 1
						Player.name = "Knight"+i+j; 
						player.setMaterial("Black"); 
						player.setClassName("Knight");
					}
					else if(i==7&&j==2||i==7&&j==5){ 
						GameObject Player = Instantiate(PlayerObject[2],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(1); // player 1
						Player.name = "Bishop"+i+j; 
						player.setMaterial("Black"); 
						player.setClassName("Bishop");
					}
					else if(i==7&&j==3){ 
						GameObject Player = Instantiate(PlayerObject[1],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(1); // player 1
						Player.name = "Queen"+i+j; 
						player.setMaterial("Black"); 
						player.setClassName("Queen");
					}
					else if(i==7&&j==4){ 
						GameObject Player = Instantiate(PlayerObject[0],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(1); // player 1
						Player.name = "King"+i+j; 
						player.setMaterial("Black");  
						player.setClassName("King");
					}
					else if(i==6){ 
						GameObject Player = Instantiate(PlayerObject[5],positionPlayer,PlayerObject[4].transform.rotation) as GameObject;
						Player.transform.parent = GameObject.Find("Players").transform;
						Player player = Player.gameObject.GetComponent("Player") as Player;
						player.setCurrentPlayer(1); // player 1
						Player.name = "Pawn"+i+j; 
						player.setMaterial("Black"); 
						player.setClassName("Pawn");
						player.setPawnOnFirstRow(true);
					}
				}
			}
		}
	}

	// active selected Player
	void mouseSelectedPlayer(){
		bool isPlayer = false;
		GameObject clickedGmObj = null;
	    if(Input.GetMouseButtonDown(0)){
	     	clickedGmObj = SDSAction.GetClickedGameObject();
			if(clickedGmObj != null){
				
				foreach(Transform child in GameObject.Find("Players").transform){
					if(!isPlayer){
						if(child.transform.name==clickedGmObj.transform.name) {
							Player player = clickedGmObj.gameObject.GetComponent("Player") as Player;
							int temp = player.getCurrentPlayer();
							if(currentPlayer == temp){
								isPlayer = true;
							}
						}	
					}
				}
				
				if(isPlayer){
					Player player = clickedGmObj.gameObject.GetComponent("Player") as Player;	
					if(currentPlayer == player.getCurrentPlayer()){
						Vector3 newPosition = new Vector3(clickedGmObj.transform.position.x,clickedGmObj.transform.position.y+PlayerPrefab.transform.localScale.y,clickedGmObj.transform.position.z);
						//move
						networkView.RPC ("moveChess", RPCMode.AllBuffered,clickedGmObj.name,newPosition);
						// status
						networkView.RPC ("createCurrentStatus", RPCMode.AllBuffered,clickedGmObj.name);
						// create Hint
						networkView.RPC ("createHint", RPCMode.AllBuffered,clickedGmObj.name);
					}		
					Debug.Log("Player "+currentPlayer+" is now "+isPlayer);
				}
			}
			else{
				Debug.Log("Invilid");
			}
	    }
	}

	// select move point
	void mouseSelectedMovePoint(){
		bool isHint = false;
		GameObject clickedGmObj = null;
	    if(Input.GetMouseButtonDown(0)){
			
	      	clickedGmObj = SDSAction.GetClickedGameObject();
			
			if(clickedGmObj != null){
				// no click in other Players
				if(clickedGmObj.name==currentPlayerActiveName){
					Vector3 newPosition = new Vector3(clickedGmObj.transform.position.x,clickedGmObj.transform.position.y-clickedGmObj.transform.localScale.y,clickedGmObj.transform.position.z);
					
					networkView.RPC ("moveChess", RPCMode.AllBuffered,currentPlayerActive.name,newPosition);
					//remove Hint
					networkView.RPC ("removeHint", RPCMode.AllBuffered,currentPlayerActiveName,true);
					//set back properties
					networkView.RPC ("setBackCurrentStatus", RPCMode.AllBuffered);
				}
				else{
					// create Active On Hint
					foreach(Transform child in GameObject.Find("Moveable Hint").transform){
						if(!isHint){
							if(clickedGmObj.transform.name==child.transform.name) {
								isHint = true;
							}
						}
					}
					if(isHint){
						if(currentPlayerActive.name != clickedGmObj.name){
							
							Vector3 newPosition = new Vector3(clickedGmObj.transform.position.x,currentPlayerActive.transform.position.y-currentPlayerActive.transform.localScale.y,clickedGmObj.transform.position.z);
							// move
							networkView.RPC ("moveChess", RPCMode.AllBuffered,currentPlayerActive.name,newPosition);
							// change Turn
							networkView.RPC ("setCurrentPlayer", RPCMode.AllBuffered, (currentPlayer+1)%2);
							networkView.RPC ("checkCurrentPlayerTurn", RPCMode.AllBuffered, currentPlayer);
							//remove Hint
							networkView.RPC ("removeHint", RPCMode.AllBuffered,currentPlayerActiveName,false);
							//set back properties
							networkView.RPC ("setBackCurrentStatus", RPCMode.AllBuffered);
						}
					}
				}
			}
			else{
				Debug.Log("Invilid");
			}
	    }
	}	
	
	// OnGUI
	void OnGUI ()
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
				GUILayout.Label("Connections: "+Network.connections.Length);
				if(Network.connections.Length>=1){
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
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("This CLIENT has disconnected from a server");
	}
	
	//Server functions called by Unity
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player connected from: " + player.ipAddress +":" + player.port);
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Player disconnected from: " + player.ipAddress+":" + player.port);
	
	}
	
	[RPC]
	void moveChess(string player, Vector3 newPos){
		GameObject obj = GameObject.Find(player);
		SDSAction.moveTo(obj,obj.transform.position,newPos,0.0f);
	}
	
	[RPC]
	void pickChess(string player, Vector3 newPos){
		GameObject obj = GameObject.Find(player);
		SDSAction.moveTo(obj,obj.transform.position,newPos,0.0f);
	}
	
	[RPC]	
	void checkCurrentPlayerTurn(int current){
		if(current==IAmPlayer){
			isCurrentMyTurn = true;
		}
		else{
			isCurrentMyTurn = false;
		}
	}
	
	[RPC]
	void setCurrentPlayer(int player){
		currentPlayer = player;
		Debug.Log("Player "+currentPlayer);
	}
	
	[RPC]
	void setIAmPlayer(int i){
		IAmPlayer = i;
	}
	
	[RPC]
	void setBackCurrentStatus(){
		currentPlayerActive = null;
		currentPlayerActiveName = "";
		isCurrentPlayerActive = false;
		Debug.Log("Now is Player "+currentPlayer+" turn.");
	}
	
	[RPC]
	void createCurrentStatus(string player){
		GameObject obj = GameObject.Find(player);
		currentPlayerActive = obj;
		currentPlayerActiveName = obj.name;
		isCurrentPlayerActive = true;
	}
	
	[RPC]
	void removeHint(string name,bool self){
		ChessController.removeMoveableHint();
		GameObject clickedGmObj = GameObject.Find(name);
		Player player = clickedGmObj.gameObject.GetComponent("Player") as Player;
		if(!self) player.setPawnOnFirstRow(false);
	}
	
	[RPC]
	void createHint(string name){
		GameObject clickedGmObj = GameObject.Find(name);
		Player player = clickedGmObj.gameObject.GetComponent("Player") as Player;
		bool temp = player.getPawnOnFirstRow();
		ChessController.creatMoveableHint(currentPlayer,currentPlayerActive,MoveableHintPrefab,minX,minZ,maxX,maxZ,temp);	
	}
	
	
}
