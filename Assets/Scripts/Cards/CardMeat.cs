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


  void OnEnable()
  {
    // 게임시작시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안됨
    textAddHealth.alpha = 0f;

    // 카드 능력치 설정
    SetCardValue();

    Invoke("FlipCard", Settings.flipDelay);
  }

  void SetCardValue()
  {
    this.addHealthVal = 4;
  }

  public void SetCardStat()
  {
    textAddHealth.text = addHealthVal.ToString();

    // addHealthVal 값 설정
    if(addHealthVal > 0) {
      textAddHealth.alpha = 1f;
    } else {
      textAddHealth.alpha = 0f;
      DieCard();
    }
  }

  private void DieCard()
  {
    // 삭제하지 않고 비활성화
    gameObject.SetActive(false);

    // 다시 뒷면으로 돌려놓음
    spriteRenderer.sprite = backSprite;
    isFlipped = !isFlipped;

    // 부모오브젝트(cards)에서 제거 
    transform.SetParent(null);

    // 카드 재정렬
    GameManager.instance.board.ArrangeBoard();
  }

  public void ClickedCard() 
  {
    if( isFlipping == true || 
        GameManager.instance.board.isCardsMoving == true ||
        GameManager.instance.isGamePlay == false
      )
      return;

    // 플레이어와 이웃 여부 확인
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
