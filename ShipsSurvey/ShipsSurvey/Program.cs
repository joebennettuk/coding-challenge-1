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
            //5 3
            Coordinates world = new Coordinates() { x = 5, y = 3 };

            //1 1 E
            //RFRFRFRF
            Ship ship1 = new Ship(world, 1, 1, (int)WorldHelper.CompassToRotation("e"));
            ship1.Right();
            ship1.Forward();
            ship1.Right();
            ship1.Forward();
            ship1.Right();
            ship1.Forward();
            ship1.Right();
            ship1.Forward();
            Console.WriteLine(ship1);

            //3 2 N
            //FRRFLLFFRRFLL
            Ship ship2 = new Ship(world, 3, 2, (int)WorldHelper.CompassToRotation("n"));
            ship2.Forward();
            ship2.Right();
            ship2.Right();
            ship2.Forward();
            ship2.Left();
            ship2.Left();
            ship2.Forward();
            ship2.Forward();
            ship2.Right();
            ship2.Right();
            ship2.Forward();
            ship2.Left();
            ship2.Left();
            Console.WriteLine(ship2);

            //0 3 W
            //LLFFFLFLFL
            Ship ship3 = new Ship(world, 2, 3, (int)WorldHelper.CompassToRotation("s"));
            ship3.Left();
            ship3.Left();
            ship3.Forward();
            ship3.Forward();
            ship3.Forward();
            ship3.Left();
            ship3.Forward();
            ship3.Left();
            ship3.Forward();
            ship3.Left();
            Console.WriteLine(ship3);

            Console.ReadLine();
        }
    }
}
