using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private enum Symmetry { x, xy, none };
    public enum BlockType { Rect, Circ, Triang};

    [Header("Generation parameters")]
    [SerializeField] private int seed;
    [SerializeField] private bool useSeed;
    [SerializeField] private float levelSize = 10;
    [SerializeField] private int randObjs = 10;
    [SerializeField] private Symmetry symm = Symmetry.none;
    [SerializeField] private Vector2 minMaxScaleMult = new Vector2(0.25f, 1f);
    [SerializeField] private Color defaultColor = Color.white;

    [Space]

    [Header("Blocks")]
    [SerializeField] private GameObject rect;
    [SerializeField] private GameObject circ;
    [SerializeField] private GameObject triang;
    [SerializeField] private GameObject spawnArea;


    private Vector2 botLeft;
    private Vector2 topRight;
    private float halfLevelWidth;
    private float halfLevelHeight;

    void Start()
    {
        // Setup
        Camera.main.orthographicSize = levelSize;

        halfLevelWidth = levelSize * 1.776618f;
        halfLevelHeight = levelSize;

        botLeft = Camera.main.ViewportToWorldPoint(new Vector2(0,0));
        topRight = Camera.main.ViewportToWorldPoint(new Vector2(1,1));

        Transform map = new GameObject("Map").transform;
        Transform solid = new GameObject("Bounds").transform;
        Transform bounds = new GameObject("Bounds").transform;
        Transform moveable = new GameObject("Bounds").transform;
        Transform spawnZones = new GameObject("Spawns").transform;
        solid.transform.parent = map;
        bounds.transform.parent = map;
        moveable.transform.parent = map;
        spawnZones.transform.parent = map;
        
        if(useSeed)
            Random.InitState(seed);

        // Generate map bounds
        PlaceBlock(BlockType.Rect, new Vector2( topRight.x, 0), new Vector2(1, halfLevelHeight * 2), Quaternion.identity, bounds);
        PlaceBlock(BlockType.Rect, new Vector2(-topRight.x, 0), new Vector2(1, halfLevelHeight * 2), Quaternion.identity, bounds);
        PlaceBlock(BlockType.Rect, new Vector2(0,  topRight.y), new Vector2(halfLevelWidth * 2, 1),  Quaternion.identity, bounds);
        PlaceBlock(BlockType.Rect, new Vector2(0, -topRight.y), new Vector2(halfLevelWidth * 2, 1),  Quaternion.identity, bounds);


        // Actually generate map pieces

        // Random ammount of solid blocks at random pos, with random scale but factored in with levelScale and mirrored accordingly
        if (symm == Symmetry.none)
        {
            for (int i = 0; i < randObjs; i++)
            {
                Vector2 randPos = new Vector2(Random.Range(-halfLevelWidth, halfLevelWidth), Random.Range(-halfLevelHeight, halfLevelHeight));
                Vector2 randScale = new Vector2(Random.Range(minMaxScaleMult.x, minMaxScaleMult.y), Random.Range(minMaxScaleMult.x, minMaxScaleMult.y)) * levelSize;

                PlaceBlock(BlockType.Rect, randPos, randScale, Quaternion.identity, solid);
            }
        }
        if (symm == Symmetry.x)
        {
            for (int i = 0; i < randObjs / 2; i++)
            {
                Vector2 randPos = new Vector2(Random.Range(0, halfLevelWidth), Random.Range(-halfLevelHeight, halfLevelHeight));
                Vector2 randScale = new Vector2(Random.Range(minMaxScaleMult.x, minMaxScaleMult.y), Random.Range(minMaxScaleMult.x, minMaxScaleMult.y)) * levelSize;

                PlaceBlock(BlockType.Rect, randPos, randScale, Quaternion.identity, solid);
                PlaceBlock(BlockType.Rect, new Vector2(-randPos.x, randPos.y), randScale, Quaternion.identity, solid);
            }
        }
        if (symm == Symmetry.xy)
        {
            for (int i = 0; i < randObjs / 4; i++)
            {
                Vector2 randPos = new Vector2(Random.Range(0, halfLevelWidth), Random.Range(0, halfLevelHeight));
                Vector2 randScale = new Vector2(Random.Range(minMaxScaleMult.x, minMaxScaleMult.y), Random.Range(minMaxScaleMult.x, minMaxScaleMult.y)) * levelSize;

                PlaceBlock(BlockType.Rect, randPos, randScale, Quaternion.identity, solid);
                PlaceBlock(BlockType.Rect, new Vector2(-randPos.x, randPos.y), randScale, Quaternion.identity, solid);
                PlaceBlock(BlockType.Rect, new Vector2(-randPos.x,-randPos.y), randScale, Quaternion.identity, solid);
                PlaceBlock(BlockType.Rect, new Vector2(randPos.x, -randPos.y), randScale, Quaternion.identity, solid);
            }
        }

        // Place spawn zones
        Vector2 randPos2 = new Vector2(Random.Range(halfLevelWidth * 0.2f, halfLevelWidth * 0.9f), 0);
        Vector2 randScale2 = Vector2.one * 4; //new Vector2(Random.Range(minMaxScaleMult.x, minMaxScaleMult.y), Random.Range(minMaxScaleMult.x, minMaxScaleMult.y)) * levelSize;

        PlaceSpawnZone(randPos2, randScale2, spawnZones);
        PlaceSpawnZone(new Vector2(-randPos2.x, randPos2.y), randScale2, spawnZones);
    }


    void Update()
    {
        
    }

    private void PlaceSpawnZone(Vector2 pos, Vector2 scale, Transform parent)
    {
        Transform s = Instantiate(spawnArea, pos, Quaternion.identity).transform;
        s.localScale = scale;
        s.transform.parent = parent;
        // TODO: clear space if any solid overlap boxray & scale & place individual spawn points apart (depending on team size later 2/3)
    }

    private Block PlaceBlock(BlockType type, Vector2 pos, Vector2 scale, Quaternion rot, Transform parent)//(BlockType type, Vector2 pos, Vector2 scale, Quaternion rot, bool moveable, float weight, bool destructible, Color color, Transform parent)
    {
        GameObject block = null;
        if (type == BlockType.Rect) block = rect;
        if (type == BlockType.Circ) block = circ;
        if (type == BlockType.Triang) block = triang;

        //TODO: add exception for non existing blocktype?

        Block b = Instantiate(block, pos, rot).GetComponent<Block>();
        b.transform.localScale = scale;
        b.transform.parent = parent;

        return b; // bad practise to return with this method name? but otherwise lots of parameters (that are often the same)
    }
}
