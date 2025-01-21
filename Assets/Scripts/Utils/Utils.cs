public static class Utils
{
    public enum TableType
    {
        Basic, CuttingBoard, Pot, GasStove, Packaging
    }   // 일반, 도마, 냄비, 가스레인지, 포장

    public enum CrateType
    {
        Bun, Cheese, Lettuce, Mushroom, Onion, Tomato, Meat, None
    }   // 빵, 치즈, 양상추, 버섯, 양파, 토마토, 고기

    public enum CookState
    {
        UnCook, Cook, OverCook
    }   // 비조리, 조리, 타버림

    public enum IngredientState
    {
        Basic, Cutting, CutOver, Grilling, GrillOver, None
    }   // 기본 상태, 칼질중 상태, 칼질 완료 상태, 구워지는중 상태, 구워진 상태

    public enum FoodType
    {
        Hamburger, Stew
    }   // 햄버거, 스튜

    public enum CharacterType
    {
        Player, NPC
    }   // 플레이어, NPC

    // 가게로 이동, 밖으로 이동(오른쪽), 밖으로 이동(왼쪽),
    // 픽업존 1 ~ 4,
    // 웨이팅존 1 ~ 8
    // 픽업 테이블에서 밖으로 이동(오른쪽), 픽업 테이블에서 밖으로 이동(왼쪽)
    public enum WaypointType
    {
        MoveStore, Outside_R, Outside_L, 
        PickupZone_1, PickupZone_2, PickupZone_3, PickupZone_4,
        WaitingZone_1, WaitingZone_2, WaitingZone_3, WaitingZone_4, WaitingZone_5, WaitingZone_6, WaitingZone_7, WaitingZone_8,
        Pickup_Outside_R, Pickup_Outside_L,
    }   

    public const float GRILL_COOK_TIME = 5f;            // 고기 굽기 완성 시간
    public const float GRILL_OVERCOOKED_TIME = 8f;      // 고기 굽기 실패 시간
    public const float CUTTING_CHEESE_COOK_TIME = 2f;       // 치즈 손질 시간
    public const float CUTTING_LETTUCE_COOK_TIME = 3.5f;    // 양상추 손질 시간
    public const float CUTTING_MUSHROOM_COOK_TIME = 3f;     // 버섯 손질 시간
    public const float CUTTING_ONION_COOK_TIME = 2.5f;      // 양파 손질 시간
    public const float CUTTING_TOMATO_COOK_TIME = 3f;       // 토마토 손질 시간
    public const float BOIL_STEW_COOK_TIME = 5f;            // 스튜 제작 시간

    public const int BURGER_TOPPING_COUNT_MAX = 5;      // 햄버거 토핑 최대 개수
}
