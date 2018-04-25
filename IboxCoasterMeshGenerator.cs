using UnityEngine;
using System;
using System.IO;

public class IboxCoasterMeshGenerator : MeshGenerator
{
    private const float buildVolumeHeight = 0.8f;

    private ShapeExtruder leftRailExtruder;

    private ShapeExtruder rightRailExtruder;

    private ShapeExtruder metalRearCrossTieExtruder;

    private ShapeExtruder metalFrontCrossTieExtruder;

    private BoxExtruder woodenVerticalSupportPostExtruder;

    private BoxExtruder collisionMeshExtruder;

    private StreamWriter streamWriter;

    private float errorMargin90deg = 0.001f;

    private float supportBeamExtension = 0.2f;

    public string path;

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
        metalRearCrossTieExtruder = new ShapeExtruder();
        metalRearCrossTieExtruder.setShape(new Vector3[8]
        {
            new Vector3(-0.046f, 0.04f, 0f),
            new Vector3(-0.022f, 0.04f, 0f),
            new Vector3(-0.022f, -0.04f, 0f),
            new Vector3(-0.046f, -0.04f, 0f),
            new Vector3(-0.046f, -0.035f, 0f),
            new Vector3(-0.03f, -0.035f, 0f),
            new Vector3(-0.03f, 0.035f, 0f),
            new Vector3(-0.046f, 0.035f, 0f)
        }, true);
        metalRearCrossTieExtruder.setUV(15, 14);
        metalRearCrossTieExtruder.closeEnds = true;
        metalFrontCrossTieExtruder = new ShapeExtruder();
        metalFrontCrossTieExtruder.setShape(new Vector3[8]
        {
            new Vector3(0.022f, 0.04f, 0f),
            new Vector3(0.046f, 0.04f, 0f),
            new Vector3(0.046f, 0.035f, 0f),
            new Vector3(0.03f, 0.035f, 0f),
            new Vector3(0.03f, -0.035f, 0f),
            new Vector3(0.046f, -0.035f, 0f),
            new Vector3(0.046f, -0.04f, 0f),
            new Vector3(0.022f, -0.04f, 0f)
        }, true);
        metalFrontCrossTieExtruder.setUV(15, 14);
        metalFrontCrossTieExtruder.closeEnds = true;
        collisionMeshExtruder = new BoxExtruder(trackWidth, 0.02666f);
        buildVolumeMeshExtruder = new BoxExtruder(trackWidth, 0.8f);
        buildVolumeMeshExtruder.closeEnds = true;
        woodenVerticalSupportPostExtruder = new BoxExtruder(0.043f, 0.043f);
        woodenVerticalSupportPostExtruder.closeEnds = true;
        woodenVerticalSupportPostExtruder.setUV(14, 14);
        base.setModelExtruders(leftRailExtruder, rightRailExtruder, metalRearCrossTieExtruder, metalFrontCrossTieExtruder, woodenVerticalSupportPostExtruder);
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

            //track beam
            Vector3 startPoint = trackPivot + normal * 0.159107f + binormal * .49f;
            Vector3 endPoint = trackPivot + normal * 0.159107f - binormal * .49f;

            bool equalHeight = Mathf.Abs(startPoint.y - endPoint.y) < 0.97f;
            //bool equalHeight = (normal.y < 0.01f && normal.y > -0.01f) || ((startPoint.y - endPoint.y) < 0.97 && (startPoint.y - endPoint.y) > -0.97f);

            metalCrossTieExtrude(startPoint, -1f * binormal, equalHeight ? Vector3.up : normal);
            metalCrossTieExtrude(endPoint, -1f * binormal, equalHeight ? Vector3.up : normal);
            metalCrossTieEnd();


            if (!(trackSegment is Station))
            {
                //Bottom beam calculation
                Vector3 bottomBeamPivot = new Vector3(trackPivot.x, Mathf.Min(startPoint.y, endPoint.y), trackPivot.z);
                Vector3 bottomBeamDirection = startPoint - endPoint;
                bottomBeamDirection.y = 0.0f;
                WriteToFile("bottomBeamDirection: (" + bottomBeamDirection.x + "," + bottomBeamDirection.y + "," + bottomBeamDirection.z + ")");

                if (bottomBeamDirection.magnitude < errorMargin90deg)
                {
                  bottomBeamDirection = new Vector3(normal.x, 0, normal.z);
                  WriteToFile("Normal: (" + normal.x + "," + normal.y + "," + normal.z + ")");
                }
                bottomBeamDirection.Normalize();

                Vector3 bottomBeamStart = new Vector3();
                Vector3 bottomBeamEnd = new Vector3();
                Vector3 bottomBeamBinormal = bottomBeamDirection.normalized;

                if ((normal.x > errorMargin90deg && normal.y < errorMargin90deg) || (normal.x < errorMargin90deg && normal.y > errorMargin90deg))
                {
                    bottomBeamStart.x = endPoint.x;
                    bottomBeamStart.z = endPoint.z;
                    bottomBeamStart.y = endPoint.y > startPoint.y ? startPoint.y : endPoint.y;

                    bottomBeamEnd = bottomBeamStart + bottomBeamDirection.normalized * 0.98f;
                } else
                {
                    bottomBeamEnd.x = startPoint.x;
                    bottomBeamEnd.z = startPoint.z;
                    bottomBeamEnd.y = endPoint.y > startPoint.y ? startPoint.y : endPoint.y;

                    bottomBeamStart = bottomBeamEnd - bottomBeamDirection.normalized * 0.98f;
                }

                if (normal.y > errorMargin90deg)
                {
                    bottomBeamStart.y -= normal.normalized.y * 1f;
                    bottomBeamEnd.y -= normal.normalized.y * 1f;
                }

                //Bottom beam extrudingright
                metalCrossTieExtrude(bottomBeamEnd, -1f * bottomBeamBinormal, Vector3.up);
                metalCrossTieExtrude(bottomBeamStart, -1f * bottomBeamBinormal, Vector3.up);
                metalCrossTieEnd();


                //Top beam extruding
                if (normal.y > errorMargin90deg)
                {
                    metalCrossTieExtrude(new Vector3(bottomBeamEnd.x, Mathf.Max(startPoint.y, endPoint.y), bottomBeamEnd.z), -1f * bottomBeamBinormal, Vector3.up);
                    metalCrossTieExtrude(new Vector3(bottomBeamStart.x, Mathf.Max(startPoint.y, endPoint.y), bottomBeamStart.z), -1f * bottomBeamBinormal, Vector3.up);
                    metalCrossTieEnd();
                }
                LandPatch terrain = GameController.Instance.park.getTerrain(trackPivot);

                if (terrain != null)
                {
                    float lowest = terrain.getLowestHeight();

                    Vector3 projectedTangentDirection = tangentPoint;
                    projectedTangentDirection.y = 0;
                    projectedTangentDirection.Normalize();
                    Vector3 rightVerticalSupportPost = new Vector3(bottomBeamStart.x, endPoint.y + supportBeamExtension, bottomBeamStart.z);
                    Vector3 leftVerticalSupportPost = new Vector3(bottomBeamEnd.x, startPoint.y + supportBeamExtension, bottomBeamEnd.z);

                    if(normal.y > errorMargin90deg)
                    {
                        leftVerticalSupportPost.y = Mathf.Max(startPoint.y, endPoint.y) + supportBeamExtension;
                        rightVerticalSupportPost.y = Mathf.Max(startPoint.y, endPoint.y) + supportBeamExtension;
                    }

                    //left post
                    woodenVerticalSupportPostExtruder.extrude(leftVerticalSupportPost, new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.extrude(new Vector3(leftVerticalSupportPost.x, lowest, leftVerticalSupportPost.z), new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.end();

                    //right post
                    woodenVerticalSupportPostExtruder.extrude(rightVerticalSupportPost, new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.extrude(new Vector3(rightVerticalSupportPost.x, lowest, rightVerticalSupportPost.z), new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.end();
                }

            }
        }
    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return MeshCombiner.start().add(leftRailExtruder, rightRailExtruder, metalRearCrossTieExtruder, metalFrontCrossTieExtruder, woodenVerticalSupportPostExtruder).end(putMeshOnGO.transform.worldToLocalMatrix);
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
    public void metalCrossTieExtrude(Vector3 middlePoint, Vector3 tangent, Vector3 normal)
    {
        metalRearCrossTieExtruder.extrude(middlePoint, tangent, normal);
        metalFrontCrossTieExtruder.extrude(middlePoint, tangent, normal);
    }
    public void metalCrossTieEnd()
    { 
        metalRearCrossTieExtruder.end();
        metalFrontCrossTieExtruder.end();
    }
    public void WriteToFile(string text)
    {
        streamWriter = File.AppendText(path + @"/mod.log");
        streamWriter.WriteLine(DateTime.Now + ": " + text);
        streamWriter.Flush();
        streamWriter.Close();
    }
}
