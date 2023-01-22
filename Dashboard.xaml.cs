// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ITATKWinUI;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public class ScriptCard
{
    public ScriptCard()
    {
        CardData = new ObservableCollection<Card>();
    }

    public ObservableCollection<Card> CardData { get; private set; }
}

public class Card
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Path { get; set; }
    public string psVersion { get; set; }
    public string featured { get; set; }
    public string pictureAssetName { get; set; }
    public DateTime dateAdded { get; set; }

}

public sealed partial class Dashboard : Page
{
    public static XDocument guiConfig;

    public Dashboard()
    {
        this.InitializeComponent();

        PopulateFeaturedTiles();

        PopulateRecentlyAdded();
    }

    private void PopulateFeaturedTiles()
    {
        List<ScriptCard> Cards = new List<ScriptCard>();

        guiConfig = XDocument.Load(@"XML\Scripts.xml");
        foreach (XElement item in from y in guiConfig.Descendants("Item") select y)
        {
            //stuff
            if (item.Attribute("featured").Value == "true")
            {
                var picAssetName = "";
                if (item.Attribute("pictureAssetName").Value == "default" || item.Attribute("pictureAssetName").Value == "Default")
                {
                    picAssetName = "Assets/FeaturedBg.jpg";
                } else {
                    picAssetName = "Assets/" + item.Attribute("pictureAssetName").Value;
                }

                ScriptCard c = new ScriptCard();
                c.CardData.Add(new Card() { Title = item.Attribute("name").Value, SubTitle = item.Attribute("description").Value, pictureAssetName = picAssetName });
                Cards.Add(c);
            }
        }

        featuredCards.Source= Cards;
    }

    private void PopulateRecentlyAdded()
    {
        List<ScriptCard> Cards = new List<ScriptCard>();

        //TODO: Get 6 most recently added scripts then create cards from them
        guiConfig = XDocument.Load(@"XML\Scripts.xml");
        foreach (XElement item in from y in guiConfig.Descendants("Item") select y)
        {
            var PSVersion = "";
            if (item.Attribute("psVersion").Value == "PS7")
            {
                PSVersion = "Assets/Powershell7.ico";
            }
            else if(item.Attribute("psVersion").Value == "PS5")
            {
                PSVersion = "Assets/Powershell5.png";
            }

            ScriptCard c = new ScriptCard();
            c.CardData.Add(new Card() { Title = item.Attribute("name").Value, SubTitle = item.Attribute("description").Value, psVersion = PSVersion });
            Cards.Add(c);
        }

        scriptCards.Source = Cards;
    }

    public string test = "";

    private void FeaturedGridView_ItemClick_1(object sender, ItemClickEventArgs e)
    {
        //TODO: What happens when you click a featured item?
        Debug.WriteLine("It ran after clicking!"); //Why isn't this working?
    }
}
