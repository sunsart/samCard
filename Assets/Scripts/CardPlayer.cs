using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardPlayer : Card
{
  [HideInInspector] public int healthVal = 10;
  [HideInInspector] public int attackVal = 5;


  void Start() 
  {
    SetCardStat();

    //게임시작시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안되기때문
    textHealth.alpha = 0f;
    textAttack.alpha = 0f;

    Invoke("FlipCard", 0.5f);
  }

  public void SetCardStat()
  {
    textHealth.text = healthVal.ToString();
    textAttack.text = attackVal.ToString();

    //무기공격력 값에 따라 텍스트 visible 조정
    if(attackVal > 0)
      textAttack.alpha = 1f;
    else if(attackVal <= 0)
      textAttack.alpha = 0f;
  }

  public void ClickedCard() 
  {
    if(isFlipping == true)
      return;

    Debug.Log("주인공 카드 클릭...");
  }
}
