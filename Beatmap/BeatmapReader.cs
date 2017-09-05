﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimePPDisplayer.Beatmap
{
    class BeatmapReader
    {
        private string m_beatmap_header;
        private List<BeatmapObject> m_object_list = new List<BeatmapObject>();

        public BeatmapReader(string file)
        {
            using (var fs = File.OpenRead(file))
            {
                using (var reader = new StreamReader(fs))
                {
                    Parser(reader);
                }
            }

        }

        private string ReadToken(string t)
        {
            return t.Replace("[", "").Replace("]", "");
        }

        private string ReadBlock(StreamReader reader)
        {
            StringBuilder builder = new StringBuilder();
            string tmp = "";
            do
            {
                tmp = reader.ReadLine();
                builder.AppendLine(tmp);
            } while (tmp != "");
            return builder.ToString();
        }

        private void ReaderObject(StreamReader reader)
        {
            string tmp = "";
            do
            {
                tmp = reader.ReadLine();
                if (tmp != "")
                {
                    var bobj = new BeatmapObject(tmp);
                    m_object_list.Add(bobj);
                }
            } while (tmp != ""&&!reader.EndOfStream);
        }

        public void Parser(StreamReader reader)
        {
            StringBuilder builder = new StringBuilder();
            string tmp = "";
            //Load Version
            builder.AppendLine(reader.ReadLine());
            builder.AppendLine(reader.ReadLine());

            //Load General
            do
            {
                string token = "";
                tmp = reader.ReadLine();
                builder.AppendLine(tmp);
                if(tmp=="")
                {
                    builder.AppendLine(tmp);
                    continue;
                }

                if (tmp[0] == '[')
                    token = ReadToken(tmp);

                switch (token)
                {
                    case "General":
                    case "Editor":
                    case "Metadata":
                    case "Difficulty":
                    case "Events":
                    case "Colours":
                        builder.Append(ReadBlock(reader));
                        break;
                    case "HitObjects":
                        ReaderObject(reader);
                        break;
                }

            } while (!reader.EndOfStream);
            m_beatmap_header = builder.ToString();
        }

        public string SubBeatmap(int end_time)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(m_beatmap_header);
            foreach(var obj in m_object_list)
            {
                if (obj.Time > end_time) break;
                builder.AppendLine(obj.ObjectStr);
            }

            return builder.ToString();
        }
    }
}
