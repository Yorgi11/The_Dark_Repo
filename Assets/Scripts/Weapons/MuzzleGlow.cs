using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleGlow : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Material basemat;
    [SerializeField] private MeshRenderer muzzle;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color glowColor;
    private Color currentColor;
    private float currIntensity = 0f;
    private float goalIntensity = 0f;
    private float rate = 4f;
    private int numshots = 0;
    private void Awake()
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", baseColor * 0f);
        muzzle.material = basemat;
    }
    void Update()
    {
        if (numshots > 0)
        {
            // sets the material of the muzzle object to the bloom material
            muzzle.material = mat;
            goalIntensity = numshots;
            currIntensity = Mathf.Lerp(currIntensity, goalIntensity, rate * Time.deltaTime);
            currentColor = Color.Lerp(currentColor, glowColor, rate * Time.deltaTime);
        }
        else
        {
            currIntensity = Mathf.Lerp(currIntensity, 0f, 1f * Time.deltaTime);
            currentColor = Color.Lerp(currentColor, baseColor, 2f * Time.deltaTime);
            // if the current color is within 0.1 of the desired base color then the material of the object is given the same material as the rest of the gun barrel
            if (currentColor.r < baseColor.r + 0.1f && currentColor.g < baseColor.g + 0.1f && currentColor.b < baseColor.b + 0.1f) muzzle.material = basemat;
        }
        // updates the color and intensity of the bloom material accordingly
        mat.SetColor("_EmissionColor", currentColor * currIntensity);
    }
    public void SetNumShots(int s)
    {
        numshots = s;
    }
}
