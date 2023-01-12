using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
  
[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
   public GameObject gameObjectToInstantiate;   //game object we are placing in room  
   private GameObject spawnedObject;          //to have a referene of the object we created
   private List<GameObject> placedPrefabList = new List<GameObject>();
 [SerializeField]
  private int maxPrefabSpawnCount = 0;
  private int placedPrefabCount;
  private GameObject placedPrefab;
   private ARRaycastManager _arRaycastManager;
   private Vector2 touchPosition; //touch postion on where we tap on screen to help place object
   static List<ARRaycastHit> hits = new List<ARRaycastHit>(); //
    // Start is called before the first frame update
    private void Awake()
    {
         _arRaycastManager = GetComponent<ARRaycastManager>();
    
    }
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
           if(Input.GetTouch(0).phase == TouchPhase.Began)
           {
              touchPosition = Input.GetTouch(index: 0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }
   
    void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        return;

        if (_arRaycastManager.Raycast(touchPosition, hits, trackableTypes:TrackableType.PlaneWithinPolygon))
        {   //takes a touch pos, takes the hits -the static list, and trackable types
            var hitPose = hits[0].pose;  //figure out if there is a swap object already, if not instantiate it 
            if (placedPrefabCount < maxPrefabSpawnCount)
            {
             SpawnPreFab(hitPose);
            }
        }
     }
    public void SetPreFabType(GameObject prefabType){
        gameObjectToInstantiate = prefabType;
    }

    private void SpawnPreFab(Pose hitPose){
            spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
            placedPrefabList.Add(spawnedObject);
            placedPrefabCount++;
    }
}