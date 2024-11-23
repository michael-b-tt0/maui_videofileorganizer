using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;


namespace VideoFileRenamer;

public class CSV_entry
{
    [Name("title")]
    public required string Title { get; set; }
    [Name("actors")]
    public required string Actors { get; set; }
    [Name("date")]
    public required DateTime Date { get; set; }
    [Ignore]

    public required Uri? scene_image_path { get; set; }


    [Name("scene_link")]

    public required Uri scene_link_path { get; set; }

    [Optional]
    public string? scene_code { get; set; }

    [Ignore]
    public string? VideoLink { get; set; }

    [Ignore]
    public required string TagsString { get; set; }

    [Name("scene_type")]

    public required string scene_type { get; set; }

    [Ignore]
    public List<string> Tags => TagsString?.Split(',').Select(tag => tag.Trim()).OrderBy(tag => tag).ToList();

    [Ignore]
    public string TagsAsString => string.Join(", ", Tags);

    [Ignore]
    // add a way to make all fullnames valid as windows filepaths
    public string FullFileName => $"{CSV_entry.SanitizeFileName(this.Title)}__{this.Actors}_{this.Date.ToString("yyyy-MM-dd")}";

    [Ignore]

    private static readonly Dictionary<string, string> filePathToSceneTypeMap = new Dictionary<string, string>
{
    { "sexart", @"H:\P3\Sexart\" },
    { "vixen", @"G:\P\vixen\" },
        {"blacked", @"G:\P\blacked\" },
        {"blackedraw", @"G:\P\blackedraw\" }

    
    // Add more key-value pairs as needed
};

    [Ignore]

    public string FullFilePath => $"{filePathToSceneTypeMap[this.scene_type]}{FullFileName}";


    public static string SanitizeFileName(string fileName)
    {
        string invalidChars = new string(Path.GetInvalidFileNameChars());
        Regex regex = new Regex($"[{Regex.Escape(invalidChars)}]");
        return regex.Replace(fileName, "");
    }
}
