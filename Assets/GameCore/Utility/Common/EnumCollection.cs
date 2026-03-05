namespace GameCore.Utility.Common
{
    public enum ColorType
    {
        Blue,
        Navy,
        Green,
        Orange,
        Pink,
        Purple,
        Red,
        Yellow,
        Brown,
        DarkGreen,
        White,
        Black,
        None
    }

    public enum BlockType
    {
        Normal,
        Freezing,
        VerticalMove,
        HorizontalMove,
        Boom,
        Lock,
        Key,
        LayerBlock,
        Obstacle
    }

    public enum BlockName
    {
        Block_2x2,
        Block_2x4,
        Block_2x6,
        Block_4x4,
        Block_L_4x4,
        Block_L_4x6,
        Block_T_4x6,
        Block_Plus_6X6,
        Block_L_4X6_Flip
    }

    public enum GateName
    {
        Bot_Left,
        Bot_Mid,
        Bot_Right,
        Left_Bot,
        Left_Mid,
        Left_Top,
        Right_Bot,
        Right_Mid,
        Right_Top,
        Top_Left,
        Top_Right
    }


    public enum LoseReason
    {
        Undetected,
        TimeOut,
        Replay,
    }

    public enum ProfileTabType
    {
        Frame,
        Avatar,
        Flag
    }
    
    // public enum BoosterType
    // {
    //     None,
    //     Freeze,
    //     Break,
    // }
}