namespace BOS.OS;

public class FileSystem
{
    public Dir? root { get; set; }

    public FileSystem(string root_dir)
    {
        root = new Dir(null, null, null, null, null);
        root.is_root = true;
        this.LoadSystem(root_dir).GetAwaiter().GetResult();
        this.root.host_location = root_dir;
    }
    
    public async Task LoadSystem(string root_path)
    {
        this.root = await Dir.LoadRootDir(root_path, this);
    }
}

public class Dir
{
    public string name { get; set; }
    public Dir? parent { get; set; }
    public Dir[]? dirs { get; set; }
    public FLE[]? files { get; set; }
    public bool is_root = false;
    public string host_location { get; set; }

    public Dir(string n, Dir? p, Dir[]? d, FLE[]? f, string h)
    {
        name = n;
        parent = p;
        dirs = d;
        files = f;
        host_location = h;
    }

    public static async Task<Dir?> LoadRootDir(string path, FileSystem fs)
    {
        DirectoryInfo inf = new DirectoryInfo(path);
        DirectoryInfo[] dirs = inf.GetDirectories();
        FileInfo[] fles = inf.GetFiles();


        Dir[]? d = new Dir[dirs.Length - 1];
        FLE[]? f = new FLE[fles.Length - 1];

        for(int i = 0; i < dirs.Length - 1; i++)
        {
            Dir dir = await LoadDir(dirs[i].FullName, fs.root);
            d[i] = dir;
        }

        for(int i = 0; i < fles.Length; i++)
        {
            FLE file = await FLE.LoadFile(fles[i].FullName, fs.root);
            f[i] = file;
        }

        Dir ret = new Dir(inf.Name, null, d, f, null);
        ret.is_root = true;
        
        return ret;
    }

    public static async Task<Dir?> LoadDir(string path, Dir? parent)
    {
        DirectoryInfo inf = new DirectoryInfo(path);
        DirectoryInfo[] dirs = inf.GetDirectories();
        FileInfo[] fles = inf.GetFiles();


        Dir[]? d = new Dir[dirs.Length - 1];
        FLE[]? f = new FLE[fles.Length - 1];

        for(int i = 0; i < dirs.Length - 1; i++)
        {
            Dir dir = await LoadDir(dirs[i].FullName, parent);
            d[i] = dir;
        }

        for(int i = 0; i < fles.Length; i++)
        {
            FLE file = await FLE.LoadFile(fles[i].FullName, parent);
            f[i] = file;
        }

        return new Dir(inf.FullName, parent, d, f, inf.FullName);
    }
}

public class FLE
{
    public string name { get; set; }
    public string extension { get; set; }
    public Dir parent { get; set; }
    public byte[] contents { get; set; }
    public string host_location { get; set; }

    public FLE(string n, string e, Dir p, byte[] c, string h)
    {
        name = n;
        extension = e;
        parent = p;
        contents = c;
        host_location = h;
    }

    public static async Task<FLE?> LoadFile(string path, Dir? parent)
    {
        FileInfo finf = new FileInfo(path);

        return new FLE(finf.Name, finf.Extension, parent, await File.ReadAllBytesAsync(finf.FullName), finf.FullName);
    }

    public async Task Write()
    {
        await File.WriteAllBytesAsync(this.host_location, this.contents);
    }

    public async Task<byte[]> Read()
    {
        return await File.ReadAllBytesAsync(this.host_location);
    }
}