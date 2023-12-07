using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {

    public double money;
    public double damagePerClick;
    public double health;
    public double maxHP;

    public int stage;
    public int stageMax;
    public int kills;
    public int killsStagesClear;

    public float timer;
    public float timerMax;

    public TMP_Text moneyText;
    public TMP_Text damagePerClickText;
    public TMP_Text stageText;
    public TMP_Text killsText;
    public TMP_Text healthText;
    public TMP_Text timerText;

    public GameObject back;
    public GameObject forward;
    public GameObject enemy;

    private Vector3 enemyScale;
    public float shrinkScale = 0.95f;
    private int counterHit = 1;

    public Image healthBar;
    public Image timerBar;

    public Sprite[] spriteList;
    public void Start() {
        enemyScale = enemy.gameObject.transform.localScale;
        damagePerClick = 30;
        stage = 1;
        stageMax = 1;
        maxHP = 10;
        health = maxHP;
        killsStagesClear = 10;
    }
    public void Update() {
            
        if (stage % 5 == 0) {
            timerBar.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
            killsStagesClear = 1;
            killsText.text = kills.ToString() + "/" + killsStagesClear + "Kills";
            stageText.text = "Boss Stage - " + stage;
            timerText.text = timer.ToString("F2");             
                timer -= Time.deltaTime;
            if (timer <= 0 ) {
                stage--;
                SetMaxHP();
                health = maxHP;
            }
        } else {
            timerBar.gameObject.SetActive(false);   
            timerText.gameObject.SetActive(false);  
            timer = 30;
            killsStagesClear = 10;
            maxHP = 10 * stage;
            killsText.text = kills.ToString() + "/" + killsStagesClear + "Kills";
            stageText.text = "Stage - " + stage;
        }

        moneyText.text = "$" + money.ToString("F2");
        damagePerClickText.text = damagePerClick.ToString() + " Damage / Click";

        
        healthText.text = health.ToString("F2") + "/" + maxHP + "HP";

        healthBar.fillAmount = (float)(health / maxHP);

        if ( stage > 1 ) back.gameObject.SetActive(true);
        else back.gameObject.SetActive(false);

        if ( stage != stageMax )  forward.gameObject.SetActive(true); 
        else forward.gameObject.SetActive(false);


        
    }

    public void Hit() {
        health -= damagePerClick;
        counterHit++;
        float currentSizePercentage = (float)(health / maxHP)*0.2f + 0.8f;
        enemy.gameObject.transform.localScale = enemyScale * currentSizePercentage;
        if (health <= 0) Kills();
    }

    public void Back() {
        counterHit = 1;
        enemy.gameObject.transform.localScale = enemyScale;
        if ( stage > 1 ) stage--;
        kills = 0;


        SetMaxHP();
        SetSprite();
        health = maxHP;
    }

    public void Forward() {
        counterHit = 1;
        enemy.gameObject.transform.localScale = enemyScale;
        if ( stage < stageMax ) stage++;
        SetMaxHP();
        SetSprite();
        health = maxHP;

    }

    public void Kills() {
        counterHit = 1;
        enemy.gameObject.transform.localScale = enemyScale;
        money += maxHP ;
        if (stage == stageMax) kills++;

        if (kills >= killsStagesClear) {
            kills = 0;
            stage++;
            stageMax++;
            SetMaxHP();
        }
        SetSprite();
        health = maxHP;
    }

    public void SetMaxHP() {
        if (stage % 5 == 0) maxHP = 10 * stage + maxHP + 200;
        else maxHP = 10 * stage;
    }
    public void SetSprite() {
        if (stage % 5 != 0) {
            int randomNumber = UnityEngine.Random.Range(0, 16);
            UnityEngine.UI.Image enemyImage = enemy.GetComponent<Image>();
            enemyImage.sprite = spriteList[randomNumber];
        } else {
            int randomNumber = UnityEngine.Random.Range(16, 19);
            UnityEngine.UI.Image enemyImage = enemy.GetComponent<Image>();
            enemyImage.sprite = spriteList[randomNumber];
        }


    }

}

