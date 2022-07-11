namespace GB.AspNetMvc
{
    public class ReadFiles
    {
        private string[] sourceFiles;
        private string destFile;

        /// <summary>
        /// Read files line by line from sourceFiles and write to destFile
        /// </summary>
        /// <param name="sourceFiles">Source for reading lines</param>
        /// <param name="destFile">Destination to write lines</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReadFiles(string[] sourceFiles, string destFile)
        {
            this.sourceFiles = sourceFiles ?? throw new ArgumentNullException(nameof(sourceFiles));
            this.destFile = destFile ?? throw new ArgumentNullException(nameof(destFile));
        }

        /// <summary>
        /// Starts reading lines from sourceFiles and writing them to destFile
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            foreach (var file in sourceFiles)
            {
                await GetStringAsync(file);
            }
        }

        private async Task GetStringAsync(string sourcePath)
        {
            try
            {
                using StreamReader reader = new(sourcePath);
                while (!reader.EndOfStream)
                {
                    var str = await reader.ReadLineAsync();
                    if (str != null)
                        await WriteStringAsync(str, destFile);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task WriteStringAsync(string str, string destPath)
        {
            try
            {
                await using StreamWriter writer = new(destPath, true);
                await writer.WriteLineAsync(str);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
