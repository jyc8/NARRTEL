using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    //basic singleton implementation
    [HideInInspector]
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    //sprites can be found here: 
    //http://www.gameartguppy.com/shop/top-tower-defense-bunny-badgers-game-art-set/

    //enemies on screen
    public List<GameObject> Enemies;
    //prefabs
    //public GameObject EnemyPrefab;
    //public GameObject PathPrefab;
    public GameObject TowerPrefab;
    //list of waypoints in the current level
    //public Transform[] Waypoints;
    //private GameObject PathPiecesParent;
    private GameObject WaypointsParent;
    //file pulled from resources
    private LevelStuffFromXML levelStuffFromXML;

    //helpful variables for our player
    [HideInInspector]
    public int MoneyAvailable { get; private set; }
    [HideInInspector]
    public float MinCarrotSpawnTime;
    [HideInInspector]
    public float MaxCarrotSpawnTime;
    public int Lives = 10;
    private int currentRoundIndex = 0;
    [HideInInspector]
    public GameState CurrentGameState;
    public SpriteRenderer BunnyGeneratorSprite;
    [HideInInspector]
    public bool FinalRoundFinished;
    public GUIText infoText;

    private object lockerObject = new object();

    // Use this for initialization
    void Start()
    {
        IgnoreLayerCollisions();

        //Enemies = new List<GameObject>();
        //PathPiecesParent = GameObject.Find("PathPieces");
        //WaypointsParent = GameObject.Find("Waypoints");
        levelStuffFromXML = Utilities.ReadXMLFile();

        CreateLevelFromXML();

        CurrentGameState = GameState.Start;

        FinalRoundFinished = false;
    }

    /// <summary>
    /// Will create necessary stuff from the object that has the XML stuff
    /// </summary>
    private void CreateLevelFromXML()
    {

        GameObject tower = Instantiate(TowerPrefab, levelStuffFromXML.Tower,
            Quaternion.identity) as GameObject;
        tower.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

        //Waypoints = GameObject.FindGameObjectsWithTag("Waypoint")
        //    .OrderBy(x => x.name).Select(x => x.transform).ToArray();

        MoneyAvailable = levelStuffFromXML.InitialMoney;
        //MinCarrotSpawnTime = levelStuffFromXML.MinCarrotSpawnTime;
        //MaxCarrotSpawnTime = levelStuffFromXML.MaxCarrotSpawnTime;
    }

    /// <summary>
    /// Will make the arrow collide only with enemies!
    /// </summary>
    private void IgnoreLayerCollisions()
    {
        int bunnyLayerID = LayerMask.NameToLayer("Asset");
        //int enemyLayerID = LayerMask.NameToLayer("Enemy");
        int arrowLayerID = LayerMask.NameToLayer("Arrow");
        int backgroundLayerID = LayerMask.NameToLayer("Background");
        //int pathLayerID = LayerMask.NameToLayer("Path");
        int towerLayerID = LayerMask.NameToLayer("Tower");
        //int carrotLayerID = LayerMask.NameToLayer("Carrot");
        //Physics2D.IgnoreLayerCollision(bunnyLayerID, enemyLayerID); //Bunny and Enemy (when dragging the bunny)
        Physics2D.IgnoreLayerCollision(arrowLayerID, backgroundLayerID); //Arrow and Background
        //Physics2D.IgnoreLayerCollision(arrowLayerID, pathLayerID); //Arrow and Path
        Physics2D.IgnoreLayerCollision(arrowLayerID, bunnyLayerID); //Arrow and Bunny
        Physics2D.IgnoreLayerCollision(arrowLayerID, towerLayerID); //Arrow and Tower
        //Physics2D.IgnoreLayerCollision(arrowLayerID, carrotLayerID); //Arrow and Carrot
    }

    private void CheckAndStartNewRound()
    {
        if (currentRoundIndex < levelStuffFromXML.Rounds.Count - 1)
        {
            currentRoundIndex++;
            //StartCoroutine(NextRound());
        }
        else
        {
            FinalRoundFinished = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentGameState)
        {
            //start state, on tap, start the game and spawn carrots!
            case GameState.Start:
                if (Input.GetMouseButtonUp(0))
                {
                    CurrentGameState = GameState.Playing;
                    //StartCoroutine(NextRound());
                    //CarrotSpawner.StartCarrotSpawning();
                }
                break;
            case GameState.Playing:
                /*if (Lives == 0) //we lost
                {
                    //no more rounds
                    StopCoroutine(NextRound());
                    //DestroyExistingEnemiesAndCarrots();
                    //CarrotSpawner.StopCarrotSpawning();
                    CurrentGameState = GameState.Lost;
                }
                else if (FinalRoundFinished && Enemies.Where(x => x != null).Count() == 0)
                {
                    //DestroyExistingEnemiesAndCarrots();
                    //CarrotSpawner.StopCarrotSpawning();
                    CurrentGameState = GameState.Won;
                }*/
                break;
            case GameState.Won:
                if (Input.GetMouseButtonUp(0))
                {//restart
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
            case GameState.Lost:
                if (Input.GetMouseButtonUp(0))
                {//restart
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
            default:
                break;
        }
    }


}
