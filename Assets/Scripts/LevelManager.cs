using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private enum Symmetry { x, xy, none };
    public enum BlockType { Rect, Circ, Triang};

    [Header("Generation parameters")]
    [SerializeField] private int seed;
    [SerializeField] private float levelSize = 10;
    [SerializeField] private int randObjs = 10;
    [SerializeField] private Symmetry randSym = Symmetry.none;
    [SerializeField] private Color defaultColor;

    [Space]

    [Header("Blocks")]
    [SerializeField] private GameObject rect;
    [SerializeField] private GameObject circ;
    [SerializeField] private GameObject triang;


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


        // Generate map bounds
        placeBlock(BlockType.Rect, new Vector2( topRight.x, 0), new Vector2(1, halfLevelHeight * 2), Quaternion.identity, false, 10, false, defaultColor);
        placeBlock(BlockType.Rect, new Vector2(-topRight.x, 0), new Vector2(1, halfLevelHeight * 2), Quaternion.identity, false, 10, false, defaultColor);
        placeBlock(BlockType.Rect, new Vector2(0,  topRight.y), new Vector2(halfLevelWidth * 2, 1), Quaternion.identity, false, 10, false, defaultColor);
        placeBlock(BlockType.Rect, new Vector2(0, -topRight.y), new Vector2(halfLevelWidth * 2, 1), Quaternion.identity, false, 10, false, defaultColor);


        // Actually generate map
        // Random ammount of blocks at random pos, with random scale but factored in with levelScale and mirrored accordingly

        if (randSym == Symmetry.none)
        {
            for (int i = 0; i < randObjs; i++)
            {
                Vector2 randPos = new Vector2(Random.Range(-halfLevelWidth, halfLevelWidth), Random.Range(-halfLevelHeight, halfLevelHeight));

                placeBlock(BlockType.Rect, randPos, Vector2.one, Quaternion.identity, false, 10, false, defaultColor);
            }
        }
        if (randSym == Symmetry.x)
        {
            for (int i = 0; i < randObjs / 2; i++)
            {
                Vector2 randPos = new Vector2(Random.Range(0, halfLevelWidth), Random.Range(-halfLevelHeight, halfLevelHeight));

                placeBlock(BlockType.Rect, randPos, Vector2.one, Quaternion.identity, false, 10, false, defaultColor);
                placeBlock(BlockType.Rect, new Vector2(-randPos.x, randPos.y), Vector2.one, Quaternion.identity, false, 10, false, defaultColor);
            }
        }
        if (randSym == Symmetry.xy)
        {
            for (int i = 0; i < randObjs / 4; i++)
            {
                Vector2 randPos = new Vector2(Random.Range(0, halfLevelWidth), Random.Range(0, halfLevelHeight));

                placeBlock(BlockType.Rect, randPos, Vector2.one, Quaternion.identity, false, 10, false, defaultColor);
                placeBlock(BlockType.Rect, new Vector2(-randPos.x, randPos.y), Vector2.one, Quaternion.identity, false, 10, false, defaultColor);
                placeBlock(BlockType.Rect, new Vector2(-randPos.x, -randPos.y), Vector2.one, Quaternion.identity, false, 10, false, defaultColor);
                placeBlock(BlockType.Rect, new Vector2(randPos.x, -randPos.y), Vector2.one, Quaternion.identity, false, 10, false, defaultColor);
            }
        }
    }


    void Update()
    {
        
    }

    private void placeBlock(BlockType type, Vector2 pos, Vector2 scale, Quaternion rot, bool moveable, float weight, bool destructible, Color color)
    {
        GameObject block = null;
        if (type == BlockType.Rect) block = rect;
        if (type == BlockType.Circ) block = circ;
        if (type == BlockType.Triang) block = triang;

        //TODO: add exception for non existing blocktype?

        Block b = Instantiate(block, pos, Quaternion.identity).GetComponent<Block>();
        b.init(scale, moveable, weight, destructible, color);
    }
}
