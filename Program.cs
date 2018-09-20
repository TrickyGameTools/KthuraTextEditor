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
        public string Objects;
        public string Settings;
        public SortedDictionary<string, byte[]> Unknown = new SortedDictionary<string, byte[]>();
        public bool allowzlib;
        public bool allowlzma;
        public Dictionary<string, string> GeneralData = new Dictionary<string, string>();
        public ListStore LsGenData = new ListStore(typeof(string), typeof(string));
        public Dictionary<object, string> LinkGeneralData = new Dictionary<object, string>();

    }

    class MainClass
    {
        // General config
        const int base_width = 1600;
        const int base_height = 1000;
        const int girl_width = 96;
        const int girl_height = 96;

        readonly static string[] kthuramusthave = "Data;Objects;Settings".Split(';');

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
        static ScrolledWindow swObjects;
        //static HBox panObjects;
        //static VScrollbar sbObjects;
        static TextView eObjects;
        static ScrolledWindow swSettings;
        static TextView eSettings;
        static ScrolledWindow swMisc;
        static TreeView vMisc;


        // Special widgets
        static List<Widget> RequiresFile = new List<Widget>(); // Only allow these when a file has actually been loaded and activated.

        // The actual stuff itself
        static readonly SortedDictionary<string, KthuraLoadedFile> Loaded = new SortedDictionary<string, KthuraLoadedFile>();
        static KthuraLoadedFile Current {
            get {
                if (OpenFiles.ItemText == "") return null;
                return Loaded[OpenFiles.ItemText];
            }
        }

        static void initMKL(){
            MKL.Lic    ("Kthura Text Editor - Program.cs","GNU General Public License 3");
            MKL.Version("Kthura Text Editor - Program.cs","18.09.19");
            QOpen.Hello();
            QuickGTK.Hello();
            ListBox.Hello();
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

        static void AutoEnable(){
            var able = Current != null;
            foreach(Widget w in RequiresFile){
                w.Sensitive = able;
            }
        }

        static void UpdateMapList(){
            OpenFiles.Clear();
            foreach (string f in Loaded.Keys) OpenFiles.AddItem(f);
        }


        static void Load(string file){
            var j = JCR6.Dir(file);
            if (j == null) { QuickGTK.Error("JCR6 failed to analyse this mapfile\n\n" + JCR6.JERROR); return;  }
            foreach(string musthave in kthuramusthave){
                if (!j.Exists(musthave)) {
                    QuickGTK.Error($"The required entry {musthave} does not appear to exist in this JCR6 file.\n\nAre you sure this is a Kthura map file?");
                    return;
                }
            }
            var lkthura = new KthuraLoadedFile();
            Loaded[file] = lkthura;
            foreach (string e in j.Entries.Keys)
            {
                var iEntry = j.Entries[e];
                lkthura.allowlzma = lkthura.allowlzma || iEntry.Storage == "lzma";
                lkthura.allowzlib = lkthura.allowzlib || iEntry.Storage == "zlib";
                switch (e)
                {
                    case "DATA":
                        lkthura.GeneralData = j.LoadStringMap("Data");
                        break;
                    case "OBJECTS":
                        lkthura.Objects = j.LoadString("Objects");
                        break;
                    case "SETTINGS":
                        lkthura.Settings = j.LoadString("Settings");
                        break;
                    default:
                        lkthura.Unknown[iEntry.Entry] = j.JCR_B(e);
                        break;

                }
            }
            UpdateMapList();
        }

        static void OnOpenFile(object sender, EventArgs a){
            var file = QuickGTK.RequestFile("Please select a Kthura Map File");
            if (file == "") return;
            if (Loaded.ContainsKey(file)) {
                QuickGTK.Warn("That file has already been loaded!");
                return;
            }
            Load(file);
        }

        static void OnFileSelect(object sender, EventArgs a){
            AutoEnable();
            var i = OpenFiles.ItemText;
            if (i == "") return;
            // Objects
            eObjects.Buffer.Text = Current.Objects;
            // Settings
            eSettings.Buffer.Text = Current.Settings;
            // Misc files (files this text editor cannot handle, but which should be loaded and saved anyway).
            var lsMisc = new ListStore(typeof(string),typeof(string));
            foreach(string k in Current.Unknown.Keys){
                var v = Current.Unknown[k];
                var l = v.Length;
                var ln = "Empty";
                if (l > 0)
                {
                    if (l == 1) { ln = "One byte"; }
                    else if ( l < 5000) { ln = $"{l} bytes"; }
                    else if (l < 5000000) { ln = $"{Math.Round((double)l / 1024)} Kilobytes"; }
                    else if (l < 50000000) { ln = $"{Math.Round((double)l / (1024 * 1024))} Megabytes"; }
                    else { ln= $"{Math.Round((double)l / (1024 * 1024 *1024))} Gigabytes"; }
                }
                lsMisc.AppendValues(k, ln);
                vMisc.Model = lsMisc;
            }


        }

        static void initGUI(){
            Application.Init();
            win = new MainWindow();
            QuickGTK.MyMainWindow = win;
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
            bOpen.Clicked += OnOpenFile;
            OpenFiles = new ListBox("Loaded Kthura Maps");
            OpenFiles.AddTo(WorkBox);
            OpenFiles.SetSizeRequest(250, base_height - girl_height);
            OpenFiles.Gadget.CursorChanged += OnFileSelect;
            Tabber = new Notebook();
            Tabber.SetSizeRequest(base_width - 250, base_height - girl_height);
            var about = new Label("Kthura Text Editor\n\nCoded by: Tricky\n(c) Jeroen P. Broks\n\nReleased under the terms of the GPL 3\n\n" + MKL.All());
            about.SetAlignment(0, 0);
            eObjects = new TextView();
            swObjects = new ScrolledWindow();
            swObjects.SetSizeRequest(base_width - 250, base_height - girl_height);
            swObjects.Add(eObjects);
            RequiresFile.Add(eObjects);
            eSettings = new TextView();
            swSettings = new ScrolledWindow();
            swSettings.SetSizeRequest(base_width - 250, base_height - girl_height);
            swSettings.Add(eSettings);
            RequiresFile.Add(eSettings);
            swMisc = new ScrolledWindow();
            vMisc = new TreeView();
            var ncMisc = new CellRendererText();
            var tcMisc = new TreeViewColumn();
            tcMisc.Title = "Unknown Entry:";
            tcMisc.PackStart(ncMisc, true);
            tcMisc.AddAttribute(ncMisc, "text", 0);
            vMisc.AppendColumn(tcMisc);
            tcMisc = new TreeViewColumn();
            tcMisc.Title = "Size:";
            tcMisc.PackStart(ncMisc, true);
            tcMisc.AddAttribute(ncMisc, "text", 1);
            vMisc.AppendColumn(tcMisc);
            swMisc.Add(vMisc);
            RequiresFile.Add(vMisc);
            Tabber.AppendPage(about, new Label("About"));
            Tabber.AppendPage(new Label("Coming Soon"), new Label("General Data"));
            Tabber.AppendPage(swObjects, new Label("Objects"));
            Tabber.AppendPage(swSettings, new Label("Settings"));
            Tabber.AppendPage(swMisc, new Label("Misc"));
            WorkBox.Add(Tabber);
            AutoEnable();
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
