# FiRaa
A machine learning model and application (FiRaa) is created as a Revit add-in with C# language, python and Revit API (Application Programming Interface) based on text mining algorithm which applies fire codes on BIM models. It can be adjusted by adding more fire codes to be applied and more datasets can be added for better classifications. The source code is available on (https://github.com/mohamedalbassel/FiRaa ).
# FiRaa Setup
Copy the files in the following path: 
C:\ProgramData\Autodesk\Revit\Addins\2020
# Add Revit Families
FiRaa can load the crystal families from mentioned path automatically, or you can add (FR_1 Hour.rfa),(FR_2 Hour.rfa),(FR_3 Hour.rfa) & (M_Path Of Travel Tag.rfa) in Revit model
# Add Revit Parameter to (ROOM) category
Add parameters: "Fire_Classification", "Fire_Occupant Load Factor", "fire rating"
# Add Revit Parameter to (Project information)
Add parameter "Building Type"
# Grasshopper plug-in
Copy the file (FiRaa.gha) in the following path: 
C:\Users\Username\AppData\Roaming\Grasshopper\Libraries
