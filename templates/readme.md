# How to use this templates?

## Manually:

1. In **Windows Explorer**, select and open the folder you want to use as template.
2. Select all files in folder, right-click the selection, and choose **Send to > Compressed (zipped) folder**. The files that you selected are compressed into a _.zip_ file.
3. Copy the _.zip_ files and paste it in the user item template location.
The default directory is **%USERPROFILE%\Documents\Visual Studio XXXX\Templates\ItemTemplates\Visual C#**.
4. Now you can use this templates when creating new item.

For example, if you need to use **Page** template, you should create _.zip_ with this files:
- Page.vstemplate
- Page.xaml
- Page.xaml.cs

## Use script:

1. You can use "make-zip.cmd" script to automatically create _.zip_ files.
2. After that you need to follow the steps starting from step 3 of the instructions above.

P.s: this script works only on Win10 and above.