using JetBrains.Annotations;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {

    private double money;
    private double damagePerClick;
    private double health;
    private double maxHP;

    private int stage;
    private int stageMax;
    private int kills;
    private int killsStagesClear;

    private double clickInShop = 1;
    private double priceClick = 10;

    private double dps = 0;
    private double dpsInShop = 1;
    private double priceDPS = 10;
    //Frequence à laquelle le DPS est appliqué sur la seconde, par exemple pour une valeur de 1/100, en 1 seconde le mob va perdre dps/100 en 1/100 seconde
    private float frequencyOfDamage = 100f ;

    private float timer;
    private float timeToKillBoss = 30 ;

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text damagePerClickText;
    [SerializeField] private TMP_Text DPSText;
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private TMP_Text dpsInShopText;
    [SerializeField] private TMP_Text clickInShopText;

    [SerializeField] private GameObject back;
    [SerializeField] private GameObject forward;
    [SerializeField] private GameObject enemy;

    private Vector3 enemyScale;

    public Image healthBar;
    public Image timerBar;

    public Sprite[] spriteList;
    public void Start() {
        damagePerClick = 1;
        enemyScale = enemy.gameObject.transform.localScale;
        stage = 1;
        stageMax = 1;
        maxHP = 10;
        health = maxHP;
        killsStagesClear = 10;
        InvokeRepeating("damageDPS", 1f/frequencyOfDamage, 1f / frequencyOfDamage);
    }
    public void Update() {
            // Boss Stages
        if (stage % 5 == 0) {
            timerBar.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
            killsStagesClear = 1;
            killsText.text = kills.ToString() + "/" + killsStagesClear + "Kills";
            stageText.text = "Boss Stage - " + stage;
            timerText.text = timer.ToString("F2") + "/" + timeToKillBoss.ToString("F0");             
                timer -= Time.deltaTime;
            if (timer <= 0 ) {
                stage--;
                SetMaxHP();
                health = maxHP;
            }
            //Normal Stages
        } else {
            timerBar.gameObject.SetActive(false);   
            timerText.gameObject.SetActive(false);  
            timer = 30;
            killsStagesClear = 10;
            maxHP = 10 * stage;
            killsText.text = kills.ToString() + "/" + killsStagesClear + "Kills";
            stageText.text = "Stage - " + stage;
        }

        DPSText.text = dps.ToString("F2") + "DPS   " ;

        dpsInShopText.text = dpsInShop.ToString("F2") + "DPS / " + priceDPS.ToString("F2") + " $";
        clickInShopText.text = clickInShop.ToString("F2") + "Click / " + priceClick.ToString("F2") + " $ ";
        moneyText.text = "$" + money.ToString("F2");
        damagePerClickText.text = damagePerClick.ToString() + " Click Damage";




        healthText.text = health.ToString("F2") + "/" + maxHP + "HP";

        healthBar.fillAmount = (float)(health / maxHP);

        if ( stage > 1 ) back.gameObject.SetActive(true);
        else back.gameObject.SetActive(false);

        if ( stage != stageMax )  forward.gameObject.SetActive(true); 
        else forward.gameObject.SetActive(false);


        
    }

    public void Hit() {
        health -= damagePerClick;

        float currentSizePercentage = (float)(health / maxHP)*0.1f + 0.9f;
        enemy.gameObject.transform.localScale = enemyScale * currentSizePercentage;
        if (health <= 0) Kills();
    }

    public void Back() {

        enemy.gameObject.transform.localScale = enemyScale;
        if ( stage > 1 ) stage--;
        kills = 0;


        SetMaxHP();
        SetSprite();
        health = maxHP;
    }

    public void Forward() {

        enemy.gameObject.transform.localScale = enemyScale;
        if ( stage < stageMax ) stage++;
        SetMaxHP();
        SetSprite();
        health = maxHP;

    }

    public void Kills() {

        enemy.gameObject.transform.localScale = enemyScale;
        
        money += maxHP / 3f ;
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
    //Set HP Of mob per stage   
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

    /*    private int dpsBought = 0;
    private float dps = 0;
    private float dpsInShop = 1;*/

    public void buyDPS() {
        if ( money >= priceDPS ) {
            money -= priceDPS;
            dps += dpsInShop;


            priceDPS *= 1.2;
            dpsInShop *= 1.4;
        }
    }

    public void damageDPS() {
        health -= dps / frequencyOfDamage;
        float currentSizePercentage = (float)(health / maxHP) * 0.1f + 0.9f;
        enemy.gameObject.transform.localScale = enemyScale * currentSizePercentage;
        if (health <= 0) Kills();   
    }

    public void buyClick() {
        if ( money >= priceClick ) {
            money -= priceClick;
            damagePerClick += clickInShop;

            priceClick *= 1.2;
            clickInShop *= 1.4;
        }
    }

}

