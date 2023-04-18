using System.Collections.Generic;
using System.IO;

namespace CS5410
{
    static class Load
    {
        // use a dynamic programming (kind-of) solution so the levels only have to be loaded once
        private static Dictionary<int, (string, string[])> s_levels = null;

        public static (string, string[]) loadLevels(int levelIndex)
        {
            // file read method grabbed from ChatGPT -- to be honest, this could just be done in loadContent or something

            if (s_levels == null)
            {
                //string levelContent = File.ReadAllText(manager.RootDirectory + "Levels/levels-all.bbiy");
                // do something with the text

                s_levels = new Dictionary<int, (string, string[])>();

                using (StreamReader reader = new StreamReader("levels-all.bbiy"))
                {
                    string title = null;
                    string[] map = null;

                    var (width, height) = (0, 0);
                    int index = 0;

                    int levelNumber = 0;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        // do something with the line

                        if (map == null && title == null)
                        {
                            title = line;
                            continue;
                        }
                        if (map == null)
                        {
                            // also grabbed from ChatGPT
                            string[] parts = line.Split('x');
                            width = int.Parse(parts[0].Trim());
                            height = int.Parse(parts[1].Trim());
                            height *= 2; // each level has 2 layers
                            index = 0;

                            map = new string[height];

                            continue;
                        }
                        if (index == height)
                        {
                            s_levels.Add(levelNumber++, (title, map));

                            title = line;
                            map = null;
                            continue;
                        }

                        map[index] = line;

                        index++;
                    }

                    s_levels.Add(levelNumber, (title, map));
                }
            }

            return s_levels[levelIndex];
        }
    }
}
