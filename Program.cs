// Lic:
// 	Kthura Text Editor
// 	
// 	
// 	
// 	
// 	(c) Jeroen P. Broks, 2018, All rights reserved
// 	
// 		This program is free software: you can redistribute it and/or modify
// 		it under the terms of the GNU General Public License as published by
// 		the Free Software Foundation, either version 3 of the License, or
// 		(at your option) any later version.
// 		
// 		This program is distributed in the hope that it will be useful,
// 		but WITHOUT ANY WARRANTY; without even the implied warranty of
// 		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// 		GNU General Public License for more details.
// 		You should have received a copy of the GNU General Public License
// 		along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 		
// 	Exceptions to the standard GNU license are available with Jeroen's written permission given prior 
// 	to the project the exceptions are needed for.
// Version: 18.09.19
// EndLic

﻿using System;
using System.Reflection;
using System.Collections.Generic;
using Gtk;
using TrickyUnits;
using TrickyUnits.GTK;
using UseJCR6;

namespace KthuraTextEditor
{
    class KthuraLoadedFile{

    }

    class MainClass
    {
        // General config
        const int base_width = 1600;
        const int base_height = 1000;
        const int girl_width = 96;
        const int girl_height = 96;

        // widgets
        static MainWindow win;
        static VBox MainBox;
        static HBox HeadBox;
        static HBox WorkBox;
        static VBox MenuBox;
        static HBox Menu1;
        static HBox Menu2;
        static Image Girl;
        static Button bGit;
        static Button bOpen;
        static Button bClose;
        static Button bSave;
        static Button bSaveAll;
        static Button bQuit;
        static ListBox OpenFiles;
        static Notebook Tabber;


        // Special widgets
        static List<Widget> RequiresFile = new List<Widget>(); // Only allow these when a file has actually been loaded and activated.

        static void initMKL(){
            MKL.Lic    ("Kthura Text Editor - Program.cs","GNU General Public License 3");
            MKL.Version("Kthura Text Editor - Program.cs","18.09.19");

        }

        static void initJCR6(){
            JCR6_lzma.Init();
            JCR6_zlib.Init();
        }

        static void initGirl(){
            Assembly asm = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = asm.GetManifestResourceStream("KthuraTextEditor.Properties.Kthura.png");
            Gdk.Pixbuf PIX = new Gdk.Pixbuf(stream);
            win.Icon = PIX;
            stream.Dispose();
            stream = asm.GetManifestResourceStream("KthuraTextEditor.Properties.Kthura.png");
            Girl = new Image(stream);
            Girl.SetSizeRequest(girl_width,girl_height);
            Girl.SetAlignment(0, 0);
            stream.Dispose();
            HeadBox.Add(Girl);
        }

        static void initGUI(){
            Application.Init();
            win = new MainWindow();
            win.Title = "Kthura Text Editor";
            win.SetSizeRequest(base_width,base_height);
            MainBox = new VBox();
            HeadBox = new HBox(); HeadBox.SetSizeRequest(base_width, girl_width);
            WorkBox = new HBox(); WorkBox.SetSizeRequest(base_width, base_height - girl_height);
            MainBox.Add(HeadBox);
            MainBox.Add(WorkBox);
            initGirl();
            MenuBox = new VBox();
            MenuBox.SetSizeRequest(base_width - girl_width, girl_height);
            HeadBox.Add(MenuBox);
            Menu1 = new HBox();
            Menu2 = new HBox();
            MenuBox.Add(Menu1);
            MenuBox.Add(Menu2);
            bGit = new Button("Github");
            bOpen = new Button("Open");
            bClose = new Button("Close"); RequiresFile.Add(bClose);
            bSave = new Button("Save"); RequiresFile.Add(bSave);
            bSaveAll = new Button("SaveAll");
            bQuit = new Button("Quit");
            Menu1.Add(bGit);
            Menu1.Add(bOpen);
            Menu1.Add(bClose);
            Menu2.Add(bSave);
            Menu2.Add(bSaveAll);
            Menu2.Add(bQuit);
            bGit.Clicked += delegate (object sender, EventArgs a) { OURI.OpenUri("https://github.com/TrickyGameTools/KthuraTextEditor"); };
            OpenFiles = new ListBox();
            OpenFiles.AddTo(WorkBox);
            OpenFiles.SetSizeRequest(250, base_height - girl_height);
            Tabber = new Notebook();
            Tabber.SetSizeRequest(base_width - 250, base_height - girl_height);
            var about = new Label("Kthura Text Editor\n\nCoded by: Tricky\n(c) Jeroen P. Broks\n\nReleased under the terms of the GPL 3\n\n" + MKL.All());
            about.SetAlignment(0, 0);
            Tabber.AppendPage(about, new Label("About"));
            Tabber.AppendPage(new Label("Coming Soon"), new Label("General Data"));
            Tabber.AppendPage(new Label("Coming Soon"), new Label("Objects"));
            Tabber.AppendPage(new Label("Coming Soon"), new Label("Misc"));
            WorkBox.Add(Tabber);
            win.Add(MainBox);
            win.ShowAll();
            Application.Run();
        }

        public static void Main(string[] args)
        {
            initMKL();
            initJCR6();
            initGUI();
        }
    }
}
