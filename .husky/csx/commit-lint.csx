#r "System.Text.Json"
#r "System.Linq"
#r "System.Linq.Expressions"
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Text.Json;
using System.Dynamic;

public class ConventionalCommitConfig
{
    public CommitTypes types { get; set; }

    public class CommitTypes
    {
         public object refactor { get; set; }
         public object fix { get; set; } 
         public object feat { get; set; }
         public object build { get; set; }
         public object chore { get; set; }
         public object style { get; set; }
         public object test { get; set; }
         public object docs { get; set; }
         public object perf { get; set; }
         public object revert { get; set; }
    }
}

const int HeaderMaxLength = 90;

var commitFile = Args[0];
var commitMessage = File.ReadAllText(commitFile).Split(Environment.NewLine, StringSplitOptions.None);
var commitHeader = commitMessage[0];
for (int i = 0; i < commitMessage.Length; ++i)
{
    Console.WriteLine($"{i}: {commitMessage[i]}");
}

if (commitHeader.Length > HeaderMaxLength)
{
    Console.WriteLine($"Commit title too long ({commitHeader.Length} characters). Should have max {HeaderMaxLength}.");
    return 1;
}

var typesFile = Args[1];
var typesFileContent = File.ReadAllText(typesFile);
var ccConfig = JsonSerializer.Deserialize<ConventionalCommitConfig>(typesFileContent);
var names = ccConfig.types.GetType().GetProperties().Select(p => p.Name);
var commitType = commitHeader.Split(':')[0].Split('(')[0];

if (!names.Any(t => t == commitType))
{
    Console.WriteLine($"Commit type is not on accepted list. Current: {commitType}. Should be one of: {string.Join(", ", names)}");
    return 1;
}

var commitSubject = commitHeader.Split(':')[1];
if (string.IsNullOrWhiteSpace(commitSubject))
{
    Console.WriteLine($"Commit subject cannot be empty.");
    return 1;
}

var lines = commitMessage.Length;
Console.WriteLine($"lines: {lines}");
if (lines == 1)
{
    return 0;
}

if (!string.IsNullOrWhiteSpace(commitMessage[1])) 
{
    Console.WriteLine($"There must be blank line between header and body.");
    return 1;
}

if (lines >= 3 && string.IsNullOrWhiteSpace(commitMessage[2]))
{
    Console.WriteLine($"Body cannot be empty.");
    return 1;
}

return 0;