using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esse script deve ser adicionado ao GameObject
public class BTRoot : MonoBehaviour
{
    public BTNode root;

    public IEnumerator Execute()
    {
        while (true)
        {
            if (root == null) yield return null; //Caso o root não tenha sido associado
            else yield return StartCoroutine(root.Run(this));
        }
    }
}
