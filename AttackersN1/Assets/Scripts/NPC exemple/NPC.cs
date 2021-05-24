using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //Criação da Behaviour Tree acontece de verdade aqui
    BTRoot behaviour;

    public int life = 3;
    public GameObject projectile;

    void Start()
    {
        behaviour = GetComponent<BTRoot>(); //Referência para o iniciador da árvore, o Root

        BTSequence combat = new BTSequence();
        combat.children.Add( new NodeVeOponente() );
        combat.children.Add( new NodeCombateOponente() );
        combat.children.Add( new NodeEsquivaOponente() );

        BTSelectorParallel selectorParallel = new BTSelectorParallel();
        selectorParallel.children.Add(new NodeVeOponente());
        selectorParallel.children.Add(new NodeVaiAteBolinha());

        BTSequence collect = new BTSequence(); //O sequence é o primeiro node a ser executado nessa árvore
        //A sequência que será executada, é adicionada abaixo na lista de filhos (children) do BTSequence
        //sequence.children.Add( node leaf aqui );
        collect.children.Add( new NodeTemBolinha() );
        collect.children.Add( selectorParallel );
        //collect.children.Add( new NodeVaiAteBolinha() );
        collect.children.Add( new NodeColetaBolinha() );

        BTSelector selector = new BTSelector();
        selector.children.Add(combat);
        selector.children.Add(collect);

        behaviour.root = selector;  //Associamos o root ao primeiro node a ser executado, para que o root inicie a árvore
        StartCoroutine(behaviour.Execute());    //O node Root inicia a árvore aqui
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            life--;
            if (life <= 0) Destroy(gameObject);
        }
    }

}
