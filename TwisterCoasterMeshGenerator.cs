using UnityEngine;

public class TwisterCoasterMeshGenerator : MeshGenerator
{
    private const float buildVolumeHeight = 0.8f;

    private ShapeExtruder leftRailExtruder;

    private ShapeExtruder rightRailExtruder;

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
        collisionMeshExtruder = new BoxExtruder(trackWidth, 0.02666f);
        buildVolumeMeshExtruder = new BoxExtruder(trackWidth, 0.8f);
        buildVolumeMeshExtruder.closeEnds = true;
        base.setModelExtruders(leftRailExtruder, rightRailExtruder);
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
            liftExtruder.extrude(vector - normal * (0.06713f + chainLiftHeight / 2f), tangentPoint, normal);
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);
    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return MeshCombiner.start().add(leftRailExtruder, rightRailExtruder).end(putMeshOnGO.transform.worldToLocalMatrix);
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
        return 0.4f;
    }

    public override float getTunnelWidth()
    {
        return 0.6f;
    }

    public override float getTunnelHeight()
    {
        return 1f;
    }
    public override float getFrictionWheelOffsetY()
    {
        return 0.15f;
    }
    protected override float railHalfHeight()
    {
        return 0.02666f;
    }
}