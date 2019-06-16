using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoors : MonoBehaviour
{
    public GameObject movableDoorPivot;
    public GameObject doorBody;
    public Item[] doorKeys;
    public Switch_Behavior activationSwitch;
    public float interactionDistance;
    public float setupTimeLaps;
    public float scripteMovementsRepetitions;
    public float smoothmovement;
    public bool cameraFeedback;
    public float feedbackCooldownTime;
    public bool startOpened;
    public bool holdSwitchToOpen;
    public bool lockState;
    public enum DisplacementType { Traslation, Rotation};
    public DisplacementType displacementType;
    public enum AxisPivot { X, Y, Z}
    public AxisPivot axisPivot;
    public int maxGrades;
    public bool invertTraslationMovement;

    private bool activated;
    [SerializeField]
    private bool opened;
    private bool locked;
    public bool finishOpen;
    private Vector3 closeRot;
    private Vector3 openRot;
    private bool justUnlocked;
    private bool justOpen;
    private bool justClose;
    [SerializeField]
    private float actualSetupTimeLaps;
    private int numKeys;
    [SerializeField]
    private Vector3 maxTraslationVector;
    [SerializeField]
    private Vector3 closePositionTraslationVector;

    public FMOD.Studio.EventInstance openEvent;
    [FMODUnity.EventRef]
    public string openDir;
    void Start()
    {
        //Debug.Log("Bounds X = " + doorBody.GetComponent<MeshRenderer>().bounds.size.x);
        //Debug.Log("Bounds Y = " + doorBody.GetComponent<MeshRenderer>().bounds.size.y);
        //Debug.Log("Bounds Z = " + doorBody.GetComponent<MeshRenderer>().bounds.size.z);
        actualSetupTimeLaps = setupTimeLaps;

        switch (displacementType)
        {
            case DisplacementType.Rotation:
                closeRot = movableDoorPivot.transform.localRotation.eulerAngles;
                switch (axisPivot)
                {
                    case AxisPivot.X:
                        openRot = new Vector3(maxGrades, 0, 0);
                        break;
                    case AxisPivot.Y:
                        openRot = new Vector3(0, maxGrades, 0);
                        break;
                    case AxisPivot.Z:
                        openRot = new Vector3(0, 0, maxGrades);
                        break;
                }
                break;
            case DisplacementType.Traslation:
                closePositionTraslationVector = movableDoorPivot.transform.localPosition;
                switch (axisPivot)
                {
                    case AxisPivot.X:
                        maxTraslationVector.x = doorBody.GetComponent<MeshRenderer>().bounds.size.x;
                        break;
                    case AxisPivot.Y:
                        maxTraslationVector.y = doorBody.GetComponent<MeshRenderer>().bounds.size.y;
                        break;
                    case AxisPivot.Z:
                        maxTraslationVector.z = doorBody.GetComponent<MeshRenderer>().bounds.size.z;
                        break;
                }
                break;
        }

        if (doorKeys.Length > 0 && activationSwitch == null)
        {
            //switch (displacementType)
            //{
            //    case DisplacementType.Rotation:
            //        closeRot = movableDoorPivot.transform.localRotation.eulerAngles;
            //        switch (axisPivot)
            //        {
            //            case AxisPivot.X:
            //                openRot = new Vector3(maxGrades, 0, 0);
            //                break;
            //            case AxisPivot.Y:
            //                openRot = new Vector3(0, maxGrades, 0);
            //                break;
            //            case AxisPivot.Z:
            //                openRot = new Vector3(0, 0, maxGrades);
            //                break;
            //        }
            //        break;
            //    case DisplacementType.Traslation:
            //        switch (axisPivot)
            //        {
            //            case AxisPivot.X:
            //                if (invertTraslationMovement)
            //                {
            //                    maxTraslation = doorBody.GetComponent<MeshRenderer>().bounds.size.x - movableDoorPivot.transform.position.x;
            //                }
            //                else
            //                {
            //                    maxTraslation = doorBody.GetComponent<MeshRenderer>().bounds.size.x + movableDoorPivot.transform.position.x;
            //                }
            //                break;
            //            case AxisPivot.Y:
            //                if (invertTraslationMovement)
            //                {
            //                    maxTraslation = doorBody.GetComponent<MeshRenderer>().bounds.size.y - movableDoorPivot.transform.position.y;
            //                }
            //                else
            //                {
            //                    maxTraslation = doorBody.GetComponent<MeshRenderer>().bounds.size.y + movableDoorPivot.transform.position.y;
            //                }
            //                break;
            //            case AxisPivot.Z:
            //                if (invertTraslationMovement)
            //                {
            //                    maxTraslation = doorBody.GetComponent<MeshRenderer>().bounds.size.z - movableDoorPivot.transform.position.z;
            //                }
            //                else
            //                {
            //                    maxTraslation = doorBody.GetComponent<MeshRenderer>().bounds.size.z + movableDoorPivot.transform.position.z;
            //                }
            //                break;
            //        }
            //        break;
            //}

            numKeys = doorKeys.Length;
            locked = lockState;
            opened = false;
            justUnlocked = true;
            finishOpen = false;
        }
        if (activationSwitch != null && doorKeys.Length == 0)
        {
            switch (displacementType)
            {
                case DisplacementType.Rotation:
                    if (startOpened)
                    {
                        movableDoorPivot.transform.localRotation = Quaternion.Euler(openRot);
                        justOpen = false;
                        justClose = true;
                        finishOpen = true;
                    }
                    if (!startOpened)
                    {
                        movableDoorPivot.transform.localRotation = Quaternion.Euler(closeRot);
                        justOpen = true;
                        justClose = false;
                        finishOpen = false;
                    }
                    break;
                case DisplacementType.Traslation:
                    if (startOpened)
                    {
                        if (invertTraslationMovement)
                        {
                            movableDoorPivot.transform.localPosition -= maxTraslationVector;
                        }
                        else
                        {
                            movableDoorPivot.transform.localPosition += maxTraslationVector;
                        }
                        
                        justOpen = false;
                        justClose = true;
                        finishOpen = true;
                    }
                    if (!startOpened)
                    {
                        movableDoorPivot.transform.localPosition = closePositionTraslationVector;
                        justOpen = true;
                        justClose = false;
                        finishOpen = false;
                    }
                    break;
            }
        }
        openEvent = FMODUnity.RuntimeManager.CreateInstance(openDir);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(openEvent, transform, GetComponent<Rigidbody>());
    }

    void Update()
    {
        if (actualSetupTimeLaps > 0)
        {
            actualSetupTimeLaps -= Time.deltaTime;
        }
        else
        {
            if (actualSetupTimeLaps != 0) actualSetupTimeLaps = 0;
        }
        if (doorKeys.Length > 0 && activationSwitch == null)
        {
            if (!locked)
            {
                if (cameraFeedback && actualSetupTimeLaps == 0 && justUnlocked)
                {
                    CameraController.instance.StartScriptedMovement(this.gameObject, feedbackCooldownTime);
                }
                
                switch (displacementType)
                {
                    case DisplacementType.Rotation:
                        if (opened && movableDoorPivot.transform.rotation.eulerAngles != openRot)
                        {
                            //Debug.Log("Opennig Door with key");
                            movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * smoothmovement);
                        }
                        else
                        {
                            finishOpen = true;
                        }
                        break;
                    case DisplacementType.Traslation:
                        if (opened && movableDoorPivot.transform.position != maxTraslationVector)
                        {
                            if (invertTraslationMovement)
                            {
                                if (movableDoorPivot.transform.localPosition != closePositionTraslationVector - maxTraslationVector)
                                {
                                    //Debug.Log("OpenableDoor: " + GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector));
                                    movableDoorPivot.transform.localPosition = Vector3.Lerp(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector, Time.deltaTime * smoothmovement);
                                }
                                else
                                if(GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector) < 0.2f)
                                {
                                    movableDoorPivot.transform.localPosition = closePositionTraslationVector - maxTraslationVector;
                                    finishOpen = true;
                                }
                            }
                            else
                            {
                                if (movableDoorPivot.transform.localPosition != closePositionTraslationVector + maxTraslationVector)
                                {
                                    movableDoorPivot.transform.localPosition = Vector3.Lerp(movableDoorPivot.transform.localPosition, closePositionTraslationVector + maxTraslationVector, Time.deltaTime * smoothmovement);
                                }
                                else
                                if (GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector + maxTraslationVector) < 0.2f)
                                {
                                    movableDoorPivot.transform.localPosition = closePositionTraslationVector + maxTraslationVector;
                                    finishOpen = true;
                                }
                            }
                        }
                        break;
                }
            }
        }
        if (activationSwitch != null && doorKeys.Length == 0)
        {
            if (holdSwitchToOpen)
            {
                if (activationSwitch.IsHoldedSwitched())
                {
                    if (cameraFeedback && actualSetupTimeLaps == 0 && justOpen && scripteMovementsRepetitions > 0)
                    {
                        //Debug.Log("openning door camera feedback");
                        justClose = true;
                        justOpen = false;
                        scripteMovementsRepetitions--;
                        CameraController.instance.StartScriptedMovement(this.gameObject, feedbackCooldownTime);
                    }
                    //Debug.Log("SwitchedActivated");
                    
                    switch (displacementType)
                    {
                        case DisplacementType.Rotation:
                            if (movableDoorPivot.transform.localRotation.eulerAngles != openRot)
                            {
                                //Debug.Log("Opennig Door");
                                movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * smoothmovement);
                            }
                            else
                            {
                                movableDoorPivot.transform.localRotation = Quaternion.Euler(openRot);
                            }
                            break;
                        case DisplacementType.Traslation:
                            if (invertTraslationMovement)
                            {
                                if (movableDoorPivot.transform.position != closePositionTraslationVector - maxTraslationVector)
                                {
                                    //Debug.Log("OpenableDoor: " + GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector));
                                    movableDoorPivot.transform.localPosition = Vector3.Lerp(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector, Time.deltaTime * smoothmovement);
                                }
                                else
                                if (GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector) < 0.2f)
                                {
                                    movableDoorPivot.transform.localPosition = closePositionTraslationVector - maxTraslationVector;
                                    finishOpen = true;
                                }
                            }
                            else
                            {
                                if (movableDoorPivot.transform.position != closePositionTraslationVector + maxTraslationVector)
                                {
                                    movableDoorPivot.transform.localPosition = Vector3.Lerp(movableDoorPivot.transform.localPosition, closePositionTraslationVector + maxTraslationVector, Time.deltaTime * smoothmovement);
                                }
                                else
                                if (GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector + maxTraslationVector) < 0.2f)
                                {
                                    finishOpen = true;
                                    movableDoorPivot.transform.localPosition = closePositionTraslationVector + maxTraslationVector;
                                }
                            }
                            break;
                    }
                }
                else
                if (!activationSwitch.IsHoldedSwitched())
                {
                    if (cameraFeedback && actualSetupTimeLaps == 0 && justClose && scripteMovementsRepetitions > 0)
                    {
                        //Debug.Log("closing door camera feedback");
                        justOpen = true;
                        justClose = false;
                        scripteMovementsRepetitions--;
                        CameraController.instance.StartScriptedMovement(this.gameObject, feedbackCooldownTime);
                    }
                    switch (displacementType)
                    {
                        case DisplacementType.Rotation:
                            if (movableDoorPivot.transform.localRotation.eulerAngles != closeRot)
                            {
                                //Debug.Log("Closing Door");
                                finishOpen = false;
                                movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(closeRot), Time.deltaTime * smoothmovement);
                            }
                            else
                            {
                                movableDoorPivot.transform.localRotation = Quaternion.Euler(closeRot);
                            }
                            break;
                        case DisplacementType.Traslation:
                            if (movableDoorPivot.transform.localPosition != closePositionTraslationVector)
                            {
                                finishOpen = false;
                                movableDoorPivot.transform.localPosition = Vector3.Lerp(movableDoorPivot.transform.localPosition, closePositionTraslationVector, Time.deltaTime * smoothmovement);
                            }
                            else
                            {
                                movableDoorPivot.transform.localPosition = closePositionTraslationVector;
                            }
                            break;
                    }
                }
            }
            if (!holdSwitchToOpen)
            {
                if (activationSwitch.IsSwitched())
                {
                    if (opened != true) OpenDoor();
                    if (cameraFeedback && actualSetupTimeLaps == 0 && justOpen && scripteMovementsRepetitions > 0)
                    {
                        //Debug.Log("openning door camera feedback");
                        justClose = true;
                        justOpen = false;
                        scripteMovementsRepetitions--;
                        CameraController.instance.StartScriptedMovement(this.gameObject, feedbackCooldownTime);
                    }
                    //Debug.Log("SwitchedActivated");
                    switch (displacementType)
                    {
                        case DisplacementType.Rotation:
                            if (movableDoorPivot.transform.localRotation.eulerAngles != openRot)
                            {
                                //Debug.Log("Opennig Door");
                                movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * smoothmovement);
                            }
                            else
                            {
                                movableDoorPivot.transform.localRotation = Quaternion.Euler(openRot);
                            }
                            break;
                        case DisplacementType.Traslation:
                            if (invertTraslationMovement)
                            {
                                if (GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector) > 0.1f)
                                {
                                    //Debug.Log("OpenableDoor: " + GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector));
                                    movableDoorPivot.transform.localPosition = Vector3.Lerp(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector, Time.deltaTime * smoothmovement);
                                }
                                else
                                //if (GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector - maxTraslationVector) < 0.1f)
                                {
                                    movableDoorPivot.transform.localPosition = closePositionTraslationVector - maxTraslationVector;
                                    finishOpen = true;
                                }
                            }
                            else
                            {
                                if (GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector + maxTraslationVector) > 0.2f)
                                {
                                    movableDoorPivot.transform.localPosition = Vector3.Lerp(movableDoorPivot.transform.localPosition, closePositionTraslationVector + maxTraslationVector, Time.deltaTime * smoothmovement);
                                }
                                else
                                //if (GenericSensUtilities.instance.DistanceBetween2Vectors(movableDoorPivot.transform.localPosition, closePositionTraslationVector + maxTraslationVector) < 0.2f)
                                {
                                    movableDoorPivot.transform.localPosition = closePositionTraslationVector + maxTraslationVector;
                                    finishOpen = true;
                                }
                            }
                            break;
                    }
                    if (finishOpen) openEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
                else
                if (!activationSwitch.IsSwitched())
                {
                    //if (opened != false) opened = false;
                    if (cameraFeedback && actualSetupTimeLaps == 0 && justClose && scripteMovementsRepetitions > 0)
                    {
                        //Debug.Log("closing door camera feedback");
                        justOpen = true;
                        justClose = false;
                        scripteMovementsRepetitions--;
                        CameraController.instance.StartScriptedMovement(this.gameObject, feedbackCooldownTime);
                    }
                    switch (displacementType)
                    {
                        case DisplacementType.Rotation:
                            if (movableDoorPivot.transform.localRotation.eulerAngles != closeRot)
                            {
                                //Debug.Log("Closing Door");
                                movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(closeRot), Time.deltaTime * smoothmovement);
                            }
                            else
                            {
                                movableDoorPivot.transform.localRotation = Quaternion.Euler(closeRot);
                            }
                            break;
                        case DisplacementType.Traslation:
                            if (movableDoorPivot.transform.localPosition != closePositionTraslationVector)
                            {
                                movableDoorPivot.transform.localPosition = Vector3.Lerp(movableDoorPivot.transform.localPosition, closePositionTraslationVector, Time.deltaTime * smoothmovement);
                            }
                            else
                            {
                                movableDoorPivot.transform.localPosition = closePositionTraslationVector;
                            }
                            break;
                    }
                }
            }
        }
    }
    private void UnlockDoor()
    {
        locked = false;
    }
    private void LockDoor()
    {
        locked = true;
    }
    public void OpenDoor()
    {
        openEvent.start();
        UnlockDoor();
        opened = true;
    }
    /*public void OpenDoorHolding(bool holding)
    {
        opened = holding;
    }*/
    public bool GetLockedActualState()
    {
        return locked;
    }
    public int GetNumKeys()
    {
        return numKeys;
    }
}
