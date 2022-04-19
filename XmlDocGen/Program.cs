using XmlDocMarkdown.Core;

var generate = XmlDocMarkdownApp.Run(args);

var target = Path.Combine(
    Directory.GetCurrentDirectory(),
        @"SqliteWasmHelper\docs\"
.Replace('\\', Path.DirectorySeparatorChar));

Console.WriteLine($"Version stamping at path {target}");

if (Directory.Exists(target))
{
    var version = $"Version {ThisAssembly.AssemblyInformationalVersion} generated on {DateTime.Now}.";
    Console.WriteLine($"Stamping with: {version}");
    var queue = new Stack<string>();
    queue.Push(target);
    while (queue.Count > 0)
    {
        var dir = queue.Pop();
        foreach (var subdir in Directory.EnumerateDirectories(dir))
        {
            queue.Push(subdir);
        }

        foreach (var file in Directory.EnumerateFiles(dir)
            .Where(f => Path.GetExtension(f).ToLower() == ".md"))
        {
            var text = File.ReadAllLines(file);
            var newText = text.Union(new[]
            {
                $"{Environment.NewLine}",
                version
            });
            File.WriteAllLines(file, newText);
        }
    }
}
else
{
    throw new DirectoryNotFoundException(target);
}

return generate;
