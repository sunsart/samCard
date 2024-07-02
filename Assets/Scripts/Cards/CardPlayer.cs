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

  [SerializeField] private TextMeshPro textHealth;
  [SerializeField] private TextMeshPro textAttack;

  // 소지 무기 타입
  public enum WeaponType
  {
      Sword,
      Spear,
      Bow
  }
  [HideInInspector] public WeaponType weaponType;
  [SerializeField] private TextMeshPro textWeaponType;

  private bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  private bool isFlipping = false;  // 앞면으로 뒤집어지는 중인지 여부

  public int healthVal;
  public int attackVal;

  public int addHealthByTurn = 0;
  public int minusHealthByTurn = 0;


  void Start() 
  {
    // 게임시작시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안됨
    textHealth.alpha = 0f;
    textAttack.alpha = 0f;

    this.weaponType = WeaponType.Sword;

    Invoke("FlipCard", Settings.flipDelay);
  }

  public void SetCardStat()
  {
    // 1. 최대 체력 limit
    if(healthVal >= Settings.playerMaxHealth)
      healthVal = Settings.playerMaxHealth;

    // 2. healthVal 값 설정
    if(healthVal > 0)
      textHealth.alpha = 1f;
    else {
      textHealth.alpha = 0f;
      //게임오버
      GameManager.instance.isGamePlay = false;
      Debug.Log("***** game over ******");
    }

    // 3. attackVal 값 설정
    if(attackVal > 0) {
      textAttack.alpha = 1f;
    } else {
      attackVal = 0;
      textAttack.alpha = 0f;
      this.textWeaponType.text = null;
    }

    // 4. text 갱신
    textHealth.text = healthVal.ToString();
    textAttack.text = attackVal.ToString();

    if(this.weaponType == WeaponType.Sword)
      this.textWeaponType.text = "SW";
    else if(this.weaponType == WeaponType.Spear)
      this.textWeaponType.text = "SP";
    else if(this.weaponType == WeaponType.Bow)
      this.textWeaponType.text = "BO";

    // if(attackVal <= 0)
    //   this.textWeaponType.text = "";
  }

  public void ClickedCard() 
  {
    if(isFlipping == true || GameManager.instance.board.isCardsMoving == true)
      return;

    Debug.Log("주인공 카드 클릭...");
  }

  public void BrocastCards()
  {
    // 턴마다 체력 1 증가
    if(this.addHealthByTurn > 0)
    {
      this.healthVal++;
      this.SetCardStat();
      this.addHealthByTurn--;
    }

    // 턴마다 체력 1 감소
    if(this.minusHealthByTurn > 0)
    {
      this.healthVal--;
      this.SetCardStat();
      this.minusHealthByTurn--;
    }
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
