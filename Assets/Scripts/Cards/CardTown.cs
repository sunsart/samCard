using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CardTown : MonoBehaviour
{
  [SerializeField] private Sprite frontSprite;
  [SerializeField] private Sprite backSprite;
  [SerializeField] private SpriteRenderer spriteRenderer;

  private bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  private bool isFlipping = false;  // 앞면으로 뒤집어지는 중인지 여부


  void OnEnable()
  {
    Invoke("FlipCard", Settings.flipDelay);
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
    GameManager.instance.board.SpawnCardFromTown(gameObject);
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
    DieCard();
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
      }

      transform.DOScale(originalScale, Settings.flipSpeed).OnComplete(() => { isFlipping = false; });
    });
  }
  
}
