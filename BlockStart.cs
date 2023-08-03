using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStart : MonoBehaviour
{
    [Header("Random Rotation")]
    public bool randomRotation = false;
    public Transform tBlock;
    [Range(0f, 45f)]
    public float rotRange;

    [Header("Crystals")]
    public bool crystals = false;
    public int crystalBlockType;

    [Header("Crystal")]
    public bool isCrystal;

    // Start is called before the first frame update
    void Start()
    {
        if (randomRotation)
        {
            tBlock.Rotate(0f, 0f, Random.Range(-rotRange, rotRange));
        }

        if (crystals)
        {
            World world = GameObject.Find("World").GetComponent<World>();

            Vector2 xp = new Vector2(Mathf.FloorToInt(transform.position.x + 1f), Mathf.FloorToInt(transform.position.y));
            Vector2 xm = new Vector2(Mathf.FloorToInt(transform.position.x - 1f), Mathf.FloorToInt(transform.position.y));
            Vector2 yp = new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y + 1f));
            Vector2 ym = new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y - 1f));

            bool fxp = world.FindBlockFromVector2(xp);
            bool fxm = world.FindBlockFromVector2(xm);
            bool fyp = world.FindBlockFromVector2(yp);
            bool fym = world.FindBlockFromVector2(ym);

            if (!fyp) {
                world.AddBlock(new Vector3(yp.x, yp.y, 0f), world.GetChunkFromVector2(yp), crystalBlockType);
            }
            else if (!fym)
            {
                world.AddBlock(new Vector3(ym.x, ym.y, 0f), world.GetChunkFromVector2(ym), crystalBlockType);
            }
            else {
                if (!fxm)
                {
                    world.AddBlock(new Vector3(xm.x, xm.y, 0f), world.GetChunkFromVector2(xm), crystalBlockType);
                }
                if (!fxp)
                {
                    world.AddBlock(new Vector3(xp.x, xp.y, 0f), world.GetChunkFromVector2(xp), crystalBlockType);
                }
            }

        }

        if (isCrystal)
        {
            World world = GameObject.Find("World").GetComponent<World>();

            Vector2 xp = new Vector2(Mathf.FloorToInt(transform.position.x + 1f), Mathf.FloorToInt(transform.position.y));
            Vector2 xm = new Vector2(Mathf.FloorToInt(transform.position.x - 1f), Mathf.FloorToInt(transform.position.y));
            Vector2 yp = new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y + 1f));
            Vector2 ym = new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y - 1f));

            bool fxp = world.GetBlockTypeFromVectror2(xp) == "LightOre";
            bool fxm = world.GetBlockTypeFromVectror2(xm) == "LightOre";
            bool fyp = world.GetBlockTypeFromVectror2(yp) == "LightOre";
            bool fym = world.GetBlockTypeFromVectror2(ym) == "LightOre";

            if (fym)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            } 
            else if (fyp) 
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                if (fxp) 
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                if (fxm)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 270);
                }
            }

        }

        Destroy(this);
    }
}
