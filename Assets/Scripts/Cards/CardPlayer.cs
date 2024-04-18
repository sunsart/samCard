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

  private bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  private bool isFlipping = false;  // 앞면으로 뒤집어지는 중인지 여부

  public int healthVal;
  public int attackVal;


  void Start() 
  {
    // 게임시작시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안됨
    textHealth.alpha = 0f;
    textAttack.alpha = 0f;

    Invoke("FlipCard", Settings.flipDelay);
  }

  public void SetCardStat()
  {
    // 최대 체력 limit
    if(healthVal >= Settings.playerMaxHealth)
      healthVal = Settings.playerMaxHealth;

    // healthVal 값 설정
    if(healthVal > 0)
      textHealth.alpha = 1f;
    else {
      textHealth.alpha = 0f;

      //게임오버
      GameManager.instance.isGamePlay = false;
    }

    // attackVal 값 설정
    if(attackVal > 0) {
      textAttack.alpha = 1f;
    } else {
      attackVal = 0;
      textAttack.alpha = 0f;
    }

    textHealth.text = healthVal.ToString();
    textAttack.text = attackVal.ToString();
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
