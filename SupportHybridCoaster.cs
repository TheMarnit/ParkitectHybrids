using System.IO;
using UnityEngine;

public class SupportHybridCoaster : SupportTrackedRide
{

    private StreamWriter streamWriter;

    public TrackSegment4 trackSegment;

    protected override void build()
    {
        base.boundingVolumes.AddRange(base.GetComponentsInChildren<BoundingVolume>());
        base.transform.position = base.origin;
        Vector3 tangent = base.tangent;
        tangent.y = 0f;
        base.transform.forward = tangent;
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
