using UnityEngine;

public class HybridCoasterSupportInstantiator : SupportInstantiator
{
    public override void instantiateSupports(MeshGenerator meshGenerator, TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        SupportConfiguration supportConfiguration = trackSegment.track.TrackedRide.supportConfiguration;
        if ((Object)supportConfiguration != (Object)null)
        {
            for (int j = 0; j < sideOffsets.Length; j++)
            {
                SupportLocation supportLocation = Object.Instantiate(supportConfiguration.supportLocationGO);
                supportLocation.trackSegment = trackSegment;

                supportLocation.t = 0f;
                supportLocation.sideOffset = sideOffsets[j];
                supportLocation.rebuild();
            }
        }
    }
}
