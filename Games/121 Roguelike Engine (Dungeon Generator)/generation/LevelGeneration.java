package generation;

import java.util.List;
import java.util.ArrayList;

import java.io.IOException;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.awt.Graphics2D;
import java.awt.Color;
import java.io.File;


public class LevelGeneration {
    
    private int[] worldSize = {6, 6};
    private Room[][] rooms;
    private List<int[]> takenPos = new ArrayList<int[]>();
    
    private int numOfRooms = 20;
    private int gridSizeX, gridSizeY;
    

    public LevelGeneration(int numOfRooms) {
        this.numOfRooms = numOfRooms;
    }
    

    public LevelGeneration() {
        this(20);
    }
    

    public void generateLevel() {
        if(numOfRooms >= worldSize[0] * worldSize[1] * 4)
            numOfRooms = worldSize[0] * worldSize[1] * 4;
        
        gridSizeX = worldSize[0];
        gridSizeY = worldSize[1];
        
        createRooms();
        createRoomDoors();
        
        int[] bossCoords = getFurthestRoomFrom(toWorldCoords(zeroVector()));
        markDoors(bossCoords, Room.ROOM_TYPE.BOSS);
        
        int[] keyCoords = getFurthestRoomFrom(bossCoords);
        markDoors(keyCoords, Room.ROOM_TYPE.KEY);
        
        takenPos.clear();
        
        try {
            saveMapImg();
        } catch(IOException ex) {
            System.out.println("<LevelGeneration> Error when writing to map.png");
        }
    }

    private void createRooms() {
        
        rooms = new Room[gridSizeX * 2][gridSizeY * 2];
        // Spawn room in the middle
        rooms[gridSizeX][gridSizeY] = new Room(zeroVector(), Room.ROOM_TYPE.SPAWN);
        takenPos.add(0, toWorldCoords(zeroVector()));
        
        float randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        for(int i = 0; i < numOfRooms - 1; ++i) {
            int[] checkPos = newPos();
            
            float randomPerc = ((float)i/ ((float)numOfRooms - 1));
            float randomCompare = lerp(randomCompareStart, randomCompareEnd, randomPerc);

            if(numberOfNeighbors(checkPos) > 1 && Math.random() > randomCompare) { 

                int iter = 0;
                do {
                    checkPos = bestNewPos();
                    iter++;
                } while(numberOfNeighbors(checkPos) > 1 && iter < 100);
                if(iter >= 50)
                    System.out.println("<LevelGeneration> No room with less than " + numberOfNeighbors(checkPos) + " neighbors");
            }

            rooms[checkPos[0]][checkPos[1]] = new Room(toGameCoords(checkPos), pickRandomRoomType());
            takenPos.add(0, checkPos);
        }
    }
    

    private int[] newPos() {
        int x = 0, y = 0;
        int[] checkPos = toWorldCoords(zeroVector());
        
        do {
            int i = Math.round((float)(Math.random() * (takenPos.size() - 1)));
            x = takenPos.get(i)[0];
            y = takenPos.get(i)[1];
            
            boolean changeXorY = Math.random() < 0.5;
            boolean subtractOrAdd = Math.random() < 0.5;
            if(changeXorY) {
                if(subtractOrAdd)
                    y += 1;
                else
                    y -= 1;
            }else {
                if(subtractOrAdd)
                    x += 1;
                else
                    x -= 1;
            }
            checkPos[0] = x;
            checkPos[1] = y;
        } while(posTaken(checkPos) || x < 0 || x >= 2 * gridSizeX || y < 0 || y >= 2 * gridSizeY); // while pos taken or outofmap
        
        return checkPos;
    }
    
    private int[] bestNewPos() {
        int x = 0, y = 0;
        int[] checkPos = toWorldCoords(zeroVector());
        
        do {
            int iter = 0;
            int i = 0;

            do {
                i = Math.round((float)(Math.random() * (takenPos.size() - 1)));
                iter++;
            } while(numberOfNeighbors(takenPos.get(i)) > 1 && iter < 100);
            x = takenPos.get(i)[0];
            y = takenPos.get(i)[1];
            
            boolean changeXorY = Math.random() < 0.5;
            boolean subtractOrAdd = Math.random() < 0.5;
            if(changeXorY) {
                if(subtractOrAdd)
                    y += 1;
                else
                    y -= 1;
            }else {
                if(subtractOrAdd)
                    x += 1;
                else
                    x -= 1;
            }
            checkPos[0] = x;
            checkPos[1] = y;
        } while(posTaken(checkPos) || x < 0 || x >= 2 * gridSizeX || y < 0 || y >= 2 * gridSizeY); // while pos taken or outofmap
        
        return checkPos;
    }

    private int numberOfNeighbors(int[] pos) {
        int count = 0;

        if(posTaken(rightVector(pos)))
            count++;
        if(posTaken(leftVector(pos)))
            count++;
        if(posTaken(upVector(pos)))
            count++;
        if(posTaken(downVector(pos)))
            count++;
            
        return count;
    }
    

    private boolean posTaken(int[] pos) {
        for(int[] i : takenPos) {
            if(i[0] == pos[0] && i[1] == pos[1])
                return true;
        }
        return false;
    }
    

    private Room.ROOM_TYPE pickRandomRoomType() {
        double rand = Math.random() * 100;
        int sum = 0;
        int index = 2;
        for(int i = 0; i < Room.room_probability.length; ++i) {
            sum += Room.room_probability[i];
            if(rand < sum) {
                index = i;
                break;
            }
        }
        return Room.ROOM_TYPE.values()[index];
    }

    private void createRoomDoors() {
        for(int x = 0; x < 2 * gridSizeX; ++x) {
            for(int y = 0; y < 2 * gridSizeY; ++y) {
                if(rooms[x][y] == null)
                    continue;
                int[] pos = {x, y};
                rooms[x][y].doorN = posTaken(upVector(pos)) ? Room.ROOM_TYPE.NORMAL : Room.ROOM_TYPE.NONE;
                rooms[x][y].doorS = posTaken(downVector(pos)) ? Room.ROOM_TYPE.NORMAL : Room.ROOM_TYPE.NONE;
                rooms[x][y].doorW = posTaken(leftVector(pos)) ? Room.ROOM_TYPE.NORMAL : Room.ROOM_TYPE.NONE;
                rooms[x][y].doorE = posTaken(rightVector(pos)) ? Room.ROOM_TYPE.NORMAL : Room.ROOM_TYPE.NONE;
            }
        }
    }


    private int[] getFurthestRoomFrom(int[] pos) {
        double max_dist = 0.0;
        int[] retPos = new int[2];
        for(int x = 0; x < 2 * gridSizeX; ++x) {
            for(int y = 0; y < 2 * gridSizeY; ++y) {

                if(rooms[x][y] == null 
                   || rooms[x][y].getType() == Room.ROOM_TYPE.KEY 
                   || rooms[x][y].getType() == Room.ROOM_TYPE.BOSS 
                   || rooms[x][y].getType() == Room.ROOM_TYPE.SPAWN)
                    continue;

                double dist = Math.sqrt(Math.pow(x - pos[0], 2) + Math.pow(y - pos[1], 2));
                if(dist > max_dist) {
                    max_dist = dist;
                    retPos[0] = x;
                    retPos[1] = y;
                }    
            }
        }
        return retPos;
    }
    

    private void markDoors(int[] pos, Room.ROOM_TYPE type) {
        rooms[pos[0]][pos[1]].setType(type);
        if(posTaken(upVector(pos))) {
            rooms[pos[0]][pos[1] - 1].doorS = type;
            rooms[pos[0]][pos[1]].doorN = type;
        }
        if(posTaken(downVector(pos))) {
            rooms[pos[0]][pos[1] + 1].doorN = type;
            rooms[pos[0]][pos[1]].doorS = type;
        }
        if(posTaken(rightVector(pos))) {
            rooms[pos[0] + 1][pos[1]].doorW = type;
            rooms[pos[0]][pos[1]].doorE = type;
        }
        if(posTaken(leftVector(pos))) {
            rooms[pos[0] - 1][pos[1]].doorE = type;
            rooms[pos[0]][pos[1]].doorW = type;
        }
    }
    

    public void saveMapImg() throws IOException {
        int width = gridSizeX * 2 * 9, height = gridSizeY * 2 * 9;

        BufferedImage bi = new BufferedImage(width + 20,height + 65, BufferedImage.TYPE_INT_ARGB);
        Graphics2D ig2 = bi.createGraphics();

        for(int x = 0; x < 2 * gridSizeX; ++x) {
            for(int y = 0; y < 2 * gridSizeY; ++y) {
                if(rooms[x][y] != null) {
                    ig2.setPaint(Color.BLACK);
                    ig2.drawRect(x * 9 + 1, y * 9 + 1, 7, 7);
                    
                    if(x == gridSizeX && y == gridSizeY) {
                        ig2.setPaint(Color.GREEN);
                        ig2.drawRect(x * 9 + 4, y * 9 + 4, 1, 1);
                        ig2.setPaint(Color.BLACK);
                    }
                    
                    if(rooms[x][y].getType() == Room.ROOM_TYPE.BOSS) {
                        ig2.setPaint(Color.RED);
                        ig2.drawRect(x * 9 + 4, y * 9 + 4, 1, 1);
                        ig2.setPaint(Color.BLACK); 
                    }else if(rooms[x][y].getType() == Room.ROOM_TYPE.LOOT) {
                        ig2.setPaint(Color.decode("#34b8ef"));
                        ig2.drawRect(x * 9 + 4, y * 9 + 4, 1, 1);
                        ig2.setPaint(Color.BLACK);
                    }else if(rooms[x][y].getType() == Room.ROOM_TYPE.TRAP) {
                        ig2.setPaint(Color.decode("#6621ce"));
                        ig2.drawRect(x * 9 + 4, y * 9 + 4, 1, 1);
                        ig2.setPaint(Color.BLACK);
                    }else if(rooms[x][y].getType() == Room.ROOM_TYPE.NORMAL) {
                        ig2.setPaint(Color.decode("#787878"));
                        ig2.drawRect(x * 9 + 4, y * 9 + 4, 1, 1);
                        ig2.setPaint(Color.BLACK);
                    }else if(rooms[x][y].getType() == Room.ROOM_TYPE.KEY) {
                        ig2.setPaint(Color.decode("#fac045"));
                        ig2.drawRect(x * 9 + 4, y * 9 + 4, 1, 1);
                        ig2.setPaint(Color.BLACK);
                    }
                    
                    if(rooms[x][y].doorN != Room.ROOM_TYPE.NONE)
                        ig2.drawRect(x * 9 + 4, y * 9, 1, 1);
                    if(rooms[x][y].doorS != Room.ROOM_TYPE.NONE)
                        ig2.drawRect(x * 9 + 4, y * 9 + 9, 1, 1);
                    if(rooms[x][y].doorW != Room.ROOM_TYPE.NONE)
                        ig2.drawRect(x * 9, y * 9 + 4, 1, 1);
                    if(rooms[x][y].doorE != Room.ROOM_TYPE.NONE)
                        ig2.drawRect(x * 9 + 9, y * 9 + 4, 1, 1);
                }
            }
        }
        ig2.setPaint(Color.decode("#787878")); //grey
        ig2.drawString("Normal Room", 12, bi.getHeight() - 50);
        
        ig2.setPaint(Color.decode("#34b8ef")); //light blue
        ig2.drawString("Loot Room", 12, bi.getHeight() - 40);
        
        ig2.setPaint(Color.decode("#fac045")); //gold
        ig2.drawString("Key Room", 12, bi.getHeight() - 30);
        
        ig2.setPaint(Color.decode("#6621ce")); //purple
        ig2.drawString("Trap Room", 12, bi.getHeight() - 20);
        
        ig2.setPaint(Color.RED);
        ig2.drawString("Boss Room", 12, bi.getHeight() - 10);
        
        ig2.setPaint(Color.GREEN);
        ig2.drawString("Spawn Room", 12, bi.getHeight() - 0);
        
        ImageIO.write(bi, "PNG", new File("map.png"));
    }
    
    public Room[][] getRooms() {
        return rooms;
    }
    
    public int[] getGridSize() {
        int[] v = {gridSizeX, gridSizeY};
        return v;
    }
    
    private float lerp(float a, float b, float f) {
        return (a * (1.0f - f)) + (b * f);
    }
    
    private int[] zeroVector() {
        int[] v = {0, 0};
        return v;
    }

    private int[] rightVector(int[] v) {
        int[] vRet = {v[0] + 1, v[1]};
        return vRet;
    }

    private int[] leftVector(int[] v) {
        int[] vRet = {v[0] - 1, v[1]};
        return vRet;
    }

    private int[] upVector(int[] v) {
        int[] vRet = {v[0], v[1] - 1};
        return vRet;
    }

    private int[] downVector(int[] v) {
        int[] vRet = {v[0], v[1] + 1};
        return vRet;
    }

    public int[] toWorldCoords(int[] pos) {
        int[] v = {pos[0] + gridSizeX, pos[1] + gridSizeY};
        return v;
    }

    public int[] toGameCoords(int[] pos) {
        int[] v = {pos[0] - gridSizeX, pos[1] - gridSizeY};
        return v;
    }
}
