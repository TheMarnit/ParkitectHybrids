using HybridCoasters;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SupportHybridCoaster : SupportTrackedRide
{
private StreamWriter streamWriter;

public TrackSegment4 trackSegment;

private BoxExtruder woodenVerticalSupportPostExtruder;

private BoxExtruder concreteVerticalSupportFooterExtruder;

private BoxExtruder woodenHorizontalSupportPostExtruder;

Vector3 leftVerticalSupportPost;

Vector3 leftVerticalSupportPost_floor;

Vector3 rightVerticalSupportPost;

Vector3 rightVerticalSupportPost_floor;

Vector3 projectedTangentDirection;

float leftVerticalSupportPost_footerHeight;

float rightVerticalSupportPost_footerHeight;

protected override void build ()
{
    base.boundingVolumes.AddRange (base.GetComponentsInChildren<BoundingVolume>());
    base.transform.position = base.origin;
    Vector3 tangent = base.tangent;
    tangent.y = 0f;
    base.transform.forward = tangent;
    woodenVerticalSupportPostExtruder = new BoxExtruder (0.043f, 0.043f);
    woodenVerticalSupportPostExtruder.closeEnds = true;
    woodenVerticalSupportPostExtruder.setUV (14, 14);
    concreteVerticalSupportFooterExtruder = new BoxExtruder (0.15f, 0.15f);
    concreteVerticalSupportFooterExtruder.closeEnds = true;
    concreteVerticalSupportFooterExtruder.setUV (0, 8);
    woodenHorizontalSupportPostExtruder = new BoxExtruder (0.03f, 0.06f);
    woodenHorizontalSupportPostExtruder.closeEnds = true;
    woodenHorizontalSupportPostExtruder.setUV (14, 14);
    render ();
}

private void render ()
{
    Transform startPositionMarker = base.transform;
    LandPatch terrain = GameController.Instance.park.getTerrain (startPositionMarker.position);

    if (terrain == null) {
        Object.Destroy (base.gameObject);
    }
    else{
        HybridCoasterMeshGenerator meshGenerator = (HybridCoasterMeshGenerator)track.TrackedRide.meshGenerator;
        foreach (SupportPosition position in meshGenerator.supportPosts [trackSegment.getStartpoint ()]) {
            leftVerticalSupportPost = new Vector3(position.verticalSupportPostLeft.x, position.verticalSupportPostLeft.y + meshGenerator.supportBeamExtension, position.verticalSupportPostLeft.z);
            rightVerticalSupportPost = new Vector3(position.verticalSupportPostRight.x, position.verticalSupportPostRight.y + meshGenerator.supportBeamExtension, position.verticalSupportPostRight.z);
            projectedTangentDirection = position.verticalSupportPostTangent;

            leftVerticalSupportPost_floor = GameController.Instance.park.getTerrain (leftVerticalSupportPost).getPoint (leftVerticalSupportPost);
            rightVerticalSupportPost_floor = GameController.Instance.park.getTerrain (rightVerticalSupportPost).getPoint (rightVerticalSupportPost);

            leftVerticalSupportPost_footerHeight = leftVerticalSupportPost_floor.y + 0.1f;
            if (GameController.Instance.park.getTerrain (leftVerticalSupportPost).WaterHeight > leftVerticalSupportPost_footerHeight) {
                leftVerticalSupportPost_footerHeight = GameController.Instance.park.getTerrain (leftVerticalSupportPost).WaterHeight + 0.025f;
            }
            rightVerticalSupportPost_footerHeight = rightVerticalSupportPost_floor.y + 0.1f;
            if (GameController.Instance.park.getTerrain (rightVerticalSupportPost).WaterHeight > rightVerticalSupportPost_footerHeight) {
                rightVerticalSupportPost_footerHeight = GameController.Instance.park.getTerrain (rightVerticalSupportPost).WaterHeight + 0.025f;
            }
            if (leftVerticalSupportPost.y > leftVerticalSupportPost_floor.y) {
                woodenVerticalSupportPostExtruder.extrude (leftVerticalSupportPost, new Vector3 (0, -1, 0), projectedTangentDirection);
                woodenVerticalSupportPostExtruder.extrude (leftVerticalSupportPost_floor, new Vector3 (0, -1, 0), projectedTangentDirection);
                woodenVerticalSupportPostExtruder.end ();
                concreteVerticalSupportFooterExtruder.extrude (new Vector3 (leftVerticalSupportPost_floor.x, leftVerticalSupportPost_footerHeight, leftVerticalSupportPost_floor.z), new Vector3 (0, -1, 0), projectedTangentDirection);
                concreteVerticalSupportFooterExtruder.extrude (new Vector3 (leftVerticalSupportPost_floor.x, leftVerticalSupportPost_floor.y - 0.05f, leftVerticalSupportPost_floor.z), new Vector3 (0, -1, 0), projectedTangentDirection);
                concreteVerticalSupportFooterExtruder.end ();
            }
            if (rightVerticalSupportPost.y > rightVerticalSupportPost_floor.y) {
                woodenVerticalSupportPostExtruder.extrude (rightVerticalSupportPost, new Vector3 (0, -1, 0), projectedTangentDirection);
                woodenVerticalSupportPostExtruder.extrude (rightVerticalSupportPost_floor, new Vector3 (0, -1, 0), projectedTangentDirection);
                woodenVerticalSupportPostExtruder.end ();
                concreteVerticalSupportFooterExtruder.extrude (new Vector3 (rightVerticalSupportPost_floor.x, rightVerticalSupportPost_footerHeight, rightVerticalSupportPost_floor.z), new Vector3 (0, -1, 0), projectedTangentDirection);
                concreteVerticalSupportFooterExtruder.extrude (new Vector3 (rightVerticalSupportPost_floor.x, rightVerticalSupportPost_floor.y - 0.05f, rightVerticalSupportPost_floor.z), new Vector3 (0, -1, 0), projectedTangentDirection);
                concreteVerticalSupportFooterExtruder.end ();
            }
        }

        List<BoxExtruder> extruders = new List<BoxExtruder>();
        if (woodenVerticalSupportPostExtruder.vertices.Count > 0) {
            extruders.Add (woodenVerticalSupportPostExtruder);
        }
        if (woodenHorizontalSupportPostExtruder.vertices.Count > 0) {
            extruders.Add (woodenHorizontalSupportPostExtruder);
        }
        if (concreteVerticalSupportFooterExtruder.vertices.Count > 0) {
            extruders.Add (concreteVerticalSupportFooterExtruder);
        }
        foreach (BoxExtruder extruder in extruders) {
            GameObject gameObject = new GameObject ("supportObject");
            gameObject.transform.parent = base.instance.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = track.TrackedRide.meshGenerator.material;
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            Mesh mesh = extruder.getMesh (base.instance.transform.worldToLocalMatrix);
            trackSegment.addGeneratedMesh (mesh);
            meshFilter.mesh = mesh;
        }
    }
}
public void WriteToFile (string text)
{
    streamWriter = File.AppendText ("C:/Users/marti/Desktop/mod.log");
    streamWriter.WriteLine (System.DateTime.Now + ": " + text);
    streamWriter.Flush ();
    streamWriter.Close ();
}
}
