#pragma strict
var lookAt : GameObject;

function Start () {

}

function Update () {
	transform.LookAt(lookAt.transform.position);
}