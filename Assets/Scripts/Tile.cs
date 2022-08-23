using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
  // [SerializeField] private Color baseColor;
  // [SerializeField] private Color activeColor;
  public bool isActive;
  
  public MeshRenderer renderer;
  // Start is called before the first frame update

  private void Awake()
  {
    // renderer.material.color = baseColor;
  }

  public void ChangeColorOnClicked(bool isActiveOnTile)
  {
    // renderer.material.color = isActiveOnTile ? activeColor : baseColor;
  }

  void Start()
  {

  }

  // Update is called once per frame
  
}
