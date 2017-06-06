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
        //private Sound

        private SaMM()
        {
            volume = 100;
            volumeradiant = 0;
            changingSong = false;
        }

        public void Update()
        {
            if (changingSong)
            {
                if (volumeradiant > 0)
                {
                    volumeradiant -= GameWorld.Instance.deltaTime;
                }
                if (volumeradiant <= 0)
                {
                    changingSong = false;
                    volumeradiant = 0;
                    if (newSong == SongState.Menu)
                    {
                        MediaPlayer.Play(mbgm);
                    }
                    if (newSong == SongState.Battle)
                    {
                        MediaPlayer.Play(bbgm);
                    }
                    lastSong = newSong;
                }
            } else
            {
                if (volumeradiant < 100)
                {
                    volumeradiant += GameWorld.Instance.deltaTime;
                }
                if (volumeradiant > 100)
                {
                    volumeradiant = 100;
                }
            }

            if (lastSong != newSong)
            {
                changingSong = true;
            }

            MediaPlayer.Volume = (volume / 100) * volumeradiant;
        }

        public void LoadContent(ContentManager content)
        {
            /*
            mbgm = content.Load<Song>("All This");
            bbgm = content.Load<Song>("Alchemists Tower");
            MediaPlayer.Play(mbgm);
            MediaPlayer.IsRepeating = true;
            */
        }

        public void NewPlaystate(SongState ss)
        {
            newSong = ss;
        }
    }
}
