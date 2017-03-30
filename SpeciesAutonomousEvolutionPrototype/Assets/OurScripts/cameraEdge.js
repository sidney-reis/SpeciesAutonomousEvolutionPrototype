#pragma strict
 
public var Boundary : int = 15;
public var speed : int = 20;
 
private var theScreenWidth : int;
private var theScreenHeight : int;
 
function Start() 
{
    theScreenWidth = Screen.width;
    theScreenHeight = Screen.height;
}
 
function Update() 
{
    if ((Input.mousePosition.x > theScreenWidth - Boundary) && Input.mousePosition.x < theScreenWidth)
    {
        transform.position.x += (10 * (speed * Time.deltaTime));
    }
     
    if ((Input.mousePosition.x < 0 + Boundary) && Input.mousePosition.x > 0)
    {
        transform.position.x -= (10 * (speed * Time.deltaTime));
    }
     
    if ((Input.mousePosition.y > theScreenHeight - Boundary) && Input.mousePosition.y < theScreenHeight)
    {
        transform.position.z += (10 * (speed * Time.deltaTime));
    }
     
    if ((Input.mousePosition.y < 0 + Boundary) && Input.mousePosition.y > 0)
    {
        transform.position.z -= (10 * (speed * Time.deltaTime));
    }
     
}    
 
function OnGUI() 
{
    
}