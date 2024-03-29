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

  public Vector2[] cardPositions = new Vector2[9];

  //카드 오브젝트를 저장할 이차원배열
  [HideInInspector] GameObject [,] cellArr = new GameObject[3,3];
  [HideInInspector] GameObject [,] cardArr = new GameObject[3,3];

  [SerializeField] private GameObject cellPrefab;
  [SerializeField] private GameObject cardPlayerPrefab;
  [SerializeField] private GameObject cardEnemyPrefab;

  [HideInInspector] public GameObject cardPlayerObj;

  void Start()
  {
    InitBoard();
  }

  void InitBoard() 
  {
    for(int i=0; i<cardPositions.Length; i++)
    {
      Vector2 pos = cardPositions[i];

      if(i==4)
      {
        this.cardPlayerObj = Instantiate(cardPlayerPrefab, pos, quaternion.identity);
        cardPlayerObj.GetComponent<CardPlayer>().posNum = i;
        continue;
      }

      GameObject cardEnemyObj = Instantiate(cardEnemyPrefab, pos, quaternion.identity);
      cardEnemyObj.GetComponent<CardEnemy>().posNum = i;
    }
  }

  public bool IsNeighbor(int clickedCardNum) 
  {
    bool isNeighbor = false;

    int playerNum = this.cardPlayerObj.GetComponent<Card>().posNum;
    int enemyNum = clickedCardNum;

    if ((playerNum - enemyNum)==1 && (playerNum % 3) != 0)
    {
      moveDir = Direction.Left;
      isNeighbor = true;
      Debug.Log("왼쪽");
    }
    else if ((playerNum - enemyNum)==-1 && (playerNum % 3) != 2)
    {
      moveDir = Direction.Right;
      isNeighbor = true;
      Debug.Log("오른쪽");
    } 
    else if ((playerNum - enemyNum)==3 && playerNum >= 3)
    {
      moveDir = Direction.Up;
      isNeighbor = true;
      Debug.Log("위쪽");
    } 
    else if ((playerNum - enemyNum)==-3 && playerNum <= 5)
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
 
  // public void MoveCardChain()
  // {
  //   //이동방향은 moveDir
  //   //왼쪽 : col 값에서 -1
  //   //오른쪽
  //   //아래쪽
  //   //위쪽

  //   int rowCount = 3;
  //   int colCount = 3;
  //   for(int row=0; row<rowCount; row++) 
  //   {
  //     for(int col=0; col<colCount; col++)
  //     {
  //       //배열에서 차례대로 하나씩 꺼내서
  //       GameObject obj = cardArr[row, col];

  //       //moveDir 방향으로 이동할 수 있는지 확인 (moveDir 방향에 오브젝트가 있는지 확인)
  //       if(col-1 < 0)
  //         continue;

  //       if(cardArr[row, col-1] == null)
  //       {
  //         Debug.Log("xxx");
  //         //왼쪽으로 이동가능
  //         GameObject targetObj = cellArr[row, col-1];
  //         float posX = targetObj.transform.position.x;
  //         float posY = targetObj.transform.position.y;
  //         Vector3 pos = new Vector3(posX, posY, 0f);
  //         obj.GetComponent<Card>().MoveCard(pos);

  //         cardArr[row, col] = null;
  //         continue;
  //       }

  //       if(col+1 == colCount)
  //       {
  //         //카드 이동으로 마지막 배열자리에 카드 생성
  //         float spaceY = 2.5f;
  //         float spaceX = 2f;
  //         float positionX = (col - (int)(colCount/2)) * spaceX;
  //         float positionY = (row - (int)(rowCount/2)) * spaceY;
  //         Vector3 pos = new Vector3(positionX, positionY, 0f);
  //         GameObject cardObj = Instantiate(cardEnemyPrefab, pos, quaternion.identity);
  //         Card card = cardObj.GetComponent<Card>();
  //         card.posX = col;
  //         card.posY = row;
  //         cardArr[row, col] = cardObj;
  //       }
  //     }
  //   }

  //   //fromObj.GetComponent<Card>().MoveCard(toObj);
  //   //이동후 위치 갱신 필요
  //   //fromObj.GetComponent<Card>().posX = toObj.GetComponent<Card>().posX;
  //   //fromObj.GetComponent<Card>().posY = toObj.GetComponent<Card>().posY;
  // }

  public void DestroyCard(GameObject obj)
  {
    //cardArr[obj.GetComponent<Card>().posX, obj.GetComponent<Card>().posY] = null;
    MovePlayerCard(obj.GetComponent<Card>().posNum);
    Destroy(obj);
    //MoveCardChain();
  }

  public void MovePlayerCard(int num)
  {
    Vector2 toMovePos = this.cardPositions[num];
    this.cardPlayerObj.GetComponent<Card>().MoveCard(toMovePos);

    //플레이어의 moveDir 방향을 알고 있음
    //플레이어 왼쪽 이동 > 플레이어 오른쪽 카드 존재 > 오른쪽 카드 왼쪽으로 한칸 이동
    //플레이어 왼쪽 이동 > 플레이어 오른쪽 카드 부존재 > 새로운 카드 생성 
    //같은 방식으로
    NextCardMove();
  }

  public void NextCardMove()
  {

  }

}
