using UnityEngine;
using System.Collections;

public class Hint : MonoBehaviour {
	private bool isActive;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setActive(bool active){
		isActive = active;
	}
	
	public bool getActive(){
		return isActive;
	}
}
