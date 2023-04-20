using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace CS5410
{
    public static class LoadInput
    {
        public static async void LoadInputMap()
        {
            await Task.Run(() =>
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
                                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Systems.InputSystem));
                                        ser.ReadObject(fs); // since the keymap is a static object, we don't actually need to do anything with the object here

                                    }
                                }
                            }
                        }

                        catch (IsolatedStorageException)
                        {
                            // don't do anything, the file wasn't found
                        }
                    }
                }
            );
        }

        public static async void SaveInputMap(Systems.InputSystem sys)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("InputMap.json", FileMode.Create))
                        {
                            if (fs != null)
                            {
                                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Systems.InputSystem));
                                ser.WriteObject(fs, sys);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // do nothing
                    }
                }
            }
            );
        }
    }
}
