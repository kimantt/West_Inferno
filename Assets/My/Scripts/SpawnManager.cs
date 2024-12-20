using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy; // ������ �����۵�
    public Transform standardTransform; // �÷��̾��� Ʈ������

    private float lastSpawnTime; // ������ ���� ����
    public float maxDistance = 40f; // �÷��̾� ��ġ�κ��� �������� ��ġ�� �ִ� �ݰ�

    private float timeBetSpawn; // ���� ����

    [SerializeField] private float timeBetSpawnMax; // �ִ� �ð� ����
    [SerializeField] private float timeBetSpawnMin; // �ּ� �ð� ����

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

        // ���� ���ݰ� ������ ���� ���� �ʱ�ȭ
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;

        killCount = 0;
    }


    // �ֱ������� ������ ���� ó�� ����
    private void Update()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn && standardTransform != null)
        {
            lastSpawnTime = Time.time; // ������ ���� �ð� ����
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax); // ���� �ֱ⸦ �������� ����
            Spawn(); // ���� ������ ����
        }

        monsterCount.text = "Monster : " + monsters.Count;
        killText.text = "Kill : " + killCount;
    }

    // ���� ������ ���� ó��
    private void Spawn()
    {
        // �÷��̾� ��ó�� �׺� �޽����� ���� ��ġ�� �����ɴϴ�.
        Vector3 spawnPosition = Utility.GetRandomPointOnNavMesh(standardTransform.position, maxDistance, NavMesh.AllAreas);
        spawnPosition += Vector3.up * 0.5f; // �ٴڿ��� 0.5��ŭ ���� �ø��ϴ�.

        // ������ �� �ϳ��� �������� ��� ���� ��ġ�� �����մϴ�.
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
