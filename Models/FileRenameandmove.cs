using FFMpegCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VideoFileRenamer;

public static class FileRenameandmove
{
    public static async Task<(string Result, bool HasMoved)> RenameAsync(string oldfilename, string newfilename, IProgress<int> progress = null)
    {
        try
        {
            string extension = Path.GetExtension(oldfilename);

            string newFilePath = Path.ChangeExtension(newfilename, extension);

            if (File.Exists(newFilePath))
            {
                
                return ($"Error: Destination file already exists {newFilePath}.", true);
                // Handle the situation where the destination file already exists
            }
            else
            {
                await Task.Run(() =>
                {
                    progress?.Report(1);
                    File.Move(oldfilename, newFilePath);
                    progress?.Report(2);
                    

                });


                return ($"Success: File has been moved to {newFilePath}.", true);
            }
        } catch (IOException ex)
        {
            
            return ($"Error moving and renaming file: {ex.Message}", false);
        }
       

    }
}
