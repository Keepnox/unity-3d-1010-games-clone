using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  [SerializeField] private int width, height;
  [SerializeField] private Tile tilePrefab;
  [SerializeField] private Transform cam;
  [SerializeField] private Transform TileOuter;
  [SerializeField] private Spawner spawner;
  [SerializeField] private Spawner currentSpawner;
  Tile [,] spawnedTilesArr;
  private GameObject _PickedObject;

  void Start()
  {
    spawnedTilesArr = new Tile[width,height];
    GenerateGrid();
  }

  // Update is called once per frame
  void GenerateGrid()
  
  {
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        Tile spawnedTile = Instantiate(tilePrefab, TileOuter);
        spawnedTile.transform.position = new Vector3(x, y);
        spawnedTilesArr[x, y] = spawnedTile;
        spawnedTile.name = $"Tile {x} {y}";
      }
    }
    
    

    cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height/2 - 0.5f, -20);
  }
  
  
  public void OnEnable()
  {
    FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
    FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
    FingerGestures.OnFingerMove += FingerGestures_OnFingerMove;
  }



  public void OnDisable()
  {
    FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
    FingerGestures.OnFingerMove -= FingerGestures_OnFingerMove;
    FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
  }

  private void fingerController(int fingerindex, GameObject tappedGameObject, string getTag, Action fingerCallback)
  {
    if (fingerindex == 0)
    {
      if (tappedGameObject != null && tappedGameObject.CompareTag(getTag))
      {
        fingerCallback();
      }
    }
  }


  // ReSharper disable Unity.PerformanceAnalysis
  private void FingerGestures_OnFingerUp(int fingerindex, Vector2 fingerpos, float timehelddown)
  {
    fingerController(fingerindex, _PickedObject, "Parca", () =>
    {
     Vector3 fingerUpPosition = GetWorldPos(fingerpos);
     
     if (fingerUpPosition.x < width && fingerUpPosition.x >= 0 && fingerUpPosition.y < height && fingerUpPosition.y >= 0)
     {
       Vector3 backObj = spawnedTilesArr[Mathf.RoundToInt(fingerUpPosition.x), Mathf.RoundToInt(fingerUpPosition.y)].transform.position;
       if (backObj != null)
       {
         currentSpawner.currentParca.transform.position = new Vector3(backObj.x, backObj.y + 1f, -0.01f);
         currentSpawner.currentParca.transform.SetParent(spawnedTilesArr[Mathf.RoundToInt(fingerUpPosition.x), Mathf.RoundToInt(fingerUpPosition.y) + 1].transform);
         currentSpawner.currentParca = null;
         Debug.Log(currentSpawner.parcaArr.Length);
         currentSpawner.SpawnParcaMethod();
       }  
     }
     
     else
     {
      Debug.Log("bos yok");  
     }
    });
  }  
  
  // ReSharper disable Unity.PerformanceAnalysis
  private void FingerGestures_OnFingerMove(int fingerindex, Vector2 fingerpos)
  {
    
    fingerController(fingerindex,_PickedObject,"Parca", () =>
    {
      currentSpawner = _PickedObject.GetComponent<Spawner>();
      currentSpawner.currentParca.transform.position = new Vector3(GetWorldPos(fingerpos).x,GetWorldPos(fingerpos).y + 1f,-0.05f);
      
    });

  }

  // ReSharper disable Unity.PerformanceAnalysis
  private void FingerGestures_OnFingerDown(int fingerindex, Vector2 fingerpos)
  {
    _PickedObject = PickObject(fingerpos);
    fingerController(fingerindex,_PickedObject,"GridItem", () =>
    {
      changeColorTapOnGridItem(_PickedObject);
    });
    
  }

  private static void changeColorTapOnGridItem(GameObject _PickedObject)
  {
    Tile _tile = _PickedObject.GetComponent<Tile>();
    if (_tile.isActive)
    {
      _tile.isActive = false;
      _tile.ChangeColorOnClicked(_tile.isActive);
    }
    else
    {
      _tile.isActive = true;
      _tile.ChangeColorOnClicked(_tile.isActive);
    }
  }

  GameObject PickObject(Vector2 screenPos)
  {
    Ray ray = Camera.main.ScreenPointToRay(screenPos);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit))
    {
      return hit.collider.gameObject;
    }

    return null;
  }

  Vector3 GetWorldPos(Vector2 screenPos)
  {
    Ray ray = Camera.main.ScreenPointToRay(screenPos);
    float t = -ray.origin.z / ray.direction.z;
    return ray.GetPoint(t);
  }

  

  
}




