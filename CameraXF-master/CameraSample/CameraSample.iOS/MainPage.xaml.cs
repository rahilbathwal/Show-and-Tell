using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Xml.Linq;

namespace CameraSample.iOS
{
    public partial class MainPage : ContentPage
    {

        const string subscriptionKey = "8424e3c410ff4e65bfda848a92ba237b";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Language",
                Name = "Object",
                CompressionQuality = 65    
            });
            if (photo != null)
            {
                string imageFilePath = photo.Path;
                MakeAnalysisRequest(imageFilePath);
            }
        }


        private async void MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "visualFeatures=Categories,Description,Color&language=en";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                //Source.Text = contentString;

                TranslateText(JsonPrettyPrint(contentString));

            }
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {

            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            /*string INDENT_STRING = "    ";
            var indent = 0;
            var quoted = false;*/
            var sb = new StringBuilder();
            for (var i = 0; i < json.Length; i++)
            {
                var ch = json[i];
                if (ch.Equals('t'))
                {
                    if (json[i + 1].Equals('e'))
                    {
                        if (json[i + 2].Equals('x'))
                        {
                            if (json[i + 3].Equals('t'))
                            {
                                if (json[i + 4].Equals('"'))
                                {
                                    var count = 0;
                                    var j = i + 5;
                                    while (count < 2)
                                    {
                                        if (json[j].Equals('"'))
                                        {
                                            count = count + 1;
                                        }
                                        else
                                        {
                                            sb.Append(json[j]);
                                        }
                                        j++;
                                    }
                                }
                            }
                        }
                    }    
                }        
                /*switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && json[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }*/
            }
            return sb.ToString();
        }
        static string host = "https://api.microsofttranslator.com";         static string path = "/V2/Http.svc/Translate";          // NOTE: Replace this example key with a valid subscription key.         static string key = "51bad81ee5334749b05724c9b94d1464";  
        private async void TranslateText(string input)         {             HttpClient client = new HttpClient();             client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);              List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>() {                 new KeyValuePair<string, string> (input, "fr-fr"),                 new KeyValuePair<string, string> (input, "sp-sp")             };             int c = 0;
            English.Text = input;             foreach (KeyValuePair<string, string> i in list)             {
                                 string uri = host + path + "?to=" + i.Value + "&text=" + System.Net.WebUtility.UrlEncode(i.Key);                  HttpResponseMessage response = await client.GetAsync(uri);                  string result = await response.Content.ReadAsStringAsync();                 // NOTE: A successful response is returned in XML. You can extract the contents of the XML as follows.
               // XElement x = new XElement();                 var content = XElement.Parse(result).Value;
                if (c == 0)
                {
                    French.Text = content;
                }
                else{
                    Spanish.Text = content;
                }
                 //return content;
                //return result;             }
         } 
    }
    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}
