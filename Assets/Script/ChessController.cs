using UnityEngine;
using System.Collections;

public class ChessController : MonoBehaviour {
		
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void creatMoveableHint(int turn,GameObject obj,GameObject hintObject, float minX, float minZ, float maxX, float maxZ,bool isFirstTurn){
		Player player = obj.gameObject.GetComponent("Player") as Player;
		string temp = player.getClassName();
		
		Vector3 positionPlayer;
		GameObject Hint;
		
		float positionY = obj.transform.position.y-obj.transform.localScale.y-hintObject.transform.localScale.y*2;
		float positionX = obj.transform.position.x;
		float positionZ = obj.transform.position.z;
		
		float width = obj.transform.localScale.x;
		float startPointX = obj.transform.position.x;
		float startPointZ = obj.transform.position.z;
		
		switch(temp){
			case "Pawn": 
				if(isFirstTurn){
					if(turn%2==0){
						for(int i = 0; i < 2; i++){
							positionX = startPointX+width*(i+1);
							positionZ = startPointZ;
						
							if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
								positionPlayer = new Vector3(positionX,positionY,positionZ);
								Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
								Hint.name = "hint";
								Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
							}
						}
					}
					else{
						for(int i = 0; i < 2; i++){
							positionX = startPointX-width*(i+1);
							positionZ = startPointZ;
						
							if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
								positionPlayer = new Vector3(positionX,positionY,positionZ);
								Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
								Hint.name = "hint";
								Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
							}
						}
					}
				}
				else{
					if(turn%2==0){
						positionX = startPointX+width;
						positionZ = startPointZ;
					
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else{
						positionX = startPointX-width;
						positionZ = startPointZ;
					
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
				}
				
				break;
			case "Rook": 
				//x
				for(int i = 1; i < 16; i++){
					if(i<8){
						positionX = startPointX+width*i;
						positionZ = startPointZ;
					
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else if(i>8){
						positionX = startPointX-width*(i%8);
						positionZ = startPointZ;
					
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
				}
				
				// z
				for(int j = 1; j < 16; j++){
					if(j<8){
						positionX = startPointX;
						positionZ = startPointZ+width*j;
					
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else if(j>8){
						positionX = startPointX;
						positionZ = startPointZ-width*(j%8);
						
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
				}
				break;
			case "Knight": 
				for(int i = 0; i < 8;i++){
					if(i==0){
						positionX = startPointX+width*2;
						positionZ = startPointZ-width;
					}
					else if(i==1){
						positionX = startPointX+width;
						positionZ = startPointZ-width*2;
					}
					else if(i==2){
						positionX = startPointX-width;
						positionZ = startPointZ-width*2;
					}
					else if(i==3){
						positionX = startPointX-width*2;
						positionZ = startPointZ-width;
					}
					else if(i==4){
						positionX = startPointX-width*2;
						positionZ = startPointZ+width;
					}
					else if(i==5){
						positionX = startPointX-width;
						positionZ = startPointZ+width*2;
					}
					else if(i==6){
						positionX = startPointX+width;
						positionZ = startPointZ+width*2;
					}
					else if(i==7){
						positionX = startPointX+width*2;
						positionZ = startPointZ+width;
					}
				
					if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
						positionPlayer = new Vector3(positionX,positionY,positionZ);
						Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
						Hint.name = "hint";
						Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
					}
				}
				break;
			case "Bishop": 
				//x
				for(int i = 1; i < 16; i++){
					if(i<8){
						positionX = startPointX+width*i;
						positionZ = startPointZ-width*i;
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else if(i>8){
						positionX = startPointX-width*(i%8);
						positionZ = startPointZ+width*(i%8);
						positionPlayer = new Vector3(positionX,positionY,positionZ);
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					
				}
				// z
				for(int j = 1; j < 16; j++){
					if(j<8){
						positionX = startPointX+width*j;
						positionZ = startPointZ+width*j;
						positionPlayer = new Vector3(positionX,positionY,positionZ);
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else if(j>8){
						positionX = startPointX-width*(j%8);
						positionZ = startPointZ-width*(j%8);
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					
				}
				break;
			case "Queen": 
				//x
				for(int i = 1; i < 16; i++){
					if(i<8){
						positionX = startPointX+width*i;
						positionZ = startPointZ-width*i;
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else if(i>8){
						positionX = startPointX-width*(i%8);
						positionZ = startPointZ+width*(i%8);
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					
				}
				// z
				for(int j = 1; j < 16; j++){
					if(j<8){
						positionX = startPointX+width*j;
						positionZ = startPointZ+width*j;
						
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else if(j>8){
						positionX = startPointX-width*(j%8);
						positionZ = startPointZ-width*(j%8);
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					
				}
				//x
				for(int i = 1; i < 16; i++){
					if(i<8){
						positionX = startPointX+width*i;
						positionZ = startPointZ;
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else if(i>8){
						positionX = startPointX-width*(i%8);
						positionZ = startPointZ;
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
				}
				
				// z
				for(int j = 1; j < 16; j++){
					if(j<8){
						positionX = startPointX;
						positionZ = startPointZ+width*j;
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
					else if(j>8){
						positionX = startPointX;
						positionZ = startPointZ-width*(j%8);
						if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
							positionPlayer = new Vector3(positionX,positionY,positionZ);
							Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
							Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
						}
					}
				}
			
				break;
			case "King": 
				for(int i = 0; i < 8;i++){
					if(i==0){
						positionX = startPointX+width;
						positionZ = startPointZ-width;
					}
					else if(i==1){
						positionX = startPointX+width;
						positionZ = startPointZ;
					}
					else if(i==2){
						positionX = startPointX+width;
						positionZ = startPointZ+width;
					}
					else if(i==3){
						positionX = startPointX-width;
						positionZ = startPointZ-width;
					}
					else if(i==4){
						positionX = startPointX-width;
						positionZ = startPointZ;
					}
					else if(i==5){
						positionX = startPointX-width;
						positionZ = startPointZ+width;
					}
					else if(i==6){
						positionX = startPointX;
						positionZ = startPointZ+width;
					}
					else if(i==7){
						positionX = startPointX;
						positionZ = startPointZ-width;
					}
				
					if(positionX >= minX && positionX <= maxX && positionZ >= minZ && positionZ <= maxZ){
						positionPlayer = new Vector3(positionX,positionY,positionZ);
						Hint = Instantiate(hintObject,positionPlayer,hintObject.transform.rotation) as GameObject;
						Hint.name = "hint";
							Hint.transform.parent = GameObject.Find("Moveable Hint").transform;
					}
				}

				break;
		}
	}
	
	public static void removeMoveableHint(){
		foreach(Transform child in GameObject.Find("Moveable Hint").transform){
			if(child!=null){
				Destroy(child.gameObject);
			}
		}
	}
	
}
