using Gasimo.Streamer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamerSampleMemory : MonoBehaviour
{

    public StreamerArea area;

    private void Awake()
    {
        StreamerCore.Instance.LoadAreaWithDependencies(area, 1);
    }

}
