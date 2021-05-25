using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode
{
    public enum Status { RUNNING, SUCCESS, FAILURE }
    public Status status;   //Status atual

    public List<BTNode> children = new List<BTNode>();  //Lista com os nodes filhos

    //Método que roda o comportamneto do node, ele recebe a Behaviour Tree como parâmentro (isso possibilidade acessar o GameObject)
    public abstract IEnumerator Run(BTRoot root);

    //Método para debug
    public void Print(string message = "")
    {
        string color = "cyan";
        if (status == Status.SUCCESS) color = "green";
        if (status == Status.FAILURE) color = "orange";

        Debug.Log($"<color={color}> {this} : {status} </color>.  {message}");
    }
}
