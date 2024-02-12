using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Rigidbody currentlyDraggedRigidbody;
    private Vector3 cursorOffset;
    private int originalLayer;

    [Header("Force Settings")]
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float throwForce;
    public RectTransform cursor;

    private void Update()
    {
        PlayerReach playerReach = GetComponent<PlayerReach>();

        if (playerReach != null)
        {
            RaycastHit hit;
            if (playerReach.GetRaycastHit(out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Rigidbody hitRigidbody = hit.collider.GetComponent<Rigidbody>();
                    if (hitRigidbody != null)
                    {
                        isDragging = true;
                        currentlyDraggedRigidbody = hitRigidbody;
                        originalLayer = currentlyDraggedRigidbody.gameObject.layer;

                        int tempLayer = LayerMask.NameToLayer("TempLayer");
                        currentlyDraggedRigidbody.gameObject.layer = tempLayer;

                        cursorOffset = currentlyDraggedRigidbody.transform.position - hit.point;

                        currentlyDraggedRigidbody.isKinematic = true;
                    }
                }
            }
        }

        if (isDragging && currentlyDraggedRigidbody != null)
        {
            Vector3 cursorScreenPosition = cursor.position;
            cursorScreenPosition.z = Camera.main.nearClipPlane;
            Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);

            Vector3 targetPosition = cursorWorldPosition + cursorOffset;

            MoveWithCollisions(targetPosition);

            if (Input.GetMouseButtonDown(1))
            {
                currentlyDraggedRigidbody.gameObject.layer = originalLayer;

                isDragging = false;
                currentlyDraggedRigidbody.isKinematic = false;
                currentlyDraggedRigidbody = null;
            }
        }

        if (Input.GetMouseButtonUp(0) && currentlyDraggedRigidbody != null)
        {
            ThrowObject();
        }
    }

    private void MoveWithCollisions(Vector3 targetPosition)
    {
        currentlyDraggedRigidbody.MovePosition(Vector3.Lerp(currentlyDraggedRigidbody.transform.position, targetPosition, smoothSpeed * Time.deltaTime));
    }

    private void ThrowObject()
    {
        currentlyDraggedRigidbody.gameObject.layer = originalLayer;
        isDragging = false;
        currentlyDraggedRigidbody.isKinematic = false;


        Vector3 forcetoAdd = transform.forward * throwForce;

        currentlyDraggedRigidbody.AddForce(forcetoAdd, ForceMode.Impulse);
        currentlyDraggedRigidbody = null;
    }
}

