using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SupportHybridCoaster : SupportTrackedRide
{

    private StreamWriter streamWriter;

    public TrackSegment4 trackSegment;

    private BoxExtruder woodenVerticalSupportPostExtruder;

    private BoxExtruder woodenHorizontalSupportPostExtruder;

    private float crossBeamSpacing = 1f;

    Vector3 leftVerticalSupportPost;

    Vector3 leftVerticalSupportPost_floor;

    Vector3 rightVerticalSupportPost;

    Vector3 rightVerticalSupportPost_floor;

    private float beamWidth = 0.98f;

    private float supportBeamExtension = 0.2f;

    protected override void build()
    {
        base.boundingVolumes.AddRange(base.GetComponentsInChildren<BoundingVolume>());
        base.transform.position = base.origin;
        Vector3 tangent = base.tangent;
        tangent.y = 0f;
        base.transform.forward = tangent;
        woodenVerticalSupportPostExtruder = new BoxExtruder(0.043f, 0.043f);
        woodenVerticalSupportPostExtruder.closeEnds = true;
        woodenVerticalSupportPostExtruder.setUV(14, 14);
        woodenHorizontalSupportPostExtruder = new BoxExtruder(0.03f, 0.06f);
        woodenHorizontalSupportPostExtruder.closeEnds = true;
        woodenHorizontalSupportPostExtruder.setUV(14, 14);
        render();
    }

    private void render()
    {
        WriteToFile(trackSegment.ToString());
        Transform startPositionMarker = base.transform;
        LandPatch terrain = GameController.Instance.park.getTerrain(startPositionMarker.position);

        if (terrain == null)
        {
            Object.Destroy(base.gameObject);
        }
        else
        {
            float supportInterval = trackSegment.getLength(0) / ((float)Mathf.RoundToInt(trackSegment.getLength(0) / this.crossBeamSpacing) * 2);
            float pos = 0;

            int index = 0;
            while (pos <= trackSegment.getLength(0) + 0.1f)
            {
                WriteToFile(pos + "/" + trackSegment.getLength(0));
                index++;
                float tForDistance = trackSegment.getTForDistance(pos, 0);
                pos += supportInterval;

                Vector3 normal = trackSegment.getNormal(tForDistance);
                Vector3 tangentPoint = trackSegment.getTangentPoint(tForDistance);
                Vector3 binormal = Vector3.Cross(normal, tangentPoint).normalized;
                Vector3 trackPivot = track.TrackedRide.meshGenerator.getTrackPivot(trackSegment.getPoint(tForDistance, 0), Vector3.up);
                Vector3 projectedTangentDirection = tangentPoint;
                projectedTangentDirection.y = 0;
                projectedTangentDirection.Normalize();

                WriteToFile(trackPivot.ToString());

                leftVerticalSupportPost = trackPivot + (new Vector3(0, 1, 0) * supportBeamExtension) + (binormal * (beamWidth / 2.5f));
                rightVerticalSupportPost = trackPivot + (new Vector3(0, 1, 0) * supportBeamExtension) - (binormal * (beamWidth / 2.5f));
                leftVerticalSupportPost_floor = GameController.Instance.park.getTerrain(leftVerticalSupportPost).getPoint(leftVerticalSupportPost);
                rightVerticalSupportPost_floor = GameController.Instance.park.getTerrain(rightVerticalSupportPost).getPoint(rightVerticalSupportPost);


                if (pos > supportInterval)
                {
                    woodenVerticalSupportPostExtruder.extrude(leftVerticalSupportPost, new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.extrude(leftVerticalSupportPost_floor, new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.end();

                    woodenVerticalSupportPostExtruder.extrude(rightVerticalSupportPost, new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.extrude(rightVerticalSupportPost_floor, new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.end();
                }
            }

            List<BoxExtruder> extruders = new List<BoxExtruder>();
            if (woodenVerticalSupportPostExtruder.vertices.Count > 0)
            {
                extruders.Add(woodenVerticalSupportPostExtruder);
            }
            if (woodenHorizontalSupportPostExtruder.vertices.Count > 0)
            {
                extruders.Add(woodenHorizontalSupportPostExtruder);
            }
            foreach (BoxExtruder extruder in extruders)
            {
                GameObject gameObject = new GameObject("supportObject");
                gameObject.transform.parent = base.instance.transform;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = track.TrackedRide.meshGenerator.material;
                MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
                Mesh mesh = extruder.getMesh(base.instance.transform.worldToLocalMatrix);
                trackSegment.addGeneratedMesh(mesh);
                meshFilter.mesh = mesh;
            }
            /*
            Vector3 point = terrain.getPoint (startPositionMarker.position);
            Vector3 position = startPositionMarker.position;
            float num = position.y - point.y;
            if (num < 0.25f) {
                Object.Destroy (base.gameObject);
            }
            else{
                GameObject gameObject = new GameObject();
                gameObject.transform.parent = startPositionMarker.transform;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.parent = base.instance.transform;
                Vector3 localScale = base.transform.localScale;
                localScale.y = num;
                gameObject.transform.localScale = localScale;
                BoundingBox boundingBox = base.instance.AddComponent<BoundingBox>();
                boundingBox.setManuallyPositioned ();
                boundingBox.layers = BoundingVolume.Layers.Support;
                boundingBox.setBounds (base.getBounds (startPositionMarker.position, point, 0.12f));
                boundingBox.setPosition (startPositionMarker.position, Quaternion.identity);
                base.boundingVolumes.Add (boundingBox);
                GameObject gameObject2 = Object.Instantiate (base.supportBaseGO);
                gameObject2.transform.parent = startPositionMarker.transform;
                gameObject2.transform.localPosition = Vector3.zero;
                Transform transform = gameObject2.transform;
                Vector3 position2 = startPositionMarker.position;
                transform.Translate (0f, 0f - (position2.y - point.y), 0f);
                gameObject2.transform.localRotation = Quaternion.identity;
                gameObject2.transform.parent = base.instance.transform;
            }
        */
        }
        }
    public void WriteToFile(string text)
    {
        streamWriter = File.AppendText("C:/Users/marti/Desktop/mod.log");
        streamWriter.WriteLine(System.DateTime.Now + ": " + text);
        streamWriter.Flush();
        streamWriter.Close();
    }
}
