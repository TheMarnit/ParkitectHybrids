using UnityEngine;

public class HybridCoasterSupportInstantiator : SupportInstantiator
{
public override void instantiateSupports (MeshGenerator meshGenerator, TrackSegment4 trackSegment, GameObject putMeshOnGO)
{
    multipleSupportsPerSegment = true;
    distanceBetweenSupports = 0.5f;
    int num = 1;
    num = Mathf.Max (num, Mathf.RoundToInt (trackSegment.getLength (0) / distanceBetweenSupports));

    float num2 = trackSegment.getLength (0) / (float)num;
    float num3 = num2;
    for (int i = 0; i < num; i++) {
        float tForDistance = trackSegment.getTForDistance (num3, 0);
        num3 += num2;
        SupportConfiguration supportConfiguration = trackSegment.track.TrackedRide.supportConfiguration;
        if ((Object)supportConfiguration != (Object)null) {
            for (int j = 0; j < sideOffsets.Length; j++) {
                SupportLocation supportLocation = Object.Instantiate (supportConfiguration.supportLocationGO);
                supportLocation.trackSegment = trackSegment;

                supportLocation.t = tForDistance;
                supportLocation.sideOffset = sideOffsets [j];
                supportLocation.rebuild ();
            }
        }
    }
}
}
