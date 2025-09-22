using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] private LayerMask playerLayer;

    [Header("Items")]
    public Item equippedItem;
    public Item secondaryItem;
    [SerializeField] private GameObject primaryHand;
    [SerializeField] private GameObject offHand;

    [Header("Stats")]
    public float health = 10.0f;
    public float speed { get; private set; }
    public float strength {get; private set;}
    public float distance {get; private set;}

    private float dotTimer;
    private float dotDamage;
    private float maxHealth;
    
    public void SetStats(int sp, int st, int di){
        speed = sp;
        strength = st;
        distance = di;
    }

    public void TakeDamage(float amount, float time = 0){
        if (time > 0){
            dotTimer = time;
            dotDamage = amount;
            Debug.Log("Applied DOT");
        }//Initiate DOT
        else {
            health -= amount;
            Debug.Log("Took " + amount + " damage!");
        }

        //Check health state
        if (health >= maxHealth){
            health = maxHealth;
        }else if (health <= 0){
            DestroySelf();
        }
    }
    private void DestroySelf(){
        Debug.Log("Health at 0... Destroyed.");
        //Any animations and stuff for when dying here
        gameObject.SetActive(false);
    }

    public void Start(){
        maxHealth = health;
        equippedItem.ModelSwap(primaryHand);
    }
    public void Update(){
        if (dotTimer > 0){
            TakeDamage(dotDamage * Time.deltaTime);
            Debug.Log("Taking DOT: " + dotDamage + " --> " + this.health);
            dotTimer -= Time.deltaTime;
            //DOT independent of frame rate so ppl with 1000fps dont just phase out of existence.
        }
    }
}
