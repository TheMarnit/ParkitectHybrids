using UnityEngine;
using System;
using System.IO;
using TrackedRiderUtility;
using System.Collections.Generic;
using HybridCoasters;

public class HybridCoasterMeshGenerator : MeshGenerator
{
    private const float buildVolumeHeight = 0.8f;

    private float beamSpacing = 0.5f;

    private float tieSpacing = 0.2f;

    private ShapeExtruder iboxLeftRailExtruder;

    private ShapeExtruder iboxRightRailExtruder;

    private ShapeExtruder topperLeftRailExtruder;

    private ShapeExtruder topperRightRailExtruder;

    private BoxExtruder topperLeftPlankExtruder_1;

    private BoxExtruder topperLeftPlankExtruder_2;

    private BoxExtruder topperLeftPlankExtruder_3;

    private BoxExtruder topperLeftPlankExtruder_4;

    private BoxExtruder topperLeftPlankExtruder_5;

    private BoxExtruder topperLeftPlankExtruder_6;

    private BoxExtruder topperRightPlankExtruder_1;

    private BoxExtruder topperRightPlankExtruder_2;

    private BoxExtruder topperRightPlankExtruder_3;

    private BoxExtruder topperRightPlankExtruder_4;

    private BoxExtruder topperRightPlankExtruder_5;

    private BoxExtruder topperRightPlankExtruder_6;

    private ShapeExtruder metalFrontCrossTieExtruder_1;

    private ShapeExtruder metalFrontCrossTieExtruder_2;

    private ShapeExtruder metalFrontCrossTieExtruder_3;

    private ShapeExtruder metalRearCrossTieExtruder_1;

    private ShapeExtruder metalRearCrossTieExtruder_2;

    private ShapeExtruder metalRearCrossTieExtruder_3;

    private ShapeExtruder metalIBeamExtruder_1;

    private ShapeExtruder metalIBeamExtruder_2;

    private ShapeExtruder metalIBeamExtruder_3;

    private ShapeExtruder metalTopperCrossTie_1;

    private ShapeExtruder metalTopperCrossTie_2;

    private ShapeExtruder metalTopperCrossTie_3;

    private ShapeExtruder metalTopperCrossTie_4;

    private ShapeExtruder metalTopperCrossTie_5;

    private ShapeExtruder metalTopperCrossTie_6;

    private ShapeExtruder metalTopperCrossTie_7;

    private ShapeExtruder metalTopperCrossTie_8;

    private BoxExtruder woodenVerticalSupportPostExtruder;

    private BoxExtruder collisionMeshExtruder;

    private BoxExtruder customBuildVolumeMeshExtruder;

    private StreamWriter streamWriter;

    public Material metalMaterial;

    public float supportBeamExtension = 0.2f;

    private float beamWidth = 0.98f;

    private float invertHeadSpace = 1f;

    private float iBeamBankingSwitch = 30.0f;

    public bool useTopperTrack = false;

    public string path;

    public Dictionary<Vector3, List<SupportPosition>> supportPosts = new Dictionary<Vector3, List<SupportPosition>>();

    protected override void Initialize()
    {
        base.Initialize();
        trackWidth = 0.427692f;
    }

    public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.prepare(trackSegment, putMeshOnGO);
        putMeshOnGO.GetComponent<Renderer>().sharedMaterial = material;
        topperLeftRailExtruder = new ShapeExtruder();
        topperLeftRailExtruder.setShape(new Vector3[8]
            {
            new Vector3 (0.053846f, -0.002f, 0f),
            new Vector3 (0.053846f, -0.026f, 0f),
            new Vector3 (0.051846f, -0.028f, 0f),
            new Vector3 (-0.027029f, -0.028f, 0f),
            new Vector3 (-0.027029f, -0.0065f, 0f),
            new Vector3 (-0.048772f, -0.0065f, 0f),
            new Vector3 (-0.048772f, 0f, 0f),
            new Vector3 (0.051845f, 0f, 0f)
            }, true);
        topperLeftRailExtruder.setUV(15, 15);
        topperRightRailExtruder = new ShapeExtruder();
        topperRightRailExtruder.setShape(new Vector3[8]
            {
            new Vector3 (0.048772f, -0.0065f, 0f),
            new Vector3 (0.027029f, -0.0065f, 0f),
            new Vector3 (0.027028f, -0.028f, 0f),
            new Vector3 (-0.051846f, -0.028f, 0f),
            new Vector3 (-0.053845f, -0.026f, 0f),
            new Vector3 (-0.053846f, -0.002f, 0f),
            new Vector3 (-0.051845f, 0f, 0f),
            new Vector3 (0.048772f, 0f, 0f)
            }, true);
        topperRightRailExtruder.setUV(14, 15);
        topperLeftPlankExtruder_1 = new BoxExtruder(0.054058f, 0.013286f);
        topperLeftPlankExtruder_1.setUV(14, 14);
        topperLeftPlankExtruder_2 = new BoxExtruder(0.054058f, 0.013286f);
        topperLeftPlankExtruder_2.setUV(14, 14);
        topperLeftPlankExtruder_3 = new BoxExtruder(0.054058f, 0.013286f);
        topperLeftPlankExtruder_3.setUV(14, 14);
        topperLeftPlankExtruder_4 = new BoxExtruder(0.054058f, 0.013286f);
        topperLeftPlankExtruder_4.setUV(14, 14);
        topperLeftPlankExtruder_5 = new BoxExtruder(0.054058f, 0.013286f);
        topperLeftPlankExtruder_5.setUV(14, 14);
        topperLeftPlankExtruder_6 = new BoxExtruder(0.054058f, 0.013286f);
        topperLeftPlankExtruder_6.setUV(14, 14);
        topperRightPlankExtruder_1 = new BoxExtruder(0.054058f, 0.013286f);
        topperRightPlankExtruder_1.setUV(14, 14);
        topperRightPlankExtruder_2 = new BoxExtruder(0.054058f, 0.013286f);
        topperRightPlankExtruder_2.setUV(14, 14);
        topperRightPlankExtruder_3 = new BoxExtruder(0.054058f, 0.013286f);
        topperRightPlankExtruder_3.setUV(14, 14);
        topperRightPlankExtruder_4 = new BoxExtruder(0.054058f, 0.013286f);
        topperRightPlankExtruder_4.setUV(14, 14);
        topperRightPlankExtruder_5 = new BoxExtruder(0.054058f, 0.013286f);
        topperRightPlankExtruder_5.setUV(14, 14);
        topperRightPlankExtruder_6 = new BoxExtruder(0.054058f, 0.013286f);
        topperRightPlankExtruder_6.setUV(14, 14);
        iboxLeftRailExtruder = new ShapeExtruder();
        iboxLeftRailExtruder.setShape(new Vector3[14]
            {
            new Vector3 (0.046103f, 0f, 0f),
            new Vector3 (0.048103f, -0.002f, 0f),
            new Vector3 (0.048103f, -0.026f, 0f),
            new Vector3 (0.046103f, -0.028f, 0f),
            new Vector3 (0.021286f, -0.028f, 0f),
            new Vector3 (0.021286f, -0.1144f, 0f),
            new Vector3 (0.048772f, -0.1144f, 0f),
            new Vector3 (0.048772f, -0.119108f, 0f),
            new Vector3 (-0.054515f, -0.119108f, 0f),
            new Vector3 (-0.054515f, -0.1144f, 0f),
            new Vector3 (-0.032771f, -0.1144f, 0f),
            new Vector3 (-0.032771f, -0.0065f, 0f),
            new Vector3 (-0.054515f, -0.0065f, 0f),
            new Vector3 (-0.054515f, 0f, 0f)
            }, true);
        iboxLeftRailExtruder.setUV(15, 15);
        iboxRightRailExtruder = new ShapeExtruder();
        iboxRightRailExtruder.setShape(new Vector3[14]
            {
            new Vector3 (0.054515f, 0f, 0f),
            new Vector3 (0.054515f, -0.0065f, 0f),
            new Vector3 (0.032771f, -0.0065f, 0f),
            new Vector3 (0.032771f, -0.1144f, 0f),
            new Vector3 (0.054515f, -0.1144f, 0f),
            new Vector3 (0.054515f, -0.119108f, 0f),
            new Vector3 (-0.048772f, -0.119108f, 0f),
            new Vector3 (-0.048772f, -0.1144f, 0f),
            new Vector3 (-0.021286f, -0.1144f, 0f),
            new Vector3 (-0.021286f, -0.028f, 0f),
            new Vector3 (-0.046103f, -0.028f, 0f),
            new Vector3 (-0.048103f, -0.026f, 0f),
            new Vector3 (-0.048103f, -0.002f, 0f),
            new Vector3 (-0.046103f, 0f, 0f)
            }, true);
        iboxRightRailExtruder.setUV(14, 15);
        metalFrontCrossTieExtruder_1 = new ShapeExtruder();
        metalFrontCrossTieExtruder_1.setShape(new Vector3[4]
            {
            new Vector3 (0.022f, 0.04f, 0f),
            new Vector3 (0.046f, 0.04f, 0f),
            new Vector3 (0.046f, 0.035f, 0f),
            new Vector3 (0.03f, 0.035f, 0f)
            }, true);
        metalFrontCrossTieExtruder_1.setUV(15, 14);
        metalFrontCrossTieExtruder_1.closeEnds = true;
        metalFrontCrossTieExtruder_2 = new ShapeExtruder();
        metalFrontCrossTieExtruder_2.setShape(new Vector3[4]
            {
            new Vector3 (0.03f, 0.035f, 0f),
            new Vector3 (0.03f, -0.035f, 0f),
            new Vector3 (0.022f, -0.04f, 0f),
            new Vector3 (0.022f, 0.04f, 0f)
            }, true);
        metalFrontCrossTieExtruder_2.setUV(15, 14);
        metalFrontCrossTieExtruder_2.closeEnds = true;
        metalFrontCrossTieExtruder_3 = new ShapeExtruder();
        metalFrontCrossTieExtruder_3.setShape(new Vector3[4]
            {
            new Vector3 (0.03f, -0.035f, 0f),
            new Vector3 (0.046f, -0.035f, 0f),
            new Vector3 (0.046f, -0.04f, 0f),
            new Vector3 (0.022f, -0.04f, 0f)
            }, true);
        metalFrontCrossTieExtruder_3.setUV(15, 14);
        metalFrontCrossTieExtruder_3.closeEnds = true;
        metalRearCrossTieExtruder_1 = new ShapeExtruder();
        metalRearCrossTieExtruder_1.setShape(new Vector3[4]
            {
            new Vector3 (-0.046f, 0.04f, 0f),
            new Vector3 (-0.022f, 0.04f, 0f),
            new Vector3 (-0.03f, 0.035f, 0f),
            new Vector3 (-0.046f, 0.035f, 0f)
            }, true);
        metalRearCrossTieExtruder_1.setUV(15, 14);
        metalRearCrossTieExtruder_1.closeEnds = true;
        metalRearCrossTieExtruder_2 = new ShapeExtruder();
        metalRearCrossTieExtruder_2.setShape(new Vector3[4]
            {
            new Vector3 (-0.046f, -0.035f, 0f),
            new Vector3 (-0.03f, -0.035f, 0f),
            new Vector3 (-0.022f, -0.04f, 0f),
            new Vector3 (-0.046f, -0.04f, 0f)
            }, true);
        metalRearCrossTieExtruder_2.setUV(15, 14);
        metalRearCrossTieExtruder_2.closeEnds = true;
        metalRearCrossTieExtruder_3 = new ShapeExtruder();
        metalRearCrossTieExtruder_3.setShape(new Vector3[4]
            {
            new Vector3 (-0.022f, 0.04f, 0f),
            new Vector3 (-0.022f, -0.04f, 0f),
            new Vector3 (-0.03f, -0.035f, 0f),
            new Vector3 (-0.03f, 0.035f, 0f)
            }, true);
        metalRearCrossTieExtruder_3.setUV(15, 14);
        metalRearCrossTieExtruder_3.closeEnds = true;
        metalIBeamExtruder_1 = new ShapeExtruder();
        metalIBeamExtruder_1.setShape(new Vector3[4]
            {
            new Vector3 (-0.021f, 0.04f, 0f),
            new Vector3 (0.021f, 0.04f, 0f),
            new Vector3 (0.021f, 0.035f, 0f),
            new Vector3 (-0.021f, 0.035f, 0f)
            }, true);
        metalIBeamExtruder_1.setUV(15, 14);
        metalIBeamExtruder_1.closeEnds = true;
        metalIBeamExtruder_2 = new ShapeExtruder();
        metalIBeamExtruder_2.setShape(new Vector3[4]
            {
            new Vector3 (-0.005f, 0.037f, 0f),
            new Vector3 (0.005f, 0.037f, 0f),
            new Vector3 (0.005f, -0.037f, 0f),
            new Vector3 (-0.005f, -0.037f, 0f)
            }, true);
        metalIBeamExtruder_2.setUV(15, 14);
        metalIBeamExtruder_2.closeEnds = true;
        metalIBeamExtruder_3 = new ShapeExtruder();
        metalIBeamExtruder_3.setShape(new Vector3[4]
            {
            new Vector3 (-0.021f, -0.035f, 0f),
            new Vector3 (0.021f, -0.035f, 0f),
            new Vector3 (0.021f, -0.04f, 0f),
            new Vector3 (-0.021f, -0.04f, 0f)
            }, true);
        metalIBeamExtruder_3.setUV(15, 14);
        metalIBeamExtruder_3.closeEnds = true;

        metalTopperCrossTie_1 = new ShapeExtruder();
        metalTopperCrossTie_1.setShape(new Vector3[4]
            {
            new Vector3 (-0.030167f, -0.12228f, 0f),
            new Vector3 (-0.002167f, -0.12228f, 0f),
            new Vector3 (-0.000001f, -0.120113f, 0f),
            new Vector3 (-0.030167f, -0.120113f, 0f)
            }, true);
        metalTopperCrossTie_1.setUV(15, 14);
        metalTopperCrossTie_1.closeEnds = true;
        metalTopperCrossTie_2 = new ShapeExtruder();
        metalTopperCrossTie_2.setShape(new Vector3[4]
            {
            new Vector3 (-0.002167f, -0.12228f, 0f),
            new Vector3 (-0.002167f, -0.150113f, 0f),
            new Vector3 (-0.000001f, -0.150113f, 0f),
            new Vector3 (-0.000001f, -0.120113f, 0f)
            }, true);
        metalTopperCrossTie_2.setUV(15, 14);
        metalTopperCrossTie_2.closeEnds = true;
        metalTopperCrossTie_3 = new ShapeExtruder();
        metalTopperCrossTie_3.setShape(new Vector3[4]
            {
            new Vector3 (0.000001f, 0.290799f, 0f),
            new Vector3 (0.030166f, 0.290799f, 0f),
            new Vector3 (0.030166f, 0.288799f, 0f),
            new Vector3 (0.002166f, 0.288799f, 0f)
            }, true);
        metalTopperCrossTie_3.setUV(15, 14);
        metalTopperCrossTie_3.closeEnds = true;
        metalTopperCrossTie_4 = new ShapeExtruder();
        metalTopperCrossTie_4.setShape(new Vector3[4]
            {
            new Vector3 (0.000001f, 0.290799f, 0f),
            new Vector3 (0.002166f, 0.288799f, 0f),
            new Vector3 (0.002166f, 0.250466f, 0f),
            new Vector3 (0.000001f, 0.250466f, 0f)
            }, true);
        metalTopperCrossTie_4.setUV(15, 14);
        metalTopperCrossTie_4.closeEnds = true;
        metalTopperCrossTie_5 = new ShapeExtruder();
        metalTopperCrossTie_5.setShape(new Vector3[4]
            {
            new Vector3 (0f, -0.250868f, 0f),
            new Vector3 (0.002166f, -0.250868f, 0f),
            new Vector3 (0.002166f, -0.289201f, 0f),
            new Vector3 (0f, -0.291201f, 0f)
            }, true);
        metalTopperCrossTie_5.setUV(15, 14);
        metalTopperCrossTie_5.closeEnds = true;
        metalTopperCrossTie_6 = new ShapeExtruder();
        metalTopperCrossTie_6.setShape(new Vector3[4]
            {
            new Vector3 (0.002166f, -0.289201f, 0f),
            new Vector3 (0.030166f, -0.289201f, 0f),
            new Vector3 (0.030166f, -0.291201f, 0f),
            new Vector3 (0f, -0.291201f, 0f)
            }, true);
        metalTopperCrossTie_6.setUV(15, 14);
        metalTopperCrossTie_6.closeEnds = true;
        metalTopperCrossTie_7 = new ShapeExtruder();
        metalTopperCrossTie_7.setShape(new Vector3[5]
            {
            new Vector3 (-0.275188f, -0.006497f, 0f),
            new Vector3 (-0.275188f, -0.042196f, 0f),
            new Vector3 (-0.250868f, -0.042196f, 0f),
            new Vector3 (-0.241076f, -0.02686f, 0f),
            new Vector3 (-0.241076f, -0.006497f, 0f)
            }, true);
        metalTopperCrossTie_7.setUV(14, 15);
        metalTopperCrossTie_7.closeEnds = true;
        metalTopperCrossTie_8 = new ShapeExtruder();
        metalTopperCrossTie_8.setShape(new Vector3[5]
            {
            new Vector3 (-0.275188f, -0.006497f, 0f),
            new Vector3 (-0.275188f, -0.042196f, 0f),
            new Vector3 (-0.250868f, -0.042196f, 0f),
            new Vector3 (-0.241076f, -0.02686f, 0f),
            new Vector3 (-0.241076f, -0.006497f, 0f)
            }, true);
        metalTopperCrossTie_8.setUV(15, 15);
        metalTopperCrossTie_8.closeEnds = true;
        collisionMeshExtruder = new BoxExtruder(trackWidth, 0.02666f);
        buildVolumeMeshExtruder = new BoxExtruder(beamWidth, beamWidth);
        customBuildVolumeMeshExtruder = new BoxExtruder(0.1f, 0.1f);
        customBuildVolumeMeshExtruder.closeEnds = true;
        woodenVerticalSupportPostExtruder = new BoxExtruder(0.043f, 0.043f);
        woodenVerticalSupportPostExtruder.closeEnds = true;
        woodenVerticalSupportPostExtruder.setUV(14, 14);
        base.setModelExtruders(topperLeftPlankExtruder_1, topperLeftPlankExtruder_2, topperLeftPlankExtruder_3, topperLeftPlankExtruder_4, topperLeftPlankExtruder_5, topperLeftPlankExtruder_6, topperRightPlankExtruder_1, topperRightPlankExtruder_2, topperRightPlankExtruder_3, topperRightPlankExtruder_4, topperRightPlankExtruder_5, topperRightPlankExtruder_6, woodenVerticalSupportPostExtruder);
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
        if (useTopperTrack)
        {
            topperLeftRailExtruder.extrude(middlePoint, tangentPoint, normal);
            topperLeftPlankExtruder_1.extrude(middlePoint - (normal * -0.034561f), tangentPoint, normal);
            topperLeftPlankExtruder_2.extrude(middlePoint - (normal * -0.050133f), tangentPoint, normal);
            topperLeftPlankExtruder_3.extrude(middlePoint - (normal * -0.065763f), tangentPoint, normal);
            topperLeftPlankExtruder_4.extrude(middlePoint - (normal * -0.081394f), tangentPoint, normal);
            topperLeftPlankExtruder_5.extrude(middlePoint - (normal * -0.097025f), tangentPoint, normal);
            topperLeftPlankExtruder_6.extrude(middlePoint - (normal * -0.112511f), tangentPoint, normal);
            topperRightRailExtruder.extrude(middlePoint2, tangentPoint, normal);
            topperRightPlankExtruder_1.extrude(middlePoint2 - (normal * -0.034561f), tangentPoint, normal);
            topperRightPlankExtruder_2.extrude(middlePoint2 - (normal * -0.050133f), tangentPoint, normal);
            topperRightPlankExtruder_3.extrude(middlePoint2 - (normal * -0.065763f), tangentPoint, normal);
            topperRightPlankExtruder_4.extrude(middlePoint2 - (normal * -0.081394f), tangentPoint, normal);
            topperRightPlankExtruder_5.extrude(middlePoint2 - (normal * -0.097025f), tangentPoint, normal);
            topperRightPlankExtruder_6.extrude(middlePoint2 - (normal * -0.112511f), tangentPoint, normal);
        }
        else
        {
            iboxLeftRailExtruder.extrude(middlePoint, tangentPoint, normal);
            iboxRightRailExtruder.extrude(middlePoint2, tangentPoint, normal);
        }
        collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
        if (liftExtruder != null)
        {
            liftExtruder.extrude(vector - normal * (0.16f + chainLiftHeight / 2f), tangentPoint, normal);
        }

        //calculating beams
        if (t == 0f)
        {
            supportPosts[trackSegment.getStartpoint()] = new List<SupportPosition>();
            float supportInterval = trackSegment.getLength(0) / (float)Mathf.RoundToInt(trackSegment.getLength(0) / this.beamSpacing);
            float pos = 0;
            while (pos <= trackSegment.getLength(0) + 0.1f)
            {
                float tForDistance = trackSegment.getTForDistance(pos, 0);
                normal = trackSegment.getNormal(tForDistance);
                tangentPoint = trackSegment.getTangentPoint(tForDistance);
                Vector3 binormal = Vector3.Cross(normal, tangentPoint).normalized;
                trackPivot = base.getTrackPivot(trackSegment.getPoint(tForDistance, 0), normal);
                SupportPosition position = new SupportPosition();
                position.topBarVisible = false;
                position.bottomBarVisible = false;
                float trackDirection = Mathf.Repeat(Mathf.Atan2(tangentPoint.x, tangentPoint.z) * Mathf.Rad2Deg, 360.0f);
                trackDirection += 45;
                bool trackFacingXPositive = false;
                bool trackFacingXNegative = false;
                bool trackFacingZPositive = false;
                bool trackFacingZNegative = false;
                if (trackDirection < 90)
                    trackFacingZPositive = true;
                else if (trackDirection < 180)
                    trackFacingXPositive = true;
                else if (trackDirection < 270)
                    trackFacingZNegative = true;
                else
                    trackFacingXNegative = true;

                float trackBanking = 0f;

                Vector3 bottomBeamDirection = new Vector3();

                if (trackFacingXPositive)
                {
                    trackBanking = Mathf.Repeat(Mathf.Atan2(normal.z, -normal.y), Mathf.PI * 2.0f) * Mathf.Rad2Deg;
                    if (trackBanking > 180)
                        trackBanking -= 360;
                    Vector2 tangentProjection = new Vector2(tangentPoint.x, tangentPoint.z);

                    Vector2 normalProjection = Rotate(tangentProjection, 90);
                    bottomBeamDirection.z = normalProjection.y;
                    bottomBeamDirection.x = normalProjection.x;
                    bottomBeamDirection.Normalize();
                    bottomBeamDirection *= Math.Abs(trackBanking) > 90 ? 1.0f : -1.0f;
                }
                if (trackFacingXNegative)
                {
                    trackBanking = Mathf.Repeat(Mathf.Atan2(-normal.z, -normal.y), Mathf.PI * 2.0f) * Mathf.Rad2Deg;
                    if (trackBanking > 180)
                        trackBanking -= 360;

                    bottomBeamDirection.z = tangentPoint.x;
                    bottomBeamDirection.x = tangentPoint.z;

                    Vector2 tangentProjection = new Vector2(tangentPoint.x, tangentPoint.z);

                    Vector2 normalProjection = Rotate(tangentProjection, 90);
                    bottomBeamDirection.z = normalProjection.y;
                    bottomBeamDirection.x = normalProjection.x;
                    bottomBeamDirection.Normalize();
                    bottomBeamDirection *= Math.Abs(trackBanking) > 90 ? 1.0f : -1.0f;
                }
                if (trackFacingZPositive)
                {
                    trackBanking = Mathf.Repeat(Mathf.Atan2(normal.x, -normal.y), Mathf.PI * 2.0f) * Mathf.Rad2Deg;
                    if (trackBanking > 180)
                        trackBanking -= 360;

                    bottomBeamDirection.z = tangentPoint.x;
                    bottomBeamDirection.x = tangentPoint.z;

                    Vector2 tangentProjection = new Vector2(tangentPoint.x, tangentPoint.z);

                    Vector2 normalProjection = Rotate(tangentProjection, 90);
                    bottomBeamDirection.z = normalProjection.y;
                    bottomBeamDirection.x = normalProjection.x;
                    bottomBeamDirection.Normalize();
                    bottomBeamDirection *= Math.Abs(trackBanking) <= 90 ? -1.0f : 1.0f;
                }
                if (trackFacingZNegative)
                {
                    trackBanking = Mathf.Repeat(Mathf.Atan2(-normal.x, -normal.y), Mathf.PI * 2.0f) * Mathf.Rad2Deg;
                    if (trackBanking > 180)
                        trackBanking -= 360;

                    bottomBeamDirection.z = tangentPoint.x;
                    bottomBeamDirection.x = tangentPoint.z;

                    Vector2 tangentProjection = new Vector2(tangentPoint.x, tangentPoint.z);

                    Vector2 normalProjection = Rotate(tangentProjection, 90);
                    bottomBeamDirection.z = normalProjection.y;
                    bottomBeamDirection.x = normalProjection.x;
                    bottomBeamDirection.Normalize();
                    bottomBeamDirection *= Math.Abs(trackBanking) > 90 ? 1.0f : -1.0f;
                }

                position.trackBanking = trackBanking;

                //track beam
                Vector3 startPoint = trackPivot + normal * 0.159107f + binormal * (beamWidth / 2);
                Vector3 endPoint = trackPivot + normal * 0.159107f - binormal * (beamWidth / 2);

                bool equalHeight = Mathf.Abs(startPoint.y - endPoint.y) < 0.97f;

                //Bottom beam calculation
                Vector3 bottomBeamPivot = new Vector3(trackPivot.x, Mathf.Min(startPoint.y, endPoint.y), trackPivot.z);

                Vector3 bottomBeamStart = new Vector3();
                Vector3 bottomBeamEnd = new Vector3();

                Vector3 bottomBeamBinormal = bottomBeamDirection.normalized;

                Vector3 planePosition = new Vector3();
                Vector3 planeSpanVector1 = endPoint - startPoint;
                Vector3 planeSpanVector2 = tangentPoint;
                Vector3 bottomLinePosition = new Vector3();
                Vector3 topLinePosition = new Vector3();
                Vector3 lineSpanVector = bottomBeamDirection;
                bool attachToStartPoint = false;
                if (((trackFacingXNegative || trackFacingXPositive) ? -1.0f : 1.0) * ((Mathf.Abs(trackBanking) <= 90) ? -1.0f : 1.0f) * trackBanking < 0)
                {
                    bottomBeamStart.x = endPoint.x;
                    bottomBeamStart.z = endPoint.z;
                    bottomBeamStart.y = endPoint.y > startPoint.y ? startPoint.y : endPoint.y;

                    bottomBeamEnd = bottomBeamStart - bottomBeamDirection.normalized * beamWidth;

                    planePosition = endPoint;
                    bottomLinePosition = bottomBeamStart;
                    topLinePosition = new Vector3(bottomLinePosition.x, Mathf.Max(startPoint.y, endPoint.y), bottomLinePosition.z);
                    attachToStartPoint = false;
                }
                else
                {
                    bottomBeamEnd.x = startPoint.x;
                    bottomBeamEnd.z = startPoint.z;
                    bottomBeamEnd.y = endPoint.y > startPoint.y ? startPoint.y : endPoint.y;

                    bottomBeamStart = bottomBeamEnd + bottomBeamDirection.normalized * beamWidth;

                    planePosition = startPoint;
                    bottomLinePosition = bottomBeamEnd;
                    topLinePosition = new Vector3(bottomLinePosition.x, Mathf.Max(startPoint.y, endPoint.y), bottomLinePosition.z);
                    attachToStartPoint = true;
                }



                if (Mathf.Abs(trackBanking) > 90)
                {
                    bottomBeamStart.y -= ((Mathf.Abs(trackBanking) / 90) - 1) * invertHeadSpace;
                    bottomBeamEnd.y -= ((Mathf.Abs(trackBanking) / 90) - 1) * invertHeadSpace;
                    position.topBarVisible = true;
                }
                if (Mathf.Abs(trackBanking) > iBeamBankingSwitch)
                {
                    position.bottomBarVisible = true;
                }
                position.bottomBarLeft = bottomBeamStart;
                position.bottomBarRight = bottomBeamEnd;
                position.topBarLeft = new Vector3(bottomBeamEnd.x, Mathf.Max(startPoint.y, endPoint.y), bottomBeamEnd.z);
                position.topBarRight = new Vector3(bottomBeamStart.x, Mathf.Max(startPoint.y, endPoint.y), bottomBeamStart.z);
                position.barsTangent = -1f * bottomBeamBinormal;

                if (Mathf.Abs(trackBanking) < 90)
                {
                    position.topBarLeft.y += (1 - (Mathf.Abs(trackBanking) / 90)) * invertHeadSpace;
                    position.topBarRight.y += (1 - (Mathf.Abs(trackBanking) / 90)) * invertHeadSpace;
                }

                Vector3 projectedTangentDirection = tangentPoint;
                projectedTangentDirection.y = 0;
                projectedTangentDirection.Normalize();



                Vector3 intersectionPoint = new Vector3();

                if (Math.Abs(trackBanking) > 90 && position.topBarVisible)
                {
                    intersectionPoint = IntersectLineAndPlane(planePosition, planeSpanVector1, planeSpanVector2, topLinePosition, lineSpanVector);
                    if (!float.IsNaN(intersectionPoint.x))
                    {
                        if (attachToStartPoint)
                        {
                            endPoint = intersectionPoint;
                        }
                        else
                        {
                            startPoint = intersectionPoint;
                        }
                    }
                }
                else if (Math.Abs(trackBanking) > 0.1)
                {
                    intersectionPoint = IntersectLineAndPlane(planePosition, planeSpanVector1, planeSpanVector2, bottomLinePosition, lineSpanVector);
                }

                if (true)
                {
                    WriteToFile("IntersectionPoint:" + intersectionPoint);
                    WriteToFile("PlanePosition:" + planePosition);
                    WriteToFile("Magnitude:" + (intersectionPoint - planePosition).magnitude);
                    WriteToFile("Difference:" + (intersectionPoint - planePosition));
                    WriteToFile("TrackBanking" + trackBanking);
                    WriteToFile("planeSpanVector1" + planeSpanVector1);
                    WriteToFile("planeSpanVector2" + planeSpanVector2);
                    WriteToFile("topLinePosition" + topLinePosition);
                    WriteToFile("bottomLinePasition" + bottomLinePosition);
                    WriteToFile("lineSpanVector" + lineSpanVector);
                }
                if (!float.IsNaN(intersectionPoint.x) && (intersectionPoint - planePosition).magnitude > 0.5 && (intersectionPoint - planePosition).magnitude < 1.5)
                {
                    if (attachToStartPoint)
                    {
                        endPoint = intersectionPoint;
                    }
                    else
                    {
                        startPoint = intersectionPoint;
                    }
                }


                Vector3 leftVerticalSupportPost = bottomBeamEnd;
                Vector3 rightVerticalSupportPost = bottomBeamStart;

                Vector3 middleOfBeam = (bottomBeamStart + bottomBeamEnd) / 2;
                if (!position.topBarVisible)
                {
                    if (trackBanking < iBeamBankingSwitch && trackBanking > -90f)
                    {
                        leftVerticalSupportPost = ((bottomBeamEnd - middleOfBeam) * 0.8f) + middleOfBeam;
                    }
                    if (trackBanking > -iBeamBankingSwitch && trackBanking < 90f)
                    {
                        rightVerticalSupportPost = ((bottomBeamStart - middleOfBeam) * 0.8f) + middleOfBeam;
                    }
                }
                if (Mathf.Abs(trackBanking) > 90)
                {
                    leftVerticalSupportPost.y = Mathf.Max(startPoint.y, endPoint.y);
                    rightVerticalSupportPost.y = Mathf.Max(startPoint.y, endPoint.y);
                }
                else
                {
                    leftVerticalSupportPost.y = startPoint.y;
                    rightVerticalSupportPost.y = endPoint.y;
                }





                if (pos > 0f)
                {
                    if (Mathf.Abs(trackBanking) <= iBeamBankingSwitch)
                    {
                        float distance = 1 / Mathf.Sin((90 - Mathf.Abs(trackBanking)) * Mathf.Deg2Rad);
                        if (attachToStartPoint)
                        {
                            endPoint = startPoint - ((startPoint - endPoint) * distance);
                        }
                        else
                        {
                            startPoint = endPoint - ((endPoint - startPoint) * distance);
                        }
                    }
                    position.iBeamLeft = startPoint;
                    position.iBeamRight = endPoint;
                    position.iBeamTangent = -1f * binormal;
                    position.iBeamNormal = equalHeight ? Vector3.up : normal;
                }
                position.verticalSupportPostLeft = leftVerticalSupportPost;
                position.verticalSupportPostRight = rightVerticalSupportPost;
                position.verticalSupportPostTangent = projectedTangentDirection;
                supportPosts[trackSegment.getStartpoint()].Add(position);
                customBuildVolumeMeshExtruder.setWidth(Vector3.Distance(position.bottomBarLeft, position.bottomBarRight));
                customBuildVolumeMeshExtruder.setHeight(Vector3.Distance((position.bottomBarLeft + position.bottomBarRight) / 2, (position.topBarLeft + position.topBarRight) / 2) + (Mathf.Abs(position.trackBanking) > 90 ? supportBeamExtension : 0f));
                Vector3 middle = (position.topBarLeft + position.topBarRight + position.bottomBarLeft + position.bottomBarRight) / 4;
                if (pos > 0f)
                {
                    customBuildVolumeMeshExtruder.extrude(new Vector3(middle.x, middle.y + (Mathf.Abs(position.trackBanking) > 90 ? supportBeamExtension / 2 : 0f), middle.z), Vector3.Cross(position.barsTangent, Vector3.up) * (Mathf.Abs(position.trackBanking) > 90 ? 1f : -1f), Vector3.up);
                }
                pos += supportInterval;
            }
            customBuildVolumeMeshExtruder.end();
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);
        float tieInterval = trackSegment.getLength(0) / (float)Mathf.RoundToInt(trackSegment.getLength(0) / this.tieSpacing);
        float pos = 0;

        // Topper Crossties
        while (pos <= trackSegment.getLength(0) + 0.1f)
        {
            float tForDistance = trackSegment.getTForDistance(pos, 0);
            pos += tieInterval;
            Vector3 normal = trackSegment.getNormal(tForDistance);
            Vector3 tangentPoint = trackSegment.getTangentPoint(tForDistance);
            Vector3 binormal = Vector3.Cross(normal, tangentPoint).normalized;
            Vector3 trackPivot = base.getTrackPivot(trackSegment.getPoint(tForDistance, 0), normal);

            metalTopperCrossTie_1.extrude(trackPivot + binormal * .3f, binormal, normal);
            metalTopperCrossTie_1.extrude(trackPivot - binormal * .3f, binormal, normal);
            metalTopperCrossTie_1.end();
            metalTopperCrossTie_2.extrude(trackPivot + binormal * .3f, binormal, normal);
            metalTopperCrossTie_2.extrude(trackPivot - binormal * .3f, binormal, normal);
            metalTopperCrossTie_2.end();
            metalTopperCrossTie_3.extrude(trackPivot - normal * -0.021113f, normal, -1f * binormal);
            metalTopperCrossTie_3.extrude(trackPivot - normal * -0.150113f, normal, -1f * binormal);
            metalTopperCrossTie_3.end();
            metalTopperCrossTie_4.extrude(trackPivot - normal * -0.021113f, normal, -1f * binormal);
            metalTopperCrossTie_4.extrude(trackPivot - normal * -0.150113f, normal, -1f * binormal);
            metalTopperCrossTie_4.end();
            metalTopperCrossTie_5.extrude(trackPivot - normal * -0.021113f, normal, -1f * binormal);
            metalTopperCrossTie_5.extrude(trackPivot - normal * -0.150113f, normal, -1f * binormal);
            metalTopperCrossTie_5.end();
            metalTopperCrossTie_6.extrude(trackPivot - normal * -0.021113f, normal, -1f * binormal);
            metalTopperCrossTie_6.extrude(trackPivot - normal * -0.150113f, normal, -1f * binormal);
            metalTopperCrossTie_6.end();
            metalTopperCrossTie_7.extrude(trackPivot + tangentPoint * -0.00213f, tangentPoint * -1, normal);
            metalTopperCrossTie_7.extrude(trackPivot + tangentPoint * -0.00001f, tangentPoint * -1, normal);
            metalTopperCrossTie_7.end();
            metalTopperCrossTie_8.extrude(trackPivot + tangentPoint * -0.00213f, tangentPoint, normal);
            metalTopperCrossTie_8.extrude(trackPivot + tangentPoint * -0.00001f, tangentPoint, normal);
            metalTopperCrossTie_8.end();
        }

        //Rendering beams
        int i = 0;
        LandPatch terrain = GameController.Instance.park.getTerrain(trackSegment.getStartpoint());
        foreach (SupportPosition position in supportPosts[trackSegment.getStartpoint()])
        {
            if (i > 0)
            {
                if (terrain == null)
                {
                    //left post
                    woodenVerticalSupportPostExtruder.extrude(new Vector3(position.verticalSupportPostLeft.x, position.verticalSupportPostLeft.y + supportBeamExtension, position.verticalSupportPostLeft.z), new Vector3(0, -1, 0), position.verticalSupportPostTangent);
                    woodenVerticalSupportPostExtruder.extrude(position.verticalSupportPostLeft, new Vector3(0, -1, 0), position.verticalSupportPostTangent);
                    woodenVerticalSupportPostExtruder.end();

                    //right post
                    woodenVerticalSupportPostExtruder.extrude(new Vector3(position.verticalSupportPostRight.x, position.verticalSupportPostRight.y + supportBeamExtension, position.verticalSupportPostRight.z), new Vector3(0, -1, 0), position.verticalSupportPostTangent);
                    woodenVerticalSupportPostExtruder.extrude(position.verticalSupportPostRight, new Vector3(0, -1, 0), position.verticalSupportPostTangent);
                    woodenVerticalSupportPostExtruder.end();
                }
                if (!(trackSegment is Station))
                {
                    //bottom beam
                    if (position.bottomBarVisible)
                    {
                        metalCrossTieExtrude(position.bottomBarLeft, position.barsTangent, Vector3.up);
                        metalCrossTieExtrude(position.bottomBarRight, position.barsTangent, Vector3.up);
                        metalCrossTieEnd();
                    }

                    //top beam
                    if (position.topBarVisible)
                    {
                        metalCrossTieExtrude(position.topBarLeft, position.barsTangent, Vector3.up);
                        metalCrossTieExtrude(position.topBarRight, position.barsTangent, Vector3.up);
                        metalCrossTieEnd();
                    }
                }
                //i beam
                if (position.bottomBarVisible)
                {
                    metalIBeamExtrude(position.iBeamLeft, position.iBeamTangent, position.iBeamNormal);
                    metalIBeamExtrude(position.iBeamRight, position.iBeamTangent, position.iBeamNormal);
                    metalIBeamEnd();
                }
                else
                {
                    metalCrossTieExtrude(position.iBeamLeft, position.iBeamTangent, position.iBeamNormal);
                    metalCrossTieExtrude(position.iBeamRight, position.iBeamTangent, position.iBeamNormal);
                    metalCrossTieEnd();
                }
            }
            i++;

        }

        //rendering extruders
        List<ShapeExtruder> metalShapeExtruders = new List<ShapeExtruder>();
        if (useTopperTrack)
        {
            metalShapeExtruders.Add(topperLeftRailExtruder);
            metalShapeExtruders.Add(topperRightRailExtruder);

            metalShapeExtruders.Add(metalTopperCrossTie_1);
            metalShapeExtruders.Add(metalTopperCrossTie_2);
            metalShapeExtruders.Add(metalTopperCrossTie_3);
            metalShapeExtruders.Add(metalTopperCrossTie_4);
            metalShapeExtruders.Add(metalTopperCrossTie_5);
            metalShapeExtruders.Add(metalTopperCrossTie_6);
            metalShapeExtruders.Add(metalTopperCrossTie_7);
            metalShapeExtruders.Add(metalTopperCrossTie_8);
        }
        else
        {
            metalShapeExtruders.Add(iboxLeftRailExtruder);
            metalShapeExtruders.Add(iboxRightRailExtruder);
        }
        if (metalFrontCrossTieExtruder_1.vertices.Count > 0)
        {
            metalShapeExtruders.Add(metalFrontCrossTieExtruder_1);
            metalShapeExtruders.Add(metalFrontCrossTieExtruder_2);
            metalShapeExtruders.Add(metalFrontCrossTieExtruder_3);
            metalShapeExtruders.Add(metalRearCrossTieExtruder_1);
            metalShapeExtruders.Add(metalRearCrossTieExtruder_2);
            metalShapeExtruders.Add(metalRearCrossTieExtruder_3);
        }
        if (metalIBeamExtruder_1.vertices.Count > 0)
        {
            metalShapeExtruders.Add(metalIBeamExtruder_1);
            metalShapeExtruders.Add(metalIBeamExtruder_2);
            metalShapeExtruders.Add(metalIBeamExtruder_3);
        }
        foreach (ShapeExtruder extruder in metalShapeExtruders)
        {
            GameObject gameObject = new GameObject("metalObject");
            gameObject.transform.parent = putMeshOnGO.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = metalMaterial;
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            Mesh mesh = extruder.getMesh(putMeshOnGO.transform.worldToLocalMatrix);
            //trackSegment.addGeneratedMesh (mesh);
            meshFilter.mesh = mesh;
        }
    }

    public Vector3 IntersectLineAndPlane(Vector3 planePoint, Vector3 planeSpanVector1, Vector3 planeSpanVector2, Vector3 linePoint, Vector3 lineSpanVector)
    {
        //Parametric plane to cartesian form:
        Vector3 planeNormal = Vector3.Cross(planeSpanVector1, planeSpanVector2).normalized;
        //Cartesian form would be planeNormal.x * x + planeNormal.y * y + planeNormal.z * z - Vector3.Dot(planeNormal, planePoint) == 0;
        float intersectionX = linePoint.x - (lineSpanVector.x * (planeNormal.x * linePoint.x
                                                                 + planeNormal.y * linePoint.y
                                                                 + planeNormal.z * linePoint.z
                                                                 - Vector3.Dot(planeNormal, planePoint)))
                              / (planeNormal.x * lineSpanVector.x
                                 + planeNormal.y * lineSpanVector.y
                                 + planeNormal.z * lineSpanVector.z);
        float intersectionY = linePoint.y - (lineSpanVector.y * (planeNormal.x * linePoint.x
                                                                 + planeNormal.y * linePoint.y
                                                                 + planeNormal.z * linePoint.z
                                                                 - Vector3.Dot(planeNormal, planePoint)))
                              / (planeNormal.x * lineSpanVector.x
                                 + planeNormal.y * lineSpanVector.y
                                 + planeNormal.z * lineSpanVector.z);
        float intersectionZ = linePoint.z - (lineSpanVector.z * (planeNormal.x * linePoint.x
                                                                 + planeNormal.y * linePoint.y
                                                                 + planeNormal.z * linePoint.z
                                                                 - Vector3.Dot(planeNormal, planePoint)))
                              / (planeNormal.x * lineSpanVector.x
                                 + planeNormal.y * lineSpanVector.y
                                 + planeNormal.z * lineSpanVector.z);

        return new Vector3(intersectionX, intersectionY, intersectionZ);
    }

    public Vector2 Rotate(Vector2 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tempX = vector.x;
        float tempY = vector.y;

        vector.x = (cos * tempX) - (sin * tempY);
        vector.y = (sin * tempX) + (cos * tempY);
        return vector;
    }


    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return MeshCombiner.start().add(topperLeftPlankExtruder_1, topperLeftPlankExtruder_2, topperLeftPlankExtruder_3, topperLeftPlankExtruder_4, topperLeftPlankExtruder_5, topperLeftPlankExtruder_6, topperRightPlankExtruder_1, topperRightPlankExtruder_2, topperRightPlankExtruder_3, topperRightPlankExtruder_4, topperRightPlankExtruder_5, topperRightPlankExtruder_6, woodenVerticalSupportPostExtruder).end(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Mesh getCollisionMesh(GameObject putMeshOnGO)
    {
        return collisionMeshExtruder.getMesh(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Extruder getBuildVolumeMeshExtruder()
    {
        return customBuildVolumeMeshExtruder;
    }

    public override float getCenterPointOffsetY()
    {
        return 0.27f;
    }

    public override float trackOffsetY()
    {
        return 0.23f;
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
    /*
     * public override float getLSMOffsetY()
     * {
     * return 0.115f;
     * }
     */
    protected override float railHalfHeight()
    {
        return 0.02666f;
    }
    public void metalCrossTieExtrude(Vector3 middlePoint, Vector3 tangent, Vector3 normal)
    {
        metalFrontCrossTieExtruder_1.extrude(middlePoint, tangent, normal);
        metalFrontCrossTieExtruder_2.extrude(middlePoint, tangent, normal);
        metalFrontCrossTieExtruder_3.extrude(middlePoint, tangent, normal);
        metalRearCrossTieExtruder_1.extrude(middlePoint, tangent, normal);
        metalRearCrossTieExtruder_2.extrude(middlePoint, tangent, normal);
        metalRearCrossTieExtruder_3.extrude(middlePoint, tangent, normal);
    }
    public void metalIBeamExtrude(Vector3 middlePoint, Vector3 tangent, Vector3 normal)
    {
        metalIBeamExtruder_1.extrude(middlePoint, tangent, normal);
        metalIBeamExtruder_2.extrude(middlePoint, tangent, normal);
        metalIBeamExtruder_3.extrude(middlePoint, tangent, normal);
    }
    public void metalCrossTieEnd()
    {
        metalFrontCrossTieExtruder_1.end();
        metalFrontCrossTieExtruder_2.end();
        metalFrontCrossTieExtruder_3.end();
        metalRearCrossTieExtruder_1.end();
        metalRearCrossTieExtruder_2.end();
        metalRearCrossTieExtruder_3.end();
    }
    public void metalIBeamEnd()
    {
        metalIBeamExtruder_1.end();
        metalIBeamExtruder_2.end();
        metalIBeamExtruder_3.end();
    }

    public void WriteToFile(string text)
    {
        streamWriter = File.AppendText(path + @"/mod.log");
        streamWriter.WriteLine(DateTime.Now + ": " + text);
        streamWriter.Flush();
        streamWriter.Close();
    }
}
