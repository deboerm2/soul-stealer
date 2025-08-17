using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPossession : MonoBehaviour
{
    private HashSet<GameObject> bodiesInRange = new HashSet<GameObject>();
    private GameObject takeoverTarget;

    private void Update()
    {
        #region Takeover detection
        if (bodiesInRange.Count > 0)
        {
            //Select the closest possesable target
            foreach (GameObject body in bodiesInRange)
            {
                if (!body.GetComponent<BodyTakeover>().isPossesable)
                {
                    continue;
                }
                else if (takeoverTarget == null)
                    takeoverTarget = body;
                else
                {
                    if ((takeoverTarget.transform.position - PlayerController.Instance.currentBody.transform.position).magnitude >
                        (body.transform.position - PlayerController.Instance.currentBody.transform.position).magnitude)
                    {
                        takeoverTarget = body;
                    }
                }
            }
            //Manually select a target within range
        }
        else
            takeoverTarget = null;

        #endregion
        #region Possesion
        if (PlayerController.Instance.plControls.FindAction("possession").triggered)
        {
            if (PlayerController.Instance.isPossessing)
            {
                PlayerController.Instance.BodySwap();
                if (!takeoverTarget.GetComponent<BodyTakeover>().isPossesable && takeoverTarget != null)
                {
                    takeoverTarget = null;
                }
            }
            else if (takeoverTarget != null)
            {
                PlayerController.Instance.BodySwap(takeoverTarget);
            }
            print("TAPPITY TAAP");
        }
        else if (PlayerController.Instance.plControls.FindAction("targetedPossession").triggered)
        {
            print("TARGET NOWWWW");
        }

        //keep invis body on top of possessed body, help with enemy targeting
        if (PlayerController.Instance.isPossessing)
        {
            PlayerController.Instance.mainBody.transform.position = PlayerController.Instance.currentBody.transform.position;
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BodyTakeover>() != null)
        {
            AddBodyInRange(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BodyTakeover>() != null)
        {
            RemoveBodyInRange(other.gameObject);
        }
    }

    //adds a gameobject to the hashSet used by player to know which bodies are even in range to be considered possessable candidates
    public void AddBodyInRange(GameObject body)
    {
        bodiesInRange.Add(body);
    }
    //removes a gameobject from the hashSet used by player to know which bodies are even in range to be considered possessable candidates
    public void RemoveBodyInRange(GameObject body)
    {
        bodiesInRange.Remove(body);
    }
}
