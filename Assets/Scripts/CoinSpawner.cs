using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int coinCount = 20;

    void Start()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(-9f, 9f));
            Instantiate(coinPrefab, pos, Quaternion.identity);
        }
    }
}
