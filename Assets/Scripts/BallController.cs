using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BallController : MonoBehaviour
{
    [SerializeField]
    private float timeToMax = 2;

    [SerializeField]
    private float maxForce = 5;

    [SerializeField]
    private SpriteRenderer arrowRenderer = null;

    [SerializeField]
    private Transform arrowPivot = null, theArrow = null, shortArrow = null, longArrow = null;

    [SerializeField]
    private Camera mainCamera = null;

    [SerializeField, HideInInspector]
    private Rigidbody ballRB = null;

    private float timeHeld = 0;

    private Vector3 aimDirection = Vector3.forward;

    private void OnEnable()
    {
        if (null == ballRB)
            ballRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        arrowPivot.position = transform.position;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData))
        {
            Vector3 mouseWorldPos = hitData.point;
            mouseWorldPos = new Vector3(mouseWorldPos.x, transform.position.y, mouseWorldPos.z);

            aimDirection = mouseWorldPos - transform.position;

            arrowPivot.transform.rotation = Quaternion.LookRotation(aimDirection);
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            arrowRenderer.enabled = true;

            timeHeld += Time.deltaTime;

            if (timeHeld > timeToMax)
                timeHeld = timeToMax;

            var heldRatio = timeHeld / timeToMax;

            theArrow.position = Vector3.Lerp(shortArrow.position, longArrow.position, heldRatio);
            theArrow.localScale = Vector3.Lerp(shortArrow.localScale, longArrow.localScale, heldRatio);
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            ballRB.AddForce(aimDirection * (timeHeld / timeToMax * maxForce), ForceMode.Impulse);
            timeHeld = 0;
            arrowRenderer.enabled = false;
        }
    }
}