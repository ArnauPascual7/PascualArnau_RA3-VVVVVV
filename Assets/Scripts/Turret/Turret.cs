using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject Bullet;
    public float SpawnInterval = 2.0f;
    public float BulletSpeed = 3f;
    private float timer;

    [SerializeField] private Transform shotPoint;

    public Stack<GameObject> BulletStack = new Stack<GameObject>();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= SpawnInterval)
        {
            if (BulletStack.Count == 0)
            {
                SpawnBullet();
            }
            else
            {
                SpawnerStackPop();
            }

            timer = 0f;
        }
    }

    private void SpawnBullet()
    {
        Bullet.GetComponent<TurretBullet>().spawner = this;

        Bullet.GetComponent<TurretBullet>().speed = BulletSpeed;

        Instantiate(Bullet, shotPoint.position, transform.rotation);
    }

    public void SpawnerStackPush(GameObject go)
    {
        BulletStack.Push(go);
        go.SetActive(false);
    }

    private void SpawnerStackPop()
    {
        GameObject go = BulletStack.Pop();

        go.SetActive(true);

        go.transform.position = shotPoint.position;
        go.transform.rotation = Quaternion.identity;

        go.GetComponent<TurretBullet>().speed = BulletSpeed;
    }
}
