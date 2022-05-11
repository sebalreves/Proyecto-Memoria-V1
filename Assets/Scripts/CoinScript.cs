using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {
    public static int totalCoins = 0;
    public static int coinsCollected = 0;
    private bool collected;
    // Start is called before the first frame update
    void Start() {
        totalCoins++;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!collected && other.CompareTag("Player")) {
            // Debug.Log("Player hit");
            collected = true;
            coinsCollected++;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            Destroy(gameObject, 1f);
        }
    }
}
