﻿using BottleCodes.Animation;
using UnityEngine;

namespace BottleCodes
{
    [RequireComponent(typeof(BottleData))]
    [RequireComponent(typeof(BottleColorController))]
    [RequireComponent(typeof(BottleAnimationController))]
    [RequireComponent(typeof(BottleTransferController))]
    [RequireComponent(typeof(BottleAnimationSpeedUp))]
    [RequireComponent(typeof(BottleSpriteRendererOrderController))]
    [RequireComponent(typeof(BottleFindRotationPointAndDirection))]
    public class BottleController : MonoBehaviour
    {
        public BottleData BottleData { get; private set; }
        public BottleColorController BottleColorController { get; private set; }
        public BottleAnimationController BottleAnimationController { get; private set; }
        public BottleTransferController BottleTransferController { get; private set; }
        public BottleAnimationSpeedUp BottleAnimationSpeedUp { get; private set; }
        public FillAndRotationValues FillAndRotationValues { get; private set; }
        public BottleSpriteRendererOrderController BottleSpriteRendererOrderController { get; private set; }
        public SpriteRenderer BottleSpriteRenderer { get; private set; }
        public BottleFindRotationPointAndDirection BottleFindRotationPointAndDirection { get; private set; }


        [Header("Bottle Sprite Renderer")] public SpriteRenderer BottleMaskSR;


        // Game manager
        private GameManager _gm;


        [Header("Bottle Helper")] [SerializeField]
        public Bottle HelperBottle;

        private void Awake()
        {
            BottleData = GetComponent<BottleData>();
            BottleColorController = GetComponent<BottleColorController>();
            BottleAnimationController = GetComponent<BottleAnimationController>();
            BottleTransferController = GetComponent<BottleTransferController>();
            BottleAnimationSpeedUp = GetComponent<BottleAnimationSpeedUp>();
            BottleSpriteRendererOrderController = GetComponent<BottleSpriteRendererOrderController>();
            BottleFindRotationPointAndDirection = GetComponent<BottleFindRotationPointAndDirection>();
            BottleSpriteRenderer = GetComponent<SpriteRenderer>();

            FillAndRotationValues = FillAndRotationValues.Instance;
        }

        private void Start()
        {
            _gm = GameManager.Instance;
            BottleMaskSR.material = _gm.Mat;

            BottleColorController.SetFillAmount(FillAndRotationValues.GetFillCurrentAmount(BottleData));
            BottleColorController.UpdateColorsOnShader(BottleData);
            BottleColorController.UpdateTopColorValues(BottleData);
        }

        public void UpdateAfterUndo()
        {
            BottleColorController.SetFillAmount(FillAndRotationValues.GetFillCurrentAmount(BottleData));
            BottleColorController.UpdateColorsOnShader(BottleData);
            BottleData.TopColor = BottleData.PreviousTopColor;
            BottleColorController.UpdateTopColorValues(BottleData);
        }

        public bool IsBottleEmpty()
        {
            return BottleData.NumberOfColorsInBottle <= 0;
        }

        public void StartColorTransfer()
        {
            AddActionBottleToActionBottleList();

            // chose rotation point and direction
            BottleFindRotationPointAndDirection.ChoseRotationPointAndDirection(BottleTransferController
                .BottleControllerRef);

            var bottleControllerRef = BottleTransferController.BottleControllerRef;
            var bottleRefData = bottleControllerRef.BottleData;

            // get how many water color will pour
            BottleTransferController.NumberOfColorsToTransfer = Mathf.Min(BottleData.NumberOfTopColorLayers,
                4 - bottleRefData.NumberOfColorsInBottle);

            // setting array color values to pouring water color
            for (var i = 0; i < BottleTransferController.NumberOfColorsToTransfer; i++)
                bottleRefData.BottleColors[bottleRefData.NumberOfColorsInBottle + i] = BottleData.TopColor;

            // updating colors on shader
            bottleControllerRef.BottleColorController.UpdateColorsOnShader(bottleRefData);

            // setting render order
            BottleSpriteRendererOrderController.SetSortingOrder(BottleSpriteRenderer, BottleMaskSR);

            // call move bottle
            StartAnimationChain();

            // call pre rotate bottle
            PreRotateBottle();
        }

        private void AddActionBottleToActionBottleList()
        {
            _gm.InActionBottleList.Add(this);
        }

        private void StartAnimationChain()
        {
            BottleAnimationController.DisableCollider();
            BottleFindRotationPointAndDirection.ChoseMovePosition(BottleTransferController);

            BottleAnimationController.StartAnimationChain();
        }

        private void PreRotateBottle()
        {
            BottleAnimationController.PlayPreRotateTween();
        }

        public void SetOriginalPositionTo(Vector3 position)
        {
            BottleAnimationController.SetOriginalPosition(position);
        }
    }
}