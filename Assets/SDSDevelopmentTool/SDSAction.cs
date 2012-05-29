using UnityEngine;
using System.Collections;

public class SDSAction : MonoBehaviour {
	
	public static bool isDraging;

	
	public static void moveTo(GameObject obj,Vector3 currentPos,Vector3 newPos,float duration){
		Player player = obj.gameObject.GetComponent("Player") as Player;
		player.setMovement(currentPos,newPos,duration);
	}
	
	// get click GameObject
	public static GameObject GetClickedGameObject()
	{
   	 	// Builds a ray from camera point of view to the mouse position
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    RaycastHit hit;
	    // Casts the ray and get the first game object hit
	    if (Physics.Raycast(ray, out hit))
	       return hit.transform.gameObject;
	    else
	       return null;
	}
	
}
