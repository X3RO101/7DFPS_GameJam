using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [SerializeField] private PlayerInfo playerInfo;

    [Header("Bobbing")]
    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float frequency = 10f;

    [SerializeField] private Transform cam = null;
    [SerializeField] private Transform camHolder = null;

    private float toggleSpeed = 3.0f;
    private Vector3 startPos;
    [SerializeField] private CharacterController controller;

    [Header("Leaning")]
    [SerializeField] private Transform leanPivot;
    [SerializeField] private float leanAngle;
    [SerializeField] private float leanSmoothing;
    private float currentLean;
    private float targetLean;

    // Start is called before the first frame update
    void Start()
    {
        startPos = cam.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateLeaning();
        CheckMotion();
        ResetPosition();
        cam.LookAt(FocusTarget());
    }

    private void PlayMotion(Vector3 motion)
    {
        cam.localPosition += motion;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        if (speed < toggleSpeed) return;
        if (!controller.isGrounded) return;

        PlayMotion(SwayMotion());
    }

    private Vector3 SwayMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency * 0.5f) * amplitude * 2f;
        return pos;
    }

    private void ResetPosition()
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1f * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + camHolder.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15.0f;
        return pos;
    }

    private void CalculateLeaning()
    {
        if (Input.GetAxis("Horizontal") > 0.25f)
            targetLean = -leanAngle;
        else if (Input.GetAxis("Horizontal") < -0.25f)
            targetLean = leanAngle;
        else
            targetLean = 0;


        currentLean = Mathf.Lerp(currentLean, targetLean, leanSmoothing);
        leanPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, currentLean));
    }
}
