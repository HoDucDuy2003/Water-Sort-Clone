using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public BottleData bottleData { get; private set; }
    public BottleColorController bottleColorController { get; private set; }
    public FillAndRotationValues fillAndRotationValues { get; private set; }




    [Header("Bottle Sprite Renderer")] 
    public SpriteRenderer BottleMaskSR;


    public AnimationCurve ScaleAndRotationMultiplierCurve;
    public AnimationCurve FillAmountCurve;
    public AnimationCurve RotationSpeedMultiplier;

    [SerializeField] private float timeToRotate;

    public float[] fillAmounts;
    public float[] rotationValues;

    private int rotationIndex = 0;

    private void Awake()
    {
        bottleData = GetComponent<BottleData>();
        bottleColorController = GetComponent<BottleColorController>();

        fillAndRotationValues =FillAndRotationValues.Instance;
    }
    void Start()
    {
        bottleColorController.SetFillAmount(fillAmounts[bottleData.numberOfColorsInBottle]);
        bottleColorController.UpdateColorsOnShader(bottleData);
        bottleColorController.UpdateTopColorValues(bottleData);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(RotateBottle(timeToRotate));
        }
    }


    IEnumerator RotateBottle(float _timeToRotate)
    {
        float t = 0;
        float lerpValue;
        float angleValue;

        while(t < _timeToRotate)
        {
            lerpValue = t / _timeToRotate;
            angleValue = Mathf.Lerp(0.0f, 90.0f, lerpValue);

            transform.eulerAngles = new Vector3(0, 0, angleValue);

            bottleColorController.SetSARM(ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
            if (fillAmounts[bottleData.numberOfColorsInBottle] > FillAmountCurve.Evaluate(angleValue))
            {
                BottleMaskSR.material.SetFloat(ShaderValueNames.FillAmount, FillAmountCurve.Evaluate(angleValue));
            }


            t += Time.deltaTime * RotationSpeedMultiplier.Evaluate(angleValue);
            

            yield return new WaitForEndOfFrame();
        }

        angleValue = 90.0f;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleColorController.SetSARM(ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
        BottleMaskSR.material.SetFloat(ShaderValueNames.FillAmount, FillAmountCurve.Evaluate(angleValue));


        StartCoroutine(RotateBottleBack(_timeToRotate));
    }

    IEnumerator RotateBottleBack(float _timeToRotate)
    {
        float t = 0;
        float lerpValue;
        float angleValue;

        while (t < _timeToRotate)
        {
            lerpValue = t / _timeToRotate;
            angleValue = Mathf.Lerp(90.0f, 0.0f, lerpValue);

            transform.eulerAngles = new Vector3(0, 0, angleValue);
            bottleColorController.SetSARM(ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
      
            t += Time.deltaTime;


            yield return new WaitForEndOfFrame();
        }

        angleValue = 0.0f;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleColorController.SetSARM(ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
        BottleMaskSR.material.SetFloat(ShaderValueNames.FillAmount, FillAmountCurve.Evaluate(angleValue));
    }

}
