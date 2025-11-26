using UnityEngine;
using System.Collections.Generic;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;
    public ParticleSystem snowPrefab;
    public int poolSize = 12;

    private Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            var ps = Instantiate(snowPrefab, transform);
            ps.gameObject.SetActive(false);
            pool.Enqueue(ps);
        }
    }

    public void PlayAt(Vector3 worldPos)
    {
        var ps = pool.Dequeue();
        ps.transform.position = worldPos;
        ps.gameObject.SetActive(true);
        ps.Play();

        StartCoroutine(ReturnToPool(ps));
    }

    private System.Collections.IEnumerator ReturnToPool(ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.main.duration);
        ps.gameObject.SetActive(false);
        pool.Enqueue(ps);
    }
}