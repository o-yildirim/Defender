using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class UIManagers : MonoBehaviour
{
    

    public Text damageCostText;
    public Text asCostText;
    public Text knockbackCostText;
    public Text buddyCostText;

    public Text damageLevelText;
    public Text asLevelText;
    public Text knockbackLevelText;

    public Text shotgunCostText;
    public Text shotgunUpgradeInfoText;
    public Text buddyUpgradeInfoText;

    public Button shotgunB;
    public Button attackSpeedB;
    public Button knockbackB;
    public Button buddyButton;
    

    public GameObject standartGun;
    public GameObject shotGun;

    public int damageUpgradeCost = 1;
    public int knockbackUpgradeCost = 1;
    public int attackSpeedUpgradeCost = 1;
    public int shotGunUpgradeCost = 10;
    public int buddyUpgradeCost = 300;

    private int damageLevel = 0;
    private int knockbackLevel = 0;
    private int attackSpeedLevel = 0;
    private bool shotgunUpgraded = false;


    private int maxKnockbackLevel = 5;
    private int maxAttackSpeedLevel = 5;

    private void Start()
    {
        damageCostText.text = damageUpgradeCost.ToString();
        knockbackCostText.text = knockbackUpgradeCost.ToString();
        asCostText.text = attackSpeedUpgradeCost.ToString();
        shotgunCostText.text = shotGunUpgradeCost.ToString();
        buddyCostText.text = buddyUpgradeCost.ToString();
    

        
    }

    
    public void upgradeDamage()
    {
        if (GameController.instance.playerGold >= damageUpgradeCost)
        {
            GameController.instance.playerGold -= damageUpgradeCost;
            GameController.instance.goldText.text = GameController.instance.playerGold.ToString();
            damageUpgradeCost += 30;
            damageLevel++;
            GameController.instance.playerDamage += 15;
            damageCostText.text = damageUpgradeCost.ToString();
            damageLevelText.text = damageLevel + "/-";
            
        }
    }


    public void upgradeKnockBack()
    {
        if (knockbackLevel < maxKnockbackLevel && GameController.instance.playerGold >= knockbackUpgradeCost)
        {
            GameController.instance.playerGold -= knockbackUpgradeCost;
            GameController.instance.goldText.text = GameController.instance.playerGold.ToString();
            knockbackUpgradeCost += 25;
            knockbackLevel++;
            GameController.instance.knockBackForce += 0.3f;
            knockbackCostText.text = knockbackUpgradeCost.ToString();
            knockbackLevelText.text = knockbackLevel + "/" + maxKnockbackLevel;
        }
        if (knockbackLevel == maxKnockbackLevel)
        {
            
           knockbackCostText.enabled = false;
           knockbackB.gameObject.SetActive(false);
        }

    }
    public void upgradeAttackSpeed()
    {
        if (attackSpeedLevel < maxAttackSpeedLevel && GameController.instance.playerGold >= attackSpeedUpgradeCost)
        {
            GameController.instance.playerGold -= attackSpeedUpgradeCost;
            GameController.instance.goldText.text = GameController.instance.playerGold.ToString();
            attackSpeedUpgradeCost += 35;
            attackSpeedLevel++;
            GameController.instance.attackSpeed -= 0.015f;
            asCostText.text = attackSpeedUpgradeCost.ToString();
            asLevelText.text = attackSpeedLevel + "/" + maxAttackSpeedLevel;
        }
       if(attackSpeedLevel == maxAttackSpeedLevel)
        {
            asCostText.enabled = false;
            attackSpeedB.gameObject.SetActive(false);
        }


    }

    public void shotgunUpgrade()
    {

        
        if (!shotgunUpgraded && GameController.instance.playerGold >= shotGunUpgradeCost)
        {
            GameController.instance.playerGold -= shotGunUpgradeCost;
            GameController.instance.goldText.text = GameController.instance.playerGold.ToString();
            shotgunUpgraded = true;
            shotgunUpgradeInfoText.text = "Upgraded!";
            shotgunCostText.enabled = false;
    
            shotgunB.gameObject.SetActive(false);

            standartGun.SetActive(false);
            GameObject.Find("Player").GetComponent<PlayerShooting>().enabled = false;


            shotGun.SetActive(true);
            GameObject.Find("Player").GetComponent<PlayerShotgunShooting>().enabled = true;

           // GameController.instance.enemySpawnRate -= 1f;
            GameController.instance.enemyHealth = 140f;

        }
         
         

    }
    public void buddyUpgrade()
    {


        if ( GameController.instance.playerGold >= buddyUpgradeCost)
        {
            GameController.instance.playerGold -= buddyUpgradeCost;
            GameController.instance.goldText.text = GameController.instance.playerGold.ToString();

            buddyButton.gameObject.SetActive(false);

            buddyUpgradeInfoText.text = "Operating!";
            buddyCostText.enabled = false;
            buddyUpgradeCost += 120;
            buddyCostText.text = buddyUpgradeCost.ToString();

            GameController.instance.summonBuddies();
         
        }



    }

}
