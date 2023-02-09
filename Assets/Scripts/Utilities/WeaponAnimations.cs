using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimations : MonoBehaviour
{
    [SerializeField] Transform Model;
    [SerializeField] Transform MagModel;
    [SerializeField] Transform FullModel;
    [SerializeField] Transform SwayTarget;

    bool snap;

    private float t;

    private bool isIn;
    private bool isOut;

    private Vector3 currentRotation = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    private Vector3 targetRotation = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;

    private Vector3 currentImpulse = Vector3.zero;

    private Quaternion quat1;

    private Gun gun;
    private void Start()
    {
        gun = GetComponent<Gun>();
        SwayTarget.localPosition = new Vector3(0f, 0f, 2f);
    }
    public void Recoil(float recoilx, float recoily, float recoilz, float recoilSlide)
    {
        currentImpulse = new Vector3(-recoilx,recoily,recoilz);
        targetRotation += currentImpulse;
        targetPosition += recoilSlide * 0.1f * -Model.forward;
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snap ? gun.GetSnappiness(): 12f * Time.deltaTime);
        currentPosition = Vector3.Slerp(currentPosition, targetPosition, snap ? gun.GetSnappiness() : 12f * Time.deltaTime);
        Model.localRotation = Quaternion.Euler(currentRotation) * quat1;
        Model.localPosition = currentPosition;
        if (!snap) snap = true;
    }
    public void ADS(Vector3 newPos, Vector3 newRot)
    {
        targetRotation = Vector3.Lerp(targetRotation, newRot, gun.GetAdsSpeed() * Time.deltaTime);
        targetPosition = Vector3.Lerp(targetPosition, newPos, gun.GetAdsSpeed() * Time.deltaTime);
    }
    public void IdleSway(float idlex, float idley, float idleSwaySpeed)
    {
        t += idleSwaySpeed * gun.GetConcentration() * Time.deltaTime * 0.1f;
        transform.forward = (SwayTarget.transform.position - transform.position).normalized;
        SwayTarget.transform.localPosition = new Vector3(-Mathf.Sin(14 * Mathf.PI * t) * idlex, Mathf.Cos(21 * Mathf.PI * t)* idley, 1f);
    }
    public void LookSway(float smooth, float swayMultiplier)
    {
        quat1 = Quaternion.Slerp(quat1, (Quaternion.AngleAxis(-Input.GetAxisRaw("Mouse Y") * swayMultiplier, Vector3.right) * Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * swayMultiplier, Vector3.up)), smooth * Time.deltaTime);
    }
    public Vector3 GetCurrentRotation()
    {
        return currentImpulse;
    }
}
