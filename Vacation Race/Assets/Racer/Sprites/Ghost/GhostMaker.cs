using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMaker : MonoBehaviour
{
    /*All this should be controlled by the GameManager Eventually*/



    public GameObject ghostPrefab;

    private readonly float ghost_deathSpeed = 4f;

    public void MakeGhost(Color col)
    {
        StartCoroutine(MakingGhost(col, ghost_deathSpeed));
    }

    public void MakeGhost(Color col, float timer)
    {
        StartCoroutine(MakingGhost(col, timer));
    }


    private IEnumerator MakingGhost(Color col, float timer)
    {
        yield return new WaitForEndOfFrame();

        GameObject ghostIntance = Instantiate(ghostPrefab, transform.position, Quaternion.identity);

        ghostIntance.transform.SetParent(GameObject.Find("GameManager/Ghosts").transform);

        ghostIntance.GetComponent<SpriteRenderer>().sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite;

        ghostIntance.GetComponent<SpriteMask>().sprite = ghostIntance.GetComponent<SpriteRenderer>().sprite;

        ghostIntance.GetComponent<Animator>().speed = timer;

        ghostIntance.name = gameObject.name + " Ghost";

        ghostIntance.GetComponent<SpriteRenderer>().material.color = col;

        GameObject addonParent = new GameObject("Addons");

        addonParent.transform.SetParent(ghostIntance.transform);

        addonParent.transform.localPosition = transform.Find("Sprite/Head Addon Point").localPosition;
        addonParent.transform.localScale = Vector3.one;


        foreach (Transform child in transform.Find("Sprite/Head Addon Point"))
        {
            if (child.GetComponent<Sprites_Anim>().canHaveGhost)
            {
                GameObject ghostAddon = Instantiate(ghostPrefab, addonParent.transform);

                ghostAddon.GetComponent<SpriteRenderer>().sprite = child.GetComponent<SpriteRenderer>().sprite;

                ghostAddon.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

                ghostAddon.GetComponent<Animator>().speed = timer;

                ghostAddon.name = child.name + " Ghost";

                ghostAddon.GetComponent<SpriteRenderer>().material.color = col;

                ghostAddon.transform.localPosition = Vector3.zero;

                ghostAddon.transform.localScale = Vector3.one;
            }
        }
    }
}
