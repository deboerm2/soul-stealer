using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPossession : MonoBehaviour
{
    private HashSet<GameObject> bodiesInRange = new HashSet<GameObject>();
    public GameObject takeoverTarget { get; private set; }

    private bool isTargeting;

    private void Update()
    {
        #region Takeover detection
        if (bodiesInRange.Count > 0)
        {
            //Manually select a target within range
            if (isTargeting)
            {
                Targeting();
            }
            else
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
            }
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
            isTargeting = true;
            Time.timeScale = .3f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else if(!PlayerController.Instance.plControls.FindAction("targetedPossession").inProgress && isTargeting)
        {
            EndTargeting();
        }

        //keep invis body on top of possessed body, help with enemy targeting
        if (PlayerController.Instance.isPossessing)
        {
            PlayerController.Instance.mainBody.transform.position = PlayerController.Instance.currentBody.transform.position;
        }
        #endregion
    }

    void Targeting()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            takeoverTarget = null;
            foreach (GameObject body in bodiesInRange)
            {
                if (Vector3.Distance(hit.point, body.transform.position) > 1.5f)
                {
                    continue;
                }
                if (takeoverTarget == null || Vector3.Distance(hit.point, takeoverTarget.transform.position) > Vector3.Distance(hit.point, body.transform.position))
                {
                    takeoverTarget = body;
                    continue;
                }
            }
        }
        else
        {
            takeoverTarget = null;
            print("NADA");
        }
        
    }
    void EndTargeting()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isTargeting = false;
        print("RELEASED!");
        if(takeoverTarget != null)
            PlayerController.Instance.BodySwap(takeoverTarget);
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
