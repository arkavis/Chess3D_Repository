using UnityEngine;
using System.Collections;

public class MouseEvent : MonoBehaviour {
	
	private Vector3 currentClickedObjectPostion;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject clickedGmObj = null;
	    if(Input.GetMouseButtonDown(0)){
	       clickedGmObj = GetClickedGameObject();
	       if(clickedGmObj != null){
	       		Debug.Log(clickedGmObj.name);
				//Destroy(clickedGmObj);
				
				//Debug.Log("PositionX "+clickedGmObj.transform.position.x+" PositionY "+clickedGmObj.transform.position.y+" PositionZ "+clickedGmObj.transform.position.z);
				currentClickedObjectPostion = clickedGmObj.transform.position;
				//Debug.Log("X "+currentClickedObjectPostion.x + " Y " + currentClickedObjectPostion.y +" Z "+currentClickedObjectPostion.z);
			}
	    }
	}
	
	GameObject GetClickedGameObject()
	{
		
		/*
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		Physics.Raycast(ray, out hit);
		Debug.Log("This hit at " + hit.point );
		*/
		
   	 	// Builds a ray from camera point of view to the mouse position
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    RaycastHit hit;
	    // Casts the ray and get the first game object hit
	    if (Physics.Raycast(ray, out hit))
	       return hit.transform.gameObject;
	    else
	       return null;
	}
	
	public Vector3 getCurrentClickedObjectPostion(){
		return currentClickedObjectPostion;
	}
}
