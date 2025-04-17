# Character Frequency Counter

CLI tool that counts the frequency of characters in a text file, filters out punctuation, symbols, whitespace, control characters.

## Features

- Counts occurrences of each character in a text file
- Ignores punctuation, symbols, whitespace, and control characters
- Displays results ranked by frequency
- Shows progress while processing large files
- Buffer based to handle alrge files
- Optional case sensitivity

## Usage

```bash
frequency.exe <input_file> [--ignore-case]
```

### Examples

Case sensitive (default):
```bash
frequency.exe sample.txt
```

Case insensitive:
```bash
frequency.exe sample.txt --ignore-case
```

### Output

Top ten most frequent are marked with Rank:

#### Case-sensitive mode:
```
Bytes read: 1000/1000
Total characters: 36

Rank: 1: e (12)
Rank: 2: E (8)
Rank: 3: a (7)
Rank: 4: A (6)
Rank: 5: i (6)
Rank: 6: n (5)
Rank: 7: s (5)
Rank: 8: h (5)
Rank: 9: r (4)
Rank: 10: d (4)
: l (3)
: u (2)
...
```

#### Case-insensitive mode:
```
Bytes read: 1000/1000
Total characters: 36

Rank: 1: e (14)
Rank: 2: t (10)
Rank: 3: a (8)
Rank: 4: o (6)
Rank: 5: i (6)
Rank: 6: n (5)
Rank: 7: s (5)
Rank: 8: h (5)
Rank: 9: r (4)
Rank: 10: d (4)
: l (3)
: u (2)
...
```

## Notes

- Special characters (punctuation, symbols) are filtered out, assumed which others would be required
- Whitespace (spaces, tabs, newlines) is ignored
- Shows progress
- Default mode is case-sensitive (uppercase and lowercase letters are counted separately)
- With --ignore-case option, 'A' and 'a' are counted as the same character with all being cnoverted to lower case

## Building

```bash
dotnet build
```

## Running Tests

```bash
dotnet test frequency-tests
```