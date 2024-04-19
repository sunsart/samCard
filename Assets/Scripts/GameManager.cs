using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  public Board board;
  public PoolManager pool;

  public TextMeshProUGUI textCoin;
  
  [HideInInspector] public int turn = 0;
  [HideInInspector] public int coin = 0;
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

  public void SetUiText()
  {
    this.textCoin.text = coin.ToString();
  }

}
