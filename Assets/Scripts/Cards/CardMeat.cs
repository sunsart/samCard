using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class CardMeat : MonoBehaviour
{
  [SerializeField] private Sprite frontSprite;
  [SerializeField] private Sprite backSprite;
  [SerializeField] private SpriteRenderer spriteRenderer;

  [SerializeField] protected TextMeshPro textAddHealth;

  private bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  private bool isFlipping = false;  // 앞면으로 뒤집어지는 중인지 여부

  public int addHealthVal;


  void Start() 
  {
    // 게임시작시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안됨
    textAddHealth.alpha = 0f;

    Invoke("FlipCard", Settings.flipDelay);
  }

  public void SetCardStat()
  {
    textAddHealth.text = addHealthVal.ToString();

    // addHealthVal 값 설정
    if(addHealthVal > 0) {
      textAddHealth.alpha = 1f;
    } else {
      textAddHealth.alpha = 0f;
      Destroy(gameObject);
      GameManager.instance.board.ArrangeBoard();
    }
  }

  public void ClickedCard() 
  {
    if(isFlipping == true || GameManager.instance.board.isCardsMoving == true)
      return;

    //플레이어와 이웃 여부 확인
    if(GameManager.instance.board.IsNeighborPlayer(gameObject))
    {
      GameManager.instance.CountTurn();
      ActionThisCard();
    }
  }

  private void ActionThisCard()
  {
    //player 값 변경
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    playerObj.GetComponent<CardPlayer>().healthVal += this.addHealthVal;
    playerObj.GetComponent<CardPlayer>().SetCardStat();

    //meat 값 변경
    this.addHealthVal = 0;
    this.SetCardStat();
  }

  public void FlipCard() 
  {
    isFlipping = true;

    Vector3 originalScale = transform.localScale;
    Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);
    transform.DOScale(targetScale, Settings.flipSpeed).OnComplete(() => 
    {
      isFlipped = !isFlipped;

      if(isFlipped)
      {
        spriteRenderer.sprite = frontSprite;
        SetCardStat();
      }

      transform.DOScale(originalScale, Settings.flipSpeed).OnComplete(() => { isFlipping = false; });
    });
  }
  
}
