using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TargetController : MonoBehaviour
{

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject particles;
    private bool exploded = false;
    private float deathTimer = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Explode()
    {
        body.SetActive(false);
        particles.SetActive(true);
        exploded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer < 0)
            {
                Destroy(gameObject);
            }
        }
        }
}
