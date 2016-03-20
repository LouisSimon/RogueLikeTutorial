using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MovingObject
{

    public const int WALL_DAMAGE = 1;
    public const int POINTS_PER_FOOD = 10;
    public const int POINTS_PER_SODA = 20;
    public const float RESTART_LEVEL_DELAY = 1f;

    public Text foodText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private Animator animator;
    private Vector2 touchOrigin = -Vector2.one;
    private int _food = -1;
    private int Food
    {
        get { return _food; }
        set
        {
            string foodTextString = "Food: " + value;
            if (_food != -1)
            {
                if (_food < value)
                {
                    foodTextString = "+ " + (value - _food) + " " + foodTextString;
                }
                else if (_food > value + 1)
                {
                    foodTextString = "- " + (_food - value) + " " + foodTextString;
                }
            }
            _food = value;
            foodText.text = foodTextString;
        }
    }

    // Use this for initialization
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        Food = GameManager.instance.playerFoodPoints;
        base.Start();
    }
    
    void Update()
    {
        int horizontal = 0;
        int vertical = 0;
        if (GameManager.instance.playerTurn)
        {
            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
                horizontal = (int)Input.GetAxisRaw("Horizontal");
                vertical = (int)Input.GetAxisRaw("Vertical");

                if (horizontal != 0)
                {
                    vertical = 0;
                }
            #else
                if (Input.touchCount > 0)
                {
                    Touch myTouch = Input.touches[0];
                    if (myTouch.phase == TouchPhase.Began)
                    {
                        touchOrigin = myTouch.position;
                    }
                    else if(myTouch.phase ==TouchPhase.Ended && touchOrigin.x>=0)
                    {
                        Vector2 touchEnd = myTouch.position;
                        float x = touchEnd.x - touchOrigin.x;
                        float y = touchEnd.y - touchOrigin.y;
                        touchOrigin = -Vector2.one;
                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            horizontal = x > 0 ? 1 : -1;
                        }
                        else
                        {
                            vertical = y > 0 ? 1 : -1;
                        }
                    }
                }
            #endif

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove<Wall>(horizontal, vertical);
            }
        }
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = Food;
    }

    private void CheckIfGameOver()
    {
        if (Food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        Food--;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if (Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }
        CheckIfGameOver();
        GameManager.instance.playerTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(WALL_DAMAGE);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        Food -= loss;
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Exit":
                Invoke("Restart", RESTART_LEVEL_DELAY);
                enabled = false;
                break;
            case "Food":
                Food += POINTS_PER_FOOD;
                SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
                other.gameObject.SetActive(false);
                break;
            case "Soda":
                Food += POINTS_PER_SODA;
                SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
                other.gameObject.SetActive(false);
                break;
        }
    }
}
