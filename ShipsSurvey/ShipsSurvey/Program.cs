using System;
using System.Collections.Generic;
using System.IO;

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

        //rotation in degrees to compass enum
        public static Compass RotationToCompass(int rotation)
        {
            return (Compass)rotation;
        }

        //string to rotation to compass enum
        public static Compass CompassToRotation(string direction)
        {
            Enum.TryParse(direction.ToUpper(), out Compass c);
            return c;
        }
    }

    public struct Coordinates
    {
        public int x, y;
    }

    public class Ship
    {
        public Coordinates location;
        Coordinates world;
        int rotation;
        Coordinates lostLocation;
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

        public bool Forward()
        {
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
            {
                return true;
            }

            //goodbye ship - only want the first location that ship went out of bounds
            if (alive)
            {
                lostLocation = location;
            }
            alive = false;
            return alive;
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
                Console.WriteLine(ship);
                Console.WriteLine(ship.instructions);
            }

            Console.ReadLine();
        }
    }
}
