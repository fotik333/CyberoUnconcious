using System;
using System.Collections;
using System.Collections.Generic;
using MisterGames.Common.Routines;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Lights
{
    [Serializable]
    public struct Params
    {
        [Header("Light settings")]
        public float minLightIntensity;
        public Vector2 maxLightIntensityRange;
    
        [Header("Emissive object settings")]
        public float minEmissionIntensity;
        public Vector2 maxEmissionIntensityRange;
    
        [Header("Common settings")]
        public Vector2 timeRange;
        [Tooltip("Delay between animations")]
        public float maxDelayBase;
    }

    [ExecuteInEditMode]
    public class WavyLight : AnimatedLightWithMeshBase
    {
        public AnimationCurve curve;
        public Params animationConfig;
        public TimeDomain Domain;

        public override IEnumerator Animate()
        {
            Debug.Log("ANIMATE");
            var timer = 0f;
            var maxIntensity = GetRandomMaxIntensity();
            var maxEmissionIntensity = GetRandomMaxEmissionIntensity();
            var time = GetRandomAnimationTime() / Multiplier;
            Debug.Log(time);

            while (timer < time)
            {
                var value = curve.Evaluate(timer / time);
                Light.intensity = (maxIntensity - animationConfig.minLightIntensity) * value + animationConfig.minLightIntensity;

                if (emissiveObject)
                {
                    var emission = (maxEmissionIntensity - animationConfig.minEmissionIntensity) * value + animationConfig.minEmissionIntensity;
                    emissiveObject.material.SetFloat("EmissionIntensityOverride", emission);
                }

                Debug.Log(timer);
                timer += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            yield return new WaitForSeconds(Random.value * animationConfig.maxDelayBase * DelayMultiplier);
        }

        private float GetRandomMaxIntensity()
        {
            return Random.value * (animationConfig.maxLightIntensityRange[1] - animationConfig.maxLightIntensityRange[0]) + animationConfig.maxLightIntensityRange[0];
        }

        private float GetRandomMaxEmissionIntensity()
        {
            return Random.value * (animationConfig.maxEmissionIntensityRange[1] - animationConfig.maxEmissionIntensityRange[0]) + animationConfig.maxEmissionIntensityRange[0];
        }

        private float GetRandomAnimationTime()
        {
            return Random.value * (animationConfig.timeRange[1] - animationConfig.timeRange[0]) + animationConfig.timeRange[0];
        }
    }
}