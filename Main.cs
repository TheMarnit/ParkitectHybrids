using System.Collections.Generic;
using System.Linq;
using TrackedRiderUtility;
using UnityEngine;

namespace HybridCoasters
{
    public class Main : IMod
    {
        private TrackRiderBinder binder;
        private AssetBundle assetBundle;

        public GameObject FrontCartGo;
        public GameObject CartGo;
        public GameObject SideCrossBeamsGo;

        GameObject _go;

        public void onEnabled()
        {
            var dsc = System.IO.Path.DirectorySeparatorChar;
            //assetBundle = AssetBundle.LoadFromFile(Path + dsc + "assetbundle" + dsc + "assetpack");
            //SideCrossBeamsGo = assetBundle.LoadAsset<GameObject>("21a3f09b79e34f147a2b6017d2b6c05b");
            assetBundle = AssetBundle.LoadFromFile(Path + dsc + "assetbundle" + dsc + "corkscrewassetpack");
            SideCrossBeamsGo = assetBundle.LoadAsset<GameObject>("c184c4f392587465f9bf2c86e6615e78");
            FrontCartGo = assetBundle.LoadAsset<GameObject>("01be2cec49bbb476381a537d75ad047e");
            CartGo = assetBundle.LoadAsset<GameObject>("7c1045f838c59460db2bfebd3df04a47");

            binder = new TrackRiderBinder("kvwQwhKWWG");
            TrackedRide iboxCoaster =
                binder.RegisterTrackedRide<TrackedRide>("Floorless Coaster", "IboxCoaster", "Steel Hybrid Coaster");
            TrackedRide topperCoaster =
                binder.RegisterTrackedRide<TrackedRide>("Floorless Coaster", "TopperCoaster", "Wooden Hybrid Coaster");
            IboxCoasterMeshGenerator iboxTrackGenerator =
                binder.RegisterMeshGenerator<IboxCoasterMeshGenerator>(iboxCoaster);
            IboxCoasterMeshGenerator topperTrackGenerator =
                binder.RegisterMeshGenerator<IboxCoasterMeshGenerator>(topperCoaster);
            TrackRideHelper.PassMeshGeneratorProperties(TrackRideHelper.GetTrackedRide("Floorless Coaster").meshGenerator,
                iboxCoaster.meshGenerator);
            TrackRideHelper.PassMeshGeneratorProperties(TrackRideHelper.GetTrackedRide("Floorless Coaster").meshGenerator,
                topperCoaster.meshGenerator);

            iboxCoaster.canCurveLifts = true;
            topperCoaster.canCurveLifts = true;
            iboxCoaster.description = "A rollercoaster combining a steel track and a mix of wooden and steel supports to allow elements not normally found on wooden coasters.";
            topperCoaster.description = "A rollercoaster combining a wooden track and a mix of wooden and steel supports to allow elements not normally found on wooden coasters.";
            iboxTrackGenerator.path = Path;
            topperTrackGenerator.path = Path;
            iboxTrackGenerator.crossBeamGO = null;
            topperTrackGenerator.crossBeamGO = null;
            iboxTrackGenerator.supportInstantiator = null;
            topperTrackGenerator.supportInstantiator = null;
            iboxTrackGenerator.stationPlatformGO = TrackRideHelper.GetTrackedRide("Steel Coaster").meshGenerator.stationPlatformGO;
            topperTrackGenerator.stationPlatformGO = TrackRideHelper.GetTrackedRide("Steel Coaster").meshGenerator.stationPlatformGO;
            iboxTrackGenerator.frictionWheelsGO = TrackRideHelper.GetTrackedRide("Junior Coaster").meshGenerator.frictionWheelsGO;
            topperTrackGenerator.frictionWheelsGO = TrackRideHelper.GetTrackedRide("Junior Coaster").meshGenerator.frictionWheelsGO;
            iboxTrackGenerator.material = TrackRideHelper.GetTrackedRide("Wooden Coaster").meshGenerator.material;
            topperTrackGenerator.material = TrackRideHelper.GetTrackedRide("Wooden Coaster").meshGenerator.material;
            iboxTrackGenerator.metalMaterial = TrackRideHelper.GetTrackedRide("Steel Coaster").meshGenerator.material;
            topperTrackGenerator.metalMaterial = TrackRideHelper.GetTrackedRide("Steel Coaster").meshGenerator.material;
            topperTrackGenerator.useTopperTrack = true;

            iboxCoaster.price = 1200;
            topperCoaster.price = 1200;
            iboxCoaster.carTypes = new CoasterCarInstantiator[] { };
            topperCoaster.carTypes = new CoasterCarInstantiator[] { };
            iboxCoaster.meshGenerator.customColors = new[]
            {
                new Color(132f / 255f, 40f / 255f, 137f / 255f, 1), new Color(23f / 255f, 133f / 255f, 30f / 255f, 1),
                new Color(180 / 255f, 180f / 255f, 180f / 255f, 1),new Color(108f / 255f, 70f / 255f, 23f / 255f, 1)
            };
            topperCoaster.meshGenerator.customColors = new[]
            {
                new Color(132f / 255f, 40f / 255f, 137f / 255f, 1), new Color(23f / 255f, 133f / 255f, 30f / 255f, 1),
                new Color(180 / 255f, 180f / 255f, 180f / 255f, 1),new Color(108f / 255f, 70f / 255f, 23f / 255f, 1)
            };
            iboxCoaster.dropsImportanceExcitement = 0.665f;
            topperCoaster.dropsImportanceExcitement = 0.665f;
            iboxCoaster.inversionsImportanceExcitement = 0.673f;
            topperCoaster.inversionsImportanceExcitement = 0.673f;
            iboxCoaster.averageLatGImportanceExcitement = 0.121f;
            topperCoaster.averageLatGImportanceExcitement = 0.121f;
            iboxCoaster.accelerationImportanceExcitement = 0.525f;
            topperCoaster.accelerationImportanceExcitement = 0.525f;

            CoasterCarInstantiator iboxCoasterCarInstantiator =
                binder.RegisterCoasterCarInstaniator<CoasterCarInstantiator>(iboxCoaster, "RmcCoasterInsantiator",
                    "Hybrid Coaster Cars", 1, 15, 6);
            CoasterCarInstantiator topperCoasterCarInstantiator =
                binder.RegisterCoasterCarInstaniator<CoasterCarInstantiator>(topperCoaster, "RmcCoasterInsantiator",
                    "Hybrid Coaster Cars", 1, 15, 6);

            BaseCar frontCar = binder.RegisterCar<BaseCar>(Object.Instantiate(FrontCartGo), "RmcCoaster_Front_Car",
                .35f, 0f, true, new[]
                {
                    new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255),
                    new Color(73f / 255, 73f / 255, 73f / 255)
                }
            );
            iboxCoasterCarInstantiator.frontVehicleGO = frontCar;
            topperCoasterCarInstantiator.frontVehicleGO = frontCar;
            iboxCoasterCarInstantiator.frontVehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
                new Vector3(110, 0, 0);
            topperCoasterCarInstantiator.frontVehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
                new Vector3(110, 0, 0);

            List<Transform> transforms = new List<Transform>();
            Utility.recursiveFindTransformsStartingWith("wheel", frontCar.transform, transforms);
            foreach (var transform in transforms)
            {
                transform.gameObject.AddComponent<FrictionWheelAnimator>();
            }

            BaseCar backCar = binder.RegisterCar<BaseCar>(Object.Instantiate(CartGo), "RmcCoaster_Back_Car", .35f,
                -.3f, false, new[]
                {
                    new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255),
                    new Color(73f / 255, 73f / 255, 73f / 255)
                }
            );
            iboxCoasterCarInstantiator.vehicleGO = backCar;
            topperCoasterCarInstantiator.vehicleGO = backCar;
            iboxCoasterCarInstantiator.vehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
                new Vector3(110, 0, 0);
            topperCoasterCarInstantiator.vehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
                new Vector3(110, 0, 0);

            Utility.recursiveFindTransformsStartingWith("wheel", backCar.transform, transforms);
            foreach (var transform in transforms)
            {
                transform.gameObject.AddComponent<FrictionWheelAnimator>();
            }

            binder.Apply();
            assetBundle.Unload(false);
        }

        public void onDisabled()
        {
            binder.Unload();
        }
        
        public string Name => "Hybrid Coasters";

        public string Description => "Adds Hybrid Rollercoasters";

        string IMod.Identifier => "Marnit@ParkitectHybridCoasters";
	
	
        public string Path
        {
            get
            {
                return ModManager.Instance.getModEntries().First(x => x.mod == this).path;
            }
        }
    }
}
