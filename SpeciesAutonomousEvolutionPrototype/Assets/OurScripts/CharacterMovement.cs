using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {
    private Animator anim;
    public Rigidbody rb;
    SpeciesAttributes attributes;
    Text fatigueText;
    int character;
    Vector3 updatedSpriteSize;
    SpriteRenderer spriteRenderer;

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag.Equals("Food") == true)
        {
            foodEaten();
            target.gameObject.SetActive(false);
            StartCoroutine(RespawnFood(target.gameObject));
        }
        if(target.gameObject.tag.Equals("RandomFood") == true)
        {
            foodEaten();
            Destroy(target.gameObject);
            RandomFoodGenerator.randomFoodCount--;
        }
    }

    IEnumerator RespawnFood(GameObject food)
    {
        yield return new WaitForSeconds(60);
        food.SetActive(true);
    }

    private void foodEaten()
    {
        attributes.hungry += 200;
        if (attributes.hungry > 300)
        {
            attributes.hungry = 300;
        }
        if (character == PlayerInfo.selectedCreature)
        {
            if (attributes.hungry >= 150)
            {
                PlayerModel.CurrentModel.foods++;
            }
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
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        rb.velocity = new Vector3(0, -10, 0);

        if(spriteRenderer.sprite != null)
        {
            updatedSpriteSize = spriteRenderer.sprite.bounds.size;
            updatedSpriteSize.z = 4;
            gameObject.GetComponent<BoxCollider>().size = updatedSpriteSize;
            gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
        }
        
        if ((character == PlayerInfo.selectedCreature)&&(!attributes.dying))
        {
            if (Input.GetKey(KeyCode.W) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.z = (float)(position.z + (0.1 + attributes.movementUpgrade * 0.05) * GameConstants.movementSpeed);
                this.transform.position = position;
                //PlayerModel.CurrentModel.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
            }
            if (Input.GetKey(KeyCode.A) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.x = (float)(position.x - (0.1 + attributes.movementUpgrade * 0.05) * GameConstants.movementSpeed);
                this.transform.position = position;
                //PlayerModel.CurrentModel.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
                SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.flipX = true;
            }
            if (Input.GetKey(KeyCode.S) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.z = (float)(position.z - (0.1 + attributes.movementUpgrade * 0.05) * GameConstants.movementSpeed);
                this.transform.position = position;
                //PlayerModel.CurrentModel.steps++;
                attributes.movementRemaining--;
                fatigueText.text = attributes.movementRemaining.ToString();
            }
            if (Input.GetKey(KeyCode.D) && attributes.movementRemaining > 0)
            {
                anim.SetBool("walking", true);
                Vector3 position = this.transform.position;
                position.x = (float)(position.x + (0.1 + attributes.movementUpgrade * 0.05) * GameConstants.movementSpeed);
                this.transform.position = position;
                //PlayerModel.CurrentModel.steps++;
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
