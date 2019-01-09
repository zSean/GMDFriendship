using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Taken from previous project. Own work
public abstract class StatusEffect : MonoBehaviour {

    protected float buffTimer;    // Duration of the effect.
    protected float maxBuffTimer = 0f;
    protected bool dispellable = true; // If the effect can be dispelled or not in-game.
    protected bool buff = true; // Whether buff or debuff
    protected bool stackable = false;   // If this buff is stackable

    protected int procType = (int)CharacterStates.none;

    protected string buffName;  // Name of the buff.

    protected GameObject parent = null;   // The object that assigned this buff. Can also be assigned null.

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }
    public GameObject GetParent()
    {
        return parent;
    }
    public float GetBuffTimer()
    {
        return buffTimer;
    }
    public float GetMaxBuffTimer()
    {
        return maxBuffTimer;
    }
    public void SetBuffTimer(float setTimer)
    {
        maxBuffTimer = setTimer;
        buffTimer = maxBuffTimer;
    }

    public string GetBuffName()
    {
        return buffName;
    }

    public void SetBuffName(string name)
    {
        buffName = name;
    }

    public bool IsStackable()
    {
        return stackable;
    }

    public void SetStackable(bool canStack)
    {
        stackable = canStack;
    }

    public abstract void Init();    //Initializing the buff
    public abstract void Activate(int characterState, GameObject sender);    // Buff effect.
    public abstract void Stack(StatusEffect sameEffect);   // What happens when another buff of the same type is applied.
    public abstract void TransferBuff(GameObject target);   // Typically applies to projectiles and attacks
    public abstract string buffDescription();   // String description of the buff.
    public abstract void buffDestroy(); // What happens when the buff duration runs out.
}
