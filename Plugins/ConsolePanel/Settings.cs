﻿using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace ConsolePanel
{
    [Serializable]
    class Settings
    {
        private ConsoleColor background = ConsoleColor.Black;
        private ConsoleColor foreground = ConsoleColor.White;

        private TerminalProvider provider = TerminalProvider.Bash;
        private string minttyCmd = @"%PROGRAMFILES%\Git\usr\bin\mintty.exe";
        private string bashCmd = "-e \"/c/Program Files/Git/bin/bash\"";

        [Category("Cmd"), DisplayName("Background Color"), DefaultValue(ConsoleColor.Black)]
        public ConsoleColor BackgroundColor
        {
            get
            {
                return this.background;
            }
            set
            {
                this.background = value;
            }
        }

        [Category("Cmd"), DisplayName("Foreground Color"), DefaultValue(ConsoleColor.White)]
        public ConsoleColor ForegroundColor
        {
            get
            {
                return this.foreground;
            }
            set
            {
                this.foreground = value;
            }
        }

        [DisplayName("Terminal"), DefaultValue(TerminalProvider.Bash)]
        public TerminalProvider TerminalProvider
        {
            get
            {
                return this.provider;
            }
            set
            {
                this.provider = value;
            }
        }

        [Category("Bash"), DisplayName("Command"), DefaultValue(@"%PROGRAMFILES%\Git\usr\bin\mintty.exe")]
        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string MinttyCommand
        {
            get
            {
                return this.minttyCmd;
            }
            set
            {
                this.minttyCmd = value;
            }
        }

        [Category("Bash"), DisplayName("Parameters"), DefaultValue("-e \"/c/Program Files/Git/bin/bash\"")]
        public string BashCommand
        {
            get
            {
                return this.bashCmd;
            }
            set
            {
                this.bashCmd = value;
            }
        }
    }

    enum TerminalProvider
    {
        Bash,
        Cmd
    }
}
