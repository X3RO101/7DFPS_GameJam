using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDSway : MonoBehaviour
{
    [SerializeField] private float maxSwaySpeed = 5f;                //how fast it can sway
    [SerializeField] private float swaySmoothAmount = 1f;            //how smooth will the sway be
    [SerializeField] private float leanStrength;
    [SerializeField] private float leanSmoothing;
    private float currentLean;
    private float targetLean;

    private Vector3 defaultPosition = Vector3.zero; //the default position the UI belongs to
    private Vector2 impact = Vector2.zero;          //the force added onto the UI to provide "swaying"

    private void Start()
    {
        defaultPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        UpdateImpact(); //Update and ove HUD accordingly

        //Adding of force from calculating speed of swaying the mouse
        float moveX = Input.GetAxis("Mouse X") * swaySmoothAmount;
        float moveY = Input.GetAxis("Mouse Y") * swaySmoothAmount;

        moveX = Mathf.Clamp(moveX, -maxSwaySpeed, maxSwaySpeed) + impact.x;
        moveY = Mathf.Clamp(moveY, -maxSwaySpeed, maxSwaySpeed) + impact.y;

        if (Input.GetAxis("Horizontal") > 0.1f)
            targetLean = -leanStrength;
        else if (Input.GetAxis("Horizontal") < -0.1f)
            targetLean = leanStrength;
        else
            targetLean = 0;
        currentLean = Mathf.Lerp(currentLean, targetLean, leanSmoothing);

        Vector3 newPos = new Vector3(-moveX - currentLean, -moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos + defaultPosition, Time.unscaledDeltaTime * swaySmoothAmount);
    }

    private void UpdateImpact()
    {
        impact = Vector2.Lerp(impact, Vector2.zero, 5f);
    }

    public void AddImpact(Vector2 dir, float force)
    {
        impact = dir * force;
    }
}
