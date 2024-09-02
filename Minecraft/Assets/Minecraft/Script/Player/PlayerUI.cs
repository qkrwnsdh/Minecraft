using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Transform quickSlot;
    [SerializeField] private Transform playerState;
    [SerializeField] private Transform interaction;
    [SerializeField] private Transform inventory;
    [SerializeField] private Transform setting;
    [SerializeField] private Transform craft;
    [SerializeField] private Transform furnace;
    [SerializeField] private Transform chest;

    [SerializeField] private Transform shieldTransform;
    [SerializeField] private Transform healthTransform;
    [SerializeField] private Transform foodTransform;

    private Transform[] shields;
    private Transform[] healths;
    private Transform[] foods;

    private readonly int ARRAY_LENGTH = 10;

    private void Awake()
    {
        InitializationInstances();
        InitializationSetups();
    }

    private void InitializationInstances()
    {
        shields = new Transform[ARRAY_LENGTH];
        healths = new Transform[ARRAY_LENGTH];
        foods = new Transform[ARRAY_LENGTH];
    }

    private void InitializationSetups()
    {
        for (int i = 0; i < ARRAY_LENGTH; i++)
        {
            shields[i] = shieldTransform.GetChild(i);
            healths[i] = healthTransform.GetChild(i);
            foods[i] = foodTransform.GetChild(i);
        }
    }

    public void SetShields(int shield) => SetStatus(shields, shield);
    public void SetHealths(int health) => SetStatus(healths, health);
    public void SetFoods(int food) => SetStatus(foods, food);

    private void SetStatus(Transform[] elements, int value)
    {
        // value를 2로 나눈 값을 setValue로 설정합니다. 최소값은 0
        int setValue = Mathf.Max(0, value / 2);

        for (int i = 0; i < elements.Length; i++)
        {
            bool isHalf = i == setValue && value % 2 == 1;
            bool isFull = i < setValue;

            elements[i].GetChild(0).gameObject.SetActive(isHalf);
            elements[i].GetChild(1).gameObject.SetActive(isFull);
        }
    }

    #region Interaction
    public void Inventory()
    { 
    
    }

    public void Setting()
    { 
    
    }
    #endregion
}
