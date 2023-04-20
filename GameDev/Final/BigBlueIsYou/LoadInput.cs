using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public static class LoadInput
    {
        public static bool s_saving
        {
            get;
            set;
        }

        public static async void LoadInputMap()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    if (storage.FileExists("InputMap.json"))
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("InputMap.json", FileMode.Open))
                        {
                            if (fs != null)
                            {
                                Systems.InputSystem.s_keyCommands = await JsonSerializer.DeserializeAsync<Dictionary<Keys, Systems.Commands>>(fs);
                            }
                        }
                    }
                }
                catch (IsolatedStorageException)
                {
                    // do nothing
                }
            }
        }

        public static async Task SaveInputMap()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    using (IsolatedStorageFileStream fs = storage.OpenFile("InputMap.json", FileMode.Create))
                    {
                        if (fs != null)
                        {
                            await JsonSerializer.SerializeAsync(fs, Systems.InputSystem.s_keyCommands);
                        }
                    }
                }

                catch (IsolatedStorageException)
                {
                    // do nothing
                }
            }
        }
    }
}
