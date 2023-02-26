using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField] private GameObject Holder;
    [SerializeField] private GameObject Model;
    [SerializeField] private Transform SwayTarget;
    [SerializeField] private PlayerCam cam;

    private float t;

    private Vector3 currentRotation = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    private Vector3 targetRotation = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;

    private Quaternion quat1;
    public void Recoil(float recoilx, float recoily, float recoilz, float recoilSlide, float snappiness, float camFactor)
    {
        targetRotation += new Vector3(-recoilx, recoily, recoilz);
        targetPosition += recoilSlide * 0.1f * Vector3.forward;
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        currentPosition = Vector3.Slerp(currentPosition, targetPosition, snappiness * Time.deltaTime);
        Model.transform.localRotation = Quaternion.Euler(currentRotation) * quat1;
        Model.transform.localPosition = currentPosition;
        cam.CamRecoil(currentRotation * camFactor);
    }
    public void ADS(Vector3 newPos, Vector3 newRot, float adsSpeed)
    {
        targetRotation = Vector3.Lerp(targetRotation, newRot, adsSpeed * Time.deltaTime);
        targetPosition = Vector3.Lerp(targetPosition, newPos, adsSpeed * Time.deltaTime);
    }
    public void IdleSway(float idlex, float idley, float idleSwaySpeed, float concentration)
    {
        t += idleSwaySpeed * concentration * Time.deltaTime * 0.1f;
        SwayTarget.transform.localPosition = new Vector3(-Mathf.Sin(14 * Mathf.PI * t) * idlex, Mathf.Cos(21 * Mathf.PI * t) * idley, 1f);
    }
    public void PointAtSway(float snappiness)
    {
        transform.forward = Vector3.Lerp(transform.forward, (SwayTarget.position - transform.position).normalized, snappiness * Time.deltaTime);
    }
    public void LookSway(float smooth, float swayMultiplier)
    {
        quat1 = Quaternion.Slerp(quat1, (Quaternion.AngleAxis(-Input.GetAxisRaw("Mouse Y") * swayMultiplier, Vector3.right) * Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * swayMultiplier, Vector3.up)), smooth * Time.deltaTime);
    }
    public void SwapWeap(float swapSpeed,Vector3 start, Vector3 end)
    {
        Holder.transform.localRotation = Quaternion.Euler(Vector3.Lerp(start, end, swapSpeed * Time.deltaTime));
        //if (Holder.transform.localRotation.eulerAngles.x > 50f) gameObject.SetActive(false);
    }
}
