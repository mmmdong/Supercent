using UnityEngine;

public class Define
{
    #region STRING

    // Animation Hash
    public static readonly string ANIMATION_RUN = "Run";
    public static readonly string ANIMATION_IDLE = "Idle";
    public static readonly string ANIMATION_PICKING = "Picking";
    public static readonly string ANIMATION_TAKE = "Take";
    public static readonly string CHECKPOINT_OPEN = "Open"; 
    
    // Path
    public static readonly string PATH_OBJECTS = "Prefabs/PooledObject";
    
    #endregion

    #region INTEGER

    public static readonly int MONEY_COST = 10;
    public static readonly int WORKER_COUNT = 3;
    public static readonly int PRISONER_MONEY_COUNT = 2;
    public static readonly int COP_HANDCUFF_THRESHOLD = 10;

    #endregion

    #region FLOAT

    public static readonly float PROPSETTING_TIME = 0.05f;
    public static readonly float STACK_GAP = 0.15f;
    public static readonly float ARRIVE_THRESHOLD = 0.15f;
    public static readonly float WORKER_PATROL_DISTANCE = 14f;
    public static readonly float COP_HANDCUFF_WAIT_TIME = 3f;
    public static readonly float COP_SPAWN_WAIT_TIME = 3f;

    #endregion
    
    #region VECTOR3

    public static readonly Vector3 CAM_POS = new Vector3(6, 10, -6);

    #endregion

    #region ENUM

    public enum PooledEnum
    {
        // 바닥 구성
        Rock,
        
        // Prop
        Prop_Rock,
        Prop_Money,
        Prop_Handcuff,
        
        // Effect
        Hit_Particle,
        LevelUp_Particle,
        
        // Unit
        Prisoner,
        Worker,
        Cop
    }

    public enum ConsumeDest
    {
        Hire_Cop,
        Hire_Worker,
        LevelUp,
    }

    #endregion
}