using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoFileRenamer;

public static class CsvLoader
{
    public static List<CSV_entry> load_csv(string csvpath = "")
    {
        List<CSV_entry> fileScenes = new List<CSV_entry>();

        
            try
            {
                using (var reader = new StreamReader(csvpath))
                using (var csv = new CsvReader(reader, CultureInfo.GetCultureInfo("en-GB")))
                {
                    fileScenes = csv.GetRecords<CSV_entry>().ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions according to your application's needs
                Debug.WriteLine($"Error loading CSV: {ex.Message}");
            }
        return fileScenes;
    }
        
    }

