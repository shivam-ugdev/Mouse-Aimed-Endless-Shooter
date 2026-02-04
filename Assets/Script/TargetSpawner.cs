using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Sprite[] targetSprite;

    [SerializeField] private BoxCollider2D cd;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private float cooldown;
    public float timer;

    private int sushiCreated=0;
    private int sushiMilestone = 10;
    private float midAirTime = 1.0f;

    private void Start()
    {
        timer = cooldown;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = cooldown;
            sushiCreated++;

            if(sushiCreated> sushiMilestone && cooldown> 0.5f)
            {
                sushiMilestone += 10;
                cooldown = Mathf.Max(0.6f, cooldown - 0.23f);
                midAirTime = Mathf.Max(0.23f, midAirTime - 0.1f);
            }



            GameObject newTarget = Instantiate(targetPrefab);

            float randomX = Random.Range(cd.bounds.min.x, cd.bounds.max.x);
             
            newTarget.transform.position=new Vector2(randomX,transform.position.y);
            int randomIndex = Random.Range(0, targetSprite.Length);
            newTarget.GetComponent<SpriteRenderer>().sprite = targetSprite[randomIndex];
            StartCoroutine(HandleTargetFall(newTarget));
        }
    }

    private IEnumerator HandleTargetFall(GameObject target)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

       
        if (rb != null)
        {
            while (target.transform.position.y > 2f)
            {
                yield return null;  
            }

            rb.gravityScale = 0;
            target.transform.position = new Vector2(target.transform.position.x, 2f);
            yield return new WaitForSeconds(midAirTime);
            rb.gravityScale = 1;
        }
    }
}
