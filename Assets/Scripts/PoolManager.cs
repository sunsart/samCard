using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class PoolManager : MonoBehaviour
{
  // Prefab을 저장하는 배열 
  public GameObject[] prefabs;

  // 오브젝트 Pool들을 저장하는 리스트
  // Prefabs와 리스트들은 일대일 관계
  private List<GameObject>[] pools;


  void Awake()
  {
    pools = new List<GameObject>[this.prefabs.Length];

    for (int i=0; i<pools.Length; i++)
    {
      // 모든 오브젝트풀 리스트를 초기화
      pools[i] = new List<GameObject>();
    }
  }

  public GameObject GetObjectFromPoll(int index, Vector2 pos)
  {
      GameObject select = null;

      foreach (GameObject obj in pools[index])
      {
        // 오브젝트가 비활성화(대기 상태)인지 확인
        if (!obj.activeSelf)
        {
          select = obj;
          select.SetActive(true);
          select.transform.position = pos;
          break;
        }
      }

      if (select == null)
      {
        select = Instantiate(prefabs[index], pos, quaternion.identity);
        pools[index].Add(select);
      }

      return select;
  }

}

