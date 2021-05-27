using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabEnemyToSpawn;
    public bool IsPaused { get; private set; } = true;

    private float _currentTimeStep;
    private int _currentEnemiesToSpawn;
    private float timer;

    void Update() { if (_currentEnemiesToSpawn != 0 && !IsPaused) Spawn(); }

    private void Spawn()
    {
        if (timer <= 0f)
        {
            StartCoroutine(SpawnWithTimeWait(.3f));

            timer = 0f;
            timer += Time.deltaTime + _currentTimeStep;
        }
        else
            timer -= Time.deltaTime;
    }
    private IEnumerator SpawnWithTimeWait(float timeToWait)
    {
        for (int i = 0; i < _currentEnemiesToSpawn; i++)
        {
            Instantiate(prefabEnemyToSpawn, transform.position, transform.rotation);
            yield return new WaitForSeconds(timeToWait);
        }
    }

    public void InitSpawn(int enemiesNumber, float timeStep)
    {
        _currentEnemiesToSpawn = enemiesNumber;
        _currentTimeStep = timeStep;
        timer = Time.deltaTime + _currentTimeStep;
        StartCoroutine(SpawnWithTimeWait(0f));
        IsPaused = false;
    }
    public void SetTimeStep(float timeStep) => _currentTimeStep = timeStep;
    public void SetEnemiesNumber(int enemiesNumber) => _currentEnemiesToSpawn = enemiesNumber;
    public void Pause(bool state) => IsPaused = state;
    
    //Nunca usados
    public void SetSpawn(int enemiesNumber, float timeStep)
    {
        SetEnemiesNumber(enemiesNumber);
        SetTimeStep(timeStep);
    }
}
