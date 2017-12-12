using System.Runtime.Serialization;
/// <summary>
/// The Memory representation of a block
/// </summary>

public class Block
{

    public facing face { set; get; }

    
    public rotation rotate { set; get; }

    
    public int type { set; get; }

    /// <summary>
    /// The constructor for the block. Notice that block type is an int. This is only for when we create the physical block,
    /// we can lookup what type it is within our blocktype class
    /// </summary>
    /// <param name="active"></param>
    /// <param name="UID">The UID of the block type</param>
    public Block(int TID, facing face, rotation rot)
    {
        this.type = TID;
        this.face = face;
        this.rotate = rot;
    }

    /// <summary>
    /// Create a non-active block of type t facing 0, and not rotated
    /// </summary>
    /// <param name="TID">The TID of the block type</param>
    public Block(int TID) : this(TID, facing.BACK, rotation.R0) { }

    /// <summary>
    /// Create a decidably active block of type t facing default, and not rotated
    /// </summary>
    /// <param name="TID">The TID of the block type</param>
    public Block(bool b, int TID) : this(TID, facing.BACK, rotation.R0) { }

    /// <summary>
    /// For creating empty blocks in chunks
    /// </summary>
    public Block(): this(0, facing.BACK, rotation.R0){}
}

public enum facing
{
    BACK = 1,
    RIGHT = 2,
    FRONT = 3,
    LEFT = 4,
    UP = 5,
    DOWN = 6
}

public enum rotation
{
    R0 = 0,
    R90 = 1,
    R180 = 2,
    R270 = 3
}
