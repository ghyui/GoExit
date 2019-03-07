using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject startPoint;

    [SerializeField]
    MainCharacter mainCharacterPrefab;
    [SerializeField]
    List<Monster> monsterPrefabs;

    MainCharacter mainCharacter;
    // Use this for initialization
    void Start()
    {
        mainCharacter = Instantiate(mainCharacterPrefab);
        mainCharacter.transform.SetPositionAndRotation(startPoint.transform.position, startPoint.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
