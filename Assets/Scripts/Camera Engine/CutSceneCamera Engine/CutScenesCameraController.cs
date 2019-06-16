﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CutScenesCameraController : MonoBehaviour
{
    public GameObject cameraPivot;
    public Camera myCamera;
    public SceneParameters[] scenes;

    public enum CutSceneCameraBehavior { MOVE_BETWEEN_KEYS, CHANGE_SCENE };
    public CutSceneCameraBehavior actualBehavior;

    public Vector3 actualLocalRotation;
    public float rotationSmoothness;
    public float actualFieldOfView;
    public float actualDepthOfField;
    public float actualSpeed;
    public float depthSmoothness;
    

    public SceneParameters actualScene;
    public int actualSceneID;
    public CutSceneKeyPoints actualKeyPoint;
    public int actualKeyPointID;

    public float actualMovementTime;
    public Vector3 origin;

    public PostProcessVolume globalVolume;
    private DepthOfField depthOfField;

    void Start()
    {
        if (globalVolume != null)
        {
            globalVolume.profile.TryGetSettings(out depthOfField);
        }
        actualSceneID = 0;
        actualScene = scenes[actualSceneID];
        actualKeyPointID = 0;
        actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
        actualBehavior = CutSceneCameraBehavior.MOVE_BETWEEN_KEYS;
        //ChangeBehavior(CutSceneCameraBehavior.MOVE_BETWEEN_KEYS);
        //origin = cameraPivot.transform.position;
        myCamera.transform.localRotation = Quaternion.Euler(actualKeyPoint.localRotation);
        cameraPivot.transform.position = actualScene.sceneKeyPoints[actualKeyPointID].transform.position;
        origin = actualKeyPoint.transform.position;
        actualKeyPointID++;
        actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
        actualDepthOfField = actualKeyPoint.depthOfField;
        SetDepthOfView(actualDepthOfField);
    }

    void Update()
    {
        switch(actualBehavior)
        {
            case CutSceneCameraBehavior.MOVE_BETWEEN_KEYS:
                ///Set DEPHT OF FIELD
                if (actualDepthOfField > actualKeyPoint.depthOfField || actualDepthOfField < actualKeyPoint.depthOfField)
                {
                    actualDepthOfField = Mathf.Lerp(actualDepthOfField, actualScene.sceneKeyPoints[actualKeyPointID].depthOfField, Time.deltaTime * depthSmoothness);
                }
                else
                {
                    actualDepthOfField = actualKeyPoint.depthOfField;
                }
                ///SET LOCAL ROTATION
                if (myCamera.transform.localRotation.eulerAngles != actualKeyPoint.localRotation)
                {
                    myCamera.transform.localRotation = Quaternion.Lerp(myCamera.transform.localRotation, Quaternion.Euler(actualKeyPoint.localRotation), Time.deltaTime * rotationSmoothness);
                }
                else
                {
                    myCamera.transform.localRotation = Quaternion.Euler(actualKeyPoint.localRotation);
                }

                if (GenericSensUtilities.instance.DistanceBetween2Vectors(cameraPivot.transform.position, actualKeyPoint.transform.position) > 0.1f)
                {
                    //Debug.Log("MovingCamera");
                    MoveCamera(origin, actualKeyPoint.transform.position, actualKeyPoint.speed);
                    //myCamera.transform.localEulerAngles = Vector3.Lerp(myCamera.transform.localEulerAngles, actualKeyPoint.localRotation, Time.deltaTime * 5);
                }
                else
                {
                    //if (actualKeyPointID < actualScene.sceneKeyPoints.Length - 1)
                    //{
                    //    actualKeyPointID++;
                    //    actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
                    //    origin = cameraPivot.transform.position;
                    //    //ChangeBehavior(CutSceneCameraBehavior.MOVE_BETWEEN_KEYS);
                    //}
                    //ChangeBehavior(CutSceneCameraBehavior.CHANGE_KEY);
                    if (actualKeyPoint.endKey)
                    {
                        ChangeBehavior(CutSceneCameraBehavior.CHANGE_SCENE);
                    }
                    else
                    {
                        ChangeBehavior(CutSceneCameraBehavior.MOVE_BETWEEN_KEYS);
                    }
                }
                break;
            case CutSceneCameraBehavior.CHANGE_SCENE:
                ChangeScene();
                ChangeBehavior(CutSceneCameraBehavior.MOVE_BETWEEN_KEYS);
                break;
            //case CutSceneCameraBehavior.CHANGE_SCENE:
            //    ChangeScene();
            //    ChangeBehavior(CutSceneCameraBehavior.MOVE_BETWEEN_KEYS);
            //    break;
        }
        SetDepthOfView(actualDepthOfField);
    }
    private void ChangeScene()
    {
        if (actualSceneID < scenes.Length - 1)
        {
            actualSceneID++;
        }
        else
        {
            actualSceneID = 0;
        }
        actualScene = scenes[actualSceneID];
        actualKeyPointID = 0;
        actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
    }
    //private void NextKeyPoint()
    //{
    //    if (actualKeyPointID < actualScene.sceneKeyPoints.Length - 1)
    //    {
    //        actualKeyPointID++;
    //        actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
    //    }
    //}
    private void ChangeBehavior(CutSceneCameraBehavior newBehavior)
    {
        switch (actualBehavior)
        {
            case CutSceneCameraBehavior.MOVE_BETWEEN_KEYS:
                //if (actualKeyPointID < actualScene.sceneKeyPoints.Length - 1 && !actualScene.sceneKeyPoints[actualKeyPointID].startKey)
                //{
                //    origin = actualKeyPoint.transform.position;
                //    actualKeyPointID++;
                //    actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
                //}
                //else 
                //    if(actualScene.sceneKeyPoints[actualKeyPointID].startKey)
                //{
                //    cameraPivot.transform.position = actualScene.sceneKeyPoints[actualKeyPointID].transform.position;
                //    origin = actualKeyPoint.transform.position;
                //    actualKeyPointID++;
                //    actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
                //}
                //actualMovementTime = 0;
                break;
            case CutSceneCameraBehavior.CHANGE_SCENE:
                origin = cameraPivot.transform.position;
                break;
        }
        switch (newBehavior)
        {
            case CutSceneCameraBehavior.MOVE_BETWEEN_KEYS:
                if (actualKeyPointID < actualScene.sceneKeyPoints.Length - 1 && !actualScene.sceneKeyPoints[actualKeyPointID].startKey)
                {
                    origin = actualKeyPoint.transform.position;
                    actualKeyPointID++;
                    actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
                    //actualDepthOfField = actualKeyPoint.depthOfField;
                }
                else
                    if (actualScene.sceneKeyPoints[actualKeyPointID].startKey)
                {
                    myCamera.transform.localRotation = Quaternion.Euler(actualKeyPoint.localRotation);
                    cameraPivot.transform.position = actualScene.sceneKeyPoints[actualKeyPointID].transform.position;
                    origin = actualKeyPoint.transform.position;
                    actualKeyPointID++;
                    actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
                    actualDepthOfField = actualKeyPoint.depthOfField;
                    SetDepthOfView(actualDepthOfField);
                }
                actualMovementTime = 0;

                break;
            case CutSceneCameraBehavior.CHANGE_SCENE:
                
                break;
        }
        actualBehavior = newBehavior;
    }
    private void MoveCamera(Vector3 KeyPositionOrigin, Vector3 keyPositionEnd, float time)
    {
        actualMovementTime += Time.deltaTime;
        float evaluateTime = actualMovementTime / time;
        cameraPivot.transform.position = Vector3.Lerp(KeyPositionOrigin, keyPositionEnd, evaluateTime);
    }
    public void SetDepthOfView(float depth)
    {
        depthOfField.focusDistance.value = depth;
    }
    public float GetActualDepthOfView()
    {
        return depthOfField.focusDistance.value;
    }
}
