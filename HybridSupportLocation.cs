using System;
using UnityEngine;

public class HybridSupportLocation : SupportLocation
{
    protected override Support instantiateSupport(Support supportGO, MeshGenerator meshGenerator, TrackSegment4 trackSegment, float t, Vector3 point, Vector3 normal, Vector3 tangent, Vector3 left, Vector3 centerTubeMiddle)
    {
        SupportHybridCoaster supportTrackedRide = (SupportHybridCoaster)base.instantiateSupport(supportGO, meshGenerator, trackSegment, t, point, normal, tangent, left, centerTubeMiddle);
        supportTrackedRide.trackSegment = trackSegment;
        return supportTrackedRide;
    }
}
