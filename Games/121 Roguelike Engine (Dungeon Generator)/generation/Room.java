package generation;

public class Room {
    
    // in Game coords
    private int[] pos;
    
    
    private ROOM_TYPE type;
    public ROOM_TYPE doorN, doorS, doorE, doorW;
    
    public static int[] room_probability = {15, 20, 65};
    
    public enum ROOM_TYPE {
        LOOT,
        TRAP,
        NORMAL,
        SPAWN,
        BOSS,
        KEY,
        NONE;
    }


    public Room(int[] pos, ROOM_TYPE type) {
        this.pos = pos;
        this.type = type;
    }
    
    public ROOM_TYPE getType() {
        return type;
    }

    public void setType(ROOM_TYPE type) {
        this.type = type;
    }
}