using JetBrains.Annotations;
using System.Net;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using System.Collections;

public class GameController : MonoBehaviour {

    private double money;
    private double damagePerClick;
    private double health;
    private double maxHP;

    private int stage;
    private int stageMax;
    private int kills;
    private int killsStagesClear = 10;
    private int ascensionLvl = 0;
    private int multiplicatorDPS = 1;
    private int multiplicatorClick = 1;
    private double clickInShop;
    private double priceClick;

    private double dps;
    private double dpsInShop = 1;
    private double priceDPS = 10;
    //Frequence à laquelle le DPS est appliqué sur la seconde, par exemple pour une valeur de 1/100, en 1 seconde le mob va perdre dps/100 en 1/100 seconde
    private float frequencyOfDamage = 100f;

    private float timer;
    private float timeToKillBoss = 30;

    [SerializeField] private AudioSource killSoundEffect;
    [SerializeField] private AudioSource clickSoundEffect;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text damagePerClickText;
    [SerializeField] private TMP_Text DPSText;
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private TMP_Text ascensionLvlText;
    [SerializeField] private TMP_Text dpsInShopText;
    [SerializeField] private TMP_Text clickInShopText;

    [SerializeField] private GameObject back;
    [SerializeField] private GameObject forward;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject ascension;

    [SerializeField] private Animator transition;
    private Vector3 enemyScale;

    public Image healthBar;
    public Image timerBar;

    public Sprite[] spriteList;
    public void Start() {
        transition.SetTrigger("FadeOut");
        setVariable();
        InvokeRepeating("damageDPS", 1f / frequencyOfDamage, 1f / frequencyOfDamage);
    }
    public void Update() {

        if ((ascensionLvl + 1) * 50 <= stage) {
            ascension.gameObject.SetActive(true);
        } else {
            ascension.gameObject.SetActive(false);
        }
        // Boss Stages
        if (stage % 5 == 0) {
            timerBar.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
            killsStagesClear = 1;
            killsText.text = kills.ToString() + "/" + killsStagesClear + "Kills";
            stageText.text = "Boss Stage - " + stage;
            timerText.text = timer.ToString("F2") + "/" + timeToKillBoss.ToString("F0");
            timer -= Time.deltaTime;
            if (timer <= 0) {
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

        DPSText.text = dps.ToString("F2") + "DPS   ";

        dpsInShopText.text = dpsInShop.ToString("F2") + "DPS / " + priceDPS.ToString("F2") + " $";
        clickInShopText.text = clickInShop.ToString("F2") + "Click / " + priceClick.ToString("F2") + " $ ";
        moneyText.text = "$" + money.ToString("F2");
        damagePerClickText.text = damagePerClick.ToString("F2") + " Click Damage";



        ascensionLvlText.text = "Ascension lvl"  +  ascensionLvl.ToString();
        healthText.text = health.ToString("F2") + "/" + maxHP + "HP";

        healthBar.fillAmount = (float)(health / maxHP);

        if (stage > 1) back.gameObject.SetActive(true);
        else back.gameObject.SetActive(false);

        if (stage != stageMax) forward.gameObject.SetActive(true);
        else forward.gameObject.SetActive(false);



    }

    public void Hit() {
        health -= damagePerClick;
        playClickSoundEffect();
        float currentSizePercentage = (float)(health / maxHP) * 0.1f + 0.9f;
        enemy.gameObject.transform.localScale = enemyScale * currentSizePercentage;
        if (health <= 0) Kills();
    }

    public void Back() {

        enemy.gameObject.transform.localScale = enemyScale;
        if (stage > 1) stage--;
        kills = 0;


        SetMaxHP();
        SetSprite();
        health = maxHP;
    }

    public void Forward() {

        enemy.gameObject.transform.localScale = enemyScale;
        if (stage < stageMax) stage++;
        SetMaxHP();
        SetSprite();
        health = maxHP;

    }

    public void Kills() {
        playKillSoundEffect();
        enemy.gameObject.transform.localScale = enemyScale;

        money += maxHP / 3f;
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
            int randomNumber = Random.Range(0, 16);
            Image enemyImage = enemy.GetComponent<Image>();
            enemyImage.sprite = spriteList[randomNumber];
        } else {
            int randomNumber = Random.Range(16, 19);
            Image enemyImage = enemy.GetComponent<Image>();
            enemyImage.sprite = spriteList[randomNumber];
        }


    }

    public void buyDPS() {
        if (money >= priceDPS) {
            money -= priceDPS;
            dps += dpsInShop * multiplicatorDPS;


            priceDPS *= 1.3;
            dpsInShop *= 1.2;
        }
    }

    public void damageDPS() {
        health -= dps / frequencyOfDamage;
        float currentSizePercentage = (float)(health / maxHP) * 0.1f + 0.9f;
        enemy.gameObject.transform.localScale = enemyScale * currentSizePercentage;
        if (health <= 0) Kills();
    }

    public void buyClick() {
        if (money >= priceClick) {
            money -= priceClick;
            damagePerClick += clickInShop * multiplicatorClick;

            priceClick *= 1.3;
            clickInShop *= 1.2;
        }
    }

    public void ascensionUp() {
        ascensionLvl += 1;
        if (Random.Range(0, 2) == 1) multiplicatorClick *= 2;
        else multiplicatorDPS *= 2;
        StartCoroutine(loadNextScene());
        setVariable();

    }

    public IEnumerator loadNextScene() {
        transition.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.5f);
    }

    public void setVariable() {
        clickInShop = 1;
        priceClick = 10;
        dps = 0;
        priceDPS = 10;
        dpsInShop = 1;
        damagePerClick = 1 * multiplicatorClick;
        enemyScale = enemy.gameObject.transform.localScale;
        stage = 1;
        stageMax = 1;
        maxHP = 10;
        health = maxHP;
        kills = 0;
    }
    public void playKillSoundEffect() {
        killSoundEffect.Play();
    }
    public void playClickSoundEffect() {
        clickSoundEffect.Play();
    }

}

