using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEditor.Experimental.GraphView;

public class Card : MonoBehaviour
{
  [SerializeField] private Sprite frontSprite;
  [SerializeField] private Sprite backSprite;
  [SerializeField] private SpriteRenderer spriteRenderer;

  [SerializeField] protected TextMeshPro textHealth;
  [SerializeField] protected TextMeshPro textAttack;

  protected bool isFlipped = false;   // 앞면으로 뒤집어졌는지 여부
  protected bool isFlipping = false;  // 앞면으로 뒤집어지는 중인지 여부

  
  void Start()
  {
    //자식카드에서 구현
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

        textHealth.alpha = 1f;
        
        if(textAttack != null)
          textAttack.alpha = 1f;
      }

        transform.DOScale(originalScale, Settings.flipSpeed).OnComplete(() => { isFlipping = false; });
    });
  }

}
