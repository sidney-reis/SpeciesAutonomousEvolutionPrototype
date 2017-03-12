#pragma strict
 
public var Boundary : int = 25;
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
    if (Input.mousePosition.x > theScreenWidth - Boundary)
    {
        transform.position.x += (10 * (speed * Time.deltaTime));
    }
     
    if (Input.mousePosition.x < 0 + Boundary)
    {
        transform.position.x -= (10 * (speed * Time.deltaTime));
    }
     
    if (Input.mousePosition.y > theScreenHeight - Boundary)
    {
        transform.position.y += (10 * (speed * Time.deltaTime));
    }
     
    if (Input.mousePosition.y < 0 + Boundary)
    {
        transform.position.y -= (10 * (speed * Time.deltaTime));
    }
     
}    
 
function OnGUI() 
{
    
}