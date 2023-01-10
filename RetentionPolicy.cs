namespace DiskSpaceMonitor
{
    public class RetentionPolicy
    {
        public string Path { get; set; }
        public int DeleteOlderThanDays { get; set; }

        public void Enforce(DateTime now)
        {
            var files = new DirectoryInfo(Path).GetFiles();
            var filesToDelete = files.Where(x => x.LastWriteTime.AddDays(DeleteOlderThanDays) < now.Date);
            foreach (var file in filesToDelete)
            {
                try
                {
                    Console.WriteLine($"Deleting: {file.FullName}");
                    File.Delete(file.FullName);
                }
                catch
                {
                    //Unable to delete at this time
                }
            }
        }
    }
}
