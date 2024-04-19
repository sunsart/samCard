using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CardEnemy : MonoBehaviour
{
  [SerializeField] private Sprite frontSprite;
  [SerializeField] private Sprite backSprite;
  [SerializeField] private SpriteRenderer spriteRenderer;

  [SerializeField] protected TextMeshPro textHealth;

  private bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  private bool isFlipping = false;  // 앞면으로 뒤집어지는 중인지 여부

  public int healthVal;


  void OnEnable()
  {
    // 게임시작시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안됨
    textHealth.alpha = 0f;

    // 카드 능력치 설정
    SetCardValue();

    Invoke("FlipCard", Settings.flipDelay);
  }

  void SetCardValue()
  {
    this.healthVal = UnityEngine.Random.Range(2, 10);
  }

  public void SetCardStat()
  {
    textHealth.text = healthVal.ToString();

    // healthVal 값 설정
    if(healthVal > 0)
      textHealth.alpha = 1f;
    else {
      textHealth.alpha = 0f;
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
    if(GameManager.instance.board.IsNeighborPlayer(gameObject)) {
      GameManager.instance.CountTurn();
      ActionThisCard();
    }
  }

  private void ActionThisCard()
  {
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

    // 플레이어가 무기를 소지하고 있으면
    if(playerObj.GetComponent<CardPlayer>().attackVal > 0) {
      // 플레이어의 공격력이 적의 체력보다 크거나 같으면
      if(playerObj.GetComponent<CardPlayer>().attackVal >= this.healthVal)
      {
        // 1. player
        playerObj.GetComponent<CardPlayer>().attackVal -= this.healthVal;
        playerObj.GetComponent<CardPlayer>().SetCardStat();

        // 2. enemy
        this.healthVal = 0;
        this.SetCardStat();
      }
      else // 플레이어의 공격력이 적의 체력보다 작으면
      {
        // 1. enemy
        this.healthVal -= playerObj.GetComponent<CardPlayer>().attackVal;
        this.SetCardStat();

        // 2. player
        playerObj.GetComponent<CardPlayer>().attackVal = 0;
        playerObj.GetComponent<CardPlayer>().SetCardStat();
      }
    } 
    // 플레이어가 무기를 소지하고 있지 않으면
    else {
      // 플레이어의 체력이 적의 체력보다 크다면
      if(playerObj.GetComponent<CardPlayer>().healthVal > this.healthVal) {
        // 1. player
        playerObj.GetComponent<CardPlayer>().healthVal -= this.healthVal;
        playerObj.GetComponent<CardPlayer>().SetCardStat();

        // 2. enemy
        this.healthVal = 0;
        this.SetCardStat();
      }
      // 플레이어의 체력이 적의 체력보다 작거나 같으면
      else {
        // 1. enemy
        this.healthVal -= playerObj.GetComponent<CardPlayer>().healthVal;
        this.SetCardStat();

        // 2. player
        playerObj.GetComponent<CardPlayer>().healthVal = 0;
        playerObj.GetComponent<CardPlayer>().SetCardStat();
      }
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
