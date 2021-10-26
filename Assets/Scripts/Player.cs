using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;
    List<Vector3> setPosition = new List<Vector3>();
    LineRenderer componentLineRenderer;
    void Update()
    {
        if (setPosition.Count > 0)
        {
            transform.position = new Vector3(AnimParameter(transform.position.x, setPosition[setPosition.Count - 1].x, speed), 0, AnimParameter(transform.position.z, setPosition[setPosition.Count - 1].z, speed));
            if (transform.position == setPosition[setPosition.Count - 1])
            {
                setPosition.RemoveAt(setPosition.Count - 1);
                if (componentLineRenderer != null)
                    componentLineRenderer.positionCount = setPosition.Count+1;
            }
        }
    }

    public void SetPosition(List<Vector3> finishPosition, LineRenderer lineRenderer)
    {
        componentLineRenderer = lineRenderer;
        setPosition = finishPosition;
    }

    public static float AnimParameter(float nowPar, float setPar, float speed)
    {
        if (nowPar > setPar)
        {
            nowPar -= speed * Time.deltaTime;
            if (nowPar < setPar)
                nowPar = setPar;
        }

        if (nowPar < setPar)
        {
            nowPar += speed * Time.deltaTime;
            if (nowPar > setPar)
                nowPar = setPar;
        }

        return nowPar;
    }
}
