using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoSnake.Items;

namespace MonoSnake.Data
{
    internal class Generator
    {
        private static int amt = 1;
        private float timer = 100f;
        private Random random = new Random();

        public void generatorUpdate(float dt)
        {
            if (timer > 0f)
            {
                timer -= 10f * dt;
            }
            else if (timer <= 0f && GameData.Apples.Count < 200)
            {
                addApple(amt);
                timer = 100f;
            }
            else
            {
                timer = 100f;
            }
        }

        private void addApple(int amt)
        {
            for (int i = 0; i < amt; i++)
            {
                int x = random.Next(25, (Map.width * 25) - 125);
                int y = random.Next(25, (Map.height * 25) - 125);
                Apple apple = new Apple(x, y);
                GameData.Apples.Add(apple);
            }
        }
    }
}
