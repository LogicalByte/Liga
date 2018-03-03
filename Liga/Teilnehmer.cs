using System;

namespace Liga
{
    class Teilnehmer : Verein
    {
        public string Teamname { get; }
        public int Staerke { get; }
        public int Tabellenposition { get; set; } = 0;
        public int Siege { get; set; } = 0;
        public int Unentschieden { get; set; } = 0;
        public int Niederlagen { get; set; } = 0;
        public int Tore { get; set; } = 0;
        public int Gegentore { get; set; } = 0;
        public int Tordifferenz { get; set; } = 0;
        public int Punkte { get; set; } = 0;
        public int ID { get; }

        public Teilnehmer(string bTeamname, int bTeamstaerke, int bID)
        {
            Teamname = bTeamname;
            Staerke = bTeamstaerke;
            ID = bID;
            Vereinsname = "Verein " + (bID + 1);
            Gruendungsjahr = 1905;
            Console.WriteLine(bTeamname + " (Staerke: " + bTeamstaerke + ") hinzugefuegt.");
        }

        /// <summary>
        /// Ausgabe allgemeiner Teilnehmerinformationen
        /// </summary>
        /// <param name="bSpielpaarungen"></param>
        /// <param name="bTeilnehmer"></param>
        public void Teilnehmerinfo(Spielpaarungen[] bSpielpaarungen, Teilnehmer[] bTeilnehmer)
        {
            const string format = "{0, -26} {1, 0}";

            Console.WriteLine("");
            Console.WriteLine("Informationen ueber den Teilnehmer " + Teamname + " (Staerke: " + Staerke + "):");
            Console.WriteLine("");
            Console.WriteLine(string.Format(format, "Vereinsangehoerigkeit:", Vereinsname + " (Gruendung: " + Gruendungsjahr + ")"));
            Console.WriteLine("");
            Console.WriteLine(string.Format(format, "Aktuelle Tabellenposition:", Tabellenposition));
            Console.WriteLine(string.Format(format, "Hoehster Sieg:", HoechsterSieg(bSpielpaarungen, bTeilnehmer)));
            Console.WriteLine(string.Format(format, "Hoechste Niederlage:", HoechsteNiederlage(bSpielpaarungen, bTeilnehmer)));
            Console.WriteLine("");
        }

        /// <summary>
        /// Sucht die höchste Niederlage aus den Spielpaarungen raus.
        /// </summary>
        /// <param name="bSpielpaarungen"></param>
        /// <param name="bTeilnehmer"></param>
        /// <returns>Spielergebnis mit Gegnername</returns>
        private string HoechsteNiederlage(Spielpaarungen[] bSpielpaarungen, Teilnehmer[] bTeilnehmer)
        {           
            string Rueckgabe = "";  // Rückgabestring
            int MaxTordifferenz = 0; // Höchste gefundene Tordifferenz (Betrag)

            for (int i = 0; i < bSpielpaarungen.Length; i++)
            {
                // Wenn das Team in der Spielpaarungen drin vor kommt, eine Niederlage erlitten hat und der Betrag der Tordifferenz größer als MaxTordifferenz ist...
                if (((bSpielpaarungen[i].Index1 == ID & bSpielpaarungen[i].Tore1 < bSpielpaarungen[i].Tore2) || (bSpielpaarungen[i].Index2 == ID & bSpielpaarungen[i].Tore1 > bSpielpaarungen[i].Tore2))
                    && Math.Abs(bSpielpaarungen[i].Tore1 - bSpielpaarungen[i].Tore2) >= MaxTordifferenz)
                {
                    int GegnerID = -1; // Arrayindex des Gegners
                    // Setzen der GegnerID
                    if (bSpielpaarungen[i].Index1 == ID) { GegnerID = bSpielpaarungen[i].Index2; } else { GegnerID = bSpielpaarungen[i].Index1; }
                    // Zusammensetzen des Rückgabestrings
                    if (bSpielpaarungen[i].Tore1 >= bSpielpaarungen[i].Tore2) { Rueckgabe = bSpielpaarungen[i].Tore2 + ":" + bSpielpaarungen[i].Tore1; }
                    else { Rueckgabe = bSpielpaarungen[i].Tore1 + ":" + bSpielpaarungen[i].Tore2; }
                    Rueckgabe += " (" + bTeilnehmer[GegnerID].Teamname + ")";                  
                    // Setzen der neuen MaxTordifferenz
                    MaxTordifferenz = Math.Abs(bSpielpaarungen[i].Tore1 - bSpielpaarungen[i].Tore2);
                }
            }
            // Sollte keine Niederlage gefunden worden sein
            if (Rueckgabe == "") { Rueckgabe = "---"; }
            return Rueckgabe;
        }

        /// <summary>
        /// Sucht den höchsten Sieg aus den Spielpaarungen raus.
        /// </summary>
        /// <param name="bSpielpaarungen"></param>
        /// <param name="bTeilnehmer"></param>
        /// <returns>Spielergebnis mit Gegnername</returns>
        private string HoechsterSieg(Spielpaarungen[] bSpielpaarungen, Teilnehmer[] bTeilnehmer)
        {
            string Rueckgabe = "";  // Rückgabestring
            int MaxTordifferenz = 0; // Höchste gefundene Tordifferenz (Betrag)

            for (int i = 0; i < bSpielpaarungen.Length; i++)
            {
                // Wenn das Team in der Spielpaarungen drin vor kommt, ein Sieg erklommen hat und der Betrag der Tordifferenz größer als MaxTordifferenz ist...
                if (((bSpielpaarungen[i].Index1 == ID & bSpielpaarungen[i].Tore1 > bSpielpaarungen[i].Tore2) || (bSpielpaarungen[i].Index2 == ID & bSpielpaarungen[i].Tore1 < bSpielpaarungen[i].Tore2))
                    && Math.Abs(bSpielpaarungen[i].Tore1 - bSpielpaarungen[i].Tore2) >= MaxTordifferenz)
                {
                    int GegnerID = -1; // Arrayindex des Gegners
                    // Setzen der GegnerID
                    if (bSpielpaarungen[i].Index1 == ID) { GegnerID = bSpielpaarungen[i].Index2; } else { GegnerID = bSpielpaarungen[i].Index1; }
                    // Zusammensetzen des Rückgabestrings
                    if (bSpielpaarungen[i].Tore1 >= bSpielpaarungen[i].Tore2) { Rueckgabe = bSpielpaarungen[i].Tore1 + ":" + bSpielpaarungen[i].Tore2; }
                    else { Rueckgabe = bSpielpaarungen[i].Tore2 + ":" + bSpielpaarungen[i].Tore1; }
                    Rueckgabe += " (" + bTeilnehmer[GegnerID].Teamname + ")";
                    // Setzen der neuen MaxTordifferenz
                    MaxTordifferenz = Math.Abs(bSpielpaarungen[i].Tore1 - bSpielpaarungen[i].Tore2);
                }
            }
            // Sollte kein Sieg gefunden worden sein
            if (Rueckgabe == "") { Rueckgabe = "---"; }
            return Rueckgabe;
        }

        /// <summary>
        /// Gibt die sortierte Tabelle in der Konsole aus. Da das Teilnehmerarray nicht nach Tabellenposition sortiert ist, muss jede
        /// Position einzeln durchgegangen werden.
        /// </summary>
        /// <param name="bTeilnehmer"></param>
        public static void TabelleAusgeben(Teilnehmer[] bTeilnehmer, bool bSortieren)
        {
            // Sortieren der Teilnehmer nach Punkten (und Tordifferenz)
            if (bSortieren == true) { bTeilnehmer = TabelleSortieren(bTeilnehmer); }

            Console.WriteLine("");
            const string format = "{0, -10} {1, -3} {2, -3} {3, -3} {4, -3} {5, -3} {6, -3} {7, -3} {8, -5} {9, -3}";

            Console.WriteLine(string.Format(format, "Teamname", "St", "SP", "S", "U", "N", "TO", "GT", "TD", "P"));
            Console.WriteLine("-----------------------------------------------");

            // Anzeige der Tabelle. Sofern eine Sortierung stattgefunden hat, muss das Array nach den Tabellenpositionen abgesucht werden
            if (bSortieren == true)
            {
                for (int i = 1; i <= bTeilnehmer.Length; i++)
                {

                    for (int j = 0; j <= bTeilnehmer.Length - 1; j++)
                    {
                        // i: Tabellenposition, die im Teilnehemrarray gefunden werden muss
                        if (bTeilnehmer[j].Tabellenposition == i)
                        {
                            Console.WriteLine(string.Format(format, bTeilnehmer[j].Teamname, bTeilnehmer[j].Staerke.ToString("00"),
                                                           (bTeilnehmer[j].Siege + bTeilnehmer[j].Unentschieden + bTeilnehmer[j].Niederlagen),
                                                           bTeilnehmer[j].Siege, bTeilnehmer[j].Unentschieden, bTeilnehmer[j].Niederlagen,
                                                           bTeilnehmer[j].Tore, bTeilnehmer[j].Gegentore, bTeilnehmer[j].Tordifferenz,
                                                           bTeilnehmer[j].Punkte));
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i <= bTeilnehmer.Length - 1; i++)
                {
                    Console.WriteLine(string.Format(format, bTeilnehmer[i].Teamname, bTeilnehmer[i].Staerke.ToString("00"),
                                                    (bTeilnehmer[i].Siege + bTeilnehmer[i].Unentschieden + bTeilnehmer[i].Niederlagen),
                                                    bTeilnehmer[i].Siege, bTeilnehmer[i].Unentschieden, bTeilnehmer[i].Niederlagen,
                                                    bTeilnehmer[i].Tore, bTeilnehmer[i].Gegentore, bTeilnehmer[i].Tordifferenz,
                                                    bTeilnehmer[i].Punkte));
                }
            }
}

        /// <summary>
        /// Sortieren der Teilnehmer nach Punkte und Tordifferenz.
        /// </summary>
        /// <param name="bTeilnehmer"></param>
        /// <returns></returns>
        /// <remarks>
        /// Implementierung eines InsertSort-Algorithmus. Sollte die Punktezahl gleich, aber die Tordifferenz geringer sind, wird die Position getauscht. Um die Reihenfolge
        /// des ursprünglichen Arrays nicht zu verändern, wird eine Kopie erstellt und mit diesem gearbeitet. Zum Abschluss wird geschaut, wo sich das Team im sortierten
        /// Array befindet und die Tabellenposition = Index im sortierten Array gesetzt.
        /// </remarks>
        private static Teilnehmer[] TabelleSortieren(Teilnehmer[] bTeilnehmer)
        {
            Teilnehmer[] SortierteTeilnehmer = new Teilnehmer[bTeilnehmer.Length];
            Array.Copy(bTeilnehmer, SortierteTeilnehmer, bTeilnehmer.Length);

            Console.Write("Tabelle wird sortiert...");
            for (int i = 0; i < SortierteTeilnehmer.Length - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (SortierteTeilnehmer[j - 1].Punkte < SortierteTeilnehmer[j].Punkte || 
                       (SortierteTeilnehmer[j - 1].Punkte == SortierteTeilnehmer[j].Punkte && SortierteTeilnehmer[j - 1].Tordifferenz < SortierteTeilnehmer[j].Tordifferenz))
                    {
                        Teilnehmer tmpTeilnehmer = SortierteTeilnehmer[j - 1];
                        SortierteTeilnehmer[j - 1] = SortierteTeilnehmer[j];
                        SortierteTeilnehmer[j] = tmpTeilnehmer;
                    }
                }
            }
            // Setzen der Tabellenposition, indem die Position im sortierten Array bestimmt und übertragen wird
            for (int i = 0; i <= SortierteTeilnehmer.Length - 1; i++)
            {
                for (int j = 0; j <= bTeilnehmer.Length - 1; j++)
                {
                    if (bTeilnehmer[j].ID == SortierteTeilnehmer[i].ID) { bTeilnehmer[j].Tabellenposition = i + 1; break; }
                }
            }

            Console.WriteLine("fertig!");
            return bTeilnehmer;
        }
    }
}
