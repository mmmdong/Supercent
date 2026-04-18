using UnityEngine;

public class Define
{
    #region STRING

    public static readonly string ANIMATION_RUN = "Run";
    public static readonly string ANIMATION_IDLE = "Idle";
    public static readonly string ANIMATION_PICKING = "Picking";
    public static readonly string ANIMATION_TAKE = "Take";

    public static readonly string CHECKPOINT_OPEN = "Open"; 
    #endregion

    #region INTEGER

    public static readonly int MONEY_COST = 10;

    #endregion
    #region VECTOR3

    public static readonly Vector3 CAM_POS = new Vector3(6, 10, -6);

    #endregion

    #region ENUM

    public enum PooledEnum
    {
        // 오브젝트
        Rock,
        
        // 프랍
        Prop_Rock,
        Prop_Money,
        Prop_Handcuff,
        
        // 파티클
        Hit_Particle,
        
        // 유닛
        Prisoner,
    }

    public enum ConsumeDest
    {
        Hire_Cop,
        Hire_Worker,
        LevelUp,
    }

    #endregion
}