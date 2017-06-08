using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using System.Diagnostics;

namespace MagicGladiators
{
    public enum SongState
    {
        Menu,
        Battle
    }

    public class SaMM:ILoadable, IUpdateable
    {
        //SaMM = Sound and Music Media

        private static SaMM _i;
        public static SaMM i
        {
            get
            {
                if (_i == null)
                {
                    _i = new SaMM();
                }
                return _i;
            }
        }

        //

        private float volume;
        private float volumeradiant;
        private bool changingSong;
        private Song mbgm;
        private Song bbgm;
        private SongState newSong;
        private SongState lastSong;
        private bool muted;
        //private Sound

        private SaMM()
        {
            volume = 0.06F;
            volumeradiant = 0;
            changingSong = false;
        }

        public void Update()
        {
            if (updateThread == null)
            {
                updateThread = new Thread(ThreadUpdate);
                updateThread.Start();
            }
        }

        private Thread updateThread;
        private void ThreadUpdate()
        {
            SongState nextSong;
            Debug.WriteLine("Thread Started");
            while (true)
            {
                nextSong = newSong;
                Debug.WriteLine("next song: " + nextSong.ToString() + " | new song: " + newSong.ToString() + " | lastSong: " + lastSong.ToString());
                if (lastSong != nextSong)
                {
                    Debug.WriteLine("Last song is not equal to next song");
                    if (nextSong == SongState.Battle)
                    {
                        Debug.WriteLine("Tries playing battle");
                        MediaPlayer.Play(bbgm);
                    }
                    if (nextSong == SongState.Menu)
                    {
                        Debug.WriteLine("Tries playing battle");
                        MediaPlayer.Play(mbgm);
                    }
                    lastSong = nextSong;
                }
                Thread.Sleep(200);
            }
        }

        public void LoadContent(ContentManager content)
        {
            
            mbgm = content.Load<Song>("All This");
            bbgm = content.Load<Song>("Alchemists Tower");
            MediaPlayer.Play(mbgm);
            MediaPlayer.IsRepeating = true;
            
        }

        public void NewPlaystate(SongState ss)
        {
            newSong = ss;
            
        }

        public void MuteMusic()
        {
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
        }
    }
}
