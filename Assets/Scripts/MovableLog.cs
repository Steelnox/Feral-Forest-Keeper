using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableLog : MonoBehaviour
{
    public GameObject logBody;
    public bool attchAviable;
    public bool beingPushed;
    public bool falling;
    public float angleToContact;
    public float attachDistance;
    public float weight;
    [SerializeField]
    private Vector3 lastNoPushingPos;
    public MeshRenderer bodyMeshrenderer;
    public Rigidbody myRigidbody;

    void Start()
    {
        //bodyMeshrenderer = logBody.GetComponent<MeshRenderer>();
        lastNoPushingPos = Vector3.zero;
        myRigidbody.isKinematic = true;
    }

    void Update()
    {
        CheckSideToPush();
        if (beingPushed)
        {
            if (CheckGroundDistance() > bodyMeshrenderer.bounds.extents.y / 2 + 1.0f || logBody.transform.position.y < lastNoPushingPos.y - 1.0f)
            {
                myRigidbody.isKinematic = false;
                falling = true;
            }
            else
            {
                falling = false;
            }
        }
        else
        {
            falling = false;
        }
    }
    public Vector2 CheckSideToPush()
    {
        Vector2 localDirection = new Vector2(0, 0);
        attchAviable = false;

        if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.characterModel.transform.position, transform.position) < attachDistance)
        {
            float angleBetweenPlayerForwardAndContactPointForward = Vector2.Angle(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.characterModel.transform.forward), GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerAnimationController.instance.transform.position, FindContactPoint(PlayerAnimationController.instance.transform.position))));
            localDirection = GenericSensUtilities.instance.Transform3DTo2DMovement(transform.forward);
            if (angleBetweenPlayerForwardAndContactPointForward < angleToContact && angleBetweenPlayerForwardAndContactPointForward > 0)
                attchAviable = true;
        }
        //Debug.Log("LocalDirection on rock: " + localDirection);
        return localDirection;
    }
    public void PushLog(Vector3 force)
    {
        /*playerForceInput = force;*/
        Vector3 newPosition = this.transform.position + force;
        //newPosition = transform.TransformDirection(newPosition);
        transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime / weight);
    }
    public void SetBeingPushed(bool b)
    {
        beingPushed = b;
        if (beingPushed == true)
        {
            myRigidbody.isKinematic = false;
        }
        if (beingPushed == false && falling == false)
        {
            myRigidbody.isKinematic = true;
        }
    }
    public bool CheckIfFalling()
    {
        return falling;
    }
    public float CheckGroundDistance()
    {
        float dis;
        Ray ray = new Ray(logBody.transform.position, Vector3.down * bodyMeshrenderer.bounds.extents.y / 2);
        RaycastHit rayHit;
        Physics.Raycast(ray, out rayHit);

        if (rayHit.collider != null)
        {
            Debug.DrawLine(logBody.transform.position, rayHit.point, Color.red);
            dis = GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, rayHit.point);
            //Debug.Log("Ground Distance = " + dis);
            return dis;
        }
        else
        {
            return 0;
        }
    }
    public void SetLastNoPushPosition(Vector3 _pos)
    {
        lastNoPushingPos = _pos;
    }
    public Vector3 GetLastNoPushingPosition()
    {
        return lastNoPushingPos;
    }
    public void ResetLastNoPushingPos()
    {
        lastNoPushingPos = Vector3.zero;
    }
    public Vector3 FindContactPoint(Vector3 worldPosition)
    {
        Vector3 point;

        point = bodyMeshrenderer.bounds.ClosestPoint(worldPosition);
        Debug.DrawLine(point, worldPosition, Color.cyan);
        return point;
    }
}
