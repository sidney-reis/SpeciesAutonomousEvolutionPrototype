using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
    private Animator anim;
    public Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector3(0, -10, 0);
        int character= int.Parse(gameObject.name);
        
        if(character == SpeciesSellector.selectedCharacter)
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                // position.x = (float)(position.x + 0.1);
                position.z = (float)(position.z + 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                Debug.Log(PlayerInfo.steps);
            }
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.x = (float)(position.x - 0.1);
                // position.z = (float)(position.z + 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                Debug.Log(PlayerInfo.steps);
            }
            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                // position.x = (float)(position.x - 0.1);
                position.z = (float)(position.z - 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                Debug.Log(PlayerInfo.steps);
            }
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.x = (float)(position.x + 0.1);
                // position.z = (float)(position.z - 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                Debug.Log(PlayerInfo.steps);
            }

            if ((!Input.GetKey(KeyCode.W)) & (!Input.GetKey(KeyCode.A)) & (!Input.GetKey(KeyCode.S)) & (!Input.GetKey(KeyCode.D)))
            {
                anim.SetBool("walking", false);
            }
        }
    }
}
