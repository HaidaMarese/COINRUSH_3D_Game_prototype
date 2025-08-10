using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool isGolden = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int value = isGolden ? 5 : 1;
        GameManager.Instance.AddScore(value);

        // ? Play coin pickup sound
        AudioManager.Instance.PlaySFX(
            isGolden ? AudioManager.Instance.goldenPickup : AudioManager.Instance.coinPickup
        );

        Destroy(gameObject);
    }
}
