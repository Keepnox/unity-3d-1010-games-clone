using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  // Start is called before the first frame update
  public Parcalar[] parcaArr;
  public Parcalar currentParca;
  void Awake()
  {
    SpawnParcaMethod();
  }

  public void SpawnParcaMethod()
  {
    if (currentParca == null)
    {
      currentParca = Instantiate(parcaArr[Random.Range(0, parcaArr.Length)], transform);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
