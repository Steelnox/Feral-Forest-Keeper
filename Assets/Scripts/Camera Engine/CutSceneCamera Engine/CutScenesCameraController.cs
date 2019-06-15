﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenesCameraController : MonoBehaviour
{
    public GameObject cameraPivot;
    public Camera myCamera;
    public SceneParameters[] scenes;

    public enum CutSceneCameraBehavior { MOVE_BETWEEN_KEYS, CHANGE_KEY, CHANGE_SCENE };
    public CutSceneCameraBehavior actualBehavior;

    public Vector3 actualLocalRotation;
    public float actualFieldOfView;
    public float actualDepthOfField;
    public float actualSpeed;

    public SceneParameters actualScene;
    public int actualSceneID;
    public CutSceneKeyPoints actualKeyPoint;
    public int actualKeyPointID;

    public float actualMovementTime;
    public Vector3 origin;

    void Start()
    {
        actualSceneID = 0;
        actualScene = scenes[actualSceneID];
        actualKeyPointID = 0;
        actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
        //actualBehavior = CutSceneCameraBehavior.MOVE_BETWEEN_KEYS;
        ChangeBehavior(CutSceneCameraBehavior.MOVE_BETWEEN_KEYS);
    }

    void Update()
    {
        switch(actualBehavior)
        {
            case CutSceneCameraBehavior.MOVE_BETWEEN_KEYS:
                if (GenericSensUtilities.instance.DistanceBetween2Vectors(cameraPivot.transform.position, actualScene.sceneKeyPoints[actualKeyPointID].transform.position) > 0.1f)
                {
                    Debug.Log("MovingCamera");
                    MoveCamera(origin, actualKeyPoint.transform.position, actualKeyPoint.speed);
                }
                else
                {
                    ChangeBehavior(CutSceneCameraBehavior.CHANGE_KEY);
                }
                break;
            case CutSceneCameraBehavior.CHANGE_KEY:
                if (actualScene.sceneKeyPoints[actualKeyPointID].endKey)
                {
                    ChangeBehavior(CutSceneCameraBehavior.CHANGE_SCENE);
                }
                else
                {
                    if (actualKeyPointID < actualScene.sceneKeyPoints.Length - 1)
                    {
                        actualKeyPointID++;
                        actualKeyPoint = actualScene.sceneKeyPoints[actualKeyPointID];
                    }
                    ChangeBehavior(CutSceneCameraBehavior.MOVE_BETWEEN_KEYS);
                }
                break;
            case CutSceneCameraBehavior.CHANGE_SCENE:
                ChangeScene();
                break;
        }
    }
    private void ChangeScene()
    {
        if (actualSceneID < scenes.Length - 1)
        {
            actualScene = scenes[actualSceneID + 1];
            actualSceneID++;
        }
        else
        {
            actualSceneID = 0;
            actualScene = scenes[actualSceneID];
        }

        actualKeyPointID = 0;
    }
    private void ChangeBehavior(CutSceneCameraBehavior newBehavior)
    {
        switch (actualBehavior)
        {
            case CutSceneCameraBehavior.MOVE_BETWEEN_KEYS:
                break;
            case CutSceneCameraBehavior.CHANGE_SCENE:
                break;
        }
        switch (newBehavior)
        {
            case CutSceneCameraBehavior.MOVE_BETWEEN_KEYS:
                origin = this.transform.position;
                break;
            case CutSceneCameraBehavior.CHANGE_SCENE:
                if (actualSceneID == scenes.Length - 1)
                {
                    actualSceneID = -1;
                }
                cameraPivot.transform.position = actualScene.sceneKeyPoints[0].transform.position;
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
}
