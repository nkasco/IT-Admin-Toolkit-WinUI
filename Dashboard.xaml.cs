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
using Windows.UI.ViewManagement;

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

        DashboardHeaderText.Text = MainWindow.MainTitle;

        DashboardSubHeaderText.Text = MainWindow.MainSubTitle;

        PopulateFeaturedTiles();

        PopulateRecentlyAdded();
    }

    private void PopulateFeaturedTiles()
    {
        List<ScriptCard> Cards = new List<ScriptCard>();

        var MaxCount = 6; //TODO: FeaturedTiles MaxCount should be a configurable setting
        var CurrentCount = 0;

        guiConfig = XDocument.Load(@"XML\Scripts.xml");
        foreach (XElement item in from y in guiConfig.Descendants("Item") select y)
        {
            if (item.Attribute("featured").Value == "true" && CurrentCount < MaxCount)
            {
                CurrentCount++;
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

        //Get recently added scripts then create cards from them
        var MaxCount = 6; //TODO: RecentlyAdded MaxCount should be a configurable setting

        //Create a List that we can sort later
        List<Card> CardList = new List<Card>();

        guiConfig = XDocument.Load(@"XML\Scripts.xml");
        foreach (XElement item in from y in guiConfig.Descendants("Item") select y)
        {
            var PSVersion = "";
            if (item.Attribute("psVersion").Value == "PS7")
            {
                PSVersion = "Assets/Powershell7.ico";
            }
            else if (item.Attribute("psVersion").Value == "PS5")
            {
                PSVersion = "Assets/Powershell5.png";
            }

            Card c = new Card()
            {
                Title = item.Attribute("name").Value,
                SubTitle= item.Attribute("description").Value,
                dateAdded = DateTime.Parse(item.Attribute("dateAdded").Value),
                psVersion = PSVersion
            };

            CardList.Add(c);
        }

        //Sort the list descending by dateAdded and get the maximum amount allowed
        CardList = CardList.OrderByDescending(x => x.dateAdded).ToList();
        var actualMax = 0;
        //This prevents us from going out of the max range if there aren't that many scripts
        if(CardList.Count < MaxCount)
        {
            actualMax = CardList.Count;
        } else
        {
            actualMax = MaxCount;
        }
        CardList = CardList.GetRange(0,actualMax);

        foreach (Card item in from y in CardList select y)
        {
            ScriptCard c = new ScriptCard();
            c.CardData.Add(new Card() { Title = item.Title, SubTitle = item.SubTitle, psVersion = item.psVersion });
            Cards.Add(c);
        }

        scriptCards.Source = Cards;
    }

    private void NavigateMainFrame(string Page)
    {
        //Main Window contentPane should navigate to either item page or category page
        var window = (Application.Current as App)?.Window as MainWindow;

        //Find the category for the provided Page
        guiConfig = XDocument.Load(@"XML\Scripts.xml");
        var PageName = "";
        foreach (XElement item in from y in guiConfig.Descendants("Item") select y)
        {
            if (item.Attribute("name").Value == Page)
            {
                PageName = item.Attribute("category").Value;
            }
        }

        //Loop through the NavigationViewItem categories and navigate to the proper index
        for (var i = 0; i < window.NavigationViews.Count; i++)
        {
            if (window.NavigationViews[i].Content.ToString() == PageName)
            {
                window.mNav.SelectedItem = window.mNav.MenuItems.ElementAt(i + 3); //Add 3 to skip Dashboard, separator, and Categories header
            }
        }
    }


    private void FeaturedItemGridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        FeaturedItemsTextHeader.Text = "test";
        Card c = e.ClickedItem as Card;
        
        NavigateMainFrame(c.Title);
    }

    private void RecentlyAddedGridview_ItemClick(object sender, ItemClickEventArgs e)
    {
        Card c = e.ClickedItem as Card;

        NavigateMainFrame(c.Title);
    }
}
