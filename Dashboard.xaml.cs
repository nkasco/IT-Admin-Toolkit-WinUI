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
        Activities = new ObservableCollection<Activity>();
    }

    public string Title
    {
        get; set;
    }
    public ObservableCollection<Activity> Activities
    {
        get; private set;
    }
}

public class Activity
{
    public string Title { get; set; }
}

public sealed partial class Dashboard : Page
{
    public Dashboard()
    {
        this.InitializeComponent();

        PopulateFeaturedTiles();
    }

    private void PopulateFeaturedTiles()
    {
        List<ScriptCard> Cards = new List<ScriptCard>();

        ScriptCard c = new ScriptCard();
        c.Title = "Test Title 123";
        c.Activities.Add(new Activity { Title = "Activity Title 1" });

        Cards.Add(c);
        Cards.Add(c);
        Cards.Add(c);
        Cards.Add(c);
        Cards.Add(c);
        Cards.Add(c);

        scriptCards.Source= Cards;
    }
}
