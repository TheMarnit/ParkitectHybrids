using UnityEngine;

public class IboxCoasterMeshGenerator : MeshGenerator
{
    private const float buildVolumeHeight = 0.8f;

    private ShapeExtruder leftRailExtruder;

    private ShapeExtruder rightRailExtruder;

    private ShapeExtruder crossTieExtruder_left;

    private ShapeExtruder crossTieExtruder_right;

    private BoxExtruder collisionMeshExtruder;

    protected override void Initialize()
    {
        base.Initialize();
        trackWidth = 0.41f;
    }

    public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.prepare(trackSegment, putMeshOnGO);
        putMeshOnGO.GetComponent<Renderer>().sharedMaterial = material;
        leftRailExtruder = new ShapeExtruder();
        leftRailExtruder.setShape(new Vector3[14]
        {
            new Vector3(0.046103f, 0f, 0f),
            new Vector3(0.048103f, -0.002f, 0f),
            new Vector3(0.048103f, -0.026f, 0f),
            new Vector3(0.046103f, -0.028f, 0f),
            new Vector3(0.021286f, -0.028f, 0f),
            new Vector3(0.021286f, -0.1144f, 0f),
            new Vector3(0.048772f, -0.1144f, 0f),
            new Vector3(0.048772f, -0.119108f, 0f),
            new Vector3(-0.054515f, -0.119108f, 0f),
            new Vector3(-0.054515f, -0.1144f, 0f),
            new Vector3(-0.032771f, -0.1144f, 0f),
            new Vector3(-0.032771f, -0.0065f, 0f),
            new Vector3(-0.054515f, -0.0065f, 0f),
            new Vector3(-0.054515f, 0f, 0f)
        }, true);
        leftRailExtruder.setUV(15, 15);
        rightRailExtruder = new ShapeExtruder();
        rightRailExtruder.setShape(new Vector3[14]
        {
            new Vector3(0.054515f, 0f, 0f),
            new Vector3(0.054515f, -0.0065f, 0f),
            new Vector3(0.032771f, -0.0065f, 0f),
            new Vector3(0.032771f, -0.1144f, 0f),
            new Vector3(0.054515f, -0.1144f, 0f),
            new Vector3(0.054515f, -0.119108f, 0f),
            new Vector3(-0.048772f, -0.119108f, 0f),
            new Vector3(-0.048772f, -0.1144f, 0f),
            new Vector3(-0.021286f, -0.1144f, 0f),
            new Vector3(-0.021286f, -0.028f, 0f),
            new Vector3(-0.046103f, -0.028f, 0f),
            new Vector3(-0.048103f, -0.026f, 0f),
            new Vector3(-0.048103f, -0.002f, 0f),
            new Vector3(-0.046103f, 0f, 0f)
        }, true);
        rightRailExtruder.setUV(14, 15);
        crossTieExtruder_left = new ShapeExtruder();
        crossTieExtruder_left.setShape(new Vector3[8]
        {
            new Vector3(-0.046f, 0f, 0f),
            new Vector3(-0.022f, 0f, 0f),
            new Vector3(-0.022f, -0.08f, 0f),
            new Vector3(-0.046f, -0.08f, 0f),
            new Vector3(-0.046f, -0.075f, 0f),
            new Vector3(-0.03f, -0.075f, 0f),
            new Vector3(-0.03f, -0.005f, 0f),
            new Vector3(-0.046f, -0.005f, 0f)
        }, true);
        crossTieExtruder_left.setUV(15, 14);
        crossTieExtruder_left.closeEnds = true;
        crossTieExtruder_right = new ShapeExtruder();
        crossTieExtruder_right.setShape(new Vector3[8]
        {
            new Vector3(0.022f, 0f, 0f),
            new Vector3(0.046f, 0f, 0f),
            new Vector3(0.046f, -0.005f, 0f),
            new Vector3(0.03f, -0.005f, 0f),
            new Vector3(0.03f, -0.075f, 0f),
            new Vector3(0.046f, -0.075f, 0f),
            new Vector3(0.046f, -0.08f, 0f),
            new Vector3(0.022f, -0.08f, 0f)
        }, true);
        crossTieExtruder_right.setUV(15, 14);
        crossTieExtruder_right.closeEnds = true;
        collisionMeshExtruder = new BoxExtruder(trackWidth, 0.02666f);
        buildVolumeMeshExtruder = new BoxExtruder(trackWidth, 0.8f);
        buildVolumeMeshExtruder.closeEnds = true;
        base.setModelExtruders(leftRailExtruder, rightRailExtruder, crossTieExtruder_left, crossTieExtruder_right);
    }

    public override void sampleAt(TrackSegment4 trackSegment, float t)
    {
        base.sampleAt(trackSegment, t);
        Vector3 normal = trackSegment.getNormal(t);
        Vector3 trackPivot = getTrackPivot(trackSegment.getPoint(t), normal);
        Vector3 tangentPoint = trackSegment.getTangentPoint(t);
        Vector3 normalized = Vector3.Cross(normal, tangentPoint).normalized;
        Vector3 middlePoint = trackPivot + normalized * trackWidth / 2f;
        Vector3 middlePoint2 = trackPivot - normalized * trackWidth / 2f;
        Vector3 vector = trackPivot + normal * getCenterPointOffsetY();
        leftRailExtruder.extrude(middlePoint, tangentPoint, normal);
        rightRailExtruder.extrude(middlePoint2, tangentPoint, normal);
        collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
        if (liftExtruder != null)
        {
            liftExtruder.extrude(vector - normal * (0.16f + chainLiftHeight / 2f), tangentPoint, normal);
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);
        float sample = trackSegment.getLength(0) / (float)Mathf.RoundToInt(trackSegment.getLength(0) / this.crossBeamSpacing);
        float pos = 0.25f;
        int index = 0;
        while (pos < trackSegment.getLength(0))
        {
            float tForDistance = trackSegment.getTForDistance(pos, 0);

            index++;
            pos += sample;

            Vector3 normal = trackSegment.getNormal(tForDistance);
            Vector3 tangentPoint = trackSegment.getTangentPoint(tForDistance);
            Vector3 binormal = Vector3.Cross(normal, tangentPoint).normalized;
            Vector3 trackPivot = base.getTrackPivot(trackSegment.getPoint(tForDistance, 0), normal);
            Vector3 binormalFlat = Vector3.Cross(Vector3.up, tangentPoint).normalized;

            crossTieExtruder_left.extrude(trackPivot + normal * 0.119107f + binormal * .49f, -1f * binormal, normal);
            crossTieExtruder_left.extrude(trackPivot + normal * 0.119107f - binormal * .49f, -1f * binormal, normal);
            crossTieExtruder_left.end();
            crossTieExtruder_right.extrude(trackPivot + normal * 0.119107f + binormal * .49f, -1f * binormal, normal);
            crossTieExtruder_right.extrude(trackPivot + normal * 0.119107f - binormal * .49f, -1f * binormal, normal);
            crossTieExtruder_right.end();

        }
    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return MeshCombiner.start().add(leftRailExtruder, rightRailExtruder, crossTieExtruder_left, crossTieExtruder_right).end(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Mesh getCollisionMesh(GameObject putMeshOnGO)
    {
        return collisionMeshExtruder.getMesh(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Extruder getBuildVolumeMeshExtruder()
    {
        return buildVolumeMeshExtruder;
    }

    public override float getCenterPointOffsetY()
    {
        return 0.27f;
    }

    public override float trackOffsetY()
    {
        return 0.24f;
    }

    public override float getTunnelOffsetY()
    {
        return 0.15f;
    }

    public override float getTunnelWidth()
    {
        return 0.8f;
    }

    public override float getTunnelHeight()
    {
        return 0.8f;
    }
    public override float getFrictionWheelOffsetY()
    {
        return 0.115f;
    }
    protected override float railHalfHeight()
    {
        return 0.02666f;
    }
}