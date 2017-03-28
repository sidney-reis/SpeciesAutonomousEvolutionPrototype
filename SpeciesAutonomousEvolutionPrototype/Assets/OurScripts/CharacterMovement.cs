﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {
    private Animator anim;
    public Rigidbody rb;
    SpeciesAttributes attributes;
    Text fatigueText;

    void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetInteger("selectedSpecies", PlayerInfo.selectedSpecies);
        attributes = GetComponent<SpeciesAttributes>();
        fatigueText = GameObject.Find("FatigueText").GetComponent<Text>();
        fatigueText.text = attributes.movementRemaining.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector3(0, -10, 0);
        int character= int.Parse(gameObject.name);
        
        if(character == SpeciesSellector.selectedCharacter)
        {
            if (Input.GetKey(KeyCode.W) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                // position.x = (float)(position.x + 0.1);
                position.z = (float)(position.z + 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
                //Debug.Log(PlayerInfo.steps);
            }
            if (Input.GetKey(KeyCode.A) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.x = (float)(position.x - 0.1);
                // position.z = (float)(position.z + 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
                //Debug.Log(PlayerInfo.steps);
            }
            if (Input.GetKey(KeyCode.S) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                // position.x = (float)(position.x - 0.1);
                position.z = (float)(position.z - 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
                //Debug.Log(PlayerInfo.steps);
            }
            if (Input.GetKey(KeyCode.D) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.x = (float)(position.x + 0.1);
                // position.z = (float)(position.z - 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
                //Debug.Log(PlayerInfo.steps);
            }

            if ((!Input.GetKey(KeyCode.W)) & (!Input.GetKey(KeyCode.A)) & (!Input.GetKey(KeyCode.S)) & (!Input.GetKey(KeyCode.D)))
            {
                anim.SetBool("walking", false);
                if (attributes.movementRemaining < 300)
                {
                    attributes.movementRemaining++;
                    fatigueText.text = attributes.movementRemaining.ToString();
                }
            }
        }
    }
}
