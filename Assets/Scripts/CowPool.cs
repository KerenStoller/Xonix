using System.Collections.Generic;
using UnityEngine;

public class CowPool : MonoBehaviour
{
    public static CowPool Instance;
    
    public GameObject cowPrefab;
    public int poolSize = 3;
    private List<GameObject> _pool;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        Instance = this;

        _pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject cow = Instantiate(cowPrefab);
            cow.SetActive(false);
            _pool.Add(cow);
        }
    }

    public GameObject GetCow()
    {
        foreach (var cow in _pool)
        {
            if (!cow.activeInHierarchy)
            {
                cow.SetActive(true);
                return cow;
            }
        }
        
        // Expand _pool if needed (optional)
        GameObject newCow = Instantiate(cowPrefab);
        newCow.SetActive(true);
        _pool.Add(newCow);
        return newCow;
    }

    public void ReturnCow(GameObject cow)
    {
        cow.SetActive(false);
    }
}
