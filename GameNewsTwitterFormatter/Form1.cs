using System;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string URL = urlTextBox.Text;
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            /* Looking for steampowered.com or youtube.com addresses.
             * Checks only for youtu in the string provided because it's easier;
             * Why check for both youtu.be and youtube when we can just check for youtu, right?
             */
            if (URL.Contains("steampowered") || URL.Contains("youtu"))
            {
                doc = web.Load(URL);
                if (URL.Contains("steampowered"))
                {
                    /*********************
                     * [GAME_NAME] <meta property="og:title" content="MONSTER HUNTER: WORLD on Steam">
                     * [STORE_LINK] <meta property="og:url" content="https://store.steampowered.com/app/582010/MONSTER_HUNTER_WORLD/">
                     * [COVER_IMAGE] <meta property="og:image" content="https://steamcdn-a.akamaihd.net/steam/apps/582010/header.jpg?t=1533890901">
                       ******************/

                    var nameNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']");
                    var urlNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:url']");
                    var coverImageNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']");

                    // grab the attributes of the game lash requested
                    string name = nameNode.Attributes["content"].Value;
                    string url = urlNode.Attributes["content"].Value;
                    string coverImage = coverImageNode.Attributes["content"].Value;

                    // now we need to massage the text strings for name and url
                    var trimIndex = name.IndexOf(" on Steam");
                    name = name.Remove(trimIndex);
                    // remove the last /
                    url = url.Remove(url.Length - 1, 1);
                    trimIndex = url.LastIndexOf("/");
                    url = url.Remove(trimIndex);

                    outputRichTextBox.Text = name + "\n" +
                                             url + "\n" +
                                             coverImage;
                }
                else if (URL.Contains("youtu"))
                {
                    /**********************
                     * [VIDEO_NAME]
                     * [VIDEO_LINK] {full link ... NOT youtu.be ... also without all the ref trash ... so for example: https://www.youtube.com/watch?v=riJ5JdF2lSY instead of https://www.youtube.com/watch?v=riJ5JdF2lSY&feature=em-uploademail}
                     * ********************/
                }
            }
            else
            {
                outputRichTextBox.Text = "Unrecognized URL: " + URL + ".\n" + "Domain is not supported at this time.";
            }
        }
    }
}
