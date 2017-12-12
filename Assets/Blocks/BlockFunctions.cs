using System;
using System.Collections.Generic;
using UnityEngine;

public static class BlockFunctions
{
    static BlockManager blockMan = BlockManager.Instance;
    static System.Random random = Randomizer.random;
    static int maxBlocks = IndividualManager.Instance.maxIndividualSize;

    #region coordinate Manipulation

    /// <summary>
    /// Given a list of open spaces, returns an open space to work with
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static double randomOpenSpace(List<double> input)
    {
        int index = random.Next(0, input.Count);
        return input[index];
    }

    /// <summary>
    /// Given an x, y, and z value, calculates the offset into a 1D array
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static double calculateOffset(int x, int y, int z)
    {
        return x + maxBlocks * (y + z * maxBlocks);
    }

    /// <summary>
    /// Outs an x, y, and z value for a given offset
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="x">The x location</param>
    /// <param name="y">The y location</param>
    /// <param name="z">The z location</param>
    public static void calculateCoords(double offset, out int x, out int y, out int z)
    {
        z = (int)(offset / (maxBlocks * maxBlocks));
        offset -= (z * maxBlocks * maxBlocks);
        y = (int)(offset / maxBlocks);
        x = (int)(offset % maxBlocks);
    }

    #endregion

    #region block finding
    /// <summary>
    /// Finds the payloadID as defined in the block manager
    /// </summary>
    /// <returns></returns>
    internal static int findBlockID(string blockName)
    {
        int payloadID = 0;
        ///Finding the TID of the payload
        foreach (BlockType type in BlockManager.Instance.blockTypes)
        {
            if (type.Name.ToLower().Contains(blockName.ToLower()))
            {
                payloadID = type.UID;
                ///We no longer need to keep looking
                break;
            }
        }

        return payloadID;
    }
    #endregion

    #region Neighbour Checking
    /// <summary>
    /// Returns 
    /// if a block is surrounded on all sides.
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="index"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <returns></returns>
    private static bool surrounded(double index, Dictionary<double, Block> individual)
    {
        int x, y, z = 0;
        calculateCoords(index, out x, out y, out z);

        ///Here we simply look for what our neighbours would be
        ///We don't care if they exist outside of the bounds because
        ///we only care if we are surrounded or not
        double xPos = calculateOffset(x + 1, y, z);
        double yPos = calculateOffset(x, y + 1, z);
        double zPos = calculateOffset(x, y, z + 1);
        double xNeg = calculateOffset(x - 1, y, z);
        double yNeg = calculateOffset(x, y - 1, z);
        double zNeg = calculateOffset(x, y, z - 1);

        ///If all of our neighbours exist in the individual, then we are surrounded
        return (individual.ContainsKey(xPos) &&
                    individual.ContainsKey(yPos) &&
                    individual.ContainsKey(zPos) &&
                    individual.ContainsKey(xNeg) &&
                    individual.ContainsKey(yNeg) &&
                    individual.ContainsKey(zNeg));

    }

    /// <summary>
    /// When we place a block, we potentially have added more faces for new blocks to be added (i.e. adding onto our faces).
    /// Because of this, we need to add these new spots to our open list.
    /// This function calculates whether a spot will be open or not, and will only return open spaces that are actually open.
    /// It will return all open indexes surrounding the placed blocks.
    /// </summary>
    /// <param name="index">the space of the block just added to the current blocks</param>
    /// <param name="currentBlcoks"></param>
    /// <returns></returns>
    public static List<double> addNeighboursToOpen(double index, Dictionary<double, Block> currentBlocks)
    {
        int x, y, z = 0;
        calculateCoords(index, out x, out y, out z);

        ///I am aware that doing it like this is the absolute least efficient way of doing this,
        ///however to calculate the logic of this is actually a lot more intensive
        double xPos = calculateOffset(x + 1, y, z);
        double yPos = calculateOffset(x, y + 1, z);
        double zPos = calculateOffset(x, y, z + 1);
        double xNeg = calculateOffset(x - 1, y, z);
        double yNeg = calculateOffset(x, y - 1, z);
        double zNeg = calculateOffset(x, y, z - 1);

        List<double> output = new List<double>();

        output.Add(xNeg);
        output.Add(xPos);
        output.Add(yNeg);
        output.Add(yPos);
        output.Add(zNeg);
        output.Add(zPos);

        return output;


    }
    #endregion

    #region block bitmask usage
    /// <summary>
    /// Takes a face and a rotation, and returns what that face should be after rotation
    /// </summary>
    /// <param name="before">the face</param>
    /// <param name="rotate">the rotation value</param>
    /// <returns></returns>
    internal static facing rotateFace(facing before, rotation rotate)
    {
        facing retFace = before;
        #region 90degrees
        if (rotate == rotation.R90)
        {
            switch (before)
            {
                case facing.DOWN:
                    retFace = facing.LEFT;
                    break;
                case facing.LEFT:
                    retFace = facing.UP;
                    break;
                case facing.UP:
                    retFace = facing.RIGHT;
                    break;
                case facing.RIGHT:
                    retFace = facing.DOWN;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 180degrees
        if (rotate == rotation.R180)
        {
            switch (before)
            {
                case facing.DOWN:
                    retFace = facing.UP;
                    break;
                case facing.LEFT:
                    retFace = facing.RIGHT;
                    break;
                case facing.UP:
                    retFace = facing.DOWN;
                    break;
                case facing.RIGHT:
                    retFace = facing.LEFT;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 270degrees
        if (rotate == rotation.R270)
        {
            switch (before)
            {
                case facing.DOWN:
                    retFace = facing.RIGHT;
                    break;
                case facing.LEFT:
                    retFace = facing.DOWN;
                    break;
                case facing.UP:
                    retFace = facing.LEFT;
                    break;
                case facing.RIGHT:
                    retFace = facing.UP;
                    break;
                default:
                    break;
            }
        }
        #endregion
        return retFace;

    }

    /// <summary>
    /// Takes a face and an orientation, and returns what the face should be after orientation 
    /// </summary>
    /// <param name="before"></param>
    /// <param name="orientation"></param>
    /// <returns></returns>
    internal static facing orientFace(facing before, facing orientation)
    {
        facing retFace = before;
        #region downward facing
        if (orientation == facing.DOWN)
        {
            switch (before)
            {
                case facing.DOWN:
                    retFace = facing.FRONT;
                    break;
                case facing.FRONT:
                    retFace = facing.UP;
                    break;
                case facing.UP:
                    retFace = facing.BACK;
                    break;
                case facing.BACK:
                    retFace = facing.DOWN;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region frontward facing
        if (orientation == facing.FRONT)
        {
            switch (before)
            {
                case facing.DOWN:
                    retFace = facing.UP;
                    break;
                case facing.FRONT:
                    retFace = facing.BACK;
                    break;
                case facing.UP:
                    retFace = facing.DOWN;
                    break;
                case facing.BACK:
                    retFace = facing.FRONT;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region upward facing
        if (orientation == facing.UP)
        {
            switch (before)
            {
                case facing.DOWN:
                    retFace = facing.BACK;
                    break;
                case facing.FRONT:
                    retFace = facing.DOWN;
                    break;
                case facing.UP:
                    retFace = facing.FRONT;
                    break;
                case facing.BACK:
                    retFace = facing.UP;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region leftward facing
        if (orientation == facing.LEFT)
        {
            switch (before)
            {
                case facing.LEFT:
                    retFace = facing.FRONT;
                    break;
                case facing.FRONT:
                    retFace = facing.RIGHT;
                    break;
                case facing.RIGHT:
                    retFace = facing.BACK;
                    break;
                case facing.BACK:
                    retFace = facing.LEFT;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region rightward facing
        if (orientation == facing.RIGHT)
        {
            switch (before)
            {
                case facing.LEFT:
                    retFace = facing.BACK;
                    break;
                case facing.FRONT:
                    retFace = facing.LEFT;
                    break;
                case facing.RIGHT:
                    retFace = facing.FRONT;
                    break;
                case facing.BACK:
                    retFace = facing.RIGHT;
                    break;
                default:
                    break;
            }
        }
        #endregion
        return retFace;
    }

    #endregion

    #region block insertion
    /// <summary>
    /// Pass in the structure which holds your blocks, and this will add a random block to it if able.
    /// </summary>
    /// <param name="structure"></param>
    /// <returns>The altered datastructure, null if it failed</returns>
    internal static bool insertBlockAtRandom(IndividualDatastructure structure)
    {
        List<BlockType> blocks = blockMan.blockTypes;
        BlockType desired = blocks[random.Next(0, blocks.Count - 1)];
        return insertBlockTypeAtRandom(structure, desired);        
    }

    /// <summary>
    /// Generates a random block. Note this is nto used in the following or previous functions
    /// and it should be self evident why.
    /// </summary>
    /// <returns></returns>
    internal static Block generateRandomBlock()
    {
        List<BlockType> blocks = blockMan.blockTypes;
        BlockType desired = blocks[random.Next(0, blocks.Count - 1)];
        facing face = Randomizer.RandomEnumValue<facing>();
        rotation rot = Randomizer.RandomEnumValue<rotation>();
        Block pBlock = new Block(desired.UID, face, rot);
        return pBlock;
    }

    /// <summary>
    /// Pass in the structure which holds your blocks, as well as the type of block you would like added, and
    /// this will add that block at a random spot to your structure
    /// </summary>
    /// <param name="structure"></param>
    /// <param name="type"></param>
    /// <returns>The altered datastructure, null if failed</returns>
    internal static bool insertBlockTypeAtRandom(IndividualDatastructure structure, BlockType type)
    {
        double openSpace = BlockFunctions.randomOpenSpace(structure.openSpaces);
        ///This is our block
        facing face = Randomizer.RandomEnumValue<facing>();
        rotation rot = Randomizer.RandomEnumValue<rotation>();
        Block pBlock = new Block(type.UID, face, rot);

        ///If we've gotten to this point, then we can successfully place ourself into the individual
        /*
         * IVE BEEN GETTING AN ERROR WHERE WE TRY TO PLACE BLOCKS THAT ALREADY EXIST.
         * NO IDEA WHY THIS HAPPENS AND IT SHOULDN'T SO I'M SIMPLY PUTTING THIS CONDITIONAL HERE AS A JANK FIX
         */
        if (structure.contents.ContainsKey(openSpace))
        {
            return false;
        }
        structure.contents.Add(openSpace, pBlock);


        ///Remove the open space from structure.openSpaces
        structure.openSpaces.Remove(openSpace);
        ///Add the possible neighbours of the placed block to our open neighbours list 
        List<double> openConsiderations = BlockFunctions.addNeighboursToOpen(openSpace, structure.contents);

        ///Iterating over the open spots we are considering, and check to see if they are already filled or not.
        ///Then adding them to our open spaces list if they are not.

        foreach (double possible in openConsiderations)
        {
            if (structure.contents.ContainsKey(possible))
            {
                continue;
            }
            structure.openSpaces.Add(possible);
        }
        return true;
    }

    #endregion

    #region GameObject Manipulation

    /// <summary>
    /// Using a blocks properties, creates the physical block as a gameobject in 3D space.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    internal static GameObject createBlock(double index, Dictionary<double, Block> individual)
    {
        int x, y, z;
        calculateCoords(index, out x, out y, out z);
        ///figure out what block
        Block me = individual[index];
        ///create the block
        GameObject blockModel = GameObject.Instantiate(blockMan.blockTypes[me.type].model);
        ///set the position of the block
        blockModel.transform.position = new Vector3(x, y, z);
        ///orient the block
        orientBlock(me, blockModel);
        ///enable the collider
        blockModel.GetComponent<Collider>().enabled = true;
        /*
         * 
         * 
        ///add a rigidbody
        Rigidbody mine = blockModel.AddComponent<Rigidbody>();
        ///set its weight
        mine.mass = blockMan.blockTypes[me.type].weight;
        ///set gravity on/off
        mine.useGravity = true;
        ///set drag to 0
        mine.drag = 0;
        ///turn off collisions
        mine.detectCollisions = false;
        ///If we are surrounded, don't bother rendering me
        */
        if (surrounded(index, individual))
        {
            blockModel.GetComponent<MeshRenderer>().enabled = false;
        }

        return blockModel;
    }



    /// <summary>
    /// Using a blocks properties, creates the physical block as a gameobject in 3D space.
    ///
    /// NOTE THAT THIS IS REALLY INEFFICIENT AS EVERY SINGLE BLOCK IS RENDERED
    /// AND GIVEN A BOX COLLIDER. AVOID THIS AS MUCH AS POSSIBLE
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    internal static GameObject createBlock(double index, Block type)
    {
        int x, y, z;
        calculateCoords(index, out x, out y, out z);

        ///create the block
        GameObject blockModel = GameObject.Instantiate(blockMan.blockTypes[type.type].model);
        ///set block position
        blockModel.transform.position = new Vector3(x, y, z);
        ///orient the block
        orientBlock(type, blockModel);
        ///Enable the collider
        blockModel.GetComponent<Collider>().enabled = true;
        ///Add a rigidbody
        /*
         * Rigidbody mine = blockModel.AddComponent<Rigidbody>();
        ///set the mass
        mine.mass = blockMan.blockTypes[type.type].weight;
        ///turn gravity on/off
        mine.useGravity = true;
        ///turn off drag
        mine.drag = 0;
        ///turn off collisions
        mine.detectCollisions = false;
        */

        return blockModel;

    }

    /// <summary>
    /// Takes a block, and the physical game object associated with it, and orients it to the rotation, and facing defined in
    /// the block data
    /// </summary>
    /// <param name="block"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    internal static void orientBlock(Block block, GameObject model)
    {
        rotation rot = block.rotate;
        model.transform.Rotate(0, 0, 90 * (int)rot);
        faceBlock(block, model);
    }

    /// <summary>
    /// Takes a block, and the physical game object associated with it, and makes it face properly.
    /// </summary>
    /// <param name="block"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    internal static void faceBlock(Block block, GameObject model)
    {
        facing face = block.face;
        ///Again, an incredibly shitty way to do this, but like fuck I'm bad at linear algebra
        switch (face)
        {
            case facing.UP:
                model.transform.Rotate(90, 0, 0, 0);
                break;
            case facing.RIGHT:
                model.transform.Rotate(0, 90, 0, 0);
                break;
            case facing.LEFT:
                model.transform.Rotate(0, -90, 0, 0);
                break;
            case facing.FRONT:
                model.transform.Rotate(180, 0, 0, 0);
                break;
            case facing.DOWN:
                model.transform.Rotate(90, 0, 0, 0);
                break;
            default:
                break;
        }
    }
    #endregion

}

