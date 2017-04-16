#pragma strict
 
public var Boundary : int = 1;
public var speed : int = 160;
 
private var theScreenWidth : int;
private var theScreenHeight : int;
 
function Start() 
{
    theScreenWidth = Screen.width;
    theScreenHeight = Screen.height;
    Cursor.lockState = CursorLockMode.Confined;
}
 
function Update() 
{
    var speed = 50;
    var Boundary = 15;

    if ((Input.mousePosition.x > theScreenWidth - Boundary) && Input.mousePosition.x < theScreenWidth)
    {
        transform.position.x += speed * Time.deltaTime;
    }
     
    if ((Input.mousePosition.x < 0 + Boundary) && Input.mousePosition.x >= 0)
    {
        transform.position.x -= speed * Time.deltaTime;
    }
     
    if ((Input.mousePosition.y > theScreenHeight - Boundary) && Input.mousePosition.y < theScreenHeight)
    {
        transform.position.z += speed * Time.deltaTime;
    }
     
    if ((Input.mousePosition.y < 0 + Boundary) && Input.mousePosition.y >= 0)
    {
        transform.position.z -= speed * Time.deltaTime;
    }
     
}    
 
function OnGUI() 
{
    
}