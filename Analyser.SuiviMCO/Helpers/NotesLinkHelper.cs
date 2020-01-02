using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.SuiviMCO.Helpers
{
    public class NotesLinkHelper
    {
        private static string LinkMask = "URL=Notes://NOTES-V3-Nø1/C1256BC60049C281/83C43EF4FAEB5D8EC12584C6005B5E9C/{0}?server=NOTES-V3-Nø1&replicaID=C1256BC60049C281";
        public static void OpenNotesDocument(string DocumentUniqueID)
        {
            string notesLink = string.Format(LinkMask, DocumentUniqueID);
            string shortCut = string.Format(@".\interact\{0}.url", DocumentUniqueID);
            System.IO.StreamWriter shortCutWriter = null;
            try
            {
                shortCutWriter = new System.IO.StreamWriter(shortCut, false);
                shortCutWriter.WriteLine("[InternetShortcut]");
                shortCutWriter.WriteLine(notesLink);
                if (shortCutWriter != null)
                {
                    shortCutWriter.Flush();
                    shortCutWriter.Close();
                }
                Process.Start(shortCut);
            }
            catch { }
        }
    }
}
