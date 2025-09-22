using UnityEngine;

public class Item : MonoBehaviour
{
    //Core Stats
    public float speed;
    public float strength;
    public float distance;
    public string type;

    //Debug Graphics
    public Ray trace;

    [SerializeField] private LayerMask ignorePlayer;
    Transform gunHitPosition;

    private float useDelayTimer;

    public Item(float sp, float st, float di)
    {
        speed = sp;
        strength = st;
        distance = di;
    }

    public void Use(Entity target)
    {
        switch (this.type.ToLower())
        {
            case "health consumable": HealthConsumableUse(target); break;
            case "pistol": PistolUse(target); break;
            default: Debug.Log("Generic type."); break;
        }
    }

    public void ModelSwap(GameObject hand)
    {
        string modelName = this.transform.gameObject.name.ToLower();
        modelName = "ItemModels/Prefabs/" + modelName;
        //Get path to resources folder.
        //Models in folder must be lower case named.
        Object model;
        Debug.Log("[Item] Model path: " + modelName);
        try
        {
            model = Resources.Load(modelName);
        }
        catch
        {
            model = Resources.Load("ItemModels/Prefabs/default");
            //Revert to default if there is no model.
        }
        Instantiate(model, hand.transform);
        //Load item in scene.
    }

    #region Use Functions
    private void HealthConsumableUse(Entity target)
    {
        useDelayTimer = this.speed;
        if (distance > 0) { target.TakeDamage(this.strength, this.distance); }
        else { target.TakeDamage(this.strength); }
    }

    private void PistolUse(Entity target = null)
    {
        if (!(useDelayTimer > 0))
        {
            if (target != null)
            {
                RaycastHit hit;
                Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, ignorePlayer);
                Debug.Log("[Pistol] Hit " + hit.transform.name);
                try { hit.transform.gameObject.GetComponent<Entity>().TakeDamage(this.strength); }
                catch { Debug.Log("[Pistol] Not an entity"); }
            }
            else
            {
                Debug.Log("[Pistol::NonPlayer] Hit " + target.name);
                target.TakeDamage(this.strength);
            }//Use this if item equipped to a non-player entity.
            useDelayTimer = 1 / (this.speed / 60); //Speed = rpm, speed/60 = frequency(Hz), 1/f = T(s)
        }
        else { Debug.Log("[Pistol] Not Ready"); }
    }
    #endregion

    private void Update()
    {
        if (useDelayTimer > 0) { useDelayTimer -= Time.deltaTime; }
    }
}