using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CardBarrier : MonoBehaviour
{
  [SerializeField] private Sprite frontSprite;
  [SerializeField] private Sprite backSprite;
  [SerializeField] private SpriteRenderer spriteRenderer;

  [SerializeField] private TextMeshPro textMinusHealth;

  private bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  private bool isFlipping = false;  // 앞면으로 뒤집어지는 중인지 여부

  private int minusHealthVal;


  void OnEnable()
  {
    // 게임시작시 카드가 뒤집히기전 뒷면상태에서 텍스트가 표시되면 안됨
    textMinusHealth.alpha = 0f;

    // 카드 능력치 설정
    SetCardValue();

    Invoke("FlipCard", Settings.flipDelay);
  }

  void SetCardValue()
  {
    this.minusHealthVal = UnityEngine.Random.Range(1, 5);
  }

  public void SetCardStat()
  {
    textMinusHealth.text = minusHealthVal.ToString();

    // minusHealthVal 값 설정
    if(minusHealthVal > 0)
      textMinusHealth.alpha = 1f;
    else {
      textMinusHealth.alpha = 0f;
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
    //player 값 변경
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    playerObj.GetComponent<CardPlayer>().healthVal -= this.minusHealthVal;
    playerObj.GetComponent<CardPlayer>().SetCardStat();

    // 독우물효과 제거
    playerObj.GetComponent<CardPlayer>().minusHealthByTurn = 0;

    // 만두효과 제거
    playerObj.GetComponent<CardPlayer>().addHealthByTurn = 0;

    //meat 값 변경
    this.minusHealthVal = 0;
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
