using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {
	
	
	public Material[] material;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void setMaterial(int i){
		renderer.material = material[i];
	}
	
}
