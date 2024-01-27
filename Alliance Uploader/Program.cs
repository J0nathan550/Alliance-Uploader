// Entire code is by J0nathan550, if you wish to support me, consider to look our telegram about Alliance RolePlay project.
// https://t.me/+53617ASc4dg5ZWNi
// Thanks for using!

using FluentFTP;

AsyncFtpClient client;
Console.Title = "Alliance Uploader";

while (true)
{
    Console.Clear();
    while (true)
    {
        try
        {
            Console.Write("Type host of the FTP server: ");
            string? host = Console.ReadLine();
            Console.Write("Type username of the FTP server: ");
            string? username = Console.ReadLine();
            Console.Write("Type password of the FTP server: ");
            string? password = Console.ReadLine();

            // create an FTP client and specify the host, username and password
            // (delete the credentials to use the "anonymous" account)
            int port = -1;
            while (true)
            {
                Console.Write("Type port of the FTP server: ");
                string? portS = Console.ReadLine();
                if (int.TryParse(portS, out port))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("There was an error trying parse the port. Type just numbers.");
                }
            }

            client = new AsyncFtpClient(host, username, password, port);

            // connect to the server and automatically detect working FTP settings
            await client.AutoConnect();
            break;
        }
        catch
        {
            Console.WriteLine($"Failed to connect FTP server, with the given credentials!");
        }
    }

    Console.WriteLine("Connected! Deleting files.");

    Progress<FtpProgress> progress = new();
    progress.ProgressChanged += Progress_ProgressChanged;
    try
    {
        await client.DeleteDirectory(@"/");
    }
    catch { /* There can be files that are cannot be deleted, we ignore them. */ }

    Console.WriteLine("Everything deleted, uploading files.");
    while (true)
    {
        try
        {
            Console.Write("Before uploading, type the path to sborka: ");
            string? path = Console.ReadLine();

            if (Path.Exists(path))
            {
                await client.UploadDirectory(path, @"/", FtpFolderSyncMode.Update, FtpRemoteExists.Overwrite, FtpVerify.None, null, progress);
                break;  
            }
        }
        catch
        {
            Console.WriteLine("There was an error parsing the path to the folder, try again.");
        }
    }

    static void Progress_ProgressChanged(object? sender, FtpProgress e)
    {
        Console.WriteLine($"Uploading ({e.FileIndex} / {e.FileCount}): {e.Progress}");
    }

    await client.Disconnect();

    Console.WriteLine("Uploading done. Press any key to continue.");
    Console.ReadKey();
}
