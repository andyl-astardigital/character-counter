namespace frequency_tests;

[TestClass]
[DoNotParallelize] // faff to run the tests out of the console
public sealed class CharacterCounterTests
{
    private const string TEST_FILE = "test_input.txt";
    
    private string CaptureConsole(Action testAction)
    {
        // make sure we can read the console output for our tests
        // by redirecting the console output to a StringWriter
        var outputWriter = new StringWriter();
        var originalOut = Console.Out;
        
        try
        {
            Console.SetOut(outputWriter);
            testAction();
            return outputWriter.ToString();
        }
        finally
        {
            // reset the console stream to tidy up
            Console.SetOut(originalOut);
        }
    }

    [TestMethod]
    public void TestCharacterCountingCaseSensitive()
    {
        string consoleOutput = CaptureConsole(() =>
        {
            var args = new string[] { TEST_FILE };
            Frequency.Main(args);
        });
        
        Assert.IsTrue(consoleOutput.Contains("Total characters:"), "Output should contain total character count");
        
        Assert.IsTrue(consoleOutput.Contains("A ("), "Letter 'A' should be counted");
        Assert.IsTrue(consoleOutput.Contains("a ("), "Letter 'a' should be counted");
        
        Assert.IsTrue(consoleOutput.Contains("0 ("), "Number '0' should be counted");
        
        Assert.IsFalse(consoleOutput.Contains("  ("), "Space should not be counted as a character");
        Assert.IsFalse(consoleOutput.Contains("! ()"), "Exclamation mark should be filtered out");
        Assert.IsFalse(consoleOutput.Contains("@ ("), "At symbol should be filtered out");
        Assert.IsFalse(consoleOutput.Contains("# ("), "Hash symbol should be filtered out");
    }
    
    [TestMethod]
    public void TestCharacterCountingCaseInsensitive()
    {
        string consoleOutput = CaptureConsole(() =>
        {
            var args = new string[] { TEST_FILE, "--ignore-case" };
            Frequency.Main(args);
        });
        
        Assert.IsTrue(consoleOutput.Contains("Total characters:"), "Output should contain total character count");
        
        Assert.IsTrue(consoleOutput.Contains(": a ("), "Lowercase 'a' should be counted");
        Assert.IsFalse(consoleOutput.Contains(": A ("), "Uppercase 'A' should not appear in case-insensitive mode");
        
        // A and a are both in teh test file twice
        Assert.IsTrue(consoleOutput.Contains(": a (4)"), "Letter 'a' should show combined count");
        
        Assert.IsFalse(consoleOutput.Contains("  ("), "Space should not be counted as a character");
        Assert.IsFalse(consoleOutput.Contains("! ()"), "Exclamation mark should be filtered out");
        Assert.IsFalse(consoleOutput.Contains("@ ("), "At symbol should be filtered out");
        Assert.IsFalse(consoleOutput.Contains("# ("), "Hash symbol should be filtered out");
    }
    
    [TestMethod]
    public void TestFileNotFoundHandling()
    {
        string consoleOutput = CaptureConsole(() =>
        {
            var args = new string[] { "nonexistent_file.txt" };
            Frequency.Main(args);
        });
        Assert.IsTrue(consoleOutput.Contains("Error: Input file"), "Should display file not found error");
    }

    [TestMethod]
    public void TestNoArgumentsHandling()
    {
        string consoleOutput = CaptureConsole(() =>
        {
            var args = new string[] { };
            Frequency.Main(args);
        });
        
        string expectedError = "Error: Please provide an input file name.";
        string expectedUsage = "Usage: frequency.exe <input_file> [--ignore-case]";
        
        Assert.IsTrue(consoleOutput.Contains(expectedError), 
            $"Expected '{expectedError}' but was not found in: '{consoleOutput}'");
        Assert.IsTrue(consoleOutput.Contains(expectedUsage), 
            $"Expected '{expectedUsage}' but was not found in: '{consoleOutput}'");
    }
    
    [TestMethod]
    public void TestTopTenCharactersDisplay()
    {
        string consoleOutput = CaptureConsole(() =>
        {
            var args = new string[] { TEST_FILE };
            Frequency.Main(args);
        });
        
        Assert.IsTrue(consoleOutput.Contains("Rank: 1:"), 
            $"Expected 'Rank: 1:' but was not found in output: '{consoleOutput}'");
        
        Assert.IsFalse(consoleOutput.Contains("Rank: 11:"), 
            "Characters after rank 10 should not have rank numbers");
    }
}
