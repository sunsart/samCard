using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditor.Tilemaps;

public class Card : MonoBehaviour
{
  [SerializeField] private Sprite frontSprite;
  [SerializeField] private Sprite backSprite;
  [SerializeField] private SpriteRenderer spriteRenderer;

  [SerializeField] protected TextMeshPro textHealth;
  [SerializeField] protected TextMeshPro textAttack;

  [HideInInspector] public int posX;
  [HideInInspector] public int posY;

  protected bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  protected bool isFlipping = false;  // 앞면으로 뒤집어지는중인지 여부


  //기본적인 카드의 flip, moveTo ...
  void Start()
  {
    //자식카드에서 구현
  }

  public void FlipCard() 
  {
    isFlipping = true;

    Vector3 originalScale = transform.localScale;
    Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);
    transform.DOScale(targetScale, 0.2f).OnComplete(() => 
    {
      isFlipped = !isFlipped;

      if(isFlipped)
      {
        spriteRenderer.sprite = frontSprite;

        textHealth.alpha = 1f;
        if(textAttack != null)
          textAttack.alpha = 1f;
      }

        transform.DOScale(originalScale, 0.2f).OnComplete(() => { isFlipping = false; });
    });
  }

  //public void MoveCard(GameObject target)
  public void MoveCard()
  {
    // float spaceY = 2.5f;
    // float spaceX = 2f;
    // float positionX = (col - (int)(colCount/2)) * spaceX;
    // float positionY = (row - (int)(rowCount/2)) * spaceY;
    // Vector3 pos = new Vector3(positionX, positionY, 0f);
    // transform.DOMove(target.transform.position, 0.5f);
  }



}
