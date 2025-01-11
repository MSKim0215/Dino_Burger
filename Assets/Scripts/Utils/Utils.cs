public static class Utils
{
    public enum TableType
    {
        Basic, CuttingBoard, Pot, GasStove, Packaging
    }   // 일반, 도마, 냄비, 가스레인지, 포장

    public enum CrateType
    {
        Bun, Cheese, Lettuce, Mushroom, Onion, Tomato, Meat
    }   // 빵, 치즈, 양상추, 버섯, 양파, 토마토, 고기

    public enum CookState
    {
        UnCook, Cook, OverCook
    }   // 비조리, 조리, 타버림

    public const float GRILL_COOK_TIME = 5f;            // 고기 굽기 완성 시간
    public const float GRILL_OVERCOOKED_TIME = 8f;      // 고기 굽기 실패 시간
    public const float CUTTING_CHEESE_COOK_TIME = 4f;   // 치즈 손질 시간
}
