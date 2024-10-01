using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    // Creating a singleton of Inventory so we don't have to call FindGameObject<> everytime just call Inventory.instance
    #region Singleton
    public static Inventory instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of Inventory!");
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int capacity = 12;

    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Not Enough Space to pickup the " + item.name);
            return false;
        }
        
        items.Add(item);
        item.SetInventoryStatus(true);

        if (onItemChangedCallback != null) // we actually have a method to invoke
        {
            onItemChangedCallback.Invoke();
        }
        


        return true;
   
        
    }
    
    public void Remove(Item item)
    {
        items.Remove(item);
        item.SetInventoryStatus(false);
        if (onItemChangedCallback != null) // we actually have a method to invoke
        {
            onItemChangedCallback.Invoke();
        }
    }
}
