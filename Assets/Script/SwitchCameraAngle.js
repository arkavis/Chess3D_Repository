#pragma strict
var lookAt : GameObject;
var points : GameObject[];
var pointIndex : int = 0;
var speed : int;

private var isMoving = false;

function Start () {

}

function Update () {
	transform.LookAt(lookAt.transform.position);
	if (Input.GetKeyDown ("c"))
    { 
        ToggleCameraPoint();
    }
    	transform.position = Vector3.Lerp(transform.position, points[pointIndex].transform.position, Time.deltaTime * speed);
    	if(transform.position.x == points[pointIndex].transform.position.y){
    }
}

function ToggleCameraPoint(){
	pointIndex = (pointIndex + 1) % points.length;
	isMoving = true;
}