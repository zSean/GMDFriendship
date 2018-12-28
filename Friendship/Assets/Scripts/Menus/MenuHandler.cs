using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuHandler : MonoBehaviour {

    protected MenuHandler parent;

    public abstract void Init(MenuHandler parent);
    public abstract void ChangeMenu(int newButton);
    public abstract void Disable(bool enable);
}