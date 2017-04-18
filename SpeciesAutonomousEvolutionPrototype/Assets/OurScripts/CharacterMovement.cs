using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {
    private Animator anim;
    public Rigidbody rb;
    SpeciesAttributes attributes;
    Text fatigueText;
    int character;

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag.Equals("Food") == true)
        {
            attributes.hungry = 100;
            RespawningFood foodTarget = new RespawningFood();
            foodTarget.food = target.gameObject;
            foodTarget.respawningTime = 100;
            FoodRespawn.foodRespawning.Add(foodTarget);
            target.gameObject.SetActive(false);
        }
        if(target.gameObject.tag.Equals("RandomFood") == true)
        {
            attributes.hungry = 100;
            Destroy(target.gameObject);
            RandomFoodGenerator.randomFoodCount--;
        }
    }

    void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetInteger("selectedSpecies", PlayerInfo.selectedSpecies);
        attributes = GetComponent<SpeciesAttributes>();
        fatigueText = GameObject.Find("FatigueText").GetComponent<Text>();
        fatigueText.text = attributes.movementRemaining.ToString();
        character = int.Parse(gameObject.name);
    }
	
	void Update () {
        rb.velocity = new Vector3(0, -10, 0);
        
        if((character == PlayerInfo.selectedCreature)&&(!attributes.dying))
        {
            if (Input.GetKey(KeyCode.W) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.z = (float)(position.z + 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
            }
            if (Input.GetKey(KeyCode.A) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.x = (float)(position.x - 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
                SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.flipX = true;
            }
            if (Input.GetKey(KeyCode.S) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.z = (float)(position.z - 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
            }
            if (Input.GetKey(KeyCode.D) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.x = (float)(position.x + 0.1);
                this.transform.position = position;
                PlayerInfo.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
                SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.flipX = false;
            }

            if ((!Input.GetKey(KeyCode.W)) & (!Input.GetKey(KeyCode.A)) & (!Input.GetKey(KeyCode.S)) & (!Input.GetKey(KeyCode.D)))
            {
                anim.SetBool("walking", false);
                if (attributes.movementRemaining < SpeciesAttributes.MAX_MOVEMENT)
                {
                    attributes.movementRemaining++;
                    fatigueText.text = attributes.movementRemaining.ToString();
                }
            }
        }
    }
}
