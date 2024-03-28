using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

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

  //카드 오브젝트를 저장할 이차원배열
  [HideInInspector] GameObject [,] cellArr = new GameObject[3,3];
  [HideInInspector] GameObject [,] cardArr = new GameObject[3,3];

  [SerializeField] private GameObject cellPrefab;
  [SerializeField] private GameObject cardPlayerPrefab;
  [SerializeField] private GameObject cardEnemyPrefab;

  [HideInInspector] public GameObject cardPlayerObj;

  void Start()
  {
    InitCell();
    InitBoard();
  }

  void InitCell()
  {
    float spaceY = 2.5f;
    float spaceX = 2f;
    int rowCount = 3;
    int colCount = 3;
    
    for(int row=0; row<rowCount; row++) 
    {
      for(int col=0; col<colCount; col++)
      {
        float positionX = (col - (int)(colCount/2)) * spaceX;
        float positionY = (row - (int)(rowCount/2)) * spaceY;
        Vector3 pos = new Vector3(positionX, positionY, 0f);

        GameObject cellObj = Instantiate(cellPrefab, pos, quaternion.identity);
        Cell cell = cellObj.GetComponent<Cell>();
        cell.posX = col;
        cell.posY = row;
        cellArr[row, col] = cellObj;
      }
    }
  }

  void InitBoard() 
  {
    int rowCount = 3;
    int colCount = 3;
    for(int row=0; row<rowCount; row++) 
    {
      for(int col=0; col<colCount; col++)
      {
        GameObject posObj = cellArr[row, col];
        Vector2 pos = new Vector3(posObj.transform.position.x, posObj.transform.position.y);

        if (row==1 && col==1)
        {
          cardPlayerObj = Instantiate(cardPlayerPrefab, pos, quaternion.identity);
          Card cardPlayer = cardPlayerObj.GetComponent<CardPlayer>();
          cardPlayer.posX = col;
          cardPlayer.posY = row;
          cardArr[row, col] = cardPlayerObj;
          continue;
        }

        GameObject cardObj = Instantiate(cardEnemyPrefab, pos, quaternion.identity);
        Card card = cardObj.GetComponent<Card>();
        card.posX = col;
        card.posY = row;
        cardArr[row, col] = cardObj;
      }
    }
  }

  // void InitBoard() 
  // {
  //   float spaceY = 2.5f;
  //   float spaceX = 2f;

  //   int rowCount = 3;
  //   int colCount = 3;
  //   for(int row=0; row<rowCount; row++) 
  //   {
  //     for(int col=0; col<colCount; col++)
  //     {
  //       float positionX = (col - (int)(colCount/2)) * spaceX;
  //       float positionY = (row - (int)(rowCount/2)) * spaceY;
  //       Vector3 pos = new Vector3(positionX, positionY, 0f);

  //       if (row==1 && col==1)
  //       {
  //         cardPlayerObj = Instantiate(cardPlayerPrefab, pos, quaternion.identity);
  //         Card cardPlayer = cardPlayerObj.GetComponent<CardPlayer>();
  //         cardPlayer.posX = col;
  //         cardPlayer.posY = row;
  //         cardArr[row, col] = cardPlayerObj;
  //         continue;
  //       }

  //       GameObject cardObj = Instantiate(cardEnemyPrefab, pos, quaternion.identity);
  //       Card card = cardObj.GetComponent<Card>();
  //       card.posX = col;
  //       card.posY = row;
  //       cardArr[row, col] = cardObj;
  //     }
  //   }
  // }

  public bool IsNeighbor(int x, int y) 
  {
    bool isNeighbor = false;

    int playerPosX = cardPlayerObj.GetComponent<CardPlayer>().posX;
    int playerPosY = cardPlayerObj.GetComponent<CardPlayer>().posY;
    int cardPosX = x;
    int cardPosY = y;

    if((playerPosX-1)==cardPosX && playerPosY==cardPosY) {
      moveDir = Direction.Left;
      isNeighbor = true;
    }
    if((playerPosX+1)==cardPosX && playerPosY==cardPosY) {
      moveDir = Direction.Right;
      isNeighbor = true;
    }
    if((playerPosY-1)==cardPosY && playerPosX==cardPosX) {
      moveDir = Direction.Down;
      isNeighbor = true;
    }
    if((playerPosY+1)==cardPosY && playerPosX==cardPosX) {
      moveDir = Direction.Up;
      isNeighbor = true;
    }

    return isNeighbor;
  }

  //public void MoveCardChain(GameObject fromObj, GameObject toObj)
  public void MoveCardChain()
  {
    //이동방향은 moveDir
    //왼쪽 : col 값에서 -1
    //오른쪽
    //아래쪽
    //위쪽

    int rowCount = 3;
    int colCount = 3;
    for(int row=0; row<rowCount; row++) 
    {
      for(int col=0; col<colCount; col++)
      {
        //배열에서 차례대로 하나씩 꺼내서
        GameObject obj = cardArr[row, col];

        //moveDir 방향으로 이동할 수 있는지 확인 (moveDir 방향에 오브젝트가 있는지 확인)
        if(col-1 < 0)
          continue;

        if(cardArr[row, col-1] == null)
        {
          //왼쪽으로 이동가능
          obj.GetComponent<Card>().MoveCard();
          cardArr[row, col] = null;
        }

        if(col+1 == colCount)
        {
          //카드 이동으로 마지막 배열자리에 카드 생성
          float spaceY = 2.5f;
          float spaceX = 2f;
          float positionX = (col - (int)(colCount/2)) * spaceX;
          float positionY = (row - (int)(rowCount/2)) * spaceY;
          Vector3 pos = new Vector3(positionX, positionY, 0f);
          GameObject cardObj = Instantiate(cardEnemyPrefab, pos, quaternion.identity);
          Card card = cardObj.GetComponent<Card>();
          card.posX = col;
          card.posY = row;
          cardArr[row, col] = cardObj;
        }
      }
    }

    //fromObj.GetComponent<Card>().MoveCard(toObj);
    //이동후 위치 갱신 필요
    //fromObj.GetComponent<Card>().posX = toObj.GetComponent<Card>().posX;
    //fromObj.GetComponent<Card>().posY = toObj.GetComponent<Card>().posY;

  
  }
}
