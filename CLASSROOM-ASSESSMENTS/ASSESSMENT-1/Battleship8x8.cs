namespace ASSESSMENT_1;

using System;
using System.Collections.Generic;
using System.Text;

class Battleship8x8
{
    private long ships;
    private long shots;

    public Battleship8x8(long ships)
    {
        this.ships = ships;
        this.shots = 0L;
    }

    public bool Shoot(string shot)
    {
        int col = shot[0] - 'A';
        int row = shot[1] - '1';
        int index = row * 8 + col;
        long mask = 1L << (63 - index);
        shots |= mask;
        return (ships & mask) != 0;
    }

    public string State()
    {
        StringBuilder sb = new StringBuilder();

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                int index = r * 8 + c;
                long mask = 1L << (63 - index);

                bool hasShip = (ships & mask) != 0;
                bool hasShot = (shots & mask) != 0;

                if (hasShip && hasShot) sb.Append('☒');
                else if (hasShip) sb.Append('☐');
                else if (hasShot) sb.Append('×');
                else sb.Append('.');
            }

            if (r < 7) sb.Append('\n');
        }

        return sb.ToString();
    }
}

public class Program
{
    public static void Main()
    {
        ulong map = 0b11110000_00000111_00000000_00110000_00000010_01000000_00000000_00000000L;

        List<string> shots = new List<string> { "A1", "B2", "C3", "D4" };

        Battleship8x8 battle = new Battleship8x8(map);

        foreach (var s in shots)
            battle.Shoot(s);

        Console.WriteLine(battle.State());
    }
}