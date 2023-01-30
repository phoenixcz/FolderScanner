# Folder Scanner

## Description
Write a simple application that detects changes in a local folder. During the first run the app will analyze the content of the given folder. During each additional run the app will return changes detected since last run:

* List of new files and subfolders
* List of modified files (change of file content)
* List of deleted files and subfolders

For each file maintain the version. At the beginning all files will have version equal to 1. With each  detected change, the version is increased by 1.

Write the app as an ASP .NET MVC program.

Assume that the size of files in given folder will be less than 50MB and that number of files in each folder will be 100 at most.

App will be executed from the UI by clicking on a button (doesn't detect changes automatically).

Do not use databases.

## Example output

1. First run
    > *Input: C:\myFolder
    Output: New folder. No changes.*

2. Additional run
    > *Input: C:\myFolder
    Output:
    [A] new.txt
    [A] subfolder/anotherNew.docx
    [M] changed.txt (version 2)
    [D] subfolder/deleted.xlsx*

3. Additional run
    > *Input: C:\myFolder
    Output:
    No changes.*
