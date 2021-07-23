using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [ExecuteInEditMode]
    public class CanvasScaleController : MonoBehaviour
    {
#pragma warning disable
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private float keyAspectRatio;
        [Range(0, 1)] [SerializeField] private float matchNarrowScreen;
        [Range(0, 1)] [SerializeField] private float matchWideScreen;
        [SerializeField] private Camera mCamera;
        [SerializeField] private List<AspectAndWide> spikeAspects;

#pragma warning restore

        private void Awake()
        {
            ApplyMatch();
        }

#if UNITY_EDITOR
        private void Update()
        {
            ApplyMatch();
        }
#endif

        public void ReinitSpike()
        {
            if (spikeAspects == null || spikeAspects.Count == 0)
            {
                spikeAspects = new List<AspectAndWide>
                {
                    new AspectAndWide
                    {
                        aspect = 2.285f,
                        wideScreen = 0.83f
                    },
                    new AspectAndWide
                    {
                        aspect = 2.225f,
                        wideScreen = 0.91f
                    }
                };
            }
        }

        private void ApplyMatch()
        {
            if (mCamera == null)
            {
                return;
            }

            ReinitSpike();
            
            var fObj = spikeAspects.Find(obj => Math.Abs(obj.aspect - mCamera.aspect) < 0.005);
            if (fObj != null)
            {
                canvasScaler.matchWidthOrHeight = fObj.wideScreen;
            }
            else
            {
                canvasScaler.matchWidthOrHeight =
                    mCamera.aspect <= keyAspectRatio ? matchNarrowScreen : matchWideScreen;
            }
        }
    }

    [Serializable]
    public class AspectAndWide
    {
        public float aspect;
        public float wideScreen;
    }
}