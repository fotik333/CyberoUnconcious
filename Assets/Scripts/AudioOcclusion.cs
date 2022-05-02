using System.Collections;
using System.Collections.Generic;
using FMODUnityResonance;
using UnityEngine;

public class AudioOcclusion : MonoBehaviour
{
    private FMOD.Studio.EventInstance _instance;

    [FMODUnity.EventRef]
    public string fmodEvent;
    
    [SerializeField]
    private bool occlusionEnabled = false;
    
    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private string occlusionParameterName = null;

    [Range(0.0f, 10.0f)] [SerializeField]
    private float occlusionIntensity = 1f;

    private float _currentOcclusion = 0.0f;

    private float _nextOcclusionUpdate = 0.0f;

    private void Start()
    {
        _instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        _instance.start();
    }

    private void Update()
    {
        if (!_instance.isValid()) return;
        
        _instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        if (!occlusionEnabled)
        {
            _currentOcclusion = 0.0f;
        }
        // else if (Time.time >= _nextOcclusionUpdate)
        // {
        //     _nextOcclusionUpdate = Time.time + FmodResonanceAudio.occlusionDetectionInterval;
            _currentOcclusion = occlusionIntensity * FmodResonanceAudio.ComputeOcclusion(transform);

            _instance.getParameterByName(occlusionParameterName, out var occ);
            _instance.setParameterByName(occlusionParameterName, Mathf.Lerp(occ, _currentOcclusion, Time.deltaTime * speed));
        // }
    }
}
