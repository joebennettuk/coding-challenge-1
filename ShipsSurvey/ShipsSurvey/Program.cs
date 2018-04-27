using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShipsSurvey
{
    static class WorldHelper
    {
        public enum Compass
        {
            N = 0,
            E = 90,
            S = 180,
            W = 270
        }

        //a place to record movements that will take a ship out of bounds
        public static List<DangerMovement> DangerMovements = new List<DangerMovement>();
        
        //int input to compass enum
        public static Compass RotationToCompass(int rotation)
        {
            return (Compass)rotation;
        }

        //string input to compass enum
        public static Compass CompassToRotation(string direction)
        {
            Enum.TryParse(direction.ToUpper(), out Compass c);
            return c;
        }

        public static bool IsMovementSafe(int x, int y, string direction)
        {
            return DangerMovements.Where(dm =>
             dm.coords.x == x &&
             dm.coords.y == y &&
             dm.direction == direction).Any();
        }
    }

    public struct Coordinates
    {
        public int x, y;
    }

    public struct DangerMovement
    {
        public Coordinates coords;
        public string direction;
    }

    public class Ship
    {
        public Coordinates location;
        Coordinates world;
        int rotation;
        bool alive = true;
        public string instructions;

        public Ship(Coordinates w, int x, int y, int r)
        {
            location.x = x;
            location.y = y;
            world = w;
            rotation = r;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}{3}",
                location.x,
                location.y,
                WorldHelper.RotationToCompass(rotation),
                alive ? "" : " LOST");
        }

        public void DoInstructions()
        {
            foreach(char c in instructions)
            {
                if(c == 'F')
                    Forward();
                else if(c == 'L')
                    Left();
                else if (c == 'R')
                    Right();
            }
        }

        public void Left()
        {
            if (rotation == 0)
                rotation = 270;
            else
                rotation -= 90;
        }

        public void Right()
        {
            if (rotation == 270)
                rotation = 0;
            else
                rotation += 90;
        }

        public void Forward()
        {
            //we may need the old location for recording a movement that places us out of bounds later
            Coordinates oldCoords = location;

            //check if we have danger location recorded
            string direction = WorldHelper.RotationToCompass(rotation).ToString();            
            if (WorldHelper.IsMovementSafe(location.x, location.y, direction))
            {
                //we do not want to move forward!
                return;
            }

            //move ship forward in direction
            switch (WorldHelper.RotationToCompass(rotation))
            {
                case WorldHelper.Compass.N:
                    location.y += 1;
                    break;
                case WorldHelper.Compass.E:
                    location.x += 1;
                    break;
                case WorldHelper.Compass.S:
                    location.y -= 1;
                    break;
                case WorldHelper.Compass.W:
                    location.x -= 1;
                    break;
            }

            //check ship is in bounds
            if (location.x <= world.x && location.y <= world.y && location.x >= 0 && location.y >= 0)
                return;

            //ship went out of bounds - record the movement for future ships
            if (alive)
            {
                DangerMovement dm = new DangerMovement()
                {
                    coords = oldCoords,
                    direction = direction
                };
                WorldHelper.DangerMovements.Add(dm);
            }
            alive = false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //please place input.txt file in below directory or change path to correct location
            StreamReader file;
            string path = @"C:/input.txt";
            try
            {
                file = new StreamReader(path);
            } catch(Exception e) {
                Console.WriteLine("Please place input.txt file in this location: " + path);
                Console.ReadLine();
                return;
            }

            List<Ship> Ships = new List<Ship>();
            bool firstLine = true;
            string line;
            Coordinates world = new Coordinates();

            //read input.txt file and create objects from it
            while ((line = file.ReadLine()) != null)
            {
                //read first line and create world
                if(firstLine)
                {
                    int xInput = Int32.Parse(line.Split(' ')[0]);
                    int yInput = Int32.Parse(line.Split(' ')[1]);
                    world = new Coordinates() { x = xInput, y = yInput };
                    firstLine = false;
                    continue;
                }

                //pretty hacky way to read the input.txt file and create ship objects
                string[] splitLine = line.Split(' ');
                int xtmp = Int32.Parse(splitLine[0]);
                int ytmp = Int32.Parse(splitLine[1]);
                string compass = splitLine[2];
                Ship tmpShip = new Ship(world, xtmp, ytmp, (int)WorldHelper.CompassToRotation(compass));
                string instructions = file.ReadLine();
                tmpShip.instructions = instructions;
                file.ReadLine();
                Ships.Add(tmpShip);                
            }

            foreach (Ship ship in Ships) {
                ship.DoInstructions();
                Console.WriteLine(ship);
            }

            Console.ReadLine();
        }
    }
}
