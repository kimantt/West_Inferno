using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy; // 생성할 아이템들
    public Transform standardTransform; // 플레이어의 트랜스폼

    private float lastSpawnTime; // 마지막 생성 시점
    public float maxDistance = 40f; // 플레이어 위치로부터 아이템이 배치될 최대 반경

    private float timeBetSpawn; // 생성 간격

    [SerializeField] private float timeBetSpawnMax; // 최대 시간 간격
    [SerializeField] private float timeBetSpawnMin; // 최소 시간 간격

    [SerializeField] private List<GameObject> monsters = new List<GameObject>();

    [SerializeField] private Text monsterCount;
    [SerializeField] private Text killText;
    [SerializeField] private int killCount;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        timeBetSpawnMax = 5f;
        timeBetSpawnMin = 3f;

        // 생성 간격과 마지막 생성 시점 초기화
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;

        killCount = 0;
    }


    // 주기적으로 아이템 생성 처리 실행
    private void Update()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn && standardTransform != null)
        {
            lastSpawnTime = Time.time; // 마지막 생성 시간 갱신
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax); // 생성 주기를 랜덤으로 변경
            Spawn(); // 실제 아이템 생성
        }

        monsterCount.text = "Monster : " + monsters.Count;
        killText.text = "Kill : " + killCount;
    }

    // 실제 아이템 생성 처리
    private void Spawn()
    {
        // 플레이어 근처의 네브 메쉬위의 랜덤 위치를 가져옵니다.
        Vector3 spawnPosition = Utility.GetRandomPointOnNavMesh(standardTransform.position, maxDistance, NavMesh.AllAreas);
        spawnPosition += Vector3.up * 0.5f; // 바닥에서 0.5만큼 위로 올립니다.

        // 아이템 중 하나를 무작위로 골라 랜덤 위치에 생성합니다.
        GameObject mon = Instantiate(enemy, spawnPosition, Quaternion.identity);
        monsters.Add(mon);
    }

    public void RemoveMonsterFromList(GameObject monster)
    {
        if (monsters.Contains(monster))
        {
            monsters.Remove(monster);
            killCount += 1;
        }
    }

    public int getKillCount() { return killCount; }
}
