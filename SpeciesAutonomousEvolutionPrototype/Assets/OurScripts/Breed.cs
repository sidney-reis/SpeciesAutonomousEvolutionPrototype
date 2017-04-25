using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breed : MonoBehaviour {
    public void BreedClick ()
    {
        GameObject activeCreature = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedCreature.ToString()).gameObject;
        GameObject passiveCreature = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedMenuCreature.ToString()).gameObject;

        if (activeCreature.GetComponent<SpeciesAttributes>().libido < 25)
        {
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").SetActive(true);
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").GetComponent<Text>().text = "Not enough libido!";
        }
        else if(Vector3.Distance(activeCreature.transform.position, passiveCreature.transform.position) > 100.0)
        {
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").SetActive(true);
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").GetComponent<Text>().text = "Creature is too far!";
        }
        else
        {
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").GetComponent<Text>().text = "";
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").SetActive(false);

            double normalizedSteps;
            double normalizedFoods;
            if(PlayerTime.currentSeconds == PlayerTime.totalSeconds)
            {
                normalizedSteps = PlayerModel.CurrentModel.steps / PlayerTime.currentSeconds;
                normalizedFoods = PlayerModel.CurrentModel.foods / PlayerTime.currentSeconds;
                Debug.Log("current foods on breed: " + PlayerModel.CurrentModel.foods);
                Debug.Log("current seconds on breed: " + PlayerTime.currentSeconds);

            }
            else
            {
                normalizedSteps = ((PlayerModel.CurrentModel.steps / PlayerTime.currentSeconds) + (PlayerModel.LegacyModel.steps / PlayerTime.totalSeconds)) / 2;
                normalizedFoods = ((PlayerModel.CurrentModel.foods / PlayerTime.currentSeconds) + (PlayerModel.LegacyModel.foods / PlayerTime.totalSeconds)) / 2;
            }
            
            int newbornMovemet = 0;
            if(activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 0 && normalizedSteps >= 50)
            {
                newbornMovemet = 1;
            }
            else if(activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 1 && normalizedSteps >= 75)
            {
                newbornMovemet = 2;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 1 && normalizedSteps >= 50)
            {
                newbornMovemet = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 2 && normalizedSteps < 75)
            {
                newbornMovemet = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 2 && normalizedSteps >= 75)
            {
                newbornMovemet = 2;
            }
            newbornMovemet = 0;

            int newbornPerception = 0;
            if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 0 && normalizedFoods >= 0.01666)
            {
                newbornPerception = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 1 && normalizedFoods >= 0.025)
            {
                newbornPerception = 2;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 1 && normalizedFoods >= 0.01666)
            {
                newbornPerception = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 2 && normalizedFoods < 0.025)
            {
                newbornPerception = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 2 && normalizedFoods >= 0.025)
            {
                newbornPerception = 2;
            }
            Debug.Log("newbornPerception: " + newbornPerception);
            Debug.Log("normalizedFoods: " + normalizedFoods);

            Vector3 childPosition = new Vector3();
            Vector3 childRotation;
            Vector3 childScale;
            childPosition.x = activeCreature.transform.position.x + 5;
            childPosition.z = activeCreature.transform.position.z + 5;
            childRotation.x = 0;
            childRotation.y = 0;
            childRotation.z = 0;
            childScale.x = 5;
            childScale.y = 5;
            childScale.z = 1;
            GameObject childObject = new GameObject(PlayerInfo.playerCreaturesCount.ToString());

            childObject.AddComponent<SpriteRenderer>();
            //Sprite creatureSprite = Resources.Load<Sprite>("species_" + PlayerInfo.selectedSpecies.ToString() + "_default");
            //childSprite.sprite = creatureSprite;

            Animator childAnimator = childObject.AddComponent<Animator>();
            childAnimator.runtimeAnimatorController = Resources.Load("playerSpeciesController") as RuntimeAnimatorController;
            childAnimator.updateMode = AnimatorUpdateMode.Normal;
            childAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            
            childAnimator.SetInteger("movementUpgrade", newbornMovemet);
            childAnimator.SetInteger("perceptionUpgrade", newbornPerception);

            childObject.transform.parent = GameObject.Find("PlayerCreatures").transform;
            childObject.transform.rotation = Quaternion.Euler(childRotation);
            childObject.transform.localScale = childScale;
            BoxCollider childBox = childObject.AddComponent<BoxCollider>();
            childBox.isTrigger = false;
            childBox.material = new PhysicMaterial("None");
            Vector3 childBoxCenter;
            childBoxCenter.x = 0;
            childBoxCenter.y = 0;
            childBoxCenter.z = 0;
            childBox.center = childBoxCenter;
            Vector3 childBoxSize;
            childBoxSize.x = 0.9f;
            childBoxSize.y = 0.8f;
            childBoxSize.z = 4;
            childBox.size = childBoxSize;
            Rigidbody childRigidbody = childObject.AddComponent<Rigidbody>();
            childRigidbody.mass = 10;
            childRigidbody.drag = 0;
            childRigidbody.useGravity = true;
            childRigidbody.isKinematic = false;
            childRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            childRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            childObject.tag = "ControllableSpecies";
            Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
            childPosition.y = terrain.SampleHeight(childPosition) + 7;
            childObject.transform.position = childPosition;

            SpeciesAttributes childAttributes = childObject.AddComponent<SpeciesAttributes>();
            childAttributes.movementUpgrade = newbornMovemet;
            childAttributes.perceptionUpgrade = newbornPerception;
            childObject.AddComponent<AttributeUpdater>();
            childObject.AddComponent<CharacterMovement>();
            childObject.AddComponent<FixRotation>();
            childObject.AddComponent<PlayerAutonomousBehavior>();

            activeCreature.GetComponent<SpeciesAttributes>().libido -= 100;
            PlayerInfo.playerCreaturesCount++;

            PlayerModel.triggerBreed();
            PlayerTime.triggerBreed();
        }
    }
}
