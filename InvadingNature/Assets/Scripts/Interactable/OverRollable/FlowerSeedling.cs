using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeedling : OverRollable
{
    public Flower flower;
    public float minFloweringTime = 2f;
    public float maxFloweringTime = 4f;
    float floweringTimer = 0f;
    float fTimerThreshold = 0f;

    protected override void Start() {
        base.Start();
        floweringTimer = 0f;
        fTimerThreshold = Random.Range(minFloweringTime, maxFloweringTime);
    }

    protected override void Update() {
        base.Update();
        floweringTimer += Time.deltaTime;
        if (floweringTimer > fTimerThreshold) {
            Flower();
        }
    }

    private void Flower() {
        Vector3 nPos = transform.position;
        nPos.y = flower.transform.position.y;
        Instantiate(flower, nPos, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    public override void Interact(Player p) {
        //Nothing
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
