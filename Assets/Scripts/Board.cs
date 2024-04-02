using System;
using System.Collections;
using System.Collections.Generic;
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

  private GameObject[,] cardArr = new GameObject[3,3];

  [SerializeField] private GameObject cardPlayerPrefab;
  [SerializeField] private GameObject cardEnemyPrefab;


  void Start()
  {
    InitBoard();
  }

  void InitBoard() 
  {
    for(int i=0; i<3; i++)
    {
      for(int j=0; j<3; j++)
      {
        Vector2 cardPosition = GetCardPosition(i, j);

        if(i==1 && j==1)
        {
          GameObject cardPlayerObj = Instantiate(cardPlayerPrefab, cardPosition, quaternion.identity);
          cardPlayerObj.GetComponent<Card>().posX = i;
          cardPlayerObj.GetComponent<Card>().posY = j;
          this.cardArr[i, j] = cardPlayerObj;
          continue;
        }

        GameObject cardEnemyObj = Instantiate(cardEnemyPrefab, cardPosition, quaternion.identity);
        cardEnemyObj.GetComponent<Card>().posX = i;
        cardEnemyObj.GetComponent<Card>().posY = j;
        this.cardArr[i, j] = cardEnemyObj;
      }
    }
  }

  public bool IsNeighbor(GameObject targetObj) 
  {
    bool isNeighbor = false;

    GameObject cardPlayerObj = GameObject.FindGameObjectWithTag("Player");
    int playerPosX = cardPlayerObj.GetComponent<Card>().posX;
    int playerPosY = cardPlayerObj.GetComponent<Card>().posY;

    int targetPosX = targetObj.GetComponent<Card>().posX;
    int targetPosY = targetObj.GetComponent<Card>().posY;

    //0,0  1,0  2,0
    //0,1  1,1  2,1
    //0,2  1,2  2,2

    if ((playerPosY==targetPosY) && (playerPosX-targetPosX==1))
    {
      moveDir = Direction.Left;
      isNeighbor = true;
      Debug.Log("왼쪽");
    } 
    else if ((playerPosY==targetPosY) && (playerPosX-targetPosX==-1))
    {
      moveDir = Direction.Right;
      isNeighbor = true;
      Debug.Log("오른쪽");
    }
    else if ((playerPosX==targetPosX) && (playerPosY-targetPosY==1))
    {
      moveDir = Direction.Up;
      isNeighbor = true;
      Debug.Log("위쪽");
    }
    else if ((playerPosX==targetPosX) && (playerPosY-targetPosY==-1))
    {
      moveDir = Direction.Down;
      isNeighbor = true;
      Debug.Log("아래쪽");
    }
    else 
    {
      Debug.Log("떨어져 있음");
    }
    return isNeighbor;
  }
 
  public void ArrangeBoard(GameObject obj)
  {
    int targetNum = obj.GetComponent<Card>().posNum;

    //enemy 제거
    Destroy(obj);

    //왼쪽 이동이라고 하면.......
    foreach(GameObject moveObj in this.cardList)
    {
      int i = moveObj.GetComponent<Card>().posNum;

      if((i % 3 != 0) && (targetNum < i) && ((i/3)==(targetNum/3)))
      {
        moveObj.GetComponent<Card>().MoveCard(this.cardPositions[i-1]);
        moveObj.GetComponent<Card>().posNum--;
      }

      //오른쪽 맨 끝이면 그 자리에 새로운 카드 생성
      if((i % 3 == 2) && (targetNum < i) && ((i/3)==(targetNum/3)))
      {
        StartCoroutine(SpawnCard(i));
      }
    }
  } 

  IEnumerator SpawnCard(int posNum)
  {
    yield return new WaitForSeconds(0.5f);
    GameObject cardEnemyObj = Instantiate(cardEnemyPrefab, cardPositions[posNum], quaternion.identity);
    cardEnemyObj.GetComponent<Card>().posNum = posNum;
  }

  private Vector2 GetCardPosition(int x, int y)
  {
    //x=0 -2
    //x=1 0
    //x=2 2

    //y=0 2.5
    //y=1 0
    //y=2 -2.5

    float spaceX = 2f;
    float spaceY = -2.5f;

    float posX = (x-1) * spaceX;
    float posY = (y-1) * spaceY;

    return new Vector2(posX, posY);
  }

}
