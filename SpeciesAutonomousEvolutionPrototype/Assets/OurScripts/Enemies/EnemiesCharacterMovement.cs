using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemiesCharacterMovement : MonoBehaviour
{
    public Rigidbody rb;
    EnemiesAttributes attributes;
    Vector3 updatedSpriteSize;
    SpriteRenderer spriteRenderer;

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag.Equals("Food") == true)
        {
            foodEaten();
            RespawningFood foodTarget = new RespawningFood();
            foodTarget.food = target.gameObject;
            foodTarget.respawningTime = 100;
            FoodRespawn.foodRespawning.Add(foodTarget);
            target.gameObject.SetActive(false);
        }
        if (target.gameObject.tag.Equals("RandomFood") == true)
        {
            foodEaten();
            Destroy(target.gameObject);
            RandomFoodGenerator.randomFoodCount--;
        }
    }

    private void foodEaten()
    {
        attributes.hungry += 200;
        if (attributes.hungry > 300)
        {
            attributes.hungry = 300;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attributes = GetComponent<EnemiesAttributes>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        rb.velocity = new Vector3(0, -10, 0);

        if (spriteRenderer.sprite != null)
        {
            updatedSpriteSize = spriteRenderer.sprite.bounds.size;
            updatedSpriteSize.z = 4;
            gameObject.GetComponent<BoxCollider>().size = updatedSpriteSize;
            gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
        }
    }
}
