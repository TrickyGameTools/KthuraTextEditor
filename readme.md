# Kthura Text Editor

Kthura is a map system for games.
This is a "hack" tool only to be used by those who know what they are doing.

![](https://raw.githubusercontent.com/TrickyGameTools/KthuraTextEditor/master/Properties/Kthura.png)

# What is Kthura?

Kthura is an object oriented map system developed by Jeroen P. Broks, and it was used for the first time in the game Star Story for dungeon development.
It would later also be used in The Fairy Tale REVAMPED.
Now the "true" editor is a WYSIWYG like editor, however in WYSIWYG you cannot also dive into the true core of a system. Since Kthura files are JCR6 files with the object just set up in a text based file and the meta data in a file that's easy to modify, you can use this tool to due some pure edits that can be hard to perform in a WYSISWYG environment. Also if you "break" things in Kthura this tool can be used to manually fix those.

Warning, this tool is only meant for those who know what they are doing. If you are unsure about the inner workings of Kthura, it can be wise to avoid this tool, or only to use it to look under the hood without modifying things.



# A few pointers about the OBJECTS file

This is basically the "core" of every Kthura map, and basically where all the data Kthura needs to do its business and its magic are stored. It's technically some quick code that is parsed and processed by Kthura.
Every line prefixed with "--" is taken as a comment. Please note, when you use the WYSIWYG editor all comments are ignored when loading and when the map is being saved the comments will therfore have disappeared. It's therefore not really fruitful to add comments for maps that are going to be modified in the WYSIWYG editor later anyway... Good to know... ;)

The "LAYERS" block contains all the names of the layers and ends with "___END".
And "NEW" starts a new object and below is all the data it needs. The "NEW" block has no closure needs. It's not that stopping to ident ends an object (this is not Python you know), as needless whitespaces are ignored by the parser. NEW just counts ending the old object (if one is set) and starting a new. I won't go any further into the deep abou the meaning of each field, but basically it's just a key=value base, and it should explain itself most of all actually. More proper documentation may come in if I give the Kthura WYSIWYG based editor a more properly look, if you desire.... 


# License notices

The Kthura Text Editor has been licensed under the General Public License version 3 (GPL3). The face of Kthura (the purple skinned girl) is my property, and may only be distributed with in unmodified form in relation of Kthura or other projects by me (Jeroen Petrus Broks) in which Kthura's face has been used.



# History of the girl

Kthura is the main character of a game for which the Kthura system was originally intended. Due to some technical reasons the game never came, although the wish to create the game still exists. If I can get far enough into C# to properly do it, I may still go for it though.... But no promises yet. ;)

