using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    private const int STARTING_HP = 4;

    public Sprite dmgSprite;
    public int hp = STARTING_HP;

    private SpriteRenderer spriteRenderer;


	// Use this for initialization
	void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
