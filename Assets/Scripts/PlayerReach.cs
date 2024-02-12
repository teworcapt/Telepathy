using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerReach : MonoBehaviour
{
    public float reachDistance = 20f;

    public bool GetRaycastHit(out RaycastHit hitInfo)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.Raycast(ray, out hitInfo, reachDistance);
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            Debug.Log("Object hit: " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * reachDistance, Color.green);
        }
    }

}
