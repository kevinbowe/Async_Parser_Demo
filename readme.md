##SUMMARY
The purpose of this application is to demonstrate Asynchronous Processing. 

The application starts a task for each file in a source folder. Each task is passed a file name, a string pattern, a root path and a delay time. The task searches each line in the file looking for a pattern match. If a match occurs, the line and the number of matches in that line are added to an output object. The delay is for demonstration and is used to confirm that the tasks are actually being executed asynchronously. The delay is not necessary.

When all of the tasks complete, the results object of all of the tasks are returned as a collection.

The lines matched are written to a single output file. The total number of files processed, the total line matches count and the total pattern matches counts are written to the console.    
##CONDITIONS
1)	This is a simple C# console application. All of the code is included in a single file. Running the application is as simple as creating a console application in Visual Studio and replacing the Program.cs file with the example code.
2)	The structure of this application is intentionally simple in order to not obfuscate the Async example.
3)	There is no exception handling in this application, such as TRY/CATCH/FINALLY or USE(…){…}. This is intentional because it would obfuscate the Async example.

##INTRODUCTION
This example was inspired by a recent job interview question. This was the original question:
```
Write a function that takes a source directory path, a search string, and a destination filename. 
The function should then open all of the files in the given directory in parallel (using async technique of your choice). 
Find lines that have the search text in them.
Extract and output all lines with that search text in a file with the given output filename. 
At the end of execution, the function should output the number of files it processed, the number of lines it found the search text in, and the number of occurrences of that search term (don't assume once per line).
```
Please download **Async Parser Demo Application_062916.docx** for an indepth description of this code.