public static class Utils
{
    public enum TableType
    {
        Basic, CuttingBoard, Pot, GasStove, Packaging, Pickup, TrashCan
    }   // 일반, 도마, 냄비, 가스레인지, 포장, 픽업, 쓰레기통

    public enum CarType
    {
        Hatchback, Police, Sedan, Stationwagon, Taxi
    }

    public enum CrateType
    {
        Bun, Cheese, Lettuce, Onion, Tomato, Meat, None
    }   // 빵, 치즈, 양상추, 양파, 토마토, 고기

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

    public enum FoodState
    {
        None, Packaging
    }   // 비포장, 포장

    public enum CharacterType
    {
        Player, NPC
    }   // 플레이어, NPC

    public enum CurrencyType
    {
        Gold
    }   // 금화

    public enum ShopTabType
    {
        Ingredient, Player, Guest
    }   // 상점 탭 타입

    public enum SceneType
    {
        None, Title, MainGame, MultiGame
    }   // 타이틀, 메인게임

    public enum PoolType
    {
        GameObject, UI
    }   // 풀 타입

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

    public enum CarWaypointType
    {
        Outside_L, Outside_R,
    }

    public const float GRILL_OVERCOOKED_TIME = 8f;      // 고기 굽기 실패 시간
    public const float BOIL_STEW_COOK_TIME = 5f;            // 스튜 제작 시간

    public enum ShopItemIndex
    {
        SHOP_INCREDIENT_LETTUCE_SELL_INDEX,
        SHOP_INCREDIENT_TOMATO_SELL_INDEX,
        SHOP_INCREDIENT_ONION_SELL_INDEX,
        SHOP_INCREDIENT_CHEESE_SELL_INDEX,
        SHOP_INCREDIENT_MEAT_SELL_INDEX,
        SHOP_INCREDIENT_LETTUCE_YIELD_INDEX,
        SHOP_INCREDIENT_TOMATO_YIELD_INDEX,
        SHOP_INCREDIENT_ONION_YIELD_INDEX,
        SHOP_INCREDIENT_CHEESE_YIELD_INDEX,
        SHOP_PLAYER_CUTTING_SPEED_INDEX,
        SHOP_PLAYER_MOVE_SPEED_INDEX,
        SHOP_GUEST_PATIENT_TIME_INDEX,
        None
    }   // 상점 아이템 인덱스

    public enum GameDataIndex
    {
        IngredientsData, FoodsData, PlayersData, GuestsData, TablesData, ShopItemsData, CarsData
    }   // 게임 데이터 인덱스
}
