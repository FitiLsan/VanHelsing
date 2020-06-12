using UnityEngine;


public class Spawner : MonoBehaviour
{
    #region Fields

    public Vector3 ObjectVector;
    public GameObject SpawningPrefab;

    #endregion


    #region Awake

    private void Awake()
    {
        Instantiate(SpawningPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion
}
