using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//przestrzeń nazw dla wejścia wyjścia między innymi zapisa na dysku
using System.IO;

namespace lab_1C
{
    static class Archiving
    {

        public static void ArchivePlayers(string file, Player[] players)
        {
            using (StreamWriter stream = new StreamWriter(file))
            {
                foreach (var p in players)
                    stream.WriteLine(p.ToFileFormat());
                stream.Close();
            }
        }
        public static Player[] ReadPlayersFromFile(string file)
        {
            Player[] players = null;
            if (File.Exists(file))
            {
                var sPlayers = File.ReadAllLines(file);
                var n = sPlayers.Length;
                if (n > 0)
                {
                    players = new Player[n];
                    for (int i = 0; i < n; i++)
                        players[i] = Player.CreateFromString(sPlayers[i]);
                    return players;
                }
                
                
            }
            return players;
        }

    }
}
