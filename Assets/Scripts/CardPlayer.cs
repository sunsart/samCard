using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CardPlayer : MonoBehaviour
{
  [SerializeField] private Sprite frontSprite;
  [SerializeField] private Sprite backSprite;
  [SerializeField] private SpriteRenderer spriteRenderer;

  [SerializeField] protected TextMeshPro textHealth;
  [SerializeField] protected TextMeshPro textAttack;

  protected bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  protected bool isFlipping = false;  // 앞면으로 뒤집어지는 중인지 여부

  public int healthVal = 100;
  public int attackVal = 5;


  void Start() 
  {
    // 게임시작시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안됨
    textHealth.alpha = 0f;
    textAttack.alpha = 0f;

    Invoke("FlipCard", Settings.flipDelay);
  }

  public void SetCardStat()
  {
    textHealth.text = healthVal.ToString();
    textAttack.text = attackVal.ToString();

    // healthVal 값에 따라 텍스트 visible 조정
    if(healthVal > 0)
      textHealth.alpha = 1f;
    else
      textHealth.alpha = 0f;

    // attackVal 값에 따라 텍스트 visible 조정
    if(attackVal > 0)
      textAttack.alpha = 1f;
    else
      textAttack.alpha = 0f;
  }

  public void ClickedCard() 
  {
    if(isFlipping == true || GameManager.instance.board.isCardsMoving == true)
      return;

    Debug.Log("주인공 카드 클릭...");
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
