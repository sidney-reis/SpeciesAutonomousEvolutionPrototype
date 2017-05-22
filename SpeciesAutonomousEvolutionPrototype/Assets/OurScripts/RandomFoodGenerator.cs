using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFoodGenerator : MonoBehaviour {
    public static int randomFoodCount;
    private int maxRandomFood = 100;
    private float generateFrameInSeconds = 10;
    Vector3 terrainSize;
    Terrain terrain;

    void Start ()
    {
        randomFoodCount = 0;
        InvokeRepeating("GenerateFood", 0, generateFrameInSeconds);
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        terrainSize = terrain.terrainData.size;
	}
	
    void GenerateFood ()
    {
        if(randomFoodCount < maxRandomFood)
        {
            Vector3 foodPosition = new Vector3();
            Vector3 foodRotation;
            Vector3 foodScale;
            foodPosition.x = Random.Range(3, terrainSize.x-10);
            foodPosition.z = Random.Range(3, terrainSize.z-10);
            foodRotation.x = 0;
            foodRotation.y = 0;
            foodRotation.z = 0;
            foodScale.x = 5;
            foodScale.y = 5;
            foodScale.z = 1;
            GameObject foodObject = new GameObject("RandomFood");
            foodObject.transform.parent = gameObject.transform;
            foodObject.transform.rotation = Quaternion.Euler(foodRotation);
            foodObject.transform.localScale = foodScale;
            BoxCollider foodBox = foodObject.AddComponent<BoxCollider>();
            foodBox.isTrigger = false;
            foodBox.material = new PhysicMaterial("None");
            Vector3 foodBoxCenter;
            foodBoxCenter.x = 0;
            foodBoxCenter.y = 0;
            foodBoxCenter.z = 0;
            foodBox.center = foodBoxCenter;
            Vector3 foodBoxSize;
            foodBoxSize.x = 0.6f;
            foodBoxSize.y = 0.8f;
            foodBoxSize.z = 2;
            foodBox.size = foodBoxSize;
            SpriteRenderer foodSprite = foodObject.AddComponent<SpriteRenderer>();
            int randomFruitNumber = Random.Range(4, 7);
            Sprite fruitSprite = Resources.Load<Sprite>("fruta"+randomFruitNumber);
            foodSprite.sprite = fruitSprite;
            foodObject.tag = "RandomFood";
            foodObject.AddComponent<FoodMarks>();

            foodPosition.y = terrain.SampleHeight(foodPosition) + foodBoxSize.y + 1;
            foodObject.transform.position = foodPosition;

            randomFoodCount++;
        }
    }
}
