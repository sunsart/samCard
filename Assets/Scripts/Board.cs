using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
  //카드 이동방향 enum 값으로 설정
  public enum Direction
  {
      Right,
      Left,
      Up,
      Down
  }
  public Direction moveDir;

  [SerializeField] private GameObject cardPlayerPrefab;
  [SerializeField] private GameObject cardEnemyPrefab;

  [SerializeField] private GameObject cells;
  [SerializeField] private GameObject cards;

  GameObject playerObj;
  public LayerMask cardLayer;
  public bool isCardsMoving = false;


  void Start()
  {
    InitBoard();
  }

  void InitBoard() 
  {
    for(int i=0; i<this.cells.transform.childCount; i++)
    {
      Vector2 position = this.cells.transform.GetChild(i).transform.position;

      if(i == 4)
      {
        GameObject playerCard = Instantiate(cardPlayerPrefab, position, quaternion.identity);
        playerCard.transform.parent = this.cards.transform;
        continue;
      }

      GameObject enemyCard = Instantiate(cardEnemyPrefab, position, quaternion.identity);
      enemyCard.transform.parent = this.cards.transform;
    }
    this.playerObj = GameObject.FindGameObjectWithTag("Player");
  }
 
  public bool IsNeighborPlayer(GameObject clickedObj)
  {
    bool isNeighbor = false;

    RaycastHit2D hit = Physics2D.Raycast(clickedObj.transform.position, Vector2.left, 2.0f, LayerMask.GetMask("Card"));
    if(hit.collider != null && hit.collider.tag == "Player")
    {
      Debug.Log(" 오른쪽에 있음");
      this.moveDir = Direction.Right;
      isNeighbor = true;
      return isNeighbor;
    }

    hit = Physics2D.Raycast(clickedObj.transform.position, Vector2.right, 2.0f, LayerMask.GetMask("Card"));
    if(hit.collider != null && hit.collider.tag == "Player")
    {
      Debug.Log(" 왼쪽에 있음");
      this.moveDir = Direction.Left;
      isNeighbor = true;
      return isNeighbor;
    }

    hit = Physics2D.Raycast(clickedObj.transform.position, Vector2.up, 2.0f, LayerMask.GetMask("Card"));
    if(hit.collider != null && hit.collider.tag == "Player")
    {
      Debug.Log(" 아래쪽에 있음");
      this.moveDir = Direction.Down;
      isNeighbor = true;
      return isNeighbor;
    }

    hit = Physics2D.Raycast(clickedObj.transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Card"));
    if(hit.collider != null && hit.collider.tag == "Player")
    {
      Debug.Log(" 위쪽에 있음");
      this.moveDir = Direction.Up;
      isNeighbor = true;
      return isNeighbor;
    }

    return isNeighbor;
  }

  public void ArrangeBoard()
  {
      MoveCard();
  }

  private void MoveCard()
  {
    //카드 이동중에는 카드 터치를 먹으면 안됨
    this.isCardsMoving = true;

    for(int i=0; i<this.cards.transform.childCount; i++)
    {
      GameObject obj = this.cards.transform.GetChild(i).gameObject;
      if(CanMoveDirection(obj))
      {
        Vector2 movePos;
        if(this.moveDir == Direction.Left)
        {
          movePos = new Vector2(obj.transform.position.x - 2f, obj.transform.position.y);
          obj.transform.DOMove(movePos, Settings.moveCardSpeed).OnComplete(MoveCard);
          return;
        }
        else if(this.moveDir == Direction.Right)
        {
          movePos = new Vector2(obj.transform.position.x + 2f, obj.transform.position.y);
          obj.transform.DOMove(movePos, Settings.moveCardSpeed).OnComplete(MoveCard);
          return;
        }
        else if(this.moveDir == Direction.Up)
        {
          movePos = new Vector2(obj.transform.position.x, obj.transform.position.y + 2.5f);
          obj.transform.DOMove(movePos, Settings.moveCardSpeed).OnComplete(MoveCard);
          return;
        }
        else if(this.moveDir == Direction.Down)
        {
          movePos = new Vector2(obj.transform.position.x, obj.transform.position.y - 2.5f);
          obj.transform.DOMove(movePos, Settings.moveCardSpeed).OnComplete(MoveCard);
          return;
        }
      }
    }
    //더 이상 이동할 카드가 없으면
    SpawnCardEmptyCell();
  }

  public bool CanMoveDirection(GameObject obj)
  {
    bool canMove = false;

    RaycastHit2D hit;
    if(this.moveDir == Direction.Left)
    {
      hit = Physics2D.Raycast(obj.transform.position, Vector2.left, 2.0f, LayerMask.GetMask("Card"));
      if(hit.collider == null)
      {
        Debug.Log(" 이동가능");
        canMove = true;
        return canMove;
      }
    }
    else if(this.moveDir == Direction.Right)
    {
      hit = Physics2D.Raycast(obj.transform.position, Vector2.right, 2.0f, LayerMask.GetMask("Card"));
      if(hit.collider == null)
      {
        Debug.Log(" 이동가능");
        canMove = true;
        return canMove;
      }
    }
    else if(this.moveDir == Direction.Up)
    {
      hit = Physics2D.Raycast(obj.transform.position, Vector2.up, 2.0f, LayerMask.GetMask("Card"));
      if(hit.collider == null)
      {
        Debug.Log(" 이동가능");
        canMove = true;
        return canMove;
      }
    }
    else if(this.moveDir == Direction.Down)
    {
      hit = Physics2D.Raycast(obj.transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Card"));
      if(hit.collider == null)
      {
        Debug.Log(" 이동가능");
        canMove = true;
        return canMove;
      }
    }

    return canMove; 
  }

  //비어있는 cell 에 새로운 카드를 생성함
  private void SpawnCardEmptyCell()
  {
    for(int i=0; i<this.cells.transform.childCount; i++) 
    {
      GameObject obj = this.cells.transform.GetChild(i).gameObject;
      if(Physics2D.OverlapCircle(obj.transform.position, 0.5f, cardLayer) == false)
      {
        GameObject enemyCard = Instantiate(cardEnemyPrefab, obj.transform.position, quaternion.identity);
        enemyCard.transform.parent = this.cards.transform;

        this.isCardsMoving = false;
        return;
      }
    }
  }

}
