using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  public Board board;


  void Awake()
  {
    if(instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      DestroyImmediate(gameObject);
    }
  }

  public void TakeBattle(GameObject obj)
  {
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    GameObject enemyObj = obj;

    //int >> 값의 복사가 이루어져서 서로 다른 메모리에 저장됨
    int playerHealthVal = playerObj.GetComponent<CardPlayer>().healthVal;
    int playerAttackVal = playerObj.GetComponent<CardPlayer>().attackVal;
    int enemyHealthVal = enemyObj.GetComponent<CardEnemy>().healthVal;

    if(playerAttackVal > 0) //무기를 소지하고 있을때
    {
      if(playerAttackVal >= enemyHealthVal)  //무기공격력이 적체력보다 크거나 같으면
      {
        //enemy 처리
        enemyObj.GetComponent<CardEnemy>().healthVal = enemyHealthVal - playerAttackVal;
        enemyObj.GetComponent<CardEnemy>().SetCardStat();
        if(enemyObj.GetComponent<CardEnemy>().healthVal <= 0)
          Destroy(enemyObj);

        //player 무기공격력 처리
        playerObj.GetComponent<CardPlayer>().attackVal = playerAttackVal - enemyHealthVal;
        playerObj.GetComponent<CardPlayer>().SetCardStat();
      }
      else //무기공격력이 적체력보다 작으면
      {
        //enemy 처리
        enemyObj.GetComponent<CardEnemy>().healthVal = enemyHealthVal - playerAttackVal;
        enemyObj.GetComponent<CardEnemy>().SetCardStat();

        //player 무기공격력 처리
        playerObj.GetComponent<CardPlayer>().attackVal = playerAttackVal - enemyHealthVal;
        playerObj.GetComponent<CardPlayer>().SetCardStat();
      }
    } 
    else  //무기를 소지하고 있지 않을때
    {  
      //enemy 처리
      enemyObj.GetComponent<CardEnemy>().healthVal = enemyHealthVal - playerHealthVal;
      enemyObj.GetComponent<CardEnemy>().SetCardStat();
      if(enemyObj.GetComponent<CardEnemy>().healthVal <= 0)
      {
        board.ArrangeBoard(enemyObj);
      }

      //player 처리 
      playerObj.GetComponent<CardPlayer>().healthVal = playerHealthVal - enemyHealthVal;
      playerObj.GetComponent<CardPlayer>().SetCardStat();
      if(playerObj.GetComponent<CardPlayer>().healthVal <= 0)
        Destroy(playerObj);
    }
  }


}
