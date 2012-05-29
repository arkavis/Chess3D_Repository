using UnityEngine;
using System.Collections;

public class SetMaterial : MonoBehaviour {
	
	public Material[] myMaterial;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setmaterial(string name){
		switch(name){
			case "White":
				renderer.material = myMaterial[0];
				break;
			case "Black":
				renderer.material = myMaterial[1];
				break;
		}
	}
}
