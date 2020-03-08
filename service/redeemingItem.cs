using RedeemLibrary.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedeemLibrary.service
{
    public class redeemingItem
    {
        
            private List<redeem> AllRedeemItem { get; set; }
            private List<user> AllUsername { get; set; }
            public redeemingItem()
            {


                using (var jsonFileReader = File.OpenText(this.JsonFileName))
                {
                    this.AllRedeemItem = JsonSerializer.Deserialize<redeem[]>
                    (jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true

                    }).ToList();
                }
                using (var jsonFileReader1 = File.OpenText(this.JsonFileName1))
                {
                    this.AllUsername = JsonSerializer.Deserialize<user[]>
                    (jsonFileReader1.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true

                    }).ToList();
                }
            }
            private string JsonFileName
            {
                get
                {
                    return Path.Combine("./data/ItemList.json");
                }
            }
            private string JsonFileName1
            {
                get
                {
                    return Path.Combine("./data/userSample.json");
                }
            }
            public List<redeem> GetAllRedeemItem()
            {
                return this.AllRedeemItem;
            }
            public List<user> GetAllUsername()
            {
                return this.AllUsername;
            }
            public void GetItem(string item, string name)
            {
                var found = this.AllRedeemItem
                                .FirstOrDefault
                                 (x => x.item == item);
                var found1 = this.AllUsername
                                .FirstOrDefault
                                 (x => x.name == name);

                if (found!= null & found1 != null)
                {
                    var deductPoints = found1.remainingpoints - found.points;
                    this.AllUsername.FirstOrDefault(x => x.name == name).remainingpoints = deductPoints;
                    this.Save();
                }
            }
            private void Save()
            {
                using (var outputStream = File.OpenWrite(JsonFileName1))
                {
                    JsonSerializer.Serialize<IEnumerable<user>>(
                        new Utf8JsonWriter(outputStream, new JsonWriterOptions
                        {
                            SkipValidation = true,
                            Indented = true
                        }),

                    this.AllUsername

                    );
                }
            }
        
    }
}