using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public float maxSpeed = 5f;
    public GameObject fire;
    public UIDocument UIDocument;
    public GameObject ExplosionEffect;
    
    private float elapsedTime = 0f;
    private int score=0;
    private int highScore=0;
    private Rigidbody2D rb;
    private Label scoreText;
    private Label highScoreText;
    private Button restartButton;
    
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        fire.SetActive(false);
        scoreText=UIDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highScoreText=UIDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        
        restartButton=UIDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked+=RestartGame;
        
        highScore=PlayerPrefs.GetInt("HighScore",0);
        highScoreText.text="High Score:"+highScore;
    }
    
    //玩家行为
    void MovePlayer()
    {
        //控制加速火焰
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            fire.SetActive(true);
        }else if(Mouse.current.leftButton.wasReleasedThisFrame){
            fire.SetActive(false);
        }
        
        //控制移动
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - rb.position).normalized;
            
            transform.up = direction;
            rb.AddForce(direction * speed);
            
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
    }
    //更新分数
    void UpdateScore()
    {
        //计时器
        elapsedTime+=Time.deltaTime;
        score = (int)(elapsedTime * 10);
        scoreText.text = "Score:"+score;
    }

    private void Update()
    {
        MovePlayer();
        UpdateScore();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        GameObject effect=Instantiate(ExplosionEffect, transform.position, transform.rotation);
        Destroy(effect,1.0f);
        restartButton.style.display = DisplayStyle.Flex;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore",highScore);
            PlayerPrefs.Save();
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
