using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Ship(Coordinates w, int x, int y, int r)
        {
            location.x = x;
            location.y = y;
            world = w;
            rotation = r;
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
            Coordinates world = new Coordinates() { x = 5, y = 3 };
            Ship ship1 = new Ship(world, 1, 1, (int)WorldHelper.CompassToRotation("e"));

            ship1.Left();
            ship1.Forward();

            Console.WriteLine(String.Format("x: {0} y: {1}", ship1.location.x, ship1.location.y));
            //Console.WriteLine(WorldHelper.RotationToCompass(ship1.rotation));
            //Console.WriteLine((int)WorldHelper.CompassToRotation("w"));
            Console.ReadLine();
        }
    }
}
