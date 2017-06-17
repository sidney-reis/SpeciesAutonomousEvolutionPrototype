using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Breed : MonoBehaviour {
    public void BreedClick ()
    {
        GameObject activeCreature = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedCreature.ToString()).gameObject;
        GameObject passiveCreature = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedMenuCreature.ToString()).gameObject;

        if (activeCreature.GetComponent<SpeciesAttributes>().libido < 50)
        {
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").SetActive(true);
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").GetComponent<Text>().text = "Not enough libido!";
        }
        else if(Vector3.Distance(activeCreature.transform.position, passiveCreature.transform.position) > 12.0)
        {
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").SetActive(true);
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").GetComponent<Text>().text = "Creature is too far!";
        }
        else
        {
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").GetComponent<Text>().text = "";
            GameObject.Find("MenuCanvas/MenuBackground/BreedMenu/ErrorText").SetActive(false);

            double normalizedFoods;
            double normalizedRan;
            double normalizedDefended;
            double normalizedAttacked;

            if(PlayerTime.currentSeconds == PlayerTime.totalSeconds)
            {
                normalizedFoods = PlayerModel.CurrentModel.foods / PlayerTime.currentSeconds;
                normalizedRan = PlayerModel.CurrentModel.ran / PlayerTime.currentSeconds;
                normalizedDefended = PlayerModel.CurrentModel.defended / PlayerTime.currentSeconds;
                normalizedAttacked = PlayerModel.CurrentModel.attacked / PlayerTime.currentSeconds;
            }
            else
            {
                normalizedFoods = ((PlayerModel.CurrentModel.foods / PlayerTime.currentSeconds) + (PlayerModel.LegacyModel.foods / PlayerTime.totalSeconds)) / 2;
                normalizedRan = ((PlayerModel.CurrentModel.ran / PlayerTime.currentSeconds) + (PlayerModel.LegacyModel.ran/ PlayerTime.totalSeconds)) / 2;
                normalizedDefended = ((PlayerModel.CurrentModel.defended / PlayerTime.currentSeconds) + (PlayerModel.LegacyModel.ran / PlayerTime.totalSeconds)) / 2;
                normalizedAttacked = ((PlayerModel.CurrentModel.attacked / PlayerTime.currentSeconds) + (PlayerModel.LegacyModel.ran / PlayerTime.totalSeconds)) / 2;
            }

            /* OLD MOVEMENT SYSTEM
             
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
            */

            int newbornMovement = 0;
            int newbornDefense = 0;
            if (activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 0 && normalizedRan > (normalizedDefended * 1.25))
            {
                newbornMovement = 1;
                newbornDefense = 0;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 1 && normalizedRan > (normalizedDefended * 1.33))
            {
                newbornMovement = 2;
                newbornDefense = 0;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 1 && normalizedRan > (normalizedDefended * 1.25))
            {
                newbornMovement = 1;
                newbornDefense = 0;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 2 && normalizedRan < (normalizedDefended * 1.33))
            {
                newbornMovement = 1;
                newbornDefense = 0;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().movementUpgrade == 2 && normalizedRan > (normalizedDefended * 1.33))
            {
                newbornMovement = 2;
                newbornDefense = 0;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().deffenseUpgrade == 0 && normalizedDefended > (normalizedRan * 1.25))
            {
                newbornMovement = 0;
                newbornDefense = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().deffenseUpgrade == 1 && normalizedDefended > (normalizedRan * 1.33))
            {
                newbornMovement = 0;
                newbornDefense = 2;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().deffenseUpgrade == 1 && normalizedDefended > (normalizedRan * 1.25))
            {
                newbornMovement = 0;
                newbornDefense = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().deffenseUpgrade == 2 && normalizedDefended < (normalizedRan * 1.33))
            {
                newbornMovement = 0;
                newbornDefense = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().deffenseUpgrade == 2 && normalizedDefended > (normalizedRan * 1.33))
            {
                newbornMovement = 0;
                newbornDefense = 2;
            }

            int newbornPerception = 0;
            if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 0 && normalizedFoods >= 0.008333) // 1 fruit each 2 minutes
            {
                newbornPerception = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 1 && normalizedFoods >= 0.0125) // 1.5 fruit each 2 minutes
            {
                newbornPerception = 2;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 1 && normalizedFoods >= 0.008333)
            {
                newbornPerception = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 2 && normalizedFoods < 0.0125)
            {
                newbornPerception = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().perceptionUpgrade == 2 && normalizedFoods >= 0.0125)
            {
                newbornPerception = 2;
            }

            int newbornAttack = 0;
            if (activeCreature.GetComponent<SpeciesAttributes>().attackUpgrade == 0 && normalizedAttacked >= 0.23809) // Enough damage to kill 1 creature each 7 minutes
            {
                newbornAttack = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().attackUpgrade == 1 && normalizedAttacked >= 0.41666) // Enough damage to kill 1 creature each 4 minutes
            {
                newbornAttack = 2;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().attackUpgrade == 1 && normalizedAttacked >= 0.23809)
            {
                newbornAttack = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().attackUpgrade == 2 && normalizedAttacked < 0.41666)
            {
                newbornAttack = 1;
            }
            else if (activeCreature.GetComponent<SpeciesAttributes>().attackUpgrade == 2 && normalizedAttacked >= 0.41666)
            {
                newbornAttack = 2;
            }

            int newbornCombat = 0;

            if(newbornAttack > newbornDefense)
            {
                newbornCombat = newbornAttack;
            }
            else
            {
                newbornCombat = newbornDefense;
            }

            if(newbornAttack > 0 && newbornDefense > 0)
            {
                if(newbornAttack > newbornMovement)
                {
                    newbornMovement = 0;
                }
                else
                {
                    newbornAttack = 0;
                }
            }

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

            SpriteRenderer spriteRenderer = childObject.AddComponent<SpriteRenderer>();
            //Sprite creatureSprite = Resources.Load<Sprite>("species_" + PlayerInfo.selectedSpecies.ToString() + "_default");
            //childSprite.sprite = creatureSprite;

            Animator childAnimator = childObject.AddComponent<Animator>();
            childAnimator.runtimeAnimatorController = Resources.Load("playerSpeciesController") as RuntimeAnimatorController;
            childAnimator.updateMode = AnimatorUpdateMode.Normal;
            childAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            
            childAnimator.SetInteger("movementUpgrade", newbornMovement);
            childAnimator.SetInteger("perceptionUpgrade", newbornPerception);
            childAnimator.SetInteger("combatUpgrade", newbornCombat);


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
            childPosition.y = terrain.SampleHeight(childPosition) + 10;
            childObject.transform.position = childPosition;

            NavMeshObstacle obstacle = childObject.AddComponent<NavMeshObstacle>();
            obstacle.center = new Vector3(0f, 0f, 0f);
            obstacle.size = new Vector3(1f, 1f, 4.1f);
            obstacle.carving = true;
            obstacle.enabled = true;
            NavMeshAgent agent = childObject.AddComponent<NavMeshAgent>();
            agent.radius = 0.53f;
            agent.height = 1;
            agent.speed = (6.0f + newbornMovement * 3) * GameConstants.movementSpeed;
            agent.angularSpeed = 120;
            agent.acceleration = 99;
            agent.stoppingDistance = 0;
            agent.autoBraking = true;
            agent.avoidancePriority = 50;
            agent.autoTraverseOffMeshLink = true;
            agent.autoRepath = true;
            agent.areaMask = 1;
            agent.enabled = false;
            

            SpeciesAttributes childAttributes = childObject.AddComponent<SpeciesAttributes>();
            childAttributes.movementUpgrade = newbornMovement;
            childAttributes.perceptionUpgrade = newbornPerception;
            childAttributes.attackUpgrade = newbornAttack;
            childAttributes.deffenseUpgrade = newbornDefense;
            childObject.AddComponent<AttributeUpdater>();
            childObject.AddComponent<CharacterMovement>();
            childObject.AddComponent<FixRotation>();
            childObject.AddComponent<PlayerAutonomousBehavior>();
            
            activeCreature.GetComponent<SpeciesAttributes>().libido -= 50;
            PlayerInfo.playerCreaturesCount++;

            PlayerModel.triggerBreed();
            PlayerTime.triggerBreed();
        }
    }
}
