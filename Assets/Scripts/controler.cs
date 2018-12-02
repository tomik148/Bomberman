using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityStandardAssets.CrossPlatformInput;

public class controler : MonoBehaviour {

    public Animator animator;
    public Joystick joystick;
    public Rigidbody2D rigidb;
    public Tilemap tilemap;
    public Tilemap fireTilemap;
    public TileBase bombTile;
    public TileBase fireTile;
    public AudioSource audioSource;

    public float speed = 10;
    public int BombSize = 2;
    public int BombFuzeTime = 2;

    public bool godmod = false;

    public event Action<Vector3Int,int> OnBombPlaced;
    public event Action<Vector3Int,int> OnBombExpoded;

    // Use this for initialization
    void Start () {
        audioSource.enabled = sound.IsSoundOn;

    }
	
	// Update is called once per frame
	void Update () {
        var dx = joystick.m_HorizontalVirtualAxis.GetValue;
        var dy = joystick.m_VerticalVirtualAxis.GetValue;
        var delta = new Vector2();
        if (dx > 0.1 || dx < -0.1)
        {
            animator.SetFloat("Xvel",dx);
            delta.x += dx;
        }
        if (dy > 0.1 || dy < -0.1)
        {
            animator.SetFloat("Yvel",dy);
            delta.y += dy;
        }
        delta *= Time.deltaTime;
        delta *= speed;
        rigidb.MovePosition(rigidb.position + delta);
	}

    public void PlantBomb()
    {
        PlantBomb(tilemap.WorldToCell(transform.position));
    }

    public void PlantBomb(Vector3Int pos)
    {
        StartCoroutine("PlantBombInternal", pos);
    }

    private IEnumerator PlantBombInternal(Vector3Int pos)
    {
        //Vector3Int pos = tilemap.WorldToCell(transform.position);
        tilemap.SetTile(pos, bombTile);
        OnBombPlaced?.Invoke(pos,BombSize);
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / BombFuzeTime;
            yield return null;
        }
        //Explode
        if (tilemap.GetTile(pos).name != "BombA")
        {
             yield break;
        }
        StartCoroutine(ExplodeBomb(pos, BombSize));
    }

    public Vector3Int getPos()
    {
        return tilemap.WorldToCell(transform.position);
    }

    public IEnumerator ExplodeBomb(Vector3Int pos, int size)
    {
        fireTilemap.SetTile(pos, fireTile);
        tilemap.SetTile(pos, null);
        
        int a = 1;
        List<Vector3Int> firePos = new List<Vector3Int>();
        firePos.Add(pos);
        //up
        while (a <= size)
        {
            Vector3Int check = pos;
            check.y += a;
            var tile = tilemap.GetTile(check);
            switch (tile?.name)
            {
                case "BombA":
                    tilemap.SetTile(check, null);
                    StartCoroutine(ExplodeBomb(check, BombSize));
                    break;
                case "Wall":
                    fireTilemap.SetTile(check, fireTile);
                    tilemap.SetTile(check, null);
                    firePos.Add(new Vector3Int(check.x,check.y,check.z));
                    a = size;
                    break;
                case "Wall2":
                    a = size;
                    break;
                default:
                    fireTilemap.SetTile(check, fireTile);
                    firePos.Add(new Vector3Int(check.x, check.y, check.z));
                    break;
            }
            a++;
        }
        a = 1;
        //left
        while (a <= size)
        {
            Vector3Int check = pos;
            check.x += a;
            var tile = tilemap.GetTile(check);
            switch (tile?.name)
            {
                case "BombA":
                    tilemap.SetTile(check, null);
                    StartCoroutine(ExplodeBomb(check, BombSize));
                    break;
                case "Wall":
                    fireTilemap.SetTile(check, fireTile);
                    tilemap.SetTile(check, null);
                    firePos.Add(new Vector3Int(check.x, check.y, check.z));
                    a = size;
                    break;
                case "Wall2":
                    a = size;
                    break;
                default:
                    fireTilemap.SetTile(check, fireTile);
                    firePos.Add(new Vector3Int(check.x, check.y, check.z));
                    break;
            }
            a++;
        }
        a = 1;
        //down
        while (a <= size)
        {
            Vector3Int check = pos;
            check.y -= a;
            var tile = tilemap.GetTile(check);
            switch (tile?.name)
            {
                case "BombA":
                    tilemap.SetTile(check, null);
                    StartCoroutine(ExplodeBomb(check, BombSize));
                    break;
                case "Wall":
                    fireTilemap.SetTile(check, fireTile);
                    tilemap.SetTile(check, null);
                    firePos.Add(new Vector3Int(check.x, check.y, check.z));
                    a = size;
                    break;
                case "Wall2":
                    a = size;
                    break;
                default:
                    fireTilemap.SetTile(check, fireTile);
                    firePos.Add(new Vector3Int(check.x, check.y, check.z));
                    break;
            }
            a++;
        }
        a = 1;
        //right
        while (a <= size)
        {
            Vector3Int check = pos;
            check.x -= a;
            var tile = tilemap.GetTile(check);
            switch (tile?.name)
            {
                case "BombA":
                    tilemap.SetTile(check, null);
                    StartCoroutine(ExplodeBomb(check, BombSize));
                    break;
                case "Wall":
                    fireTilemap.SetTile(check, fireTile);
                    tilemap.SetTile(check, null);
                    firePos.Add(new Vector3Int(check.x, check.y, check.z));
                    a = size;
                    break;
                case "Wall2":
                    a = size;
                    break;
                default:
                    fireTilemap.SetTile(check, fireTile);
                    firePos.Add(new Vector3Int(check.x, check.y, check.z));
                    break;
            }
            a++;
        }
        audioSource.Play();
        float normalizedTime = 0;
        while (normalizedTime <= .5f)
        {
            normalizedTime += Time.deltaTime / BombFuzeTime;
            yield return null;
        }
        //clearFire
        foreach (var fpos in firePos)
        {
            fireTilemap.SetTile(fpos, null);
        }
        OnBombExpoded?.Invoke(pos, size);
    }

    public void Die()
    {
        if (godmod)
        {
            return;
        }
        SceneManager.LoadScene("Menu");
    }

    
    
}
