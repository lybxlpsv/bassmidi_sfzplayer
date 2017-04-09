using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lybmidi_c
{
    public partial class Form1 : Form
    {
        public int i = 0;
        public int chan;
        public ManagedBass.Midi.MidiFontEx[] font = new ManagedBass.Midi.MidiFontEx[9999999];
        public Form1()
        {
            InitializeComponent();
            
        }

        public void load_sf()
        {
            //Polyphone
            string directory = System.IO.Directory.GetCurrentDirectory();
            ManagedBass.Bass.Load(directory + @"\bass.dll");
            ManagedBass.Midi.BassMidi.Load(directory + @"\bassmidi.dll");
            ManagedBass.Bass.PluginLoad(directory + @"\bass_aac.dll");
            ManagedBass.Bass.PluginLoad(directory + @"\bass_opus.dll");
            ManagedBass.Bass.PluginLoad(directory + @"\bass_flac.dll");

            bool check1 = ManagedBass.Bass.Init(-1, 44100, 0);
            var dirs = Directory.EnumerateDirectories(directory + @"\data\");
            
            foreach (var dir in dirs)
            {
                if (dir != "samples")
                {
                    var lsbs = Directory.EnumerateDirectories(dir);
                    foreach (var lsb in lsbs)
                    {
                        var files = Directory.EnumerateFiles(lsb + @"\");
                        foreach (var sfz in files)
                        {
                            int lsbbank = int.Parse(dir.Substring(dir.Length - 3, 3));
                            int msbbank = int.Parse(lsb.Substring(lsb.Length - 3, 3));
                            string spreset = sfz.Substring(lsb.Length, sfz.Length - lsb.Length);
                            int preset = int.Parse(spreset.Substring(1, 3));
                            font[i].DestinationBank = msbbank;
                            font[i].DestinationBankLSB = lsbbank;
                            font[i].DestinationPreset = preset;
                            font[i].SoundFontBank = -1;
                            font[i].SoundFontPreset = -1;
                            font[i].Handle = ManagedBass.Midi.BassMidi.FontInit(sfz, 0);
                            i++;
                        }
                    }
                }
            }
        }

        public void load_sf2()
        {
            //EXSC
            string directory = System.IO.Directory.GetCurrentDirectory();
            ManagedBass.Bass.Load(directory + @"\bass.dll");
            ManagedBass.Midi.BassMidi.Load(directory + @"\bassmidi.dll");
            ManagedBass.Bass.PluginLoad(directory + @"\bass_aac.dll");
            ManagedBass.Bass.PluginLoad(directory + @"\bass_opus.dll");
            ManagedBass.Bass.PluginLoad(directory + @"\bass_flac.dll");
            bool check1 = ManagedBass.Bass.Init(-1, 44100, 0);
            var dirs = Directory.EnumerateDirectories(directory + @"\data\");

            foreach (var dir in dirs)
            {
                if (dir != "samples")
                {
                    var files = Directory.EnumerateFiles(dir + @"\");
                    foreach (var sfz in files)
                    {
                        int lsbbank = int.Parse(dir.Substring(dir.Length - 3, 3));
                        string sbank = sfz.Substring(dir.Length, sfz.Length - dir.Length);
                        int msbbank = int.Parse(sbank.Substring(1, 3));
                        string spreset = sfz.Substring(dir.Length, sfz.Length - dir.Length);
                        int preset = int.Parse(spreset.Substring(5, 3));
                        font[i].DestinationBank = msbbank;
                        font[i].DestinationBankLSB = lsbbank;
                        font[i].DestinationPreset = preset;
                        font[i].SoundFontBank = -1;
                        font[i].SoundFontPreset = -1;
                        font[i].Handle = ManagedBass.Midi.BassMidi.FontInit(sfz, 0);
                        i++;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            load_sf2();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ManagedBass.Bass.StreamFree(chan);
            chan = ManagedBass.Midi.BassMidi.CreateStream("test.mid", 0, 0, ManagedBass.BassFlags.Loop, 1);
            ManagedBass.Midi.BassMidi.StreamSetFonts(chan, font, i);
            ManagedBass.Bass.ChannelPlay(chan);
        }
    }
}
