using UnityEngine;
using System.Collections;

public class SpeciesSellector : MonoBehaviour {
    //public static Camera mainCam;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                //Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "ControllableSpecies")
                {
                    Debug.Log("Selecting Character");
                    PlayerInfo.selectedCreature = int.Parse(hitInfo.transform.gameObject.name);
                    //mainCam.transform.position = new Vector3(transform.position.x, transform.position.y, mainCam.transform.position.z);
                }
                else
                {
                    //Debug.Log("nopz");
                }
            }
            else
            {
                //Debug.Log("No hit");
            }
            //Debug.Log("Mouse is down");
        }
    }
}
