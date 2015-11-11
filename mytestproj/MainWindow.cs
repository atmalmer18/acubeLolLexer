using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Gtk;


public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnExecuteButtonClicked (object sender, EventArgs e)
	{
		// lexeme table view
		lexemeTree.Destroy();
		lexemeTree = new Gtk.TreeView ();
		lexemeWindow.Add (lexemeTree);

		Gtk.TreeViewColumn lexemeColumn = new Gtk.TreeViewColumn ();
		lexemeColumn.Title = "Lexeme";

		Gtk.TreeViewColumn classificationColumn = new Gtk.TreeViewColumn ();
		classificationColumn.Title = "Classification";

		lexemeTree.AppendColumn (lexemeColumn);
		lexemeTree.AppendColumn (classificationColumn);

		// lexeme cell!
		Gtk.CellRendererText lexemeNameCell = new Gtk.CellRendererText();
		lexemeColumn.PackStart(lexemeNameCell, true);

		Gtk.CellRendererText classificationNameCell = new Gtk.CellRendererText ();
		classificationColumn.PackStart (classificationNameCell, true);

		lexemeColumn.AddAttribute (lexemeNameCell, "text", 0);
		classificationColumn.AddAttribute (classificationNameCell, "text", 1);
		// end of lexeme cell

		Gtk.ListStore lexemeListStore = new Gtk.ListStore (typeof(string), typeof(string));
		var lexLength = 0;

		var code = textview3.Buffer.Text;
		//textview.Buffer.Text = code;
		List<String> lexemes = new List<String>();
		List<String> classi = new List<String>();
		//string delim = ;
		/*Regex codeDelim = new Regex (@"HAI\s*|KTHXBYE\s*|I HAS A ");
		var matcheshaiList = (from Match m in codeDelim.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			if(){
				
			}
			lexemes.Add (s);
			lexLength++;
			classi.Add("Code Delimiter");
		}*/



		MatchCollection regex = Regex.Matches(textview3.Buffer.Text, @"HAI|KTHXBYE|I HAS A |BTW |OBTW|TLDR|( ITZ )|( R )|GIMMEH |VISIBLE |(?<=(ITZ |R |SUM OF |DIFF OF |PRODUKT OF |QUOSHUNT OF |MOD OF |BIGGR OF |SMALLR OF ))\d+|(?<=(I HAS A |SUM OF |GIMMEH |DIFF OF |PRODUKT OF |QUOSHUNT OF |MOD OF |BIGGR OF |SMALLR OF ))\w[A-Za-z0-9_]*|""([^""]|"""")*""|SUM OF |DIFF OF |PRODUKT OF |QUOSHUNT OF |MOD OF |BIGGR OF |SMALLR OF ");
		foreach (Match m in regex)
		{
			lexemes.Add (m.ToString ());
			Regex codeDelim = new Regex(@"HAI|KTHXBYE");
			Regex varDec = new Regex("I HAS A ");
			Regex comment = new Regex ("BTW |OBTW|TLDR");
			Regex varass = new Regex ("( ITZ )|( R )");
			Regex gim = new Regex ("GIMMEH ");
			Regex numlit = new Regex (@"\d+");
			Regex vari = new Regex (@"\w[A-Za-z0-9_]*");
			Regex strlit = new Regex ("\"([^\"]|\"\")*\"");
			Regex strdeli = new Regex (@"""");
			Regex arith = new Regex ("SUM OF |DIFF OF |PRODUKT OF |QUOSHUNT OF |MOD OF |BIGGR OF |SMALLR OF ");
			Regex vis = new Regex ("VISIBLE ");

			if (codeDelim.IsMatch (lexemes [lexLength])) {
				classi.Add ("Code Delimiter");
			} else if (varDec.IsMatch (lexemes [lexLength])) {
				classi.Add ("Variable Declaration");
			} else if (comment.IsMatch (lexemes [lexLength])) {
				classi.Add ("Comment");
			} else if (numlit.IsMatch (lexemes [lexLength])) {
				classi.Add ("Number Literal");
			} else if (varass.IsMatch (lexemes [lexLength])) {
				classi.Add ("Variable Assignment");
			} else if (gim.IsMatch (lexemes [lexLength])) {
				classi.Add ("Input Function");
			} else if (vis.IsMatch (lexemes [lexLength])) {
				classi.Add ("Output Function");
			} else if (strlit.IsMatch (lexemes [lexLength])) {
				string replacedString = lexemes [lexLength].Replace ("\"", "");
				lexemes [lexLength] = replacedString;
				classi.Add ("String Literal");
			} else if (arith.IsMatch (lexemes [lexLength])) {
				classi.Add ("Arithmetic Operation");
			} else if (vari.IsMatch (lexemes [lexLength])) {
				classi.Add ("Variable Identifier");
			} else if (strdeli.IsMatch (lexemes [lexLength])) {
				classi.Add ("String Delimiter");
			} 
			lexLength++;
		}

		// loop this model
		for(int i=0;i<lexLength;i++){
			lexemeListStore.AppendValues (lexemes[i],classi[i]);
		} 
		// end of looping model
/*
		Regex vis = new Regex ("VISIBLE ");
		matcheshaiList = (from Match m in vis.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("Output Function");
			lexLength++;
		}


		Regex strdeli = new Regex ("(?<=VISIBLE )\"|(?<=VISIBLE \".*)\"");
		matcheshaiList = (from Match m in strdeli.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("String Delimiter");
			lexLength++;
		}

		Regex strlit = new Regex ("(?<=VISIBLE \")[^\"]*");
		matcheshaiList = (from Match m in strlit.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("String Literal");
			lexLength++;
		}



		Regex varvis = new Regex (@"(?<=(VISIBLE|GIMMEH) )\w[A-Za-z0-9_]*");
		matcheshaiList = (from Match m in varvis.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("Variable Identifier");
			lexLength++;
		}

		Regex numlit = new Regex (@"(?<=(ITZ |R |SUM OF |DIFF OF |PRODUKT OF |QUOSHUNT OF |MOD OF |BIGGR OF |SMALLR OF|SUM OF [0-9]* AN |DIFF OF [0-9]* AN |PRODUKT OF [0-9]* AN |QUOSHUNT OF [0-9]* AN |MOD OF [0-9]* AN |BIGGR OF [0-9]* AN |SMALLR OF [0-9]* AN |AN ))[0-9]*");
		matcheshaiList = (from Match m in numlit.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("Number Literal");
			lexLength++;
		}

		Regex vari = new Regex (@"(?<=(I HAS A |SUM OF ))\w[A-Za-z0-9_]*");
		matcheshaiList = (from Match m in vari.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("Variable Identifier");
			lexLength++;
		}

		Regex arith = new Regex ("SUM OF |DIFF OF |PRODUKT OF |QUOSHUNT OF |MOD OF |BIGGR OF |SMALLR OF ");
		matcheshaiList = (from Match m in arith.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("Arithmetic Operation");
			lexLength++;
		}
			

		Regex bool1 = new Regex ("BOTH OF |EITHER OF |WON OF |NOT |MOD OF |ALL OF |ANY OF ");
		matcheshaiList = (from Match m in bool1.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("Boolean Operation");
			lexLength++;
		}

		Regex comp = new Regex ("BOTH SAEM |DIFFRINT ");
		matcheshaiList = (from Match m in comp.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("Comparison Operation");
			lexLength++;
		}

		Regex concat = new Regex ("SMOOSH ");
		matcheshaiList = (from Match m in concat.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("String Concatenation");
			lexLength++;
		}

		Regex startIf = new Regex ("O RLY?");
		matcheshaiList = (from Match m in startIf.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("START OF IF-ELSE CLAUSE");
			lexLength++;
		}

		Regex ifClause = new Regex ("YA RLY");
		matcheshaiList = (from Match m in ifClause.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("IF CLAUSE");
			lexLength++;
		}

		Regex elseifClause = new Regex ("MEBBE");
		matcheshaiList = (from Match m in elseifClause.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("ELSE-IF CLAUSE");
			lexLength++;
		}

		Regex elseClause = new Regex ("NO WAI");
		matcheshaiList = (from Match m in elseClause.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("ELSE CLAUSE");
			lexLength++;
		}

		Regex endIf = new Regex ("OIC");
		matcheshaiList = (from Match m in endIf.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("END OF IF-ELSE CLAUSE");
			lexLength++;
		}	

		Regex switchKey = new Regex ("WTF?");
		matcheshaiList = (from Match m in switchKey.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("SWITCH CASE KEYWORD");
			lexLength++;
		}	

		Regex caseKey = new Regex ("OMG");
		matcheshaiList = (from Match m in caseKey.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("CASE KEYWORD");
			lexLength++;
		}

		Regex defKey = new Regex ("OMGWTF");
		matcheshaiList = (from Match m in defKey.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("DEFAULT CASE KEYWORD");
			lexLength++;
		}	

		Regex breakKey = new Regex ("GTFO");
		matcheshaiList = (from Match m in breakKey.Matches (code) select m.Value).ToList ();
		foreach (string s in matcheshaiList) {
			lexemes.Add(s);
			classi.Add("BREAK KEYWORD");
			lexLength++;
		}	
		*/

		lexemeTree.Model = lexemeListStore;

		lexemeWindow.ShowAll ();

		// symbol table view
		symbolTableTree.Destroy();
		symbolTableTree = new Gtk.TreeView ();
		symbolTableWindow.Add (symbolTableTree);

		Gtk.TreeViewColumn identifierColumn = new Gtk.TreeViewColumn ();
		identifierColumn.Title = "Identifier";

		Gtk.TreeViewColumn valueColumn = new Gtk.TreeViewColumn ();
		valueColumn.Title = "Value";

		symbolTableTree.AppendColumn (identifierColumn);
		symbolTableTree.AppendColumn (valueColumn);

		// symbol table cell!
		Gtk.CellRendererText identifierNameCell = new Gtk.CellRendererText();
		identifierColumn.PackStart(identifierNameCell, true);

		Gtk.CellRendererText valueNameCell = new Gtk.CellRendererText ();
		valueColumn.PackStart (valueNameCell, true);

		identifierColumn.AddAttribute (identifierNameCell, "text", 0);
		valueColumn.AddAttribute (valueNameCell, "text", 1);
		// end of symbol table cell

		Gtk.ListStore symbolTableListStore = new Gtk.ListStore (typeof(string), typeof(string));

		// loop this model
		MatchCollection mc = Regex.Matches(textview3.Buffer.Text, @"(?<=(^I HAS A )|\nI HAS A )\w[A-Za-z0-9_]*");
		foreach (Match m in mc)
		{
			symbolTableListStore.AppendValues (m.ToString(),"null");
		}
		// end of looping model

		symbolTableTree.Model = symbolTableListStore;

		symbolTableWindow.ShowAll ();

	}

	protected void OnFileButtonClicked (object sender, EventArgs e)
	{
		Gtk.FileChooserDialog fc=
			new Gtk.FileChooserDialog("Choose the file to open",
			                          this,
			                          FileChooserAction.Open,
			                          "Cancel",ResponseType.Cancel,
			                          "Open",ResponseType.Accept);
		fc.Filter = new FileFilter ();
		fc.Filter.AddPattern ("*.lol");

		if (fc.Run() == (int)ResponseType.Accept) 
		{
			System.IO.FileStream file = System.IO.File.OpenRead(fc.Filename);
			try
			{   // Open the text file using a stream reader.
				using (StreamReader sr = new StreamReader(fc.Filename))
				{
					// Read the stream to a string, and write the string to the console.
					String line = sr.ReadToEnd();
					textview3.Buffer.Text = line;
				}
			}
			catch (Exception error)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(error.Message);
			}
			file.Close();
		}
		//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
		fc.Destroy();
	}
}
