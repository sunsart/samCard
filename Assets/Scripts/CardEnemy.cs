using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardEnemy : Card
{
  [HideInInspector] public int healthVal = 9;


  void Start() 
  {
    SetCardStat();

    //카드생성시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안되기때문
    textHealth.alpha = 0f;

    Invoke("FlipCard", 1f);
  }

  public void SetCardStat()
  {
    textHealth.text = healthVal.ToString();
  }

  public void ClickedCard() 
  {
    if(isFlipping == true)
      return;

    //플레이어와 이웃 여부 확인
    if(GameManager.instance.board.IsNeighbor(posNum))
    {
      GameManager.instance.TakeBattle(gameObject);
    }
  }

}
