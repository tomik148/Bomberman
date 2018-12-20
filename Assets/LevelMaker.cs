using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityStandardAssets.CrossPlatformInput;

public class LevelMaker : MonoBehaviour {

    static public int[,] LevelToLoad;

    public Tilemap floor;
    public Tilemap walls;
    public Tilemap fire;

    public TileBase floorTile;
    public TileBase wallTile;
    public TileBase BwallTile;

    public GameObject player;
    public GameObject enemy;

    public Joystick joystick;

    GameObject Player = null;
    GameObject Enemy = null;

    public void PlaceBomb()
    {
        Player.GetComponent<controler>().PlantBomb();
    }

	// Use this for initialization
	void Start () {
        //LevelToLoad = new int[,]
        //{
        //    { 1 ,1 ,1 ,1 ,1 },
        //    { 1 ,4 ,0 ,0 ,1 },
        //    { 1 ,0 ,1 ,0 ,1 },
        //    { 1 ,0 ,0 ,2 ,1 },
        //    { 1 ,0 ,1 ,0 ,1 },
        //    { 1 ,2 ,0 ,0 ,1 }, 
        //    { 1 ,2 ,1 ,0 ,1 }, 
        //    { 1 ,2 ,0 ,2 ,1 }, 
        //    { 1 ,2 ,1 ,0 ,1 },
        //    { 1 ,2 ,0 ,3 ,1 },
        //    { 1 ,1 ,1 ,1 ,1 },

        //};
        

        controler c = null;
        for (int i = 0; i < LevelToLoad.GetLength(0); i++)
        {
            for (int j = 0; j < LevelToLoad.GetLength(1); j++)
            {
                var pos = new Vector3Int(i - LevelToLoad.GetLength(0)/2, j - LevelToLoad.GetLength(1)/2, 0);
                floor.SetTile(pos, floorTile);
                switch (LevelToLoad[i,j])
                {
                    case 0 : //nothing
                        break;
                    case 1 : //wall
                        walls.SetTile(pos, wallTile);
                        break;
                    case 2 : //Bwall
                        walls.SetTile(pos, BwallTile);
                        break;
                    case 4 : //player
                        Player = Instantiate(player, walls.GetCellCenterWorld(pos),Quaternion.identity);
                        c = Player.GetComponent<controler>();
                        c.joystick = joystick;
                        c.tilemap = walls;
                        c.fireTilemap = fire;
                        if (Enemy != null)
                        {
                            var x = Enemy.GetComponent<AI>();
                            x.player = c;
                        }
                        break;
                    case 3 : //enemy
                        Enemy = Instantiate(enemy, walls.GetCellCenterWorld(pos), Quaternion.identity);
                        var a = Enemy.GetComponent<AI>();
                        a.wallsMap = walls;
                        a.floorMap = floor;
                        if (c != null)
                        {
                            a.player = c;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        walls.RefreshAllTiles();
        var s = Enemy.GetComponent<AI>();
        s.doStaff = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
