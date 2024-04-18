using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  public Board board;
  public PoolManager pool;
  
  [HideInInspector] public int turn = 0;
  [HideInInspector] public bool isGamePlay = true;


  void Awake()
  {
    if(instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      DestroyImmediate(gameObject);
    }
  }

  public void CountTurn()
  {
    this.turn++;
    Debug.Log("Turn Count : " + this.turn);
  }

}
