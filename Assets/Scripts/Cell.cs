using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
  [HideInInspector] public int posX;
  [HideInInspector] public int posY;


  void Start()
  {
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    sr.sprite = null;
  } 

}
