using UnityEngine;

public class LifeSystem
{
    public float CurrentLife { get; private set; } 

    public LifeSystem(float totalLife)
    {
        CurrentLife = totalLife;
    }

    public void AddLife(float valueToAdd)
    {
        if (valueToAdd < 0)
        {
            Debug.LogError("LifeSystem.AddLife() : Valor deve ser maior que zero."); 
            return;
        }
        else
            CurrentLife += valueToAdd;
    }

    public void RemoveLife(float valueToRemove)
    {
        if (valueToRemove < 0)
        {
            Debug.LogError("LifeSystem.RemoveLife() : Valor deve ser maior que zero.");
            return;
        }
        else
            CurrentLife -= valueToRemove;
    }

    public bool IsDead()
    {
        if (CurrentLife <= 0)
            return true;
        else
            return false;
    }

}
