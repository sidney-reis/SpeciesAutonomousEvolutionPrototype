using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breed : MonoBehaviour {
    public void BreedClick ()
    {
        GameObject activeCreature = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedCreature.ToString()).gameObject;
        GameObject passiveCreature = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedMenuCreature.ToString()).gameObject;

        if (activeCreature.GetComponent<SpeciesAttributes>().libido < 100)
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
            SpriteRenderer childSprite = childObject.AddComponent<SpriteRenderer>();
            Sprite creatureSprite = Resources.Load<Sprite>("species_"+PlayerInfo.selectedSpecies.ToString()+"_default");
            childSprite.sprite = creatureSprite;
            childObject.tag = "ControllableSpecies";
            Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
            childPosition.y = terrain.SampleHeight(childPosition) + childBoxSize.y + 1;
            childObject.transform.position = childPosition;
            Animator childAnimator = childObject.AddComponent<Animator>();
            childAnimator.runtimeAnimatorController = Resources.Load("playerSpeciesController") as RuntimeAnimatorController;
            childAnimator.updateMode = AnimatorUpdateMode.Normal;
            childAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            childObject.AddComponent<SpeciesAttributes>();
            childObject.AddComponent<AttributeUpdater>();
            childObject.AddComponent<CharacterMovement>();

            activeCreature.GetComponent<SpeciesAttributes>().libido -= 100;
            PlayerInfo.playerCreaturesCount++;
        }
    }
}
