using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegHandler : MonoBehaviour
{
    public Transform raycastDownPoint;
    public Vector3 moveToTarget;
    public float moveThreshhold;
    public Vector3 moveYup;
    public float lerpSpeed;

    private FastIK fik;

    // Start is called before the first frame update
    void Start()
    {
        fik = GetComponentInChildren<FastIK>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(raycastDownPoint.position, -Vector3.up * fik.completeLength, Color.cyan);
        if (Physics.Raycast(raycastDownPoint.position, -Vector3.up, out RaycastHit hit, fik.completeLength))
        {
            moveToTarget = hit.point;
        }

        if((moveToTarget - fik.target.position).magnitude > moveThreshhold)
        {
            //Debug.Log("moving IK target");
            fik.target.position = moveToTarget;
            //StartCoroutine(MoveLegPos());
        }
    }

    public IEnumerator MoveLegPos()   //**BROKEN**
    {
        float elapsedTime = 0f;
        while(elapsedTime < lerpSpeed/2)
        {
            fik.target.position = Vector3.Lerp(fik.target.position, ((moveToTarget - fik.target.position)/2 + moveYup), lerpSpeed/2);
            elapsedTime += Time.deltaTime;
            Debug.Log("Moving first half");
        }
        while (elapsedTime < lerpSpeed)
        {
            fik.target.position = Vector3.Lerp(((moveToTarget - fik.target.position) / 2 + moveYup), moveToTarget, lerpSpeed / 2);
            elapsedTime += Time.deltaTime;
            Debug.Log("Moving last half");
        }
        yield return null;
    }
}
