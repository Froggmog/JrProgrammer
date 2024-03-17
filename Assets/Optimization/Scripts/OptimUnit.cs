using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

public class OptimUnit : MonoBehaviour
{
    private Vector3 currentVelocity;
    private float timeToVelocityChange;
    private float currentAngularVelocity;
    private float timeToAngularVelocityChange;

    private Vector3 areaSize;

    public void SetAreaSize(Vector3 size)
    {
        areaSize = size;
    }

    private void Start()
    {
        PickNewVelocity();
        PickNewVelocityChangeTime();
        PickNewAngularVelocity();
        PickNewAngularVelocityChangeTime();

        StartCoroutine(BoundaryCheck(1));
    }

    // Update is called once per frame
    void Update()
    {
        Profiler.BeginSample("HandleTime");
        HandleTime();
        Profiler.EndSample();

        Profiler.BeginSample("Rotating");
        var t = transform;


        transform.Rotate(-currentAngularVelocity * (transform.position.x / math.abs(transform.position.x)) * Time.deltaTime, 0, 0);

        transform.Rotate(0, 0, currentAngularVelocity * (transform.position.z / math.abs(transform.position.z)) * Time.deltaTime);

        Profiler.EndSample();

        Profiler.BeginSample("Moving");
        Move();
        Profiler.EndSample();


    }


    private void PickNewVelocity()
    {
        currentVelocity = Random.insideUnitSphere;
        currentVelocity.y = 0;
        currentVelocity *= 10.0f;
    }

    private IEnumerator BoundaryCheck( float waitTime){

        while(true){

            yield return new WaitForSeconds(waitTime);
            
        Profiler.BeginSample("Boundary Check");

        //check if we are moving away from the zone and invert velocity if this is the case
        if (transform.position.x > areaSize.x && currentVelocity.x > 0)
        {
            currentVelocity.x *= -1;
            PickNewVelocityChangeTime(); //we pick a new change time as we changed velocity
        }
        else if (transform.position.x < -areaSize.x && currentVelocity.x < 0)
        {
            currentVelocity.x *= -1;
            PickNewVelocityChangeTime();
        }

        if (transform.position.z > areaSize.z && currentVelocity.z > 0)
        {
            currentVelocity.z *= -1;
            PickNewVelocityChangeTime(); //we pick a new change time as we changed velocity
        }
        else if (transform.position.z < -areaSize.z && currentVelocity.z < 0)
        {
            currentVelocity.z *= -1;
            PickNewVelocityChangeTime();
        }
        Profiler.EndSample();
        }

    }

    private void PickNewAngularVelocity()
    {
        currentAngularVelocity = Random.Range(-180.0f, 180.0f);
    }

    private void PickNewVelocityChangeTime()
    {
        timeToVelocityChange = Random.Range(2.0f, 5.0f);
    }

    private void PickNewAngularVelocityChangeTime()
    {
        timeToAngularVelocityChange = Random.Range(2.0f, 5.0f);
    }

    void Move()
    {
        transform.position = transform.position + currentVelocity * Time.deltaTime;
    }

    private void HandleTime()
    {
        timeToVelocityChange -= Time.deltaTime;
        if (timeToVelocityChange < 0)
        {
            PickNewVelocity();
            PickNewVelocityChangeTime();
        }

        timeToAngularVelocityChange -= Time.deltaTime;
        if (timeToAngularVelocityChange < 0)
        {
            PickNewAngularVelocity();
            PickNewAngularVelocityChangeTime();
        }
    }
}
