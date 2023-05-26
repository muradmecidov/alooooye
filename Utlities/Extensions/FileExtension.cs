namespace BZLAND.Utlities.Extensions
{
    public static class FileExtension
    {
        public static async Task<string> SaveAsync(this IFormFile file, string rootpath)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName;
            using (FileStream fileStream = new FileStream(Path.Combine(rootpath, filename), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }
    }
}
