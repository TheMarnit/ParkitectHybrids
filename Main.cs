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

        
        GameObject _go;

        public void onEnabled()
        {
            var dsc = System.IO.Path.DirectorySeparatorChar;
            assetBundle = AssetBundle.LoadFromFile( Path + dsc + "assetbundle" + dsc + "assetpack");

            FrontCartGo =  assetBundle.LoadAsset<GameObject> ("01be2cec49bbb476381a537d75ad047e");
            CartGo =  assetBundle.LoadAsset<GameObject> ("7c1045f838c59460db2bfebd3df04a47");
            
            binder = new TrackRiderBinder("kvwQwhKWWG");
            TrackedRide iboxCoaster =
                binder.RegisterTrackedRide<TrackedRide>("Floorless Coaster", "IboxCoaster", "RMC IBox");
            IboxCoasterMeshGenerator trackGenerator =
                binder.RegisterMeshGenerator<IboxCoasterMeshGenerator>(iboxCoaster);
            TrackRideHelper.PassMeshGeneratorProperties(TrackRideHelper.GetTrackedRide("Floorless Coaster").meshGenerator,
                iboxCoaster.meshGenerator);

            trackGenerator.crossBeamGO = null;
            trackGenerator.supportInstantiator = null;
            trackGenerator.stationPlatformGO = TrackRideHelper.GetTrackedRide("Steel Coaster").meshGenerator.stationPlatformGO;

            iboxCoaster.price = 1200;
            iboxCoaster.carTypes = new CoasterCarInstantiator[] { };
            iboxCoaster.meshGenerator.customColors = new[]
            {
                new Color(132f / 255f, 40f / 255f, 137f / 255f, 1), new Color(23f / 255f, 133f / 255f, 30f / 255f, 1),
                new Color(180 / 255f, 180f / 255f, 180f / 255f, 1),new Color(108f / 255f, 70f / 255f, 23f / 255f, 1)
            };
            iboxCoaster.dropsImportanceExcitement = 0.665f;
            iboxCoaster.inversionsImportanceExcitement = 0.673f;
            iboxCoaster.averageLatGImportanceExcitement = 0.121f;
            iboxCoaster.accelerationImportanceExcitement = 0.525f;

            CoasterCarInstantiator coasterCarInstantiator =
                binder.RegisterCoasterCarInstaniator<CoasterCarInstantiator>(iboxCoaster, "CorkscrewCoasterInsantiator",
                    "Corkscrew Car", 1, 15, 6);

            BaseCar frontCar = binder.RegisterCar<BaseCar>(Object.Instantiate(FrontCartGo), "CorkScrewCoaster_Front_Car",
                .35f, 0f, true, new[]
                {
                    new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255),
                    new Color(73f / 255, 73f / 255, 73f / 255)
                }
            );
            coasterCarInstantiator.frontVehicleGO = frontCar;
            coasterCarInstantiator.frontVehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
                new Vector3(110, 0, 0);

            List<Transform> transforms = new List<Transform>();
            Utility.recursiveFindTransformsStartingWith("wheel", frontCar.transform, transforms);
            foreach (var transform in transforms)
            {
                transform.gameObject.AddComponent<FrictionWheelAnimator>();
            }

            BaseCar backCar = binder.RegisterCar<BaseCar>(Object.Instantiate(CartGo), "CorkScrewCoaster_Back_Car", .35f,
                -.3f, false, new[]
                {
                    new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255),
                    new Color(73f / 255, 73f / 255, 73f / 255)
                }
            );
            coasterCarInstantiator.vehicleGO = backCar;
            coasterCarInstantiator.vehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
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
