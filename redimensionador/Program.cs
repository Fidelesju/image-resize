using System.Drawing;
using System.Threading;

Thread thread = new Thread(Resize);
thread.Start();


static void Resize()
{
    #region "Directories"
    string input_directory = "input_directory";
    string resizer_directory = "resize_directory";

    if (!Directory.Exists(input_directory) || !Directory.Exists(resizer_directory))
    {
        Directory.CreateDirectory(input_directory);
        Directory.CreateDirectory(resizer_directory);
    }
    #endregion


    while (true)
    {
        var input_files = Directory.EnumerateFiles(input_directory);

        Console.WriteLine("Adicione a imagem que deseja redimensionar na pasta input_directory");
        Console.WriteLine("Informe o tamanho desejado para a nova imagem");
        var size = Console.ReadLine();

        foreach (var file in input_files)
        {
            FileStream fileStream;
            FileInfo fileInfo;

            fileStream = new FileStream(file, FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite);
            fileInfo = new FileInfo(file);
            string end_Directory = Environment.CurrentDirectory + @"\" + 
                resizer_directory + @"\resize_" + size + "_" + fileInfo.Name;

            ResizingImage(Image.FromStream(fileStream), Convert.ToInt32(size), end_Directory);

        }
        Thread.Sleep(new TimeSpan(0,0,5));
    }
}


static void ResizingImage(Image image, int height, string directory)
{
    double ratio = (double)height / image.Height;
    int newWidth = (int)(image.Width * ratio);
    int newHeight = (int)(image.Height * ratio);

    Bitmap newImage;
    newImage = new Bitmap(newWidth, newHeight);

    using (Graphics graphics = Graphics.FromImage(newImage))
    {
        graphics.DrawImage(image,0,0, newWidth, newHeight);
    }

    newImage.Save(directory);
    image.Dispose();
}