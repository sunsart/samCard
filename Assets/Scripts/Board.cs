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
    Destroy(obj);

    int targePosX = obj.GetComponent<Card>().posX;
    int targePosY = obj.GetComponent<Card>().posY;

    //왼쪽 이동일때, 이동만
    for(int i=0; i<3; i++)
    {
      for(int j=0; j<3; j++)
      {
        GameObject moveObj = cardArr[i, j];
        int movePosX = moveObj.GetComponent<Card>().posX;
        int movePosY = moveObj.GetComponent<Card>().posY;

        if((movePosY == targePosY) && (movePosX > targePosX))
        {
          StartCoroutine(WaitMove());
          moveObj.GetComponent<Card>().MoveCard(GetCardPosition(i-1, j));

          moveObj.GetComponent<Card>().posX = i-1;
          moveObj.GetComponent<Card>().posY = j;
          cardArr[i-1, j] = moveObj;
          cardArr[i, j] = null;
        }
      }
    }

    //생성만
    for(int i=0; i<3; i++)
    {
      for(int j=0; j<3; j++)
      {
        if(cardArr[i, j] == null)
        {
          StartCoroutine(SpawnCard(GetCardPosition(i, j)));
          break;
        }
      }
    }
  } 

  IEnumerator WaitMove()
  {
    yield return new WaitForSeconds(0.5f);
  }

  IEnumerator SpawnCard(Vector2 spawnPos)
  {
    yield return new WaitForSeconds(0.5f);
    GameObject spawnObj = Instantiate(cardEnemyPrefab, spawnPos, quaternion.identity);
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
