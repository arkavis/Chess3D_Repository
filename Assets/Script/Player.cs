using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Material[] playerMaterial;
	private string className;
	private int currentPlayer;
	
	private bool isPawnOnFirstRow;
	
	Vector3 currentPosition;
	Vector3 newPosition;
	
	private bool isMovement;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isMovement){
			float amtToMove = 7*Time.deltaTime;
			
			if(transform.position.z < newPosition.z){
				transform.Translate(Vector3.forward*amtToMove,Space.Self);
				
				if(transform.position.z > newPosition.z){
					
					if(transform.position.y > newPosition.y){
						transform.Translate(Vector3.down*amtToMove,Space.Self);
						if(transform.position.y < newPosition.y) {
							transform.position = newPosition;
							isMovement = false;
						}
					}
				}
			}
			else if(transform.position.z > newPosition.z){
				transform.Translate(Vector3.back*amtToMove,Space.Self);
				
				if(transform.position.z < newPosition.z){
					
					if(transform.position.y > newPosition.y){
						transform.Translate(Vector3.down*amtToMove,Space.Self);
						if(transform.position.y < newPosition.y) {
							transform.position = newPosition;
							isMovement = false;
						}
					}
					
				}
			}
			
			
			if(transform.position.x < newPosition.x){
				transform.Translate(Vector3.right*amtToMove,Space.Self);
				
				if(transform.position.x > newPosition.x){
					
					if(transform.position.y > newPosition.y){
						transform.Translate(Vector3.down*amtToMove,Space.Self);
						if(transform.position.y < newPosition.y) {
							transform.position = newPosition;
							isMovement = false;
						}
					}
					
				}
			}
			else if(transform.position.x > newPosition.x){
				transform.Translate(Vector3.left*amtToMove,Space.Self);
				
				if(transform.position.x < newPosition.x){
					
					if(transform.position.y > newPosition.y){
						transform.Translate(Vector3.down*amtToMove,Space.Self);
						if(transform.position.y < newPosition.y) {
							transform.position = newPosition;
							isMovement = false;
						}
					}
					
				}
			}
			
			
			if(transform.position.x == newPosition.x) {
				if(transform.position.y < newPosition.y){
					transform.Translate(Vector3.up*amtToMove,Space.Self);
					if(transform.position.y > newPosition.y){
						isMovement = false;
						transform.position = newPosition;
					}
				}
				if(transform.position.y > newPosition.y){
					transform.Translate(Vector3.down*amtToMove,Space.Self);
					if(transform.position.y < newPosition.y) {
						isMovement = false;
						transform.position = newPosition;
					}
				}
			}
		}
	}
	// set material
	public void setMaterial(string name){
		switch(name){
			case "King":
				renderer.material = playerMaterial[0];
				break;
			case "Queen":
				renderer.material = playerMaterial[1];
				break;
			case "Rook":
				renderer.material = playerMaterial[2];
				break;
			case "Bishop":
				renderer.material = playerMaterial[3];
				break;
			case "Knight":
				renderer.material = playerMaterial[4];
				break;
			case "Pawn":
				renderer.material = playerMaterial[5];
				break;
			case "White":
				foreach(Transform child in transform){
					child.renderer.material = playerMaterial[0];
				}
				break;
			case "Black":
				foreach(Transform child in transform){
					child.renderer.material = playerMaterial[1];
				}
				break;
		}
	}
	
	public void setClassName(string name){
		className = name;	
	}
	public string getClassName(){
		return className;	
	}
	
	public void setCurrentPlayer(int number){
		currentPlayer = number;	
	}
	public int getCurrentPlayer(){
		return currentPlayer;	
	}
	
	public void setPawnOnFirstRow(bool boo){
		isPawnOnFirstRow = boo;	
	}
	
	public bool getPawnOnFirstRow(){
		return isPawnOnFirstRow;	
	}
	
	public void setChildRotation(Quaternion rotate){
		foreach(Transform child in transform){
					child.rotation = rotate;
				}
	}
	
	public void setMovement(Vector3 currentPos,Vector3 newPos,float duration){
		currentPosition = currentPos;
		newPosition = newPos;
		
		isMovement = true;
	}
}
