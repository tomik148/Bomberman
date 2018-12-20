using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class AI : MonoBehaviour {

    public Tilemap wallsMap;
    public Tilemap floorMap;
    public controler player;
    public Rigidbody2D rb;

    public int bombPow = 2;
    public float speed = 1;

    bool hiding;
    bool moving = false;

    List<Vector3Int> notSafeMoves = new List<Vector3Int>();

    List<Vector3Int> moves = new List<Vector3Int>();
    private int bomb_counter = 0;
    private System.Random random = new System.Random();

    public bool doStaff = false;

    // Use this for initialization
    void Start() {
        player.OnBombPlaced += (pos, pow) => notSafeMoves.AddRange(getBombedTiles(pos, pow));
        player.OnBombExpoded += (pos, pow) =>
        {
            foreach (var item in getBombedTiles(pos, pow))
            {
                notSafeMoves.Remove(item);
            }
        };
    }



    // Update is called once per frame
    void Update() {
        if (!doStaff)
        {
            return;
        }

        if (moving)
        {
            StartCoroutine(MoveTo());
        }
        else
        { 
            doAI();
        }
    }

    IEnumerator MoveTo()
    {
        var move = moves.Last();
        while ((wallsMap.GetCellCenterWorld(move) - transform.position).magnitude > 0.01)
        {
            var newpos = transform.position + ((wallsMap.GetCellCenterWorld(move) - transform.position).normalized * Time.deltaTime * speed) ;
            rb.MovePosition(new Vector2(newpos.x,newpos.y));
            yield return null;
        }
        rb.MovePosition(wallsMap.GetCellCenterWorld(move));
        moves.Remove(move);
        if (moves.Count == 0)
        {
            moving = false;
        } 
    }

    void doAI()
    {
        if (bomb_counter > 0) bomb_counter--;

        Vector3Int? move = null;
        try
        {
            if (moves.Count > 0)
            {
                move = moves.Last();
                if (move.HasValue)
                {
                    moves.Remove(move.Value);
                }
            }
        }
        catch (Exception) { }

        if (move == null || !moveSafe(move) || enemyCheck())
        {
            moves.Clear();
            if (canPlaceBombAndEscape())
            {
                //Place a bomb
                bomb_counter = 5;
                player.PlantBomb(getPos());
                moving = true;
            }
            else
            {
                //Move randomly//TODO!!!!
                var n = getWalkableNeighbors(getPos());
                for (int i = 0; i < n.Length; ++i)
                {
                    if (moveSafe(n[i]))
                    {
                        move = n[i];
                        break;
                    }
                }
            }
        }

        if (move == null)
        {
            Debug.Log("no move");
        }
        else
        {
            //var newpos = (wallsMap.CellToWorld(move.Value))/2 * speed;
            //rb.MovePosition(newpos);
            //yield return null;
            moving = true;
            moves.Add(move.Value);
            //rb.MovePosition(wallsMap.GetCellCenterWorld(move.Value));
        }
    }

    private bool canPlaceBombAndEscape()
    {
        if (bomb_counter > 0 || !moveSafe(getPos())) return false;
        var bombtiles = getBombedTiles(getPos(), bombPow);
        var movetiles = getAllWalkableTilesFrom(getPos());
        bool good_place = false, safe = false;
        foreach (var t in bombtiles)
        {
            if (!good_place && wallsMap.GetTile(t)?.name == "Wall")
                good_place = true;
        }
        // always place a bomb if an enemy is nearby, or on a random chance
        if (isEnemyInRange(getPos()) || random.Next(10) == 0) good_place = true;
        if (!good_place) return false;
        foreach (var t in movetiles)
        {
            if (!safe && !bombtiles.Contains(t))
            {
                var path = getPath(getPos(), t);
                if (path.Length > 0)
                {
                    moves.AddRange(path);
                    safe = true;
                }
            }
        }
        return safe;
    }

    private bool isEnemyInRange(Vector3Int pos)
    {
        return getBombedTiles(pos, bombPow).Contains(player.getPos());
    }

    private bool enemyCheck()
    {
        if (bomb_counter != 0) return false;
        return isEnemyInRange(getPos());
    }

    private bool moveSafe(Vector3Int? move)
    {
        if (!move.HasValue)
        {
            return true;
        }
        return !notSafeMoves.Contains(move.Value);
    }

    Vector3Int getPos()
    {
        return wallsMap.WorldToCell(transform.position);
    }

    bool isWalkable(Vector3Int pos)
    {
        return wallsMap.GetTile(pos) == null;
    }

    bool isBreakable(Vector3Int pos)
    {
        return wallsMap.GetTile(pos)?.name == "Wall";
    }

    Vector3Int[] getAllWalkableTilesFrom(Vector3Int pos)
    {
        return FloodFillFrom(pos);
    }

    Vector3Int[] getBombedTiles(Vector3Int bombPos, int bombPow)
    {
        List<Vector3Int> ret = new List<Vector3Int>();
        ret.Add(bombPos);
        int a = 1;
        //up
        while (a <= bombPow)
        {
            Vector3Int check = bombPos;
            check.y += a;
            var tile = wallsMap.GetTile(check);
            switch (tile?.name)
            {
                case "BombA":
                    ret.AddRange(getBombedTiles(check, bombPow));
                    break;
                case "Wall":
                    ret.Add(new Vector3Int(check.x, check.y, check.z));
                    a = bombPow;
                    break;
                case "Wall2":
                    a = bombPow;
                    break;
                default:
                    ret.Add(new Vector3Int(check.x, check.y, check.z));
                    break;
            }
            a++;
        }
        a = 1;
        //left
        while (a <= bombPow)
        {
            Vector3Int check = bombPos;
            check.x += a;
            var tile = wallsMap.GetTile(check);
            switch (tile?.name)
            {
                case "BombA":
                    ret.AddRange(getBombedTiles(check, bombPow));
                    break;
                case "Wall":
                    ret.Add(new Vector3Int(check.x, check.y, check.z));
                    a = bombPow;
                    break;
                case "Wall2":
                    a = bombPow;
                    break;
                default:
                    ret.Add(new Vector3Int(check.x, check.y, check.z));
                    break;
            }
            a++;
        }
        a = 1;
        //down
        while (a <= bombPow)
        {
            Vector3Int check = bombPos;
            check.y -= a;
            var tile = wallsMap.GetTile(check);
            switch (tile?.name)
            {
                case "BombA":
                    ret.AddRange(getBombedTiles(check, bombPow));
                    break;
                case "Wall":
                    ret.Add(new Vector3Int(check.x, check.y, check.z));
                    a = bombPow;
                    break;
                case "Wall2":
                    a = bombPow;
                    break;
                default:
                    ret.Add(new Vector3Int(check.x, check.y, check.z));
                    break;
            }
            a++;
        }
        a = 1;
        //right
        while (a <= bombPow)
        {
            Vector3Int check = bombPos;
            check.x -= a;
            var tile = wallsMap.GetTile(check);
            switch (tile?.name)
            {
                case "BombA":
                    ret.AddRange(getBombedTiles(check, bombPow));
                    break;
                case "Wall":
                    ret.Add(new Vector3Int(check.x, check.y, check.z));
                    a = bombPow;
                    break;
                case "Wall2":
                    a = bombPow;
                    break;
                default:
                    ret.Add(new Vector3Int(check.x, check.y, check.z));
                    break;
            }
            a++;
        }
        return ret.ToArray();
    }

    Vector3Int[] getSafeTiles(Vector3Int bombPos, int bombPow)
    {
        return getAllWalkableTilesFrom(bombPos).Except(getBombedTiles(bombPos, bombPow)).ToArray();
    }

    Vector3Int[] getPath(Vector3Int from, Vector3Int to)
    {
        return FloodFillFromToPath(from, to);
    }

    Vector3Int[] getNeighbors(Vector3Int from)
    {
        System.Random rnd = new System.Random();
        return new Vector3Int[] { new Vector3Int(from.x + 1, from.y, from.z), new Vector3Int(from.x - 1, from.y, from.z), new Vector3Int(from.x, from.y + 1, from.z), new Vector3Int(from.x, from.y - 1, from.z), }.OrderBy(x => rnd.Next()).ToArray(); ;
    }
    Vector3Int[] getWalkableNeighbors(Vector3Int from)
    {
        List<Vector3Int> ret = new List<Vector3Int>();
        foreach (var a in getNeighbors(from))
        {
            if (isWalkable(a))
                ret.Add(a);
        }
        return ret.ToArray();
    }






    private Vector3Int[] FloodFillFrom(Vector3Int start)
    {
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();
        List<Vector3Int> visited = new List<Vector3Int>();
        if (isWalkable(start)) frontier.Enqueue(start);

        while (frontier.Count > 0)
        {
            Vector3Int a = frontier.Dequeue();
            visited.Add(a);

            //if (isWalkable(new Vector3Int(a.x - 1, a.y, 0)) && !visited.Contains()) frontier.Enqueue(new Vector3Int(a.x - 1, a.y, 0));
            //if (isWalkable(new Vector3Int(a.x + 1, a.y, 0))) frontier.Enqueue(new Vector3Int(a.x + 1, a.y, 0));
            //if (isWalkable(new Vector3Int(a.x, a.y - 1, 0))) frontier.Enqueue(new Vector3Int(a.x, a.y - 1, 0));
            //if (isWalkable(new Vector3Int(a.x, a.y + 1, 0))) frontier.Enqueue(new Vector3Int(a.x, a.y + 1, 0));

            TryAddVector3Int(frontier, new Vector3Int(a.x - 1, a.y, 0), visited);
            TryAddVector3Int(frontier, new Vector3Int(a.x + 1, a.y, 0), visited);
            TryAddVector3Int(frontier, new Vector3Int(a.x, a.y - 1, 0), visited);
            TryAddVector3Int(frontier, new Vector3Int(a.x, a.y + 1, 0), visited);


        }
        return visited.ToArray();
    }

    void TryAddVector3Int(Queue<Vector3Int> frontier, Vector3Int item, List<Vector3Int> visited)
    {
        if (isWalkable(item) && !visited.Contains(item))
            frontier.Enqueue(item);
    }

    private Vector3Int[] FloodFillFromToPath(Vector3Int start, Vector3Int to)
    {
        Queue<FFTile> frontier = new Queue<FFTile>();
        List<FFTile> visited = new List<FFTile>();
        if (isWalkable(start)) frontier.Enqueue(new FFTile(start,null));

        while (frontier.Count > 0)
        {
            if (frontier.Peek().pos == to)
            {
                break;
            }
            var a = frontier.Dequeue();
            visited.Add(a);



            //if (isWalkable(new Vector3Int(a.pos.x - 1, a.pos.y, 0))) frontier.Enqueue(new FFTile(new Vector3Int(a.pos.x - 1, a.pos.y, 0),a));
            //if (isWalkable(new Vector3Int(a.pos.x + 1, a.pos.y, 0))) frontier.Enqueue(new FFTile(new Vector3Int(a.pos.x + 1, a.pos.y, 0), a));
            //if (isWalkable(new Vector3Int(a.pos.x, a.pos.y - 1, 0))) frontier.Enqueue(new FFTile(new Vector3Int(a.pos.x, a.pos.y - 1, 0), a));
            //if (isWalkable(new Vector3Int(a.pos.x, a.pos.y + 1, 0))) frontier.Enqueue(new FFTile(new Vector3Int(a.pos.x, a.pos.y + 1, 0), a));

            TryAddFFTile(frontier, new Vector3Int(a.pos.x - 1, a.pos.y, 0), visited, a);
            TryAddFFTile(frontier, new Vector3Int(a.pos.x + 1, a.pos.y, 0), visited, a);
            TryAddFFTile(frontier, new Vector3Int(a.pos.x, a.pos.y - 1, 0), visited, a);
            TryAddFFTile(frontier, new Vector3Int(a.pos.x, a.pos.y + 1, 0), visited, a);
        }

        List<Vector3Int> path = new List<Vector3Int>();
        try
        {
            FFTile c = frontier.Dequeue();
            while (c.from != null)
            {
                path.Add(c.pos);
                c = c.from;
            }
        }
        catch (InvalidOperationException) { }

        return path.ToArray();

    }

    void TryAddFFTile(Queue<FFTile> frontier, Vector3Int item, List<FFTile> visited, FFTile a)
    {
        if (isWalkable(item) && !visited.Contains(new FFTile(item, a)))
            frontier.Enqueue(new FFTile(item, a));
    }

    class FFTile
    {
        public Vector3Int pos;
        public FFTile from;

        public FFTile(Vector3Int pos, FFTile from)
        {
            this.pos = pos;
            this.from = from;
        }
    }
}
